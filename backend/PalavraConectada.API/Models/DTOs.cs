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
// EXTERNAL API DTOs (Brazilian API)
// ═══════════════════════════════════════════════════════════════════════════

public class BrasilianApiResponse
{
    public int Occurrence { get; set; }
    public string Version { get; set; } = string.Empty;
    public List<BrasilianApiVerse> Verses { get; set; } = new();
}

public class BrasilianApiVerse
{
    public BrasilianApiBook? Book { get; set; }
    public int Chapter { get; set; }
    public int Number { get; set; }
    public string Text { get; set; } = string.Empty;
}

public class BrasilianApiBook
{
    public BrasilianApiAbbrev? Abbrev { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Author { get; set; } = string.Empty;
    public string Group { get; set; } = string.Empty;
    public string Version { get; set; } = string.Empty;
}

public class BrasilianApiAbbrev
{
    public string Pt { get; set; } = string.Empty;
    public string En { get; set; } = string.Empty;
}

