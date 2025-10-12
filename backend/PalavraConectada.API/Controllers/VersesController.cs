// Controller de Versículos - Busca e recomendação de versículos
// Como um escriba que conhece toda a Escritura
using Microsoft.AspNetCore.Mvc;
using PalavraConectada.API.Services;
using PalavraConectada.API.Data;
using Microsoft.EntityFrameworkCore;
using PalavraConectada.API.Models;

namespace PalavraConectada.API.Controllers;

/// <summary>
/// Controller para busca e recomendação de versículos bíblicos
/// </summary>
[ApiController]
[Route("api/[controller]")]
public class VersesController : ControllerBase
{
    private readonly BibleDbContext _context;
    private readonly BibleService _bibleService;
    private readonly ILogger<VersesController> _logger;

    public VersesController(
        BibleDbContext context,
        BibleService bibleService,
        ILogger<VersesController> logger)
    {
        _context = context;
        _bibleService = bibleService;
        _logger = logger;
    }

    /// <summary>
    /// Busca versículos por palavra-chave
    /// </summary>
    /// <param name="keyword">Palavra a buscar (ex: amor, fé)</param>
    /// <param name="version">Versão da Bíblia (nvi, acf, aa)</param>
    /// <returns>Lista de versículos encontrados</returns>
    [HttpGet("search")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<ActionResult<SearchVerseResponse>> SearchVerses(
        [FromQuery] string keyword,
        [FromQuery] string version = "nvi")
    {
        _logger.LogInformation("🔍 Buscando versículos: {Keyword} (versão: {Version})", 
            keyword, version);

        if (string.IsNullOrWhiteSpace(keyword))
        {
            return BadRequest(new { error = "Palavra-chave é obrigatória" });
        }

        try
        {
            var verses = await _bibleService.SearchVersesAsync(keyword, version);
            
            return Ok(new SearchVerseResponse
            {
                Keyword = keyword,
                Version = version,
                Count = verses.Count,
                Verses = verses
            });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "❌ Erro ao buscar versículos");
            return StatusCode(500, new { error = "Erro ao buscar versículos" });
        }
    }

    /// <summary>
    /// Busca versículos por emoção
    /// </summary>
    /// <param name="emotionName">Nome da emoção (ex: tristeza, alegria)</param>
    /// <param name="version">Versão da Bíblia</param>
    /// <param name="limit">Quantidade máxima de versículos</param>
    /// <returns>Versículos relacionados à emoção</returns>
    [HttpGet("by-emotion/{emotionName}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<List<Verse>>> GetVersesByEmotion(
        string emotionName,
        [FromQuery] string version = "nvi",
        [FromQuery] int limit = 10)
    {
        _logger.LogInformation("😊 Buscando versículos para emoção: {Emotion}", emotionName);

        var emotion = await _context.Emotions
            .FirstOrDefaultAsync(e => e.Name.ToLower() == emotionName.ToLower());

        if (emotion == null)
        {
            return NotFound(new { error = $"Emoção '{emotionName}' não encontrada" });
        }

        var verses = await _context.VerseEmotions
            .Where(ve => ve.EmotionId == emotion.Id)
            .Include(ve => ve.Verse)
            .OrderByDescending(ve => ve.Relevance)
            .Take(limit)
            .Select(ve => ve.Verse)
            .ToListAsync();

        // Se não tiver no banco, buscar nas APIs externas
        if (!verses.Any())
        {
            _logger.LogInformation("📡 Buscando nas APIs externas...");
            verses = await _bibleService.SearchVersesByEmotionAsync(emotionName, version);
        }

        return Ok(verses);
    }

    /// <summary>
    /// Busca um versículo aleatório
    /// </summary>
    /// <param name="version">Versão da Bíblia</param>
    /// <returns>Versículo aleatório</returns>
    [HttpGet("random")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<ActionResult<Verse>> GetRandomVerse([FromQuery] string version = "nvi")
    {
        _logger.LogInformation("🎲 Buscando versículo aleatório");

        try
        {
            var verse = await _bibleService.GetRandomVerseAsync(version);
            
            if (verse == null)
            {
                return NotFound(new { error = "Nenhum versículo encontrado" });
            }

            return Ok(verse);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "❌ Erro ao buscar versículo aleatório");
            return StatusCode(500, new { error = "Erro ao buscar versículo aleatório" });
        }
    }

    /// <summary>
    /// Recomendação inteligente baseada em texto livre
    /// Combina análise de emoção + busca de versículos
    /// </summary>
    /// <param name="request">Texto do usuário</param>
    /// <returns>Versículo recomendado para o sentimento</returns>
    [HttpPost("recommend")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<ActionResult<RecommendationResponse>> GetRecommendation(
        [FromBody] RecommendationRequest request)
    {
        _logger.LogInformation("💡 Gerando recomendação inteligente");

        if (string.IsNullOrWhiteSpace(request.Text))
        {
            return BadRequest(new { error = "Texto é obrigatório" });
        }

        try
        {
            // 1. Analisar emoção
            var emotionAnalyzer = HttpContext.RequestServices
                .GetRequiredService<EmotionAnalyzerService>();
            
            var analysis = await emotionAnalyzer.AnalyzeEmotionAsync(request.Text);

            // 2. Buscar versículos para esta emoção
            var verses = await _context.VerseEmotions
                .Where(ve => ve.Emotion.Name == analysis.DetectedEmotion)
                .Include(ve => ve.Verse)
                .OrderByDescending(ve => ve.Relevance)
                .Take(5)
                .Select(ve => ve.Verse)
                .ToListAsync();

            // 3. Se não tiver no banco, buscar externamente
            if (!verses.Any())
            {
                verses = await _bibleService.SearchVersesByEmotionAsync(
                    analysis.DetectedEmotion, 
                    request.Version);
            }

            // 4. Pegar um versículo aleatório da lista
            var recommendedVerse = verses.Any() 
                ? verses[new Random().Next(verses.Count)]
                : null;

            // 5. Buscar sugestões
            var suggestions = await emotionAnalyzer.GetSuggestionsAsync(analysis.DetectedEmotion);

            // 6. Atualizar interação com recomendação
            var interaction = await _context.UserInteractions
                .OrderByDescending(i => i.CreatedAt)
                .FirstOrDefaultAsync(i => i.DetectedEmotion == analysis.DetectedEmotion);

            if (interaction != null && recommendedVerse != null)
            {
                interaction.RecommendedVerseReference = 
                    $"{recommendedVerse.BookName} {recommendedVerse.Chapter}:{recommendedVerse.Number}";
                await _context.SaveChangesAsync();
            }

            return Ok(new RecommendationResponse
            {
                UserInput = request.Text,
                DetectedEmotion = analysis.DetectedEmotion,
                Confidence = analysis.Confidence,
                RecommendedVerse = recommendedVerse,
                AlternativeVerses = verses.Take(3).ToList(),
                Suggestions = suggestions,
                Message = analysis.Message
            });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "❌ Erro ao gerar recomendação");
            return StatusCode(500, new { error = "Erro ao gerar recomendação" });
        }
    }

    /// <summary>
    /// Obtém histórico de interações (para análise)
    /// </summary>
    /// <param name="limit">Quantidade de registros</param>
    /// <returns>Histórico de interações</returns>
    [HttpGet("history")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<ActionResult<List<UserInteraction>>> GetHistory([FromQuery] int limit = 10)
    {
        var history = await _context.UserInteractions
            .OrderByDescending(i => i.CreatedAt)
            .Take(limit)
            .ToListAsync();

        return Ok(history);
    }
}


