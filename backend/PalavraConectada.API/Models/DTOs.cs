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
    
    // NOVO: Emoções secundárias detectadas
    public List<SecondaryEmotionDto> SecondaryEmotions { get; set; } = new();
    
    // NOVO: Palavras-chave detectadas
    public List<string> DetectedKeywords { get; set; } = new();
}

public class SecondaryEmotionDto
{
    public string Name { get; set; } = string.Empty;
    public int Confidence { get; set; }
    public int Score { get; set; }
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
    
    // NOVO: Recomendação inteligente
    public VerseAnalysisDto? VerseAnalysis { get; set; } // Análise do versículo recomendado
    public List<Verse> SecondaryThemeVerses { get; set; } = new(); // Versículos por temas secundários
    public List<BibleStoryReferenceDto> RelatedStories { get; set; } = new(); // Histórias bíblicas relacionadas
    public List<SecondaryEmotionDto> SecondaryEmotions { get; set; } = new(); // Emoções secundárias
}

public class VerseAnalysisDto
{
    public Verse Verse { get; set; } = null!;
    public List<Verse> ContextVerses { get; set; } = new();
    public List<string> Themes { get; set; } = new();
    public string Summary { get; set; } = string.Empty;
    public string MainMessage { get; set; } = string.Empty;
}

public class BibleStoryReferenceDto
{
    public string Title { get; set; } = string.Empty;
    public string Reference { get; set; } = string.Empty;
    public string Theme { get; set; } = string.Empty;
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

