// Serviço de Análise de Emoções - A Inteligência do Sistema
// Como José interpretava sonhos, este serviço interpreta sentimentos
using PalavraConectada.API.Data;
using PalavraConectada.API.Models;
using Microsoft.EntityFrameworkCore;

namespace PalavraConectada.API.Services;

/// <summary>
/// Serviço que analisa o texto do usuário e detecta emoções
/// Baseado em palavras-chave e contexto
/// </summary>
public class EmotionAnalyzerService
{
    private readonly BibleDbContext _context;
    private readonly ILogger<EmotionAnalyzerService> _logger;

    public EmotionAnalyzerService(
        BibleDbContext context,
        ILogger<EmotionAnalyzerService> logger)
    {
        _context = context;
        _logger = logger;
    }

    /// <summary>
    /// Analisa um texto e detecta a emoção predominante
    /// </summary>
    /// <param name="userInput">Texto do usuário (ex: "Estou triste hoje")</param>
    /// <returns>Emoção detectada com nível de confiança</returns>
    public async Task<EmotionAnalysisResult> AnalyzeEmotionAsync(string userInput)
    {
        _logger.LogInformation("🧠 Analisando emoção: {Input}", userInput);

        if (string.IsNullOrWhiteSpace(userInput))
        {
            return new EmotionAnalysisResult
            {
                DetectedEmotion = "neutra",
                Confidence = 0,
                Message = "Nenhum texto fornecido"
            };
        }

        // Normalizar texto
        var normalizedText = userInput.ToLower().Trim();
        
        // Buscar todas as emoções do banco
        var emotions = await _context.Emotions.ToListAsync();
        
        // Lista de emoções encontradas com pontuação
        var emotionScores = new Dictionary<Emotion, int>();

        foreach (var emotion in emotions)
        {
            var keywords = emotion.Keywords.Split(',', StringSplitOptions.RemoveEmptyEntries);
            var score = 0;

            foreach (var keyword in keywords)
            {
                var normalizedKeyword = keyword.Trim().ToLower();
                
                // Palavra exata = mais pontos
                if (normalizedText.Contains($" {normalizedKeyword} ") || 
                    normalizedText.StartsWith(normalizedKeyword) ||
                    normalizedText.EndsWith(normalizedKeyword))
                {
                    score += 10;
                    _logger.LogDebug("✅ Palavra-chave encontrada: {Keyword} (emoção: {Emotion})", 
                        keyword, emotion.Name);
                }
                // Parte da palavra = menos pontos
                else if (normalizedText.Contains(normalizedKeyword))
                {
                    score += 3;
                }
            }

            if (score > 0)
            {
                emotionScores[emotion] = score;
            }
        }

        // Se não encontrou nenhuma emoção
        if (!emotionScores.Any())
        {
            _logger.LogInformation("⚠️ Nenhuma emoção detectada no texto");
            return new EmotionAnalysisResult
            {
                DetectedEmotion = "neutra",
                Confidence = 0,
                Message = "Não consegui identificar uma emoção específica. Tente frases como 'Estou triste' ou 'Estou com medo'."
            };
        }

        // Pegar a emoção com maior pontuação
        var topEmotion = emotionScores.OrderByDescending(x => x.Value).First();
        var maxScore = emotionScores.Values.Max();
        var confidence = Math.Min((maxScore / 10.0) * 100, 100); // Máximo 100%

        _logger.LogInformation("✅ Emoção detectada: {Emotion} (confiança: {Confidence}%)", 
            topEmotion.Key.Name, confidence);

        return new EmotionAnalysisResult
        {
            DetectedEmotion = topEmotion.Key.Name,
            Confidence = (int)confidence,
            EmotionId = topEmotion.Key.Id,
            Description = topEmotion.Key.Description,
            RecommendationType = topEmotion.Key.RecommendationType,
            Message = $"Detectei que você está sentindo {topEmotion.Key.Name}."
        };
    }

    /// <summary>
    /// Sugere próximas ações baseado na emoção detectada
    /// </summary>
    public async Task<List<string>> GetSuggestionsAsync(string emotionName)
    {
        var suggestions = new List<string>();

        switch (emotionName.ToLower())
        {
            case "tristeza":
                suggestions.Add("Versículos de consolo e esperança");
                suggestions.Add("História de Jó (superação do sofrimento)");
                suggestions.Add("Palavras de encorajamento");
                suggestions.Add("Oração de conforto");
                break;

            case "alegria":
                suggestions.Add("Versículos de louvor e gratidão");
                suggestions.Add("História de Davi dançando (2 Samuel 6)");
                suggestions.Add("Salmos de celebração");
                suggestions.Add("Ações de graças");
                break;

            case "medo":
                suggestions.Add("Versículos de coragem e proteção");
                suggestions.Add("História de Davi e Golias");
                suggestions.Add("Promessas de Deus sobre proteção");
                suggestions.Add("Oração por coragem");
                break;

            case "ansiedade":
                suggestions.Add("Versículos de paz e tranquilidade");
                suggestions.Add("História de Jesus acalmando a tempestade");
                suggestions.Add("Meditação bíblica");
                suggestions.Add("Respiração com versículos");
                break;

            case "solidão":
                suggestions.Add("Versículos sobre a presença de Deus");
                suggestions.Add("História de Elias no deserto");
                suggestions.Add("Promessas de companhia divina");
                suggestions.Add("Comunidade e igreja");
                break;

            case "raiva":
                suggestions.Add("Versículos sobre perdão");
                suggestions.Add("História do Filho Pródigo");
                suggestions.Add("Controle emocional na Bíblia");
                suggestions.Add("Oração por paz interior");
                break;

            case "gratidão":
                suggestions.Add("Versículos de ação de graças");
                suggestions.Add("História dos 10 leprosos");
                suggestions.Add("Salmos de louvor");
                suggestions.Add("Como expressar gratidão");
                break;

            case "esperança":
                suggestions.Add("Versículos de esperança futura");
                suggestions.Add("História de Abraão e a promessa");
                suggestions.Add("Promessas de Deus");
                suggestions.Add("Plano de Deus para você");
                break;

            default:
                suggestions.Add("Versículos inspiradores");
                suggestions.Add("Histórias bíblicas");
                suggestions.Add("Versículo do dia");
                break;
        }

        await Task.CompletedTask;
        return suggestions;
    }

    /// <summary>
    /// Extrai palavras-chave importantes do texto
    /// Remove stop words e foca no essencial
    /// </summary>
    private List<string> ExtractKeywords(string text)
    {
        // Stop words em português
        var stopWords = new HashSet<string>
        {
            "estou", "estão", "está", "sinto", "me", "muito", "hoje",
            "de", "da", "do", "com", "para", "por", "em", "um", "uma",
            "o", "a", "os", "as", "que", "se", "eu", "você"
        };

        var words = text.ToLower()
            .Split(new[] { ' ', ',', '.', '!', '?' }, StringSplitOptions.RemoveEmptyEntries)
            .Where(w => !stopWords.Contains(w) && w.Length > 2)
            .ToList();

        return words;
    }
}

/// <summary>
/// Resultado da análise de emoção
/// </summary>
public class EmotionAnalysisResult
{
    public string DetectedEmotion { get; set; } = string.Empty;
    public int EmotionId { get; set; }
    public int Confidence { get; set; } // 0-100%
    public string Description { get; set; } = string.Empty;
    public string RecommendationType { get; set; } = string.Empty;
    public string Message { get; set; } = string.Empty;
}

