// Serviço de Análise de Emoções - A Inteligência do Sistema
// Versão melhorada: detecta múltiplas emoções, scores orgânicos, contexto humano
using PalavraConectada.API.Data;
using PalavraConectada.API.Models;
using Microsoft.EntityFrameworkCore;
using System.Text.RegularExpressions;

namespace PalavraConectada.API.Services;

/// <summary>
/// Serviço que analisa o texto do usuário e detecta emoções
/// Versão melhorada: detecta múltiplas emoções, scores orgânicos, contexto melhorado
/// </summary>
public class EmotionAnalyzerService
{
    private readonly BibleDbContext _context;
    private readonly ILogger<EmotionAnalyzerService> _logger;

    // Palavras de intensidade (aumentam o score)
    private readonly HashSet<string> _intensityWords = new()
    {
        "muito", "extremamente", "totalmente", "completamente", "realmente",
        "demais", "bastante", "tanto", "tão", "super", "ultra"
    };

    // Palavras de negação (diminuem o score)
    private readonly HashSet<string> _negationWords = new()
    {
        "não", "nem", "nunca", "jamais", "nenhum", "nada"
    };

    public EmotionAnalyzerService(
        BibleDbContext context,
        ILogger<EmotionAnalyzerService> logger)
    {
        _context = context;
        _logger = logger;
    }

    /// <summary>
    /// Analisa um texto e detecta emoções (melhorado: múltiplas emoções, scores orgânicos)
    /// </summary>
    public async Task<EmotionAnalysisResult> AnalyzeEmotionAsync(string userInput)
    {
        _logger.LogInformation("🧠 Analisando emoção (melhorado): {Input}", userInput);

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
        var normalizedText = NormalizeText(userInput);
        
        // Buscar todas as emoções do banco
        var emotions = await _context.Emotions.ToListAsync();
        
        // Lista de emoções encontradas com pontuação detalhada
        var emotionScores = new Dictionary<Emotion, EmotionScore>();

        foreach (var emotion in emotions)
        {
            var score = CalculateEmotionScore(normalizedText, emotion);
            if (score.TotalScore > 0)
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

        // Ordenar por score total
        var sortedEmotions = emotionScores.OrderByDescending(x => x.Value.TotalScore).ToList();
        var topEmotion = sortedEmotions.First();
        
        // Calcular confiança de forma mais orgânica (não sempre 100%)
        var confidence = CalculateOrganicConfidence(topEmotion.Value, sortedEmotions);

        // Detectar emoções secundárias (mistura de emoções)
        var secondaryEmotions = sortedEmotions
            .Skip(1)
            .Where(e => e.Value.TotalScore >= topEmotion.Value.TotalScore * 0.5) // Pelo menos 50% do score principal
            .Take(2)
            .Select(e => new SecondaryEmotion
            {
                Name = e.Key.Name,
                Confidence = CalculateOrganicConfidence(e.Value, sortedEmotions),
                Score = e.Value.TotalScore
            })
            .ToList();

        // Gerar mensagem mais humana
        var message = GenerateHumanMessage(topEmotion.Key, confidence, secondaryEmotions, normalizedText);

        _logger.LogInformation("✅ Emoção detectada: {Emotion} (confiança: {Confidence}%)", 
            topEmotion.Key.Name, confidence);

        return new EmotionAnalysisResult
        {
            DetectedEmotion = topEmotion.Key.Name,
            Confidence = confidence,
            EmotionId = topEmotion.Key.Id,
            Description = topEmotion.Key.Description,
            RecommendationType = topEmotion.Key.RecommendationType,
            Message = message,
            SecondaryEmotions = secondaryEmotions,
            DetectedKeywords = topEmotion.Value.MatchedKeywords
        };
    }

    /// <summary>
    /// Calcula score detalhado para uma emoção
    /// </summary>
    private EmotionScore CalculateEmotionScore(string normalizedText, Emotion emotion)
    {
        var score = new EmotionScore();
        var keywords = emotion.Keywords.Split(',', StringSplitOptions.RemoveEmptyEntries);

        foreach (var keyword in keywords)
        {
            var normalizedKeyword = keyword.Trim().ToLower();
            var keywordScore = 0;
            var matched = false;

            // Verificar intensidade próxima à palavra-chave
            var hasIntensity = CheckIntensityNearKeyword(normalizedText, normalizedKeyword);
            var hasNegation = CheckNegationNearKeyword(normalizedText, normalizedKeyword);

            // Palavra exata com contexto = mais pontos
            if (Regex.IsMatch(normalizedText, $@"\b{Regex.Escape(normalizedKeyword)}\b"))
            {
                keywordScore = 10;
                matched = true;
                
                // Aumentar se tiver palavra de intensidade próxima
                if (hasIntensity)
                {
                    keywordScore += 5;
                }
                
                // Diminuir se tiver negação
                if (hasNegation)
                {
                    keywordScore = Math.Max(0, keywordScore - 8);
                }
            }
            // Parte da palavra = menos pontos
            else if (normalizedText.Contains(normalizedKeyword))
            {
                keywordScore = 3;
                matched = true;
            }

            if (matched)
            {
                score.TotalScore += keywordScore;
                score.MatchedKeywords.Add(keyword.Trim());
            }
        }

        // Bônus por múltiplas palavras-chave encontradas (indica emoção forte)
        if (score.MatchedKeywords.Count > 1)
        {
            score.TotalScore += score.MatchedKeywords.Count * 2;
        }

        return score;
    }

    /// <summary>
    /// Verifica se há palavra de intensidade próxima à palavra-chave
    /// </summary>
    private bool CheckIntensityNearKeyword(string text, string keyword)
    {
        var pattern = $@"\b({string.Join("|", _intensityWords)})\s+\w*\s*{Regex.Escape(keyword)}|{Regex.Escape(keyword)}\s+\w*\s*({string.Join("|", _intensityWords)})";
        return Regex.IsMatch(text, pattern, RegexOptions.IgnoreCase);
    }

    /// <summary>
    /// Verifica se há negação próxima à palavra-chave
    /// </summary>
    private bool CheckNegationNearKeyword(string text, string keyword)
    {
        var pattern = $@"\b({string.Join("|", _negationWords)})\s+\w*\s*{Regex.Escape(keyword)}";
        return Regex.IsMatch(text, pattern, RegexOptions.IgnoreCase);
    }

    /// <summary>
    /// Calcula confiança de forma orgânica (não sempre 100%)
    /// </summary>
    private int CalculateOrganicConfidence(EmotionScore topScore, List<KeyValuePair<Emotion, EmotionScore>> allScores)
    {
        var maxPossibleScore = 50; // Score máximo teórico
        var baseConfidence = (int)((topScore.TotalScore / (double)maxPossibleScore) * 100);
        
        // Ajustar baseado na diferença com a segunda emoção
        if (allScores.Count > 1)
        {
            var secondScore = allScores[1].Value.TotalScore;
            var difference = topScore.TotalScore - secondScore;
            
            // Se a diferença é pequena, reduzir confiança (emoções misturadas)
            if (difference < 5)
            {
                baseConfidence = (int)(baseConfidence * 0.7); // Reduzir 30%
            }
        }

        // Garantir que não seja sempre 100%
        return Math.Min(baseConfidence, 95); // Máximo 95% para parecer mais humano
    }

    /// <summary>
    /// Gera mensagem mais humana e contextual
    /// </summary>
    private string GenerateHumanMessage(Emotion emotion, int confidence, List<SecondaryEmotion> secondaryEmotions, string text)
    {
        var messages = new List<string>();

        // Mensagem principal baseada na confiança
        if (confidence >= 80)
        {
            messages.Add($"Parece que você está sentindo {emotion.Name}.");
        }
        else if (confidence >= 50)
        {
            messages.Add($"Acho que você pode estar sentindo {emotion.Name}.");
        }
        else
        {
            messages.Add($"Detectei um pouco de {emotion.Name} no que você escreveu.");
        }

        // Adicionar emoções secundárias se houver
        if (secondaryEmotions.Any())
        {
            var secondary = secondaryEmotions.First();
            messages.Add($"Também percebi um pouco de {secondary.Name}.");
        }

        // Adicionar contexto baseado no texto
        if (text.Contains("hoje") || text.Contains("agora"))
        {
            messages.Add("Vejo que isso está acontecendo agora.");
        }

        return string.Join(" ", messages);
    }

    /// <summary>
    /// Normaliza texto (remove acentos, lowercase, etc)
    /// </summary>
    private string NormalizeText(string text)
    {
        var normalized = text.ToLower().Trim();
        
        // Remover acentos básicos (simplificado)
        normalized = normalized
            .Replace("á", "a").Replace("à", "a").Replace("â", "a").Replace("ã", "a")
            .Replace("é", "e").Replace("ê", "e")
            .Replace("í", "i")
            .Replace("ó", "o").Replace("ô", "o").Replace("õ", "o")
            .Replace("ú", "u").Replace("ü", "u")
            .Replace("ç", "c");

        return normalized;
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
}

/// <summary>
/// Resultado da análise de emoção (melhorado)
/// </summary>
public class EmotionAnalysisResult
{
    public string DetectedEmotion { get; set; } = string.Empty;
    public int EmotionId { get; set; }
    public int Confidence { get; set; } // 0-100% (mais orgânico, não sempre 100%)
    public string Description { get; set; } = string.Empty;
    public string RecommendationType { get; set; } = string.Empty;
    public string Message { get; set; } = string.Empty;
    
    // NOVO: Emoções secundárias (mistura de emoções)
    public List<SecondaryEmotion> SecondaryEmotions { get; set; } = new();
    
    // NOVO: Palavras-chave detectadas
    public List<string> DetectedKeywords { get; set; } = new();
}

/// <summary>
/// Emoção secundária detectada
/// </summary>
public class SecondaryEmotion
{
    public string Name { get; set; } = string.Empty;
    public int Confidence { get; set; }
    public int Score { get; set; }
}

/// <summary>
/// Score detalhado de uma emoção
/// </summary>
internal class EmotionScore
{
    public int TotalScore { get; set; }
    public List<string> MatchedKeywords { get; set; } = new();
}
