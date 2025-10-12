using System.Net.Http.Json;
using PalavraConectada.Models;

// Serviço que busca versículos - como o profeta que busca a palavra de Deus
namespace PalavraConectada.Services
{
    public class BibleApiService
    {
        private readonly HttpClient _httpClient;
        private const string API_BASE_URL = "https://www.abibliadigital.com.br/api";

        public BibleApiService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        // Busca versículos por palavra-chave
        public async Task<SearchResult?> SearchVersesAsync(string searchTerm, string version = "nvi")
        {
            try
            {
                var url = $"{API_BASE_URL}/verses/{version}/search/{Uri.EscapeDataString(searchTerm)}";
                var result = await _httpClient.GetFromJsonAsync<SearchResult>(url);
                return result ?? new SearchResult();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erro ao buscar versículos: {ex.Message}");
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
            try
            {
                var url = $"{API_BASE_URL}/verses/{version}/random";
                return await _httpClient.GetFromJsonAsync<Verse>(url);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erro ao buscar versículo aleatório: {ex.Message}");
                return null;
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

