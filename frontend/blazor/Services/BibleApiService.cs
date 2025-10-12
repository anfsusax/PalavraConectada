using System.Net.Http.Json;
using PalavraConectada.Models;

// Serviço que busca versículos - como o profeta que busca a palavra de Deus
namespace PalavraConectada.Services
{
    public class BibleApiService
    {
        private readonly HttpClient _httpClient;
        private readonly BibleApiMockService _mockService;
        private const string API_BASE_URL = "https://www.abibliadigital.com.br/api";
        
        // ⚠️ MODO: false = usa NOSSO BACKEND (localhost:7001)
        private const bool USE_MOCK = false; // AGORA temos nosso próprio backend! 🔥

        public BibleApiService(HttpClient httpClient, BibleApiMockService mockService)
        {
            _httpClient = httpClient;
            _mockService = mockService;
            
            if (USE_MOCK)
            {
                Console.WriteLine("🎭 MODO MOCK ATIVADO - Usando dados de exemplo");
                Console.WriteLine("Para usar a API real, altere USE_MOCK = false em BibleApiService.cs");
            }
        }

        // Busca versículos por palavra-chave (POST conforme documentação)
        public async Task<SearchResult?> SearchVersesAsync(string searchTerm, string version = "nvi")
        {
            // Se modo mock, usa dados de exemplo
            if (USE_MOCK)
            {
                return await _mockService.SearchVersesAsync(searchTerm, version);
            }
            
            // Senão, usa API real
            try
            {
                var url = $"{API_BASE_URL}/verses/search";
                var body = new { version = version, search = searchTerm };
                
                var response = await _httpClient.PostAsJsonAsync(url, body);
                response.EnsureSuccessStatusCode();
                
                var result = await response.Content.ReadFromJsonAsync<SearchResult>();
                return result ?? new SearchResult();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"❌ Erro ao buscar versículos: {ex.Message}");
                Console.WriteLine("Dica: A API pode estar offline. Ative o modo mock!");
                return new SearchResult();
            }
        }

        // Busca um versículo específico por referência
        public async Task<Verse?> GetVerseAsync(string version, string bookAbbrev, int chapter, int verse)
        {
            try
            {
                var url = $"{API_BASE_URL}/verses/{version}/{bookAbbrev}/{chapter}/{verse}";
                return await _httpClient.GetFromJsonAsync<Verse>(url);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erro ao buscar versículo: {ex.Message}");
                return null;
            }
        }

        // Busca versículo aleatório - como "abrir a Bíblia e deixar Deus falar"
        public async Task<Verse?> GetRandomVerseAsync(string version = "nvi")
        {
            if (USE_MOCK)
            {
                return await _mockService.GetRandomVerseAsync(version);
            }
            
            try
            {
                var url = $"{API_BASE_URL}/verses/{version}/random";
                return await _httpClient.GetFromJsonAsync<Verse>(url);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"❌ Erro ao buscar versículo aleatório: {ex.Message}");
                return await _mockService.GetRandomVerseAsync(version);
            }
        }

        // Lista todas as versões disponíveis
        public async Task<List<BibleVersion>?> GetVersionsAsync()
        {
            try
            {
                var url = $"{API_BASE_URL}/versions";
                return await _httpClient.GetFromJsonAsync<List<BibleVersion>>(url);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erro ao buscar versões: {ex.Message}");
                return new List<BibleVersion>();
            }
        }
    }
}

