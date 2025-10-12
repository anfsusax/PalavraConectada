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
    /// 💰 Versículos sobre Riqueza e Prosperidade
    /// </summary>
    [HttpGet("theme/prosperity")]
    public async Task<ActionResult<object>> GetProsperityVerses()
    {
        // Buscar no banco
        var verses = await _context.Verses
            .Where(v => v.Text.Contains("riqueza") || 
                       v.Text.Contains("prosperar") ||
                       v.Text.Contains("abundância") ||
                       v.Text.Contains("bênção"))
            .Take(10)
            .ToListAsync();

        return Ok(new
        {
            theme = "Riqueza & Prosperidade",
            description = "Versículos sobre bênçãos, prosperidade e abundância em Deus",
            count = verses.Count,
            verses
        });
    }

    /// <summary>
    /// ✝️ Plano de Salvação - Versículos essenciais
    /// </summary>
    [HttpGet("theme/salvation")]
    public async Task<ActionResult<object>> GetSalvationVerses()
    {
        // Versículos chave sobre salvação
        var salvationReferences = new[]
        {
            new { book = "jo", chapter = 3, verse = 16 },
            new { book = "rm", chapter = 3, verse = 23 },
            new { book = "rm", chapter = 6, verse = 23 },
            new { book = "rm", chapter = 5, verse = 8 },
            new { book = "rm", chapter = 10, verse = 9 },
            new { book = "ef", chapter = 2, verse = 8 }
        };

        var verses = await _context.Verses
            .Where(v => (v.BookAbbrev == "jo" && v.Chapter == 3 && v.Number == 16) ||
                       (v.BookAbbrev == "rm" && v.Chapter == 3 && v.Number == 23) ||
                       (v.BookAbbrev == "rm" && v.Chapter == 6 && v.Number == 23) ||
                       (v.BookAbbrev == "rm" && v.Chapter == 5 && v.Number == 8) ||
                       (v.BookAbbrev == "rm" && v.Chapter == 10 && v.Number == 9) ||
                       (v.BookAbbrev == "ef" && v.Chapter == 2 && v.Number == 8))
            .ToListAsync();

        return Ok(new
        {
            theme = "Salvação em Jesus Cristo",
            description = "O caminho da salvação explicado através das Escrituras",
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
            verses
        });
    }
}

