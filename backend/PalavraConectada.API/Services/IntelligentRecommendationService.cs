// Serviço de Recomendação Inteligente - Máquina de Significado
// Busca versículos por temas secundários, padrões, histórias bíblicas e contexto
using PalavraConectada.API.Data;
using PalavraConectada.API.Models;
using Microsoft.EntityFrameworkCore;
using System.Text.RegularExpressions;

namespace PalavraConectada.API.Services;

/// <summary>
/// Serviço que faz recomendações inteligentes baseadas em:
/// - Temas secundários do texto
/// - Padrões de palavras do usuário
/// - Histórias bíblicas relacionadas
/// - Análise contextual dos versículos
/// </summary>
public class IntelligentRecommendationService
{
    private readonly BibleDbContext _context;
    private readonly BibleService _bibleService;
    private readonly ILogger<IntelligentRecommendationService> _logger;

    // Temas bíblicos com palavras-chave relacionadas
    private readonly Dictionary<string, List<string>> _biblicalThemes = new()
    {
        ["amor"] = new() { "amor", "amar", "amado", "caridade", "afeto", "ternura", "compaixão" },
        ["perdão"] = new() { "perdão", "perdoar", "perdoado", "misericórdia", "graça", "reconciliação" },
        ["esperança"] = new() { "esperança", "esperar", "confiança", "futuro", "promessa", "fé" },
        ["paz"] = new() { "paz", "tranquilidade", "calma", "serenidade", "descanso", "quietude" },
        ["coragem"] = new() { "coragem", "corajoso", "bravura", "força", "valentia", "ousadia" },
        ["gratidão"] = new() { "gratidão", "grato", "agradecer", "ação de graças", "louvor", "reconhecimento" },
        ["consolo"] = new() { "consolo", "conforto", "alívio", "encorajamento", "apoio", "sustento" },
        ["proteção"] = new() { "proteção", "proteger", "guarda", "refúgio", "abrigo", "defesa" }
    };

    // Histórias bíblicas relacionadas a emoções
    private readonly Dictionary<string, List<BibleStoryReference>> _emotionStories = new()
    {
        ["tristeza"] = new()
        {
            new() { Title = "Jó e o Sofrimento", Reference = "Jó 1-42", Theme = "Superação do sofrimento" },
            new() { Title = "Jesus no Getsêmani", Reference = "Mateus 26:36-46", Theme = "Tristeza e oração" },
            new() { Title = "Lázaro e as Irmãs", Reference = "João 11:1-44", Theme = "Consolo na perda" }
        },
        ["alegria"] = new()
        {
            new() { Title = "Davi Dançando", Reference = "2 Samuel 6:14-23", Theme = "Alegria em adorar" },
            new() { Title = "Filho Pródigo", Reference = "Lucas 15:11-32", Theme = "Alegria do retorno" },
            new() { Title = "Nascimento de Jesus", Reference = "Lucas 2:8-20", Theme = "Alegria da salvação" }
        },
        ["medo"] = new()
        {
            new() { Title = "Davi e Golias", Reference = "1 Samuel 17", Theme = "Coragem contra o medo" },
            new() { Title = "Daniel na Cova dos Leões", Reference = "Daniel 6", Theme = "Fé supera o medo" },
            new() { Title = "Jesus Acalma a Tempestade", Reference = "Marcos 4:35-41", Theme = "Proteção divina" }
        },
        ["ansiedade"] = new()
        {
            new() { Title = "Jesus Acalma a Tempestade", Reference = "Marcos 4:35-41", Theme = "Paz na ansiedade" },
            new() { Title = "Marta e Maria", Reference = "Lucas 10:38-42", Theme = "Prioridades e paz" },
            new() { Title = "Não se Preocupem", Reference = "Mateus 6:25-34", Theme = "Confiança em Deus" }
        },
        ["solidão"] = new()
        {
            new() { Title = "Elias no Deserto", Reference = "1 Reis 19:1-18", Theme = "Presença de Deus na solidão" },
            new() { Title = "Jesus no Deserto", Reference = "Mateus 4:1-11", Theme = "Companhia divina" },
            new() { Title = "Paulo na Prisão", Reference = "2 Timóteo 4:9-18", Theme = "Deus nunca abandona" }
        },
        ["raiva"] = new()
        {
            new() { Title = "Filho Pródigo", Reference = "Lucas 15:11-32", Theme = "Perdão e reconciliação" },
            new() { Title = "Jesus e os Vendedores", Reference = "João 2:13-22", Theme = "Raiva justa" },
            new() { Title = "Pedro Negando", Reference = "João 18:15-27", Theme = "Perdão após traição" }
        }
    };

    public IntelligentRecommendationService(
        BibleDbContext context,
        BibleService bibleService,
        ILogger<IntelligentRecommendationService> logger)
    {
        _context = context;
        _bibleService = bibleService;
        _logger = logger;
    }

    /// <summary>
    /// Busca versículos por temas secundários encontrados no texto
    /// </summary>
    public async Task<List<Verse>> SearchBySecondaryThemesAsync(string userText, string version = "nvi", int limit = 5)
    {
        _logger.LogInformation("🔍 Buscando versículos por temas secundários");

        var themes = ExtractThemesFromText(userText);
        var allVerses = new List<Verse>();

        foreach (var theme in themes)
        {
            if (_biblicalThemes.TryGetValue(theme, out var keywords))
            {
                // Buscar versículos com essas palavras-chave
                foreach (var keyword in keywords.Take(2)) // Limitar para não sobrecarregar
                {
                    var verses = await _bibleService.SearchVersesAsync(keyword, version, limit);
                    allVerses.AddRange(verses);
                }
            }
        }

        // Remover duplicatas e ordenar por relevância
        return allVerses
            .GroupBy(v => v.Id)
            .Select(g => g.First())
            .Take(limit)
            .ToList();
    }

    /// <summary>
    /// Identifica padrões de palavras do usuário e busca versículos contextuais
    /// </summary>
    public async Task<List<Verse>> SearchByWordPatternsAsync(string userText, string version = "nvi", int limit = 5)
    {
        _logger.LogInformation("🔍 Buscando versículos por padrões de palavras");

        // Extrair palavras significativas (não stop words)
        var significantWords = ExtractSignificantWords(userText);
        
        // Buscar versículos que contenham múltiplas dessas palavras (contexto)
        var verses = await _context.Verses
            .Where(v => v.Version == version)
            .ToListAsync();

        // Score baseado em quantas palavras significativas aparecem no versículo
        var scoredVerses = verses
            .Select(v => new
            {
                Verse = v,
                Score = significantWords.Count(word => 
                    v.Text.Contains(word, StringComparison.OrdinalIgnoreCase))
            })
            .Where(x => x.Score > 0)
            .OrderByDescending(x => x.Score)
            .Take(limit)
            .Select(x => x.Verse)
            .ToList();

        return scoredVerses;
    }

    /// <summary>
    /// Busca histórias bíblicas relacionadas à emoção
    /// </summary>
    public async Task<List<BibleStoryReference>> GetRelatedBibleStoriesAsync(string emotionName)
    {
        _logger.LogInformation("📖 Buscando histórias bíblicas para emoção: {Emotion}", emotionName);

        if (_emotionStories.TryGetValue(emotionName.ToLower(), out var stories))
        {
            await Task.CompletedTask;
            return stories;
        }

        return new List<BibleStoryReference>();
    }

    /// <summary>
    /// Faz análise resumida de um versículo (contexto e significado)
    /// </summary>
    public async Task<VerseAnalysis> AnalyzeVerseAsync(Verse verse)
    {
        _logger.LogInformation("📝 Analisando versículo: {Reference}", $"{verse.BookName} {verse.Chapter}:{verse.Number}");

        // Buscar versículos próximos para contexto
        var contextVerses = await _context.Verses
            .Where(v => v.BookAbbrev == verse.BookAbbrev &&
                       v.Chapter == verse.Chapter &&
                       v.Version == verse.Version &&
                       Math.Abs(v.Number - verse.Number) <= 2) // ±2 versículos
            .OrderBy(v => v.Number)
            .ToListAsync();

        // Identificar temas principais do versículo
        var themes = IdentifyThemesInVerse(verse.Text);

        // Gerar resumo contextual
        var summary = GenerateContextualSummary(verse, contextVerses, themes);

        await Task.CompletedTask;

        return new VerseAnalysis
        {
            Verse = verse,
            ContextVerses = contextVerses.Where(v => v.Id != verse.Id).ToList(),
            Themes = themes,
            Summary = summary,
            MainMessage = ExtractMainMessage(verse.Text)
        };
    }

    /// <summary>
    /// Extrai temas do texto do usuário
    /// </summary>
    private List<string> ExtractThemesFromText(string text)
    {
        var normalizedText = text.ToLower();
        var themes = new List<string>();

        foreach (var theme in _biblicalThemes.Keys)
        {
            if (_biblicalThemes[theme].Any(keyword => normalizedText.Contains(keyword)))
            {
                themes.Add(theme);
            }
        }

        return themes;
    }

    /// <summary>
    /// Extrai palavras significativas do texto (remove stop words)
    /// </summary>
    private List<string> ExtractSignificantWords(string text)
    {
        var stopWords = new HashSet<string>
        {
            "estou", "estão", "está", "sinto", "me", "muito", "hoje", "agora",
            "de", "da", "do", "com", "para", "por", "em", "um", "uma",
            "o", "a", "os", "as", "que", "se", "eu", "você", "não", "mas"
        };

        var words = Regex.Split(text.ToLower(), @"\W+")
            .Where(w => w.Length > 3 && !stopWords.Contains(w))
            .Distinct()
            .ToList();

        return words;
    }

    /// <summary>
    /// Identifica temas em um versículo
    /// </summary>
    private List<string> IdentifyThemesInVerse(string verseText)
    {
        var normalizedText = verseText.ToLower();
        var themes = new List<string>();

        foreach (var theme in _biblicalThemes.Keys)
        {
            if (_biblicalThemes[theme].Any(keyword => normalizedText.Contains(keyword)))
            {
                themes.Add(theme);
            }
        }

        return themes;
    }

    /// <summary>
    /// Gera resumo contextual do versículo
    /// </summary>
    private string GenerateContextualSummary(Verse verse, List<Verse> contextVerses, List<string> themes)
    {
        var summaryParts = new List<string>();

        // Adicionar contexto do livro
        summaryParts.Add($"Este versículo está em {verse.BookName}, um livro do {verse.Testament}.");

        // Adicionar temas identificados
        if (themes.Any())
        {
            summaryParts.Add($"Os temas principais são: {string.Join(", ", themes)}.");
        }

        // Adicionar contexto dos versículos próximos
        if (contextVerses.Any())
        {
            summaryParts.Add($"Este versículo faz parte de um contexto maior no capítulo {verse.Chapter}.");
        }

        return string.Join(" ", summaryParts);
    }

    /// <summary>
    /// Extrai mensagem principal do versículo
    /// </summary>
    private string ExtractMainMessage(string verseText)
    {
        // Simplificado: pegar primeira frase ou até 100 caracteres
        var sentences = verseText.Split(new[] { '.', '!', '?' }, StringSplitOptions.RemoveEmptyEntries);
        if (sentences.Any())
        {
            return sentences.First().Trim();
        }

        return verseText.Length > 100 ? verseText.Substring(0, 100) + "..." : verseText;
    }
}

/// <summary>
/// Referência a uma história bíblica
/// </summary>
public class BibleStoryReference
{
    public string Title { get; set; } = string.Empty;
    public string Reference { get; set; } = string.Empty;
    public string Theme { get; set; } = string.Empty;
}

/// <summary>
/// Análise completa de um versículo
/// </summary>
public class VerseAnalysis
{
    public Verse Verse { get; set; } = null!;
    public List<Verse> ContextVerses { get; set; } = new();
    public List<string> Themes { get; set; } = new();
    public string Summary { get; set; } = string.Empty;
    public string MainMessage { get; set; } = string.Empty;
}

