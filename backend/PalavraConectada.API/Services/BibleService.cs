// Serviço de Bíblia - Busca versículos em múltiplas fontes
// Como um escriba que conhece múltiplos manuscritos
using PalavraConectada.API.Models;
using PalavraConectada.API.Data;
using Microsoft.EntityFrameworkCore;
using System.Net.Http.Json;

namespace PalavraConectada.API.Services;

public class BibleService
{
    private readonly BibleDbContext _context;
    private readonly HttpClient _httpClient;
    private readonly ILogger<BibleService> _logger;

    public BibleService(
        BibleDbContext context,
        HttpClient httpClient,
        ILogger<BibleService> logger)
    {
        _context = context;
        _httpClient = httpClient;
        _logger = logger;
    }

    /// <summary>
    /// Busca versículos - Primeiro no banco, depois nas APIs
    /// Sistema de FALLBACK inteligente
    /// </summary>
    public async Task<List<Verse>> SearchVersesAsync(string keyword, string version = "nvi")
    {
        // 1. Tentar buscar no banco local primeiro (CACHE)
        _logger.LogInformation("🗄️ Buscando no banco local...");
        var localVerses = await _context.Verses
            .Where(v => v.Text.Contains(keyword) && v.Version == version)
            .Take(10)
            .ToListAsync();

        if (localVerses.Any())
        {
            _logger.LogInformation("✅ Encontrados {Count} versículos no cache local", localVerses.Count);
            return localVerses;
        }

        // 2. Buscar em APIs externas
        _logger.LogInformation("📡 Buscando em APIs externas...");
        
        // Tentar API brasileira primeiro
        var verses = await TryBrasilianApiAsync(keyword, version);
        
        // Se falhar, tentar API inglesa
        if (!verses.Any())
        {
            verses = await TryEnglishApiAsync(keyword);
        }

        // Se falhar, usar dados MOCK
        if (!verses.Any())
        {
            verses = GetMockVerses(keyword);
        }

        // Salvar no banco para próximas buscas
        if (verses.Any())
        {
            await SaveVersesToDatabaseAsync(verses);
        }

        return verses;
    }

    /// <summary>
    /// Busca versículos por emoção
    /// </summary>
    public async Task<List<Verse>> SearchVersesByEmotionAsync(string emotionName, string version = "nvi")
    {
        // Mapear emoção para palavra-chave
        var keyword = MapEmotionToKeyword(emotionName);
        return await SearchVersesAsync(keyword, version);
    }

    /// <summary>
    /// Busca versículo aleatório
    /// </summary>
    public async Task<Verse?> GetRandomVerseAsync(string version = "nvi")
    {
        // Primeiro tentar no banco
        var count = await _context.Verses.Where(v => v.Version == version).CountAsync();
        
        if (count > 0)
        {
            var random = new Random();
            var skip = random.Next(count);
            
            return await _context.Verses
                .Where(v => v.Version == version)
                .Skip(skip)
                .FirstOrDefaultAsync();
        }

        // Senão, usar MOCK
        return GetMockVerses("paz").FirstOrDefault();
    }

    // ═══════════════════════════════════════════════════════════════════════════
    // MÉTODOS PRIVADOS - Sistema de Fallback
    // ═══════════════════════════════════════════════════════════════════════════

    /// <summary>
    /// Tenta buscar na API brasileira (abibliadigital.com.br)
    /// </summary>
    private async Task<List<Verse>> TryBrasilianApiAsync(string keyword, string version)
    {
        try
        {
            _logger.LogInformation("🇧🇷 Tentando API brasileira...");
            
            var url = "https://www.abibliadigital.com.br/api/verses/search";
            var body = new { version, search = keyword };
            
            var response = await _httpClient.PostAsJsonAsync(url, body);
            
            if (response.IsSuccessStatusCode)
            {
                var result = await response.Content.ReadFromJsonAsync<BrasilianApiResponse>();
                
                if (result?.Verses != null && result.Verses.Any())
                {
                    _logger.LogInformation("✅ API brasileira retornou {Count} versículos", 
                        result.Verses.Count);
                    return ConvertFromBrasilianApi(result.Verses);
                }
            }
        }
        catch (Exception ex)
        {
            _logger.LogWarning(ex, "⚠️ API brasileira falhou");
        }

        return new List<Verse>();
    }

    /// <summary>
    /// Tenta buscar na API inglesa (bible-api.com) com tradução
    /// </summary>
    private async Task<List<Verse>> TryEnglishApiAsync(string keyword)
    {
        try
        {
            _logger.LogInformation("🇺🇸 Tentando API inglesa...");
            
            // Traduzir palavra-chave PT → EN
            var englishKeyword = TranslateToEnglish(keyword);
            
            // Bible API usa formato diferente (ex: John+3:16)
            // Por simplicidade, vamos retornar vazio por enquanto
            // TODO: Implementar busca na Bible API
            
            _logger.LogInformation("⚠️ API inglesa ainda não implementada");
        }
        catch (Exception ex)
        {
            _logger.LogWarning(ex, "⚠️ API inglesa falhou");
        }

        return new List<Verse>();
    }

    /// <summary>
    /// Dados MOCK quando todas as APIs falham
    /// </summary>
    private List<Verse> GetMockVerses(string keyword)
    {
        _logger.LogInformation("🎭 Usando dados MOCK");

        var mockData = new Dictionary<string, List<Verse>>
        {
            ["amor"] = new()
            {
                new Verse
                {
                    Id = 1,
                    BookName = "João",
                    BookAbbrev = "jo",
                    Chapter = 3,
                    Number = 16,
                    Text = "Porque Deus tanto amou o mundo que deu o seu Filho Unigênito, para que todo o que nele crer não pereça, mas tenha a vida eterna.",
                    Version = "nvi",
                    Author = "João",
                    Group = "Evangelhos",
                    Testament = "NT"
                }
            },
            ["paz"] = new()
            {
                new Verse
                {
                    Id = 2,
                    BookName = "João",
                    BookAbbrev = "jo",
                    Chapter = 14,
                    Number = 27,
                    Text = "Deixo-lhes a paz; a minha paz lhes dou. Não a dou como o mundo a dá. Não se perturbe o coração de vocês, nem tenham medo.",
                    Version = "nvi",
                    Author = "João",
                    Group = "Evangelhos",
                    Testament = "NT"
                }
            }
        };

        var normalizedKeyword = keyword.ToLower();
        
        return mockData.ContainsKey(normalizedKeyword) 
            ? mockData[normalizedKeyword] 
            : new List<Verse>();
    }

    /// <summary>
    /// Salva versículos no banco para cache
    /// </summary>
    private async Task SaveVersesToDatabaseAsync(List<Verse> verses)
    {
        foreach (var verse in verses)
        {
            // Verificar se já existe
            var exists = await _context.Verses.AnyAsync(v =>
                v.BookAbbrev == verse.BookAbbrev &&
                v.Chapter == verse.Chapter &&
                v.Number == verse.Number &&
                v.Version == verse.Version);

            if (!exists)
            {
                _context.Verses.Add(verse);
            }
        }

        await _context.SaveChangesAsync();
        _logger.LogInformation("💾 Salvos {Count} versículos no cache", verses.Count);
    }

    /// <summary>
    /// Mapeia emoção para palavra-chave de busca
    /// </summary>
    private string MapEmotionToKeyword(string emotionName)
    {
        return emotionName.ToLower() switch
        {
            "tristeza" => "consolo",
            "alegria" => "alegria",
            "medo" => "coragem",
            "ansiedade" => "paz",
            "solidão" => "presença",
            "raiva" => "perdão",
            "gratidão" => "graças",
            "esperança" => "esperança",
            _ => emotionName
        };
    }

    /// <summary>
    /// Traduz palavra PT para EN (dicionário simples)
    /// </summary>
    private string TranslateToEnglish(string keyword)
    {
        var dictionary = new Dictionary<string, string>
        {
            ["amor"] = "love",
            ["fé"] = "faith",
            ["paz"] = "peace",
            ["esperança"] = "hope",
            ["alegria"] = "joy",
            ["medo"] = "fear",
            ["tristeza"] = "sorrow"
        };

        return dictionary.ContainsKey(keyword.ToLower()) 
            ? dictionary[keyword.ToLower()] 
            : keyword;
    }

    /// <summary>
    /// Converte resposta da API brasileira para nosso modelo
    /// </summary>
    private List<Verse> ConvertFromBrasilianApi(List<BrasilianApiVerse> apiVerses)
    {
        return apiVerses.Select(av => new Verse
        {
            BookName = av.Book?.Name ?? "Desconhecido",
            BookAbbrev = av.Book?.Abbrev?.Pt ?? "unknown",
            Author = av.Book?.Author ?? "Desconhecido",
            Group = av.Book?.Group ?? "Desconhecido",
            Testament = "NT", // Determinar depois
            Chapter = av.Chapter,
            Number = av.Number,
            Text = av.Text,
            Version = av.Book?.Version ?? "nvi"
        }).ToList();
    }
}


