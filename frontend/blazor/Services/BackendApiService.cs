// Serviço para consumir NOSSA API backend (localhost:7001)
// Agora temos controle total e inteligência de emoções!
using System.Net.Http.Json;
using PalavraConectada.Blazor.Services;

namespace PalavraConectada.Blazor.Services;

public class BackendApiService
{
    private readonly HttpClient _httpClient;
    private readonly ILogger<BackendApiService> _logger;
    private const string API_BASE_URL = "http://localhost:7000/api";

    public BackendApiService(HttpClient httpClient, ILogger<BackendApiService> logger)
    {
        _httpClient = httpClient;
        _logger = logger;
        
        _logger.LogInformation($"🔥 BackendApiService inicializado - Usando API própria em {API_BASE_URL}");
    }

    // ═══════════════════════════════════════════════════════════════════════════
    // ANÁLISE DE EMOÇÕES
    // ═══════════════════════════════════════════════════════════════════════════

    /// <summary>
    /// Analisa o texto e detecta a emoção
    /// Ex: "Estou triste" → { emotion: "tristeza", confidence: 100 }
    /// </summary>
    public async Task<EmotionAnalysisResponse?> AnalyzeEmotionAsync(string text)
    {
        try
        {
            var url = $"{API_BASE_URL}/Emotion/analyze";
            var response = await _httpClient.PostAsJsonAsync(url, new { text });
            response.EnsureSuccessStatusCode();
            
            return await response.Content.ReadFromJsonAsync<EmotionAnalysisResponse>();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "❌ Erro ao analisar emoção");
            return null;
        }
    }

    /// <summary>
    /// Lista todas as emoções disponíveis
    /// </summary>
    public async Task<List<EmotionDto>?> GetEmotionsAsync()
    {
        try
        {
            var url = $"{API_BASE_URL}/Emotion/list";
            return await _httpClient.GetFromJsonAsync<List<EmotionDto>>(url);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "❌ Erro ao buscar emoções");
            return new List<EmotionDto>();
        }
    }

    // ═══════════════════════════════════════════════════════════════════════════
    // VERSÍCULOS
    // ═══════════════════════════════════════════════════════════════════════════

    /// <summary>
    /// Busca versículos por palavra-chave
    /// </summary>
    public async Task<SearchVerseResponse?> SearchVersesAsync(string keyword, string version = "nvi")
    {
        try
        {
            var url = $"{API_BASE_URL}/Verses/search?keyword={Uri.EscapeDataString(keyword)}&version={version}";
            return await _httpClient.GetFromJsonAsync<SearchVerseResponse>(url);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "❌ Erro ao buscar versículos");
            return null;
        }
    }

    /// <summary>
    /// Busca versículos por emoção
    /// </summary>
    public async Task<List<VerseDto>?> GetVersesByEmotionAsync(
        string emotionName, 
        string version = "nvi", 
        int limit = 10)
    {
        try
        {
            var url = $"{API_BASE_URL}/Verses/by-emotion/{emotionName}?version={version}&limit={limit}";
            return await _httpClient.GetFromJsonAsync<List<VerseDto>>(url);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "❌ Erro ao buscar versículos por emoção");
            return null;
        }
    }

    /// <summary>
    /// Versículo aleatório
    /// </summary>
    public async Task<VerseDto?> GetRandomVerseAsync(string version = "nvi")
    {
        try
        {
            var url = $"{API_BASE_URL}/Verses/random?version={version}";
            return await _httpClient.GetFromJsonAsync<VerseDto>(url);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "❌ Erro ao buscar versículo aleatório");
            return null;
        }
    }

    /// <summary>
    /// RECOMENDAÇÃO INTELIGENTE - A ESTRELA DO SHOW! ⭐
    /// Combina análise de emoção + busca de versículos
    /// </summary>
    public async Task<RecommendationResponse?> GetIntelligentRecommendationAsync(
        string text, 
        string version = "nvi")
    {
        try
        {
            var url = $"{API_BASE_URL}/Verses/recommend";
            var response = await _httpClient.PostAsJsonAsync(url, new { text, version });
            response.EnsureSuccessStatusCode();
            
            return await response.Content.ReadFromJsonAsync<RecommendationResponse>();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "❌ Erro ao gerar recomendação");
            return null;
        }
    }

    /// <summary>
    /// Busca histórico de interações
    /// </summary>
    public async Task<List<object>?> GetHistoryAsync(int limit = 10)
    {
        try
        {
            var url = $"{API_BASE_URL}/Verses/history?limit={limit}";
            return await _httpClient.GetFromJsonAsync<List<object>>(url);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "❌ Erro ao buscar histórico");
            return null;
        }
    }

    // ═══════════════════════════════════════════════════════════════════════════
    // BIBLIOTECA BÍBLICA
    // ═══════════════════════════════════════════════════════════════════════════

    /// <summary>
    /// Lista livros do Velho Testamento
    /// </summary>
    public async Task<BibleLibraryResponse?> GetOldTestamentAsync()
    {
        try
        {
            var url = $"{API_BASE_URL}/BibleLibrary/old-testament";
            return await _httpClient.GetFromJsonAsync<BibleLibraryResponse>(url);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "❌ Erro ao buscar Velho Testamento");
            return null;
        }
    }

    /// <summary>
    /// Lista livros do Novo Testamento
    /// </summary>
    public async Task<BibleLibraryResponse?> GetNewTestamentAsync()
    {
        try
        {
            var url = $"{API_BASE_URL}/BibleLibrary/new-testament";
            return await _httpClient.GetFromJsonAsync<BibleLibraryResponse>(url);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "❌ Erro ao buscar Novo Testamento");
            return null;
        }
    }

    /// <summary>
    /// Versículos sobre prosperidade
    /// </summary>
    public async Task<ThemeResponse?> GetProsperityVersesAsync()
    {
        try
        {
            var url = $"{API_BASE_URL}/BibleLibrary/theme/prosperity";
            return await _httpClient.GetFromJsonAsync<ThemeResponse>(url);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "❌ Erro ao buscar versículos de prosperidade");
            return null;
        }
    }

    /// <summary>
    /// Plano de salvação
    /// </summary>
    public async Task<SalvationResponse?> GetSalvationVersesAsync()
    {
        try
        {
            var url = $"{API_BASE_URL}/BibleLibrary/theme/salvation";
            return await _httpClient.GetFromJsonAsync<SalvationResponse>(url);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "❌ Erro ao buscar plano de salvação");
            return null;
        }
    }
}

// ═══════════════════════════════════════════════════════════════════════════
// DTOs DA BIBLIOTECA BÍBLICA
// ═══════════════════════════════════════════════════════════════════════════

public class BibleLibraryResponse
{
    public string Testament { get; set; } = string.Empty;
    public int TotalBooks { get; set; }
    public List<BookDto> Books { get; set; } = new();
}

public class BookDto
{
    public string BookName { get; set; } = string.Empty;
    public string BookAbbrev { get; set; } = string.Empty;
    public string Author { get; set; } = string.Empty;
    public string Group { get; set; } = string.Empty;
}

public class ThemeResponse
{
    public string Theme { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public int Count { get; set; }
    public List<VerseDto> Verses { get; set; } = new();
}

public class SalvationResponse
{
    public string Theme { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public List<string> Steps { get; set; } = new();
    public int Count { get; set; }
    public List<VerseDto> Verses { get; set; } = new();
}


// ═══════════════════════════════════════════════════════════════════════════
// DTOs - Tipagem forte com C#
// ═══════════════════════════════════════════════════════════════════════════

public class EmotionAnalysisResponse
{
    public string DetectedEmotion { get; set; } = string.Empty;
    public int Confidence { get; set; }
    public string Message { get; set; } = string.Empty;
    public string RecommendationType { get; set; } = string.Empty;
    public List<string> Suggestions { get; set; } = new();
    public int InteractionId { get; set; }
}

public class EmotionDto
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Keywords { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string RecommendationType { get; set; } = string.Empty;
}

public class VerseDto
{
    public int Id { get; set; }
    public string BookName { get; set; } = string.Empty;
    public string BookAbbrev { get; set; } = string.Empty;
    public string Author { get; set; } = string.Empty;
    public string Group { get; set; } = string.Empty;
    public string Testament { get; set; } = string.Empty;
    public int Chapter { get; set; }
    public int Number { get; set; }
    public string Text { get; set; } = string.Empty;
    public string Version { get; set; } = string.Empty;
}

public class SearchVerseResponse
{
    public string Keyword { get; set; } = string.Empty;
    public string Version { get; set; } = string.Empty;
    public int Count { get; set; }
    public List<VerseDto> Verses { get; set; } = new();
}

public class RecommendationResponse
{
    public string UserInput { get; set; } = string.Empty;
    public string DetectedEmotion { get; set; } = string.Empty;
    public int Confidence { get; set; }
    public string Message { get; set; } = string.Empty;
    public VerseDto? RecommendedVerse { get; set; }
    public List<VerseDto> AlternativeVerses { get; set; } = new();
    public List<string> Suggestions { get; set; } = new();
}
