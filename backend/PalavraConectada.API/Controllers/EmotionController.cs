// Controller de Emoções - Analisa sentimentos do usuário
// Como um pastor que ouve as ovelhas, este controller ouve os sentimentos
using Microsoft.AspNetCore.Mvc;
using PalavraConectada.API.Services;
using PalavraConectada.API.Data;
using Microsoft.EntityFrameworkCore;
using PalavraConectada.API.Models;

namespace PalavraConectada.API.Controllers;

/// <summary>
/// Controller para análise de emoções e recomendações personalizadas
/// </summary>
[ApiController]
[Route("api/[controller]")]
public class EmotionController : ControllerBase
{
    private readonly EmotionAnalyzerService _emotionAnalyzer;
    private readonly BibleDbContext _context;
    private readonly ILogger<EmotionController> _logger;

    public EmotionController(
        EmotionAnalyzerService emotionAnalyzer,
        BibleDbContext context,
        ILogger<EmotionController> logger)
    {
        _emotionAnalyzer = emotionAnalyzer;
        _context = context;
        _logger = logger;
    }

    /// <summary>
    /// Analisa o texto do usuário e detecta a emoção
    /// </summary>
    /// <param name="request">Texto do usuário</param>
    /// <returns>Emoção detectada com sugestões</returns>
    /// <example>
    /// POST /api/emotion/analyze
    /// Body: { "text": "Estou muito triste hoje" }
    /// </example>
    [HttpPost("analyze")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<EmotionResponse>> AnalyzeEmotion([FromBody] EmotionRequest request)
    {
        _logger.LogInformation("📥 Requisição de análise de emoção recebida");

        if (string.IsNullOrWhiteSpace(request.Text))
        {
            return BadRequest(new { error = "Texto não pode ser vazio" });
        }

        try
        {
            // Analisar emoção
            var analysis = await _emotionAnalyzer.AnalyzeEmotionAsync(request.Text);

            // Buscar sugestões
            var suggestions = await _emotionAnalyzer.GetSuggestionsAsync(analysis.DetectedEmotion);

            // Registrar interação
            var interaction = new UserInteraction
            {
                UserInput = request.Text,
                DetectedEmotion = analysis.DetectedEmotion,
                RecommendedVerseReference = string.Empty, // Será preenchido quando buscar versículo
                CreatedAt = DateTime.UtcNow
            };
            _context.UserInteractions.Add(interaction);
            await _context.SaveChangesAsync();

            // Montar resposta
            var response = new EmotionResponse
            {
                DetectedEmotion = analysis.DetectedEmotion,
                Confidence = analysis.Confidence,
                Message = analysis.Message,
                RecommendationType = analysis.RecommendationType,
                Suggestions = suggestions,
                InteractionId = interaction.Id
            };

            _logger.LogInformation("✅ Emoção '{Emotion}' detectada com {Confidence}% de confiança",
                analysis.DetectedEmotion, analysis.Confidence);

            return Ok(response);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "❌ Erro ao analisar emoção");
            return StatusCode(500, new { error = "Erro ao processar análise de emoção" });
        }
    }

    /// <summary>
    /// Lista todas as emoções disponíveis
    /// </summary>
    /// <returns>Lista de emoções</returns>
    [HttpGet("list")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<ActionResult<List<Emotion>>> GetEmotions()
    {
        var emotions = await _context.Emotions.ToListAsync();
        return Ok(emotions);
    }

    /// <summary>
    /// Busca sugestões para uma emoção específica
    /// </summary>
    /// <param name="emotionName">Nome da emoção (ex: tristeza, alegria)</param>
    /// <returns>Lista de sugestões</returns>
    [HttpGet("{emotionName}/suggestions")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<List<string>>> GetSuggestions(string emotionName)
    {
        var suggestions = await _emotionAnalyzer.GetSuggestionsAsync(emotionName);
        
        if (!suggestions.Any())
        {
            return NotFound(new { error = $"Emoção '{emotionName}' não encontrada" });
        }

        return Ok(suggestions);
    }

    /// <summary>
    /// Obtém estatísticas de uso (quais emoções são mais buscadas)
    /// </summary>
    /// <returns>Estatísticas de emoções</returns>
    [HttpGet("stats")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<ActionResult> GetEmotionStats()
    {
        var stats = await _context.UserInteractions
            .GroupBy(i => i.DetectedEmotion)
            .Select(g => new
            {
                Emotion = g.Key,
                Count = g.Count(),
                LastUsed = g.Max(i => i.CreatedAt)
            })
            .OrderByDescending(s => s.Count)
            .ToListAsync();

        return Ok(stats);
    }
}


