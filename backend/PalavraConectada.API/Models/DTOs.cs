// DTOs - Data Transfer Objects
// Objetos para transferência de dados entre API e Frontend
namespace PalavraConectada.API.Models;

// ═══════════════════════════════════════════════════════════════════════════
// EMOTION DTOs
// ═══════════════════════════════════════════════════════════════════════════

public class EmotionRequest
{
    public string Text { get; set; } = string.Empty;
}

public class EmotionResponse
{
    public string DetectedEmotion { get; set; } = string.Empty;
    public int Confidence { get; set; }
    public string Message { get; set; } = string.Empty;
    public string RecommendationType { get; set; } = string.Empty;
    public List<string> Suggestions { get; set; } = new();
    public int InteractionId { get; set; }
}

// ═══════════════════════════════════════════════════════════════════════════
// VERSE DTOs
// ═══════════════════════════════════════════════════════════════════════════

public class RecommendationRequest
{
    public string Text { get; set; } = string.Empty;
    public string Version { get; set; } = "nvi";
}

public class RecommendationResponse
{
    public string UserInput { get; set; } = string.Empty;
    public string DetectedEmotion { get; set; } = string.Empty;
    public int Confidence { get; set; }
    public string Message { get; set; } = string.Empty;
    public Verse? RecommendedVerse { get; set; }
    public List<Verse> AlternativeVerses { get; set; } = new();
    public List<string> Suggestions { get; set; } = new();
}

public class SearchVerseResponse
{
    public string Keyword { get; set; } = string.Empty;
    public string Version { get; set; } = string.Empty;
    public int Count { get; set; }
    public List<Verse> Verses { get; set; } = new();
}

// ═══════════════════════════════════════════════════════════════════════════
// EXTERNAL API DTOs - REMOVIDOS (não usamos mais APIs externas)
// Agora usamos apenas o banco de dados local
// ═══════════════════════════════════════════════════════════════════════════

// ═══════════════════════════════════════════════════════════════════════════
// NEW: BUSCA COMPLETA + FRASE MOTIVACIONAL
// ═══════════════════════════════════════════════════════════════════════════

public class SearchAllRequest
{
    public string Keyword { get; set; } = string.Empty;
    public string Version { get; set; } = "nvi";
}

public class MotivationalRequest
{
    public string Text { get; set; } = string.Empty;
    public string Version { get; set; } = "nvi";
}

