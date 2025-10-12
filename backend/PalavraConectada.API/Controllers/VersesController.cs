// Controller de Versículos - Busca e recomendação de versículos
// Como um escriba que conhece toda a Escritura
using Microsoft.AspNetCore.Mvc;
using PalavraConectada.API.Services;
using PalavraConectada.API.Data;
using Microsoft.EntityFrameworkCore;
using PalavraConectada.API.Models;

namespace PalavraConectada.API.Controllers;

/// <summary>
/// Controller para busca e recomendação de versículos bíblicos
/// </summary>
[ApiController]
[Route("api/[controller]")]
public class VersesController : ControllerBase
{
    private readonly BibleDbContext _context;
    private readonly BibleService _bibleService;
    private readonly ILogger<VersesController> _logger;

    public VersesController(
        BibleDbContext context,
        BibleService bibleService,
        ILogger<VersesController> logger)
    {
        _context = context;
        _bibleService = bibleService;
        _logger = logger;
    }

    /// <summary>
    /// Busca versículos por palavra-chave
    /// </summary>
    /// <param name="keyword">Palavra a buscar (ex: amor, fé)</param>
    /// <param name="version">Versão da Bíblia (nvi, acf, aa)</param>
    /// <returns>Lista de versículos encontrados</returns>
    [HttpGet("search")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<ActionResult<SearchVerseResponse>> SearchVerses(
        [FromQuery] string keyword,
        [FromQuery] string version = "nvi")
    {
        _logger.LogInformation("🔍 Buscando versículos: {Keyword} (versão: {Version})", 
            keyword, version);

        if (string.IsNullOrWhiteSpace(keyword))
        {
            return BadRequest(new { error = "Palavra-chave é obrigatória" });
        }

        try
        {
            var verses = await _bibleService.SearchVersesAsync(keyword, version);
            
            return Ok(new SearchVerseResponse
            {
                Keyword = keyword,
                Version = version,
                Count = verses.Count,
                Verses = verses
            });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "❌ Erro ao buscar versículos");
            return StatusCode(500, new { error = "Erro ao buscar versículos" });
        }
    }

    /// <summary>
    /// Busca versículos por emoção
    /// </summary>
    /// <param name="emotionName">Nome da emoção (ex: tristeza, alegria)</param>
    /// <param name="version">Versão da Bíblia</param>
    /// <param name="limit">Quantidade máxima de versículos</param>
    /// <returns>Versículos relacionados à emoção</returns>
    [HttpGet("by-emotion/{emotionName}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<List<Verse>>> GetVersesByEmotion(
        string emotionName,
        [FromQuery] string version = "nvi",
        [FromQuery] int limit = 10)
    {
        _logger.LogInformation("😊 Buscando versículos para emoção: {Emotion}", emotionName);

        var emotion = await _context.Emotions
            .FirstOrDefaultAsync(e => e.Name.ToLower() == emotionName.ToLower());

        if (emotion == null)
        {
            return NotFound(new { error = $"Emoção '{emotionName}' não encontrada" });
        }

        var verses = await _context.VerseEmotions
            .Where(ve => ve.EmotionId == emotion.Id)
            .Include(ve => ve.Verse)
            .OrderByDescending(ve => ve.Relevance)
            .Take(limit)
            .Select(ve => ve.Verse)
            .ToListAsync();

        // Se não tiver no banco, buscar nas APIs externas
        if (!verses.Any())
        {
            _logger.LogInformation("📡 Buscando nas APIs externas...");
            verses = await _bibleService.SearchVersesByEmotionAsync(emotionName, version);
        }

        return Ok(verses);
    }

    /// <summary>
    /// Busca um versículo aleatório
    /// </summary>
    /// <param name="version">Versão da Bíblia</param>
    /// <returns>Versículo aleatório</returns>
    [HttpGet("random")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<ActionResult<Verse>> GetRandomVerse([FromQuery] string version = "nvi")
    {
        _logger.LogInformation("🎲 Buscando versículo aleatório");

        try
        {
            var verse = await _bibleService.GetRandomVerseAsync(version);
            
            if (verse == null)
            {
                return NotFound(new { error = "Nenhum versículo encontrado" });
            }

            return Ok(verse);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "❌ Erro ao buscar versículo aleatório");
            return StatusCode(500, new { error = "Erro ao buscar versículo aleatório" });
        }
    }

    /// <summary>
    /// Recomendação inteligente baseada em texto livre
    /// Combina análise de emoção + busca de versículos
    /// </summary>
    /// <param name="request">Texto do usuário</param>
    /// <returns>Versículo recomendado para o sentimento</returns>
    [HttpPost("recommend")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<ActionResult<RecommendationResponse>> GetRecommendation(
        [FromBody] RecommendationRequest request)
    {
        _logger.LogInformation("💡 Gerando recomendação inteligente");

        if (string.IsNullOrWhiteSpace(request.Text))
        {
            return BadRequest(new { error = "Texto é obrigatório" });
        }

        try
        {
            // 1. Analisar emoção
            var emotionAnalyzer = HttpContext.RequestServices
                .GetRequiredService<EmotionAnalyzerService>();
            
            var analysis = await emotionAnalyzer.AnalyzeEmotionAsync(request.Text);

            // 2. Buscar versículos para esta emoção
            var verses = await _context.VerseEmotions
                .Where(ve => ve.Emotion.Name == analysis.DetectedEmotion)
                .Include(ve => ve.Verse)
                .OrderByDescending(ve => ve.Relevance)
                .Take(5)
                .Select(ve => ve.Verse)
                .ToListAsync();

            // 3. Se não tiver no banco, buscar externamente
            if (!verses.Any())
            {
                verses = await _bibleService.SearchVersesByEmotionAsync(
                    analysis.DetectedEmotion, 
                    request.Version);
            }

            // 4. Pegar um versículo aleatório da lista
            var recommendedVerse = verses.Any() 
                ? verses[new Random().Next(verses.Count)]
                : null;

            // 5. Buscar sugestões
            var suggestions = await emotionAnalyzer.GetSuggestionsAsync(analysis.DetectedEmotion);

            // 6. Atualizar interação com recomendação
            var interaction = await _context.UserInteractions
                .OrderByDescending(i => i.CreatedAt)
                .FirstOrDefaultAsync(i => i.DetectedEmotion == analysis.DetectedEmotion);

            if (interaction != null && recommendedVerse != null)
            {
                interaction.RecommendedVerseReference = 
                    $"{recommendedVerse.BookName} {recommendedVerse.Chapter}:{recommendedVerse.Number}";
                await _context.SaveChangesAsync();
            }

            return Ok(new RecommendationResponse
            {
                UserInput = request.Text,
                DetectedEmotion = analysis.DetectedEmotion,
                Confidence = analysis.Confidence,
                RecommendedVerse = recommendedVerse,
                AlternativeVerses = verses.Take(3).ToList(),
                Suggestions = suggestions,
                Message = analysis.Message
            });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "❌ Erro ao gerar recomendação");
            return StatusCode(500, new { error = "Erro ao gerar recomendação" });
        }
    }

    /// <summary>
    /// Obtém histórico de interações (para análise)
    /// </summary>
    /// <param name="limit">Quantidade de registros</param>
    /// <returns>Histórico de interações</returns>
    [HttpGet("history")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<ActionResult<List<UserInteraction>>> GetHistory([FromQuery] int limit = 10)
    {
        var history = await _context.UserInteractions
            .OrderByDescending(i => i.CreatedAt)
            .Take(limit)
            .ToListAsync();

        return Ok(history);
    }

    /// <summary>
    /// 🧪 TESTE: Verifica conexão com API externa brasileira
    /// </summary>
    [HttpGet("test-external-api")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<ActionResult<object>> TestExternalApi([FromQuery] string keyword = "amor")
    {
        _logger.LogInformation("🧪 TESTE: Tentando buscar '{Keyword}' na API brasileira", keyword);

        try
        {
            var verses = await _bibleService.SearchVersesAsync(keyword, "nvi");
            
            return Ok(new
            {
                success = true,
                keyword = keyword,
                versesFound = verses.Count,
                verses = verses.Take(3).ToList(), // Mostrar apenas 3 para testar
                message = verses.Any() 
                    ? "✅ API funcionando! Versículos encontrados e salvos no cache." 
                    : "⚠️ Nenhum versículo encontrado. Usando dados MOCK."
            });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "❌ Erro ao testar API externa");
            return Ok(new
            {
                success = false,
                error = ex.Message,
                message = "❌ Erro ao conectar com API externa. Usando dados MOCK."
            });
        }
    }

    /// <summary>
    /// 🌱 SEED: Popula o banco com versículos importantes
    /// Apenas para desenvolvimento - deve rodar uma vez
    /// </summary>
    [HttpPost("seed-database")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<ActionResult<object>> SeedDatabase()
    {
        _logger.LogInformation("🌱 Iniciando seed do banco de dados...");

        try
        {
            // Verificar se já existe dados
            var existingVerses = await _context.Verses.CountAsync();
            
            if (existingVerses >= 30)
            {
                return Ok(new
                {
                    success = false,
                    message = $"⚠️ Banco já contém {existingVerses} versículos. Seed não necessário.",
                    existingVerses
                });
            }

            // Buscar dados do seed
            var seedVerses = Data.SeedData.GetSeedVerses();
            var seedEmotions = Data.SeedData.GetVerseEmotions();

            // Adicionar versículos
            foreach (var verse in seedVerses)
            {
                // Verificar se já existe (evitar duplicatas)
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
            _logger.LogInformation("✅ Versículos salvos no banco");

            // Adicionar relacionamentos versículo-emoção
            foreach (var ve in seedEmotions)
            {
                var exists = await _context.VerseEmotions.AnyAsync(x =>
                    x.VerseId == ve.VerseId && x.EmotionId == ve.EmotionId);

                if (!exists)
                {
                    _context.VerseEmotions.Add(ve);
                }
            }

            await _context.SaveChangesAsync();
            _logger.LogInformation("✅ Relacionamentos versículo-emoção salvos");

            // Contar totais
            var totalVerses = await _context.Verses.CountAsync();
            var totalRelations = await _context.VerseEmotions.CountAsync();

            return Ok(new
            {
                success = true,
                message = "✅ Banco populado com sucesso!",
                versesAdded = seedVerses.Count,
                relationsAdded = seedEmotions.Count,
                totalVerses,
                totalRelations,
                nextStep = "Agora teste: GET /api/Verses/by-emotion/tristeza"
            });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "❌ Erro ao popular banco de dados");
            return StatusCode(500, new
            {
                success = false,
                error = ex.Message,
                stackTrace = ex.StackTrace
            });
        }
    }

    /// <summary>
    /// 🔍 BUSCA INTELIGENTE COMPLETA
    /// Busca TODOS os lugares da Bíblia onde uma palavra aparece
    /// </summary>
    [HttpPost("search-all")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<ActionResult<object>> SearchAllOccurrences([FromBody] SearchAllRequest request)
    {
        _logger.LogInformation("🔍 Busca completa por: {Keyword}", request.Keyword);

        if (string.IsNullOrWhiteSpace(request.Keyword))
        {
            return BadRequest(new { error = "Palavra-chave é obrigatória" });
        }

        try
        {
            // Busca no banco local
            var verses = await _context.Verses
                .Where(v => v.Text.Contains(request.Keyword) && v.Version == request.Version)
                .OrderBy(v => v.BookName)
                .ThenBy(v => v.Chapter)
                .ThenBy(v => v.Number)
                .ToListAsync();

            // Agrupar por livro
            var groupedByBook = verses
                .GroupBy(v => v.BookName)
                .Select(g => new
                {
                    book = g.Key,
                    testament = g.First().Testament,
                    occurrences = g.Count(),
                    verses = g.Select(v => new
                    {
                        chapter = v.Chapter,
                        verse = v.Number,
                        text = v.Text,
                        reference = $"{v.BookName} {v.Chapter}:{v.Number}"
                    }).ToList()
                })
                .ToList();

            var totalOccurrences = verses.Count;
            var booksFound = groupedByBook.Count;

            return Ok(new
            {
                keyword = request.Keyword,
                version = request.Version,
                totalOccurrences,
                booksFound,
                books = groupedByBook,
                summary = $"Encontrado '{request.Keyword}' em {totalOccurrences} versículo(s) de {booksFound} livro(s) da Bíblia"
            });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "❌ Erro na busca completa");
            return StatusCode(500, new { error = "Erro na busca" });
        }
    }

    /// <summary>
    /// 🤖 GERA FRASE MOTIVACIONAL COM IA
    /// Usa IA para criar uma frase inspiradora baseada nos versículos encontrados
    /// </summary>
    [HttpPost("generate-motivational")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<ActionResult<object>> GenerateMotivational([FromBody] MotivationalRequest request)
    {
        _logger.LogInformation("🤖 Gerando frase motivacional para: {Text}", request.Text);

        if (string.IsNullOrWhiteSpace(request.Text))
        {
            return BadRequest(new { error = "Texto é obrigatório" });
        }

        try
        {
            // 1. Analisar emoção
            var emotionAnalyzer = HttpContext.RequestServices.GetRequiredService<EmotionAnalyzerService>();
            var analysis = await emotionAnalyzer.AnalyzeEmotionAsync(request.Text);

            // 2. Buscar versículos relacionados
            var verses = await _context.VerseEmotions
                .Where(ve => ve.Emotion.Name == analysis.DetectedEmotion)
                .Include(ve => ve.Verse)
                .OrderByDescending(ve => ve.Relevance)
                .Take(3)
                .Select(ve => ve.Verse)
                .ToListAsync();

            if (!verses.Any())
            {
                verses = await _bibleService.SearchVersesByEmotionAsync(analysis.DetectedEmotion, request.Version);
            }

            // 3. Gerar frase motivacional usando IA
            var motivationalPhrase = GenerateMotivationalPhrase(analysis.DetectedEmotion, verses, request.Text);

            // 4. Criar resposta
            return Ok(new
            {
                userInput = request.Text,
                detectedEmotion = analysis.DetectedEmotion,
                confidence = analysis.Confidence,
                motivationalPhrase,
                versesUsed = verses.Select(v => new
                {
                    reference = $"{v.BookName} {v.Chapter}:{v.Number}",
                    text = v.Text,
                    author = v.Author
                }).ToList(),
                suggestions = await emotionAnalyzer.GetSuggestionsAsync(analysis.DetectedEmotion)
            });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "❌ Erro ao gerar frase motivacional");
            return StatusCode(500, new { error = "Erro ao gerar frase" });
        }
    }

    /// <summary>
    /// Gera frase motivacional baseada na emoção e versículos
    /// </summary>
    private string GenerateMotivationalPhrase(string emotion, List<Verse> verses, string userInput)
    {
        if (!verses.Any())
        {
            return "Que a Palavra de Deus ilumine seu caminho hoje!";
        }

        var mainVerse = verses.First();
        var phrases = emotion.ToLower() switch
        {
            "tristeza" => new[]
            {
                $"Lembre-se: '{mainVerse.Text.Split('.')[0]}.' ({mainVerse.BookName} {mainVerse.Chapter}:{mainVerse.Number}). Deus está perto de você neste momento difícil.",
                $"A Bíblia nos ensina em {mainVerse.BookName} {mainVerse.Chapter}:{mainVerse.Number}: '{mainVerse.Text.Split('.')[0]}.' Você não está sozinho(a).",
                $"Encontre consolo nestas palavras: '{mainVerse.Text.Split('.')[0]}.' Deus vê suas lágrimas e cuida de você."
            },
            "medo" => new[]
            {
                $"Coragem! A Palavra diz: '{mainVerse.Text.Split('.')[0]}.' ({mainVerse.BookName} {mainVerse.Chapter}:{mainVerse.Number}). Confie no Senhor!",
                $"Não tema! {mainVerse.BookName} {mainVerse.Chapter}:{mainVerse.Number} nos lembra: '{mainVerse.Text.Split('.')[0]}.' Deus está com você!",
                $"Lembre-se: '{mainVerse.Text.Split('.')[0]}.' Deus é maior que qualquer medo!"
            },
            "ansiedade" => new[]
            {
                $"Encontre paz em {mainVerse.BookName} {mainVerse.Chapter}:{mainVerse.Number}: '{mainVerse.Text.Split('.')[0]}.' Entregue suas preocupações a Deus.",
                $"Respire fundo e lembre: '{mainVerse.Text.Split('.')[0]}.' Deus cuida de cada detalhe da sua vida.",
                $"A Palavra nos ensina: '{mainVerse.Text.Split('.')[0]}.' Confie e descanse no Senhor."
            },
            "solidão" => new[]
            {
                $"Você nunca está só! {mainVerse.BookName} {mainVerse.Chapter}:{mainVerse.Number} promete: '{mainVerse.Text.Split('.')[0]}.'",
                $"Deus diz em Sua Palavra: '{mainVerse.Text.Split('.')[0]}.' Ele está sempre ao seu lado!",
                $"Lembre-se desta promessa: '{mainVerse.Text.Split('.')[0]}.' ({mainVerse.BookName} {mainVerse.Chapter}:{mainVerse.Number})"
            },
            "alegria" => new[]
            {
                $"Celebre! {mainVerse.BookName} {mainVerse.Chapter}:{mainVerse.Number} nos encoraja: '{mainVerse.Text.Split('.')[0]}.'",
                $"Continue nesta alegria! '{mainVerse.Text.Split('.')[0]}.' Que Deus multiplique sua felicidade!",
                $"Compartilhe esta alegria! A Bíblia diz: '{mainVerse.Text.Split('.')[0]}.' (mainVerse.BookName {mainVerse.Chapter}:{mainVerse.Number})"
            },
            "raiva" => new[]
            {
                $"Encontre paz em {mainVerse.BookName} {mainVerse.Chapter}:{mainVerse.Number}: '{mainVerse.Text.Split('.')[0]}.' O perdão liberta o coração.",
                $"A Palavra nos ensina: '{mainVerse.Text.Split('.')[0]}.' Deixe Deus transformar sua ira em paz.",
                $"Lembre-se: '{mainVerse.Text.Split('.')[0]}.' Deus pode acalmar seu coração."
            },
            "gratidão" => new[]
            {
                $"Continue agradecendo! {mainVerse.BookName} {mainVerse.Chapter}:{mainVerse.Number}: '{mainVerse.Text.Split('.')[0]}.'",
                $"A gratidão transforma! '{mainVerse.Text.Split('.')[0]}.' Que Deus continue te abençoando!",
                $"Louve ao Senhor! A Bíblia diz: '{mainVerse.Text.Split('.')[0]}.' ({mainVerse.BookName} {mainVerse.Chapter}:{mainVerse.Number})"
            },
            "esperança" => new[]
            {
                $"Mantenha a esperança! {mainVerse.BookName} {mainVerse.Chapter}:{mainVerse.Number}: '{mainVerse.Text.Split('.')[0]}.'",
                $"Deus tem planos para você! '{mainVerse.Text.Split('.')[0]}.' Confie e aguarde!",
                $"A promessa de Deus é real: '{mainVerse.Text.Split('.')[0]}.' ({mainVerse.BookName} {mainVerse.Chapter}:{mainVerse.Number})"
            },
            _ => new[]
            {
                $"Que esta palavra ilumine seu dia: '{mainVerse.Text.Split('.')[0]}.' ({mainVerse.BookName} {mainVerse.Chapter}:{mainVerse.Number})",
                $"Medite nesta verdade: '{mainVerse.Text.Split('.')[0]}.' Deus fala com você através de Sua Palavra.",
                $"Encontre força em {mainVerse.BookName} {mainVerse.Chapter}:{mainVerse.Number}: '{mainVerse.Text.Split('.')[0]}'"
            }
        };

        var random = new Random();
        return phrases[random.Next(phrases.Length)];
    }
}


