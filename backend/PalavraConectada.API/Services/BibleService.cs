// Serviço de Bíblia - Busca versículos otimizada do banco de dados
// Sistema otimizado com cache em memória para máxima performance
using PalavraConectada.API.Models;
using PalavraConectada.API.Data;
using Microsoft.EntityFrameworkCore;

namespace PalavraConectada.API.Services;

/// <summary>
/// Serviço otimizado para busca de versículos bíblicos
/// Usa banco de dados SQLite com cache em memória para máxima performance
/// </summary>
public class BibleService
{
    private readonly BibleDbContext _context;
    private readonly ILogger<BibleService> _logger;
    
    // Cache em memória para buscas frequentes
    private static readonly Dictionary<string, List<Verse>> _searchCache = new();
    private static readonly Dictionary<string, Verse> _randomVerseCache = new();
    private static readonly Dictionary<string, int> _verseCountCache = new();
    private static DateTime _lastCacheUpdate = DateTime.MinValue;
    private static readonly TimeSpan CACHE_EXPIRY = TimeSpan.FromMinutes(30); // Cache expira em 30 minutos

    public BibleService(
        BibleDbContext context,
        ILogger<BibleService> logger)
    {
        _context = context;
        _logger = logger;
    }

    /// <summary>
    /// Busca versículos por palavra-chave - OTIMIZADO
    /// Busca direto no banco de dados (já temos tudo migrado)
    /// </summary>
    public async Task<List<Verse>> SearchVersesAsync(string keyword, string version = "nvi", int limit = 10)
    {
        var cacheKey = $"{version}:{keyword.ToLower()}:{limit}";
        
        // Verificar cache
        if (_searchCache.TryGetValue(cacheKey, out var cachedVerses))
        {
            _logger.LogInformation("⚡ Cache hit para: {Keyword}", keyword);
            return cachedVerses;
        }

        _logger.LogInformation("🔍 Buscando '{Keyword}' no banco de dados (versão: {Version})", keyword, version);
        
        // Buscar no banco
        var verses = await _context.Verses
            .Where(v => v.Text.Contains(keyword) && v.Version == version)
            .OrderBy(v => v.BookName)
            .ThenBy(v => v.Chapter)
            .ThenBy(v => v.Number)
            .Take(limit)
            .ToListAsync();

        if (verses.Any())
        {
            _logger.LogInformation("✅ Encontrados {Count} versículos", verses.Count);
            
            // Armazenar no cache (limitado a 1000 entradas para não consumir muita memória)
            if (_searchCache.Count < 1000)
            {
                _searchCache[cacheKey] = verses;
            }
            
            return verses;
        }

        _logger.LogWarning("⚠️ Nenhum versículo encontrado para: {Keyword}", keyword);
        return new List<Verse>();
    }

    /// <summary>
    /// Busca versículos por emoção - OTIMIZADO
    /// </summary>
    public async Task<List<Verse>> SearchVersesByEmotionAsync(string emotionName, string version = "nvi", int limit = 10)
    {
        // Mapear emoção para palavra-chave
        var keyword = MapEmotionToKeyword(emotionName);
        return await SearchVersesAsync(keyword, version, limit);
    }

    /// <summary>
    /// Busca versículo aleatório - OTIMIZADO com cache de contagem
    /// </summary>
    public async Task<Verse?> GetRandomVerseAsync(string version = "nvi")
    {
        var cacheKey = $"random:{version}";
        
        // Verificar cache de versículo aleatório (atualiza a cada 5 minutos)
        if (_randomVerseCache.TryGetValue(cacheKey, out var cachedVerse) && 
            DateTime.UtcNow - _lastCacheUpdate < TimeSpan.FromMinutes(5))
        {
            _logger.LogInformation("⚡ Cache hit para versículo aleatório");
            return cachedVerse;
        }

        // Obter contagem total (com cache)
        var count = await GetVerseCountAsync(version);
        
        if (count == 0)
        {
            _logger.LogWarning("⚠️ Nenhum versículo encontrado no banco para versão: {Version}", version);
            return null;
        }

        // Gerar número aleatório
        var random = new Random();
        var skip = random.Next(count);
        
        var verse = await _context.Verses
            .Where(v => v.Version == version)
            .Skip(skip)
            .FirstOrDefaultAsync();

        if (verse != null)
        {
            // Atualizar cache
            _randomVerseCache[cacheKey] = verse;
            _lastCacheUpdate = DateTime.UtcNow;
        }

        return verse;
    }

    /// <summary>
    /// Obtém contagem de versículos (com cache)
    /// </summary>
    private async Task<int> GetVerseCountAsync(string version)
    {
        var cacheKey = $"count:{version}";
        
        if (_verseCountCache.TryGetValue(cacheKey, out var cachedCount))
        {
            return cachedCount;
        }

        var count = await _context.Verses
            .Where(v => v.Version == version)
            .CountAsync();

        _verseCountCache[cacheKey] = count;
        return count;
    }

    /// <summary>
    /// Limpa o cache (útil após migrações)
    /// </summary>
    public static void ClearCache()
    {
        _searchCache.Clear();
        _randomVerseCache.Clear();
        _verseCountCache.Clear();
        _lastCacheUpdate = DateTime.MinValue;
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
}
