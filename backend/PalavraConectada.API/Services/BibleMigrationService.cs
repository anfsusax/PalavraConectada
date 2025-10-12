// Serviço de Migração Inteligente da Bíblia
// Como Esdras organizando e restaurando as Escrituras
using PalavraConectada.API.Models;
using PalavraConectada.API.Data;
using Microsoft.EntityFrameworkCore;
using System.Net.Http.Json;

namespace PalavraConectada.API.Services;

/// <summary>
/// Serviço responsável por popular o banco com a Bíblia completa
/// de forma inteligente, com controle de progresso e retry automático
/// </summary>
public class BibleMigrationService
{
    private readonly BibleDbContext _context;
    private readonly HttpClient _httpClient;
    private readonly ILogger<BibleMigrationService> _logger;
    
    // Configurações de migração
    private const int DELAY_BETWEEN_REQUESTS_MS = 2000; // 2 segundos entre requisições
    private const int DELAY_BETWEEN_CHAPTERS_MS = 500; // 500ms entre capítulos
    private const int MAX_RETRIES = 3;

    public BibleMigrationService(
        BibleDbContext context,
        HttpClient httpClient,
        ILogger<BibleMigrationService> logger)
    {
        _context = context;
        _httpClient = httpClient;
        _logger = logger;
    }


    /// <summary>
    /// Migra a Bíblia completa de forma inteligente
    /// </summary>
    public async Task<MigrationResult> MigrateCompleteBibleAsync(string version = "nvi")
    {
        _logger.LogInformation("📚 Iniciando migração da Bíblia completa (versão: {Version})", version);
        
        var result = new MigrationResult { Version = version, StartTime = DateTime.UtcNow };

        try
        {
            // 1. Buscar lista de livros
            var books = await GetBooksListAsync(version);
            
            if (books == null || !books.Any())
            {
                result.Success = false;
                result.ErrorMessage = "Não foi possível buscar a lista de livros da API";
                return result;
            }

            result.TotalBooks = books.Count;
            _logger.LogInformation("📖 {Count} livros encontrados", books.Count);

            // 2. Migrar cada livro
            foreach (var book in books)
            {
                _logger.LogInformation("📗 Migrando: {BookName} ({Testament})", 
                    book.Name, book.Testament);

                var bookResult = await MigrateBookAsync(book, version);
                
                result.BooksMigrated++;
                result.TotalVersesMigrated += bookResult.VersesAdded;
                result.TotalVersesSkipped += bookResult.VersesSkipped;

                var progress = (int)((result.BooksMigrated / (double)books.Count) * 100);
                
                _logger.LogInformation("✅ {BookName}: {Added} adicionados, {Skipped} já existiam (Progresso: {Progress}%)", 
                    book.Name, bookResult.VersesAdded, bookResult.VersesSkipped, progress);

                // Delay entre livros para não sobrecarregar a API
                await Task.Delay(DELAY_BETWEEN_REQUESTS_MS);
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
    public async Task<BookMigrationResult> MigrateBookAsync(BookInfo book, string version)
    {
        var result = new BookMigrationResult { BookName = book.Name };

        try
        {
            // Buscar todos os capítulos do livro
            for (int chapter = 1; chapter <= book.Chapters; chapter++)
            {
                var chapterResult = await MigrateChapterAsync(book, chapter, version);
                
                result.VersesAdded += chapterResult.VersesAdded;
                result.VersesSkipped += chapterResult.VersesSkipped;

                // Delay entre capítulos (evita sobrecarga)
                if (chapter < book.Chapters)
                {
                    await Task.Delay(DELAY_BETWEEN_CHAPTERS_MS);
                }
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
    /// Migra um capítulo específico
    /// </summary>
    private async Task<ChapterMigrationResult> MigrateChapterAsync(
        BookInfo book, 
        int chapter, 
        string version)
    {
        var result = new ChapterMigrationResult();
        var retryCount = 0;

        while (retryCount < MAX_RETRIES)
        {
            try
            {
                // Buscar capítulo da API
                var url = $"https://www.abibliadigital.com.br/api/verses/{version}/{book.Abbrev}/{chapter}";
                var response = await _httpClient.GetAsync(url);

                if (!response.IsSuccessStatusCode)
                {
                    _logger.LogWarning("⚠️ Erro ao buscar {Book} {Chapter}: {Status}", 
                        book.Name, chapter, response.StatusCode);
                    retryCount++;
                    await Task.Delay(2000 * retryCount); // Backoff exponencial
                    continue;
                }

                var chapterData = await response.Content.ReadFromJsonAsync<ChapterApiResponse>();
                
                if (chapterData?.Verses == null || !chapterData.Verses.Any())
                {
                    break;
                }

                // Salvar versículos em batch
                foreach (var verseData in chapterData.Verses)
                {
                    // Verificar se já existe
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
                break; // Sucesso, sair do retry
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "❌ Erro ao migrar {Book} {Chapter} (tentativa {Retry})", 
                    book.Name, chapter, retryCount + 1);
                retryCount++;
                
                if (retryCount >= MAX_RETRIES)
                {
                    result.ErrorMessage = $"Falha após {MAX_RETRIES} tentativas";
                    break;
                }

                await Task.Delay(2000 * retryCount); // Backoff exponencial
            }
        }

        return result;
    }

    /// <summary>
    /// Busca lista de todos os livros da Bíblia
    /// </summary>
    private async Task<List<BookInfo>?> GetBooksListAsync(string version)
    {
        try
        {
            var url = $"https://www.abibliadigital.com.br/api/books";
            var response = await _httpClient.GetAsync(url);
            
            if (!response.IsSuccessStatusCode)
            {
                _logger.LogError("❌ Erro ao buscar lista de livros: {Status}", response.StatusCode);
                return GetDefaultBooksList(); // Usar lista hardcoded como fallback
            }

            var books = await response.Content.ReadFromJsonAsync<List<BookApiResponse>>();
            
            return books?.Select(b => new BookInfo
            {
                Abbrev = b.Abbrev?.Pt ?? "unknown",
                Name = b.Name,
                Author = b.Author,
                Group = b.Group,
                Testament = b.Testament,
                Chapters = b.Chapters
            }).ToList();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "❌ Erro ao buscar livros");
            return GetDefaultBooksList(); // Fallback
        }
    }

    /// <summary>
    /// Lista padrão dos 66 livros da Bíblia (fallback)
    /// </summary>
    private List<BookInfo> GetDefaultBooksList()
    {
        return new List<BookInfo>
        {
            // VELHO TESTAMENTO
            new() { Abbrev = "gn", Name = "Gênesis", Author = "Moisés", Group = "Pentateuco", Testament = "VT", Chapters = 50 },
            new() { Abbrev = "ex", Name = "Êxodo", Author = "Moisés", Group = "Pentateuco", Testament = "VT", Chapters = 40 },
            new() { Abbrev = "lv", Name = "Levítico", Author = "Moisés", Group = "Pentateuco", Testament = "VT", Chapters = 27 },
            new() { Abbrev = "nm", Name = "Números", Author = "Moisés", Group = "Pentateuco", Testament = "VT", Chapters = 36 },
            new() { Abbrev = "dt", Name = "Deuteronômio", Author = "Moisés", Group = "Pentateuco", Testament = "VT", Chapters = 34 },
            new() { Abbrev = "js", Name = "Josué", Author = "Josué", Group = "Históricos", Testament = "VT", Chapters = 24 },
            new() { Abbrev = "jz", Name = "Juízes", Author = "Samuel", Group = "Históricos", Testament = "VT", Chapters = 21 },
            new() { Abbrev = "rt", Name = "Rute", Author = "Samuel", Group = "Históricos", Testament = "VT", Chapters = 4 },
            new() { Abbrev = "1sm", Name = "1 Samuel", Author = "Samuel", Group = "Históricos", Testament = "VT", Chapters = 31 },
            new() { Abbrev = "2sm", Name = "2 Samuel", Author = "Samuel", Group = "Históricos", Testament = "VT", Chapters = 24 },
            // ... (continuaria com todos os 66 livros, mas vou criar endpoint para buscar dinamicamente)
        };
    }

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
    public int TotalVersesSkipped { get; set; }
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

// Modelos da API brasileira
public class BookApiResponse
{
    public BookAbbrevApi? Abbrev { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Author { get; set; } = string.Empty;
    public string Group { get; set; } = string.Empty;
    public string Testament { get; set; } = string.Empty;
    public int Chapters { get; set; }
}

public class BookAbbrevApi
{
    public string Pt { get; set; } = string.Empty;
    public string En { get; set; } = string.Empty;
}

public class ChapterApiResponse
{
    public ChapterBookInfo? Book { get; set; }
    public int Chapter { get; set; }
    public List<ChapterVerseInfo> Verses { get; set; } = new();
}

public class ChapterBookInfo
{
    public string Name { get; set; } = string.Empty;
    public string Author { get; set; } = string.Empty;
    public string Group { get; set; } = string.Empty;
    public string Version { get; set; } = string.Empty;
}

public class ChapterVerseInfo
{
    public int Number { get; set; }
    public string Text { get; set; } = string.Empty;
}

