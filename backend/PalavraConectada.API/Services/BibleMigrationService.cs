// Serviço de Migração Inteligente da Bíblia
// Como Esdras organizando e restaurando as Escrituras
using PalavraConectada.API.Models;
using PalavraConectada.API.Data;
using Microsoft.EntityFrameworkCore;

namespace PalavraConectada.API.Services;

/// <summary>
/// Serviço responsável por popular o banco com a Bíblia completa
/// de forma inteligente, com controle de progresso e retry automático
/// </summary>
public class BibleMigrationService
{
    private readonly BibleDbContext _context;
    private readonly LocalBibleJsonService _localBibleService;
    private readonly ILogger<BibleMigrationService> _logger;

    public BibleMigrationService(
        BibleDbContext context,
        LocalBibleJsonService localBibleService,
        ILogger<BibleMigrationService> logger)
    {
        _context = context;
        _localBibleService = localBibleService;
        _logger = logger;
    }


    /// <summary>
    /// Migra a Bíblia completa dos arquivos JSON locais
    /// </summary>
    public async Task<MigrationResult> MigrateBibleAsync(string version = "nvi", bool forceReimport = false)
    {
        _logger.LogInformation("📚 Iniciando migração da Bíblia completa (versão: {Version})", version);
        
        var result = new MigrationResult { Version = version, StartTime = DateTime.UtcNow };

        try
        {
            // 1. Buscar lista de livros dos arquivos JSON locais
            var books = await _localBibleService.GetBooksListAsync(version);
            
            if (books == null || !books.Any())
            {
                result.Success = false;
                result.ErrorMessage = "Não foi possível carregar os livros. Verifique se os arquivos JSON estão em biblia-master/json/";
                return result;
            }

            result.TotalBooks = books.Count;
            _logger.LogInformation("📖 {Count} livros encontrados", books.Count);

            // 2. Migrar cada livro
            foreach (var book in books)
            {
                _logger.LogInformation("📗 Migrando: {BookName} ({Testament})", 
                    book.Name, book.Testament);

                var bookResult = await MigrateBookAsync(book, version, forceReimport);
                
                result.BooksMigrated++;
                result.BooksProcessed = result.BooksMigrated;
                result.TotalVersesMigrated += bookResult.VersesAdded;
                result.TotalVersesAdded = result.TotalVersesMigrated;
                result.TotalVersesSkipped += bookResult.VersesSkipped;

                var progress = (int)((result.BooksMigrated / (double)books.Count) * 100);
                
                _logger.LogInformation("✅ {BookName}: {Added} adicionados, {Skipped} já existiam (Progresso: {Progress}%)", 
                    book.Name, bookResult.VersesAdded, bookResult.VersesSkipped, progress);
            }

            result.Success = true;
            result.EndTime = DateTime.UtcNow;
            
            _logger.LogInformation("🎉 Migração completa! {Total} versículos migrados em {Duration}", 
                result.TotalVersesMigrated, 
                result.EndTime - result.StartTime);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "❌ Erro durante migração");
            result.Success = false;
            result.ErrorMessage = ex.Message;
            result.EndTime = DateTime.UtcNow;
        }

        return result;
    }

    /// <summary>
    /// Migra um livro específico da Bíblia
    /// </summary>
    public async Task<BookMigrationResult> MigrateBookAsync(BookInfo book, string version, bool forceReimport = false)
    {
        var result = new BookMigrationResult { BookName = book.Name };

        try
        {
            // Buscar todos os capítulos do livro
            for (int chapter = 1; chapter <= book.Chapters; chapter++)
            {
                var chapterResult = await MigrateChapterAsync(book, chapter, version, forceReimport);
                
                result.VersesAdded += chapterResult.VersesAdded;
                result.VersesSkipped += chapterResult.VersesSkipped;
            }

            result.Success = true;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "❌ Erro ao migrar livro {BookName}", book.Name);
            result.Success = false;
            result.ErrorMessage = ex.Message;
        }

        return result;
    }

    /// <summary>
    /// Migra um capítulo específico dos arquivos JSON locais
    /// </summary>
    private async Task<ChapterMigrationResult> MigrateChapterAsync(
        BookInfo book, 
        int chapter, 
        string version,
        bool forceReimport = false)
    {
        var result = new ChapterMigrationResult();

        try
        {
            // Buscar capítulo dos arquivos JSON locais
            var verses = await _localBibleService.GetChapterVersesAsync(book.Abbrev, chapter, version);
            
            if (!verses.Any())
            {
                _logger.LogWarning("⚠️ Capítulo {Book} {Chapter} não encontrado nos arquivos JSON", 
                    book.Name, chapter);
                return result;
            }

            // Salvar versículos em batch
            foreach (var verseData in verses)
            {
                // Verificar se já existe (se não for reimportação forçada)
                if (!forceReimport)
                {
                    var exists = await _context.Verses.AnyAsync(v =>
                        v.BookAbbrev == book.Abbrev &&
                        v.Chapter == chapter &&
                        v.Number == verseData.Number &&
                        v.Version == version);

                    if (exists)
                    {
                        result.VersesSkipped++;
                        continue;
                    }
                }

                // Criar novo versículo
                var verse = new Verse
                {
                    BookName = book.Name,
                    BookAbbrev = book.Abbrev,
                    Author = book.Author,
                    Group = book.Group,
                    Testament = book.Testament,
                    Chapter = chapter,
                    Number = verseData.Number,
                    Text = verseData.Text,
                    Version = version
                };

                _context.Verses.Add(verse);
                result.VersesAdded++;
            }

            // Salvar em lote
            await _context.SaveChangesAsync();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "❌ Erro ao migrar {Book} {Chapter}", 
                book.Name, chapter);
            result.ErrorMessage = ex.Message;
        }

        return result;
    }

    // Métodos antigos removidos - agora usamos LocalBibleJsonService

    /// <summary>
    /// Obtém estatísticas do banco
    /// </summary>
    public async Task<DatabaseStats> GetDatabaseStatsAsync()
    {
        var stats = new DatabaseStats
        {
            TotalVerses = await _context.Verses.CountAsync(),
            TotalEmotions = await _context.Emotions.CountAsync(),
            TotalRelationships = await _context.VerseEmotions.CountAsync(),
            VersesByVersion = await _context.Verses
                .GroupBy(v => v.Version)
                .Select(g => new { Version = g.Key, Count = g.Count() })
                .ToDictionaryAsync(x => x.Version, x => x.Count),
            VersesByTestament = await _context.Verses
                .GroupBy(v => v.Testament)
                .Select(g => new { Testament = g.Key, Count = g.Count() })
                .ToDictionaryAsync(x => x.Testament, x => x.Count),
            Books = await _context.Verses
                .Select(v => v.BookName)
                .Distinct()
                .CountAsync()
        };

        return stats;
    }

    /// <summary>
    /// Limpa todos os versículos do banco de dados
    /// </summary>
    public async Task<ClearDatabaseResult> ClearAllVersesAsync()
    {
        _logger.LogWarning("🗑️ Limpando TODOS os versículos do banco de dados");
        
        var result = new ClearDatabaseResult { StartTime = DateTime.UtcNow };

        try
        {
            // Contar antes de limpar
            result.VersesDeleted = await _context.Verses.CountAsync();
            
            // Limpar todos os versículos
            _context.Verses.RemoveRange(_context.Verses);
            await _context.SaveChangesAsync();
            
            result.Success = true;
            result.EndTime = DateTime.UtcNow;
            
            _logger.LogInformation("✅ {Count} versículos removidos do banco", result.VersesDeleted);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "❌ Erro ao limpar banco de dados");
            result.Success = false;
            result.ErrorMessage = ex.Message;
            result.EndTime = DateTime.UtcNow;
        }

        return result;
    }

    /// <summary>
    /// Limpa versículos de uma versão específica
    /// </summary>
    public async Task<ClearDatabaseResult> ClearVersesByVersionAsync(string version)
    {
        _logger.LogWarning("🗑️ Limpando versículos da versão: {Version}", version);
        
        var result = new ClearDatabaseResult 
        { 
            StartTime = DateTime.UtcNow,
            Version = version
        };

        try
        {
            // Contar antes de limpar
            result.VersesDeleted = await _context.Verses
                .Where(v => v.Version == version)
                .CountAsync();
            
            // Limpar versículos da versão específica
            var versesToDelete = await _context.Verses
                .Where(v => v.Version == version)
                .ToListAsync();
            
            _context.Verses.RemoveRange(versesToDelete);
            await _context.SaveChangesAsync();
            
            result.Success = true;
            result.EndTime = DateTime.UtcNow;
            
            _logger.LogInformation("✅ {Count} versículos da versão {Version} removidos", 
                result.VersesDeleted, version);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "❌ Erro ao limpar versículos da versão {Version}", version);
            result.Success = false;
            result.ErrorMessage = ex.Message;
            result.EndTime = DateTime.UtcNow;
        }

        return result;
    }
}

// ═══════════════════════════════════════════════════════════════════════════
// MODELS PARA MIGRAÇÃO
// ═══════════════════════════════════════════════════════════════════════════

public class MigrationResult
{
    public bool Success { get; set; }
    public string Version { get; set; } = string.Empty;
    public int TotalBooks { get; set; }
    public int BooksMigrated { get; set; }
    public int TotalVersesMigrated { get; set; }
    public int TotalVersesAdded { get; set; } // Alias para TotalVersesMigrated
    public int TotalVersesSkipped { get; set; }
    public int BooksProcessed { get; set; } // Alias para BooksMigrated
    public DateTime StartTime { get; set; }
    public DateTime EndTime { get; set; }
    public TimeSpan Duration => EndTime - StartTime;
    public string? ErrorMessage { get; set; }
}

public class BookMigrationResult
{
    public bool Success { get; set; }
    public string BookName { get; set; } = string.Empty;
    public int VersesAdded { get; set; }
    public int VersesSkipped { get; set; }
    public string? ErrorMessage { get; set; }
}

public class ChapterMigrationResult
{
    public int VersesAdded { get; set; }
    public int VersesSkipped { get; set; }
    public string? ErrorMessage { get; set; }
}

public class BookInfo
{
    public string Abbrev { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public string Author { get; set; } = string.Empty;
    public string Group { get; set; } = string.Empty;
    public string Testament { get; set; } = string.Empty;
    public int Chapters { get; set; }
}

public class DatabaseStats
{
    public int TotalVerses { get; set; }
    public int TotalEmotions { get; set; }
    public int TotalRelationships { get; set; }
    public int Books { get; set; }
    public Dictionary<string, int> VersesByVersion { get; set; } = new();
    public Dictionary<string, int> VersesByTestament { get; set; } = new();
}

public class ClearDatabaseResult
{
    public bool Success { get; set; }
    public string? Version { get; set; }
    public int VersesDeleted { get; set; }
    public DateTime StartTime { get; set; }
    public DateTime EndTime { get; set; }
    public TimeSpan Duration => EndTime - StartTime;
    public string? ErrorMessage { get; set; }
}

// Modelos da API externa - REMOVIDOS (não usamos mais APIs externas)
// Agora usamos apenas LocalBibleJsonService para migração

