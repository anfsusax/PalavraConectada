// Modelos de dados - Representam a estrutura da Bíblia
namespace PalavraConectada.API.Models;

/// <summary>
/// Representa um versículo bíblico
/// Como as palavras sagradas escritas nas tábuas da lei
/// </summary>
public class Verse
{
    public int Id { get; set; }
    
    // Informações do livro
    public string BookName { get; set; } = string.Empty;
    public string BookAbbrev { get; set; } = string.Empty;
    public string Author { get; set; } = string.Empty;
    public string Group { get; set; } = string.Empty;
    public string Testament { get; set; } = string.Empty; // VT ou NT
    
    // Localização do versículo
    public int Chapter { get; set; }
    public int Number { get; set; }
    
    // Conteúdo
    public string Text { get; set; } = string.Empty;
    public string Version { get; set; } = "nvi"; // nvi, acf, aa
    
    // Metadados
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    
    // Relacionamento com emoções
    public List<VerseEmotion> VerseEmotions { get; set; } = new();
}

/// <summary>
/// Representa uma emoção/sentimento
/// Como os diversos sentimentos expressos nos Salmos
/// </summary>
public class Emotion
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty; // tristeza, alegria, medo, etc
    public string Keywords { get; set; } = string.Empty; // Palavras-chave separadas por vírgula
    public string Description { get; set; } = string.Empty;
    public string RecommendationType { get; set; } = string.Empty; // consolo, encorajamento, etc
    
    // Relacionamento
    public List<VerseEmotion> VerseEmotions { get; set; } = new();
}

/// <summary>
/// Relacionamento muitos-para-muitos entre Versículos e Emoções
/// Um versículo pode se relacionar com múltiplas emoções
/// </summary>
public class VerseEmotion
{
    public int VerseId { get; set; }
    public Verse Verse { get; set; } = null!;
    
    public int EmotionId { get; set; }
    public Emotion Emotion { get; set; } = null!;
    
    public int Relevance { get; set; } = 1; // 1-10: quão relevante é este versículo para esta emoção
}

/// <summary>
/// História bíblica para ensinar através de exemplos
/// Como Jesus ensinava por parábolas
/// </summary>
public class BibleStory
{
    public int Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Summary { get; set; } = string.Empty;
    public string Reference { get; set; } = string.Empty; // Ex: "Gênesis 1:1-31"
    public string MainLesson { get; set; } = string.Empty;
    public string RelatedEmotion { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}

/// <summary>
/// Histórico de interações do usuário
/// Para aprender e melhorar recomendações
/// </summary>
public class UserInteraction
{
    public int Id { get; set; }
    public string UserInput { get; set; } = string.Empty; // "Estou triste"
    public string DetectedEmotion { get; set; } = string.Empty;
    public string RecommendedVerseReference { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}

