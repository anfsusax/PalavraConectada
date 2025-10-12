// Controller Administrativo - Gerenciamento do sistema
// Como os sacerdotes cuidavam do templo
using Microsoft.AspNetCore.Mvc;
using PalavraConectada.API.Services;
using PalavraConectada.API.Data;
using Microsoft.EntityFrameworkCore;

namespace PalavraConectada.API.Controllers;

/// <summary>
/// Controller para funções administrativas e de manutenção
/// </summary>
[ApiController]
[Route("api/[controller]")]
public class AdminController : ControllerBase
{
    private readonly BibleMigrationService _migrationService;
    private readonly ILogger<AdminController> _logger;

    public AdminController(
        BibleMigrationService migrationService,
        ILogger<AdminController> logger)
    {
        _migrationService = migrationService;
        _logger = logger;
    }

    /// <summary>
    /// 📊 Estatísticas do banco de dados
    /// </summary>
    [HttpGet("stats")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<ActionResult<object>> GetDatabaseStats()
    {
        _logger.LogInformation("📊 Buscando estatísticas do banco");

        try
        {
            var stats = await _migrationService.GetDatabaseStatsAsync();
            
            return Ok(new
            {
                summary = $"Banco com {stats.TotalVerses} versículos de {stats.Books} livros",
                verses = stats.TotalVerses,
                books = stats.Books,
                emotions = stats.TotalEmotions,
                relationships = stats.TotalRelationships,
                byVersion = stats.VersesByVersion,
                byTestament = stats.VersesByTestament,
                databaseSize = GetDatabaseSize()
            });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "❌ Erro ao buscar estatísticas");
            return StatusCode(500, new { error = "Erro ao buscar estatísticas" });
        }
    }


    /// <summary>
    /// 📗 Migra um livro específico da Bíblia
    /// Use este endpoint para migração controlada (livro por livro)
    /// </summary>
    [HttpPost("migrate-book")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<ActionResult<object>> MigrateBook(
        [FromQuery] string bookAbbrev,
        [FromQuery] string bookName,
        [FromQuery] int chapters,
        [FromQuery] string author = "Desconhecido",
        [FromQuery] string group = "Geral",
        [FromQuery] string testament = "VT",
        [FromQuery] string version = "nvi")
    {
        _logger.LogInformation("📗 Migrando livro: {BookName}", bookName);

        try
        {
            var bookInfo = new BookInfo
            {
                Abbrev = bookAbbrev,
                Name = bookName,
                Author = author,
                Group = group,
                Testament = testament,
                Chapters = chapters
            };

            var result = await _migrationService.MigrateBookAsync(bookInfo, version);

            return Ok(new
            {
                success = result.Success,
                book = bookName,
                versesAdded = result.VersesAdded,
                versesSkipped = result.VersesSkipped,
                message = result.Success 
                    ? $"✅ {bookName} migrado com sucesso! {result.VersesAdded} versículos adicionados." 
                    : $"❌ Erro: {result.ErrorMessage}"
            });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "❌ Erro ao migrar livro {BookName}", bookName);
            return StatusCode(500, new
            {
                success = false,
                error = ex.Message
            });
        }
    }

    /// <summary>
    /// 🗑️ Limpa o banco (apenas para desenvolvimento)
    /// </summary>
    [HttpDelete("clear-verses")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<ActionResult<object>> ClearVerses()
    {
        _logger.LogWarning("⚠️ LIMPANDO BANCO DE VERSÍCULOS");

        try
        {
            var count = await _migrationService.GetDatabaseStatsAsync();
            
            // Aqui você implementaria a lógica de limpeza
            // Por segurança, vou apenas retornar as estatísticas
            
            return Ok(new
            {
                message = "⚠️ Endpoint de limpeza - use com cuidado!",
                currentStats = count,
                warning = "Implemente a lógica de limpeza se necessário"
            });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "❌ Erro ao limpar banco");
            return StatusCode(500, new { error = ex.Message });
        }
    }

    private string GetDatabaseSize()
    {
        try
        {
            var dbPath = Path.Combine(AppContext.BaseDirectory, "bible.db");
            if (System.IO.File.Exists(dbPath))
            {
                var fileInfo = new FileInfo(dbPath);
                return $"{fileInfo.Length / 1024.0:F2} KB";
            }
            return "N/A";
        }
        catch
        {
            return "N/A";
        }
    }
}

/// <summary>
/// Controller para Biblioteca Bíblica
/// Organiza a Bíblia por testamentos e temas
/// </summary>
[ApiController]
[Route("api/[controller]")]
public class BibleLibraryController : ControllerBase
{
    private readonly BibleDbContext _context;
    private readonly ILogger<BibleLibraryController> _logger;

    public BibleLibraryController(
        BibleDbContext context,
        ILogger<BibleLibraryController> logger)
    {
        _context = context;
        _logger = logger;
    }

    /// <summary>
    /// 📖 Lista livros do Velho Testamento
    /// </summary>
    [HttpGet("old-testament")]
    public async Task<ActionResult<object>> GetOldTestament()
    {
        var books = await _context.Verses
            .Where(v => v.Testament == "VT")
            .Select(v => new { v.BookName, v.BookAbbrev, v.Author, v.Group })
            .Distinct()
            .ToListAsync();

        return Ok(new
        {
            testament = "Velho Testamento",
            totalBooks = books.Count,
            books
        });
    }

    /// <summary>
    /// ✝️ Lista livros do Novo Testamento
    /// </summary>
    [HttpGet("new-testament")]
    public async Task<ActionResult<object>> GetNewTestament()
    {
        var books = await _context.Verses
            .Where(v => v.Testament == "NT")
            .Select(v => new { v.BookName, v.BookAbbrev, v.Author, v.Group })
            .Distinct()
            .ToListAsync();

        return Ok(new
        {
            testament = "Novo Testamento",
            totalBooks = books.Count,
            books
        });
    }

    /// <summary>
    /// 💰 Versículos sobre Riqueza e Prosperidade (ALEATÓRIOS)
    /// </summary>
    [HttpGet("theme/prosperity")]
    public async Task<ActionResult<object>> GetProsperityVerses()
    {
        // Buscar no banco e ALEATORIZAR
        var allVerses = await _context.Verses
            .Where(v => v.Text.Contains("riqueza") || 
                       v.Text.Contains("prosperar") ||
                       v.Text.Contains("abundância") ||
                       v.Text.Contains("bênção") ||
                       v.Text.Contains("aben") ||
                       v.Text.Contains("prosperar") ||
                       v.Text.Contains("multiplicar") ||
                       v.Text.Contains("fartura"))
            .ToListAsync();

        // Randomizar e pegar 8 versículos
        var random = new Random();
        var verses = allVerses
            .OrderBy(x => random.Next())
            .Take(8)
            .ToList();

        return Ok(new
        {
            theme = "Riqueza & Prosperidade",
            description = "Versículos sobre bênçãos, prosperidade e abundância em Deus (Aleatórios)",
            count = verses.Count,
            totalAvailable = allVerses.Count,
            verses
        });
    }

    /// <summary>
    /// ✝️ Plano de Salvação - Versículos essenciais (ALEATÓRIOS)
    /// </summary>
    [HttpGet("theme/salvation")]
    public async Task<ActionResult<object>> GetSalvationVerses()
    {
        // Buscar versículos sobre salvação, Jesus, graça, fé
        var allVerses = await _context.Verses
            .Where(v => v.Text.Contains("salvação") || 
                       v.Text.Contains("salvo") ||
                       v.Text.Contains("salva") ||
                       v.Text.Contains("Jesus") ||
                       v.Text.Contains("Cristo") ||
                       v.Text.Contains("graça") ||
                       v.Text.Contains("fé") ||
                       v.Text.Contains("crê") ||
                       v.Text.Contains("eternainst"))
            .ToListAsync();

        // Randomizar e pegar 8 versículos
        var random = new Random();
        var verses = allVerses
            .OrderBy(x => random.Next())
            .Take(8)
            .ToList();

        return Ok(new
        {
            theme = "Salvação em Jesus Cristo",
            description = "O caminho da salvação explicado através das Escrituras (Aleatórios)",
            steps = new[]
            {
                "1. Deus ama você (João 3:16)",
                "2. Todos pecaram (Romanos 3:23)",
                "3. O salário do pecado é a morte (Romanos 6:23)",
                "4. Cristo morreu por você (Romanos 5:8)",
                "5. Confesse e creia (Romanos 10:9)",
                "6. Salvação pela graça (Efésios 2:8-9)"
            },
            count = verses.Count,
            totalAvailable = allVerses.Count,
            verses
        });
    }

    /// <summary>
    /// 📖 Buscar capítulos de um livro específico
    /// </summary>
    [HttpGet("book/{bookAbbrev}/chapters")]
    public async Task<ActionResult<object>> GetBookChapters(string bookAbbrev)
    {
        var chapters = await _context.Verses
            .Where(v => v.BookAbbrev.ToLower() == bookAbbrev.ToLower())
            .Select(v => new { v.Chapter, v.BookName })
            .Distinct()
            .OrderBy(v => v.Chapter)
            .ToListAsync();

        if (!chapters.Any())
        {
            return NotFound(new { message = $"Livro '{bookAbbrev}' não encontrado no banco" });
        }

        return Ok(new
        {
            bookAbbrev,
            bookName = chapters.First().BookName,
            totalChapters = chapters.Count,
            chapters = chapters.Select(c => c.Chapter).ToList()
        });
    }

    /// <summary>
    /// 📜 Buscar versículos de um capítulo específico
    /// </summary>
    [HttpGet("book/{bookAbbrev}/chapter/{chapterNumber}")]
    public async Task<ActionResult<object>> GetChapterVerses(string bookAbbrev, int chapterNumber)
    {
        var verses = await _context.Verses
            .Where(v => v.BookAbbrev.ToLower() == bookAbbrev.ToLower() && 
                       v.Chapter == chapterNumber)
            .OrderBy(v => v.Number)
            .ToListAsync();

        if (!verses.Any())
        {
            return NotFound(new { message = $"Capítulo {chapterNumber} do livro '{bookAbbrev}' não encontrado" });
        }

        return Ok(new
        {
            bookAbbrev,
            bookName = verses.First().BookName,
            chapter = chapterNumber,
            count = verses.Count,
            verses
        });
    }

    /// <summary>
    /// 🔍 Buscar versículos por palavra-chave na biblioteca
    /// </summary>
    [HttpGet("search")]
    public async Task<ActionResult<object>> SearchLibrary([FromQuery] string keyword)
    {
        if (string.IsNullOrWhiteSpace(keyword))
        {
            return BadRequest(new { message = "Palavra-chave não pode ser vazia" });
        }

        var verses = await _context.Verses
            .Where(v => v.Text.Contains(keyword) || 
                       v.BookName.Contains(keyword))
            .Take(20)
            .ToListAsync();

        return Ok(new
        {
            keyword,
            count = verses.Count,
            verses
        });
    }
}

