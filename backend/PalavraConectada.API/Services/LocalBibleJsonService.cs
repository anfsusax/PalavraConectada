// Serviço para ler arquivos JSON da Bíblia da pasta biblia-master local
using System.Text.Json;
using PalavraConectada.API.Models;

namespace PalavraConectada.API.Services;

/// <summary>
/// Serviço que lê arquivos JSON da Bíblia da pasta biblia-master local
/// Estrutura: [{ "abbrev": "gn", "name": "Gênesis", "chapters": [[...], [...]] }]
/// </summary>
public class LocalBibleJsonService
{
    private readonly ILogger<LocalBibleJsonService> _logger;
    private readonly string _bibliaMasterPath;
    private readonly Dictionary<string, List<BibleBookJson>> _cache = new();

    public LocalBibleJsonService(ILogger<LocalBibleJsonService> logger)
    {
        _logger = logger;
        // Caminho para a pasta biblia-master (na raiz do projeto)
        // Tenta vários caminhos possíveis
        var possiblePaths = new[]
        {
            Path.Combine(Directory.GetCurrentDirectory(), "..", "..", "..", "..", "biblia-master", "json"),
            Path.Combine(Directory.GetCurrentDirectory(), "..", "..", "biblia-master", "json"),
            Path.Combine(AppContext.BaseDirectory, "..", "..", "..", "..", "biblia-master", "json"),
            Path.Combine(AppContext.BaseDirectory, "..", "..", "biblia-master", "json"),
            Path.Combine(Environment.CurrentDirectory, "biblia-master", "json")
        };

        _bibliaMasterPath = possiblePaths.FirstOrDefault(Directory.Exists) 
            ?? possiblePaths[0]; // Usa o primeiro se nenhum existir
        
        if (!Directory.Exists(_bibliaMasterPath))
        {
            _logger.LogWarning("⚠️ Pasta biblia-master/json não encontrada. Tentou: {Paths}", 
                string.Join(", ", possiblePaths));
        }
        else
        {
            _logger.LogInformation("✅ Pasta biblia-master/json encontrada em: {Path}", _bibliaMasterPath);
        }
    }

    /// <summary>
    /// Carrega os dados de uma versão específica
    /// </summary>
    public async Task<List<BibleBookJson>> LoadVersionAsync(string version)
    {
        if (_cache.TryGetValue(version, out var cached))
        {
            return cached;
        }

        var fileName = $"{version}.json";
        var filePath = Path.Combine(_bibliaMasterPath, fileName);

        if (!File.Exists(filePath))
        {
            _logger.LogError("❌ Arquivo não encontrado: {FilePath}", filePath);
            return new List<BibleBookJson>();
        }

        try
        {
            _logger.LogInformation("📂 Carregando {FileName}...", fileName);
            var jsonContent = await File.ReadAllTextAsync(filePath);
            var books = JsonSerializer.Deserialize<List<BibleBookJson>>(jsonContent, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });

            if (books != null && books.Any())
            {
                _cache[version] = books;
                _logger.LogInformation("✅ {FileName} carregado: {Count} livros", fileName, books.Count);
                return books;
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "❌ Erro ao carregar {FileName}", fileName);
        }

        return new List<BibleBookJson>();
    }

    /// <summary>
    /// Obtém todos os livros de uma versão
    /// </summary>
    public async Task<List<BookInfo>> GetBooksListAsync(string version)
    {
        var books = await LoadVersionAsync(version);
        
        if (!books.Any())
        {
            return GetDefaultBooksList();
        }

        return books.Select((book, index) => new BookInfo
        {
            Abbrev = book.Abbrev ?? "",
            Name = book.Name ?? "",
            Author = GetBookAuthor(book.Name ?? ""),
            Group = GetBookGroup(index),
            Testament = index < 39 ? "VT" : "NT", // 39 primeiros são VT
            Chapters = book.Chapters?.Count ?? 0
        }).ToList();
    }

    /// <summary>
    /// Obtém todos os versículos de um capítulo
    /// </summary>
    public async Task<List<Verse>> GetChapterVersesAsync(string bookAbbrev, int chapterNumber, string version)
    {
        var books = await LoadVersionAsync(version);
        var book = books.FirstOrDefault(b => 
            b.Abbrev?.Equals(bookAbbrev, StringComparison.OrdinalIgnoreCase) == true);

        if (book == null || book.Chapters == null)
        {
            return new List<Verse>();
        }

        // Capítulos são indexados a partir de 0, mas chapterNumber começa em 1
        var chapterIndex = chapterNumber - 1;
        if (chapterIndex < 0 || chapterIndex >= book.Chapters.Count)
        {
            return new List<Verse>();
        }

        var chapterVerses = book.Chapters[chapterIndex];
        var verses = new List<Verse>();

        for (int verseNumber = 1; verseNumber <= chapterVerses.Count; verseNumber++)
        {
            var verseText = chapterVerses[verseNumber - 1];
            if (string.IsNullOrWhiteSpace(verseText))
                continue;

            verses.Add(new Verse
            {
                BookName = book.Name ?? "",
                BookAbbrev = book.Abbrev ?? "",
                Author = GetBookAuthor(book.Name ?? ""),
                Group = GetBookGroup(books.IndexOf(book)),
                Testament = books.IndexOf(book) < 39 ? "VT" : "NT",
                Chapter = chapterNumber,
                Number = verseNumber,
                Text = verseText,
                Version = version
            });
        }

        return verses;
    }

    /// <summary>
    /// Busca versículos por palavra-chave
    /// </summary>
    public async Task<List<Verse>> SearchVersesAsync(string keyword, string version)
    {
        var books = await LoadVersionAsync(version);
        var verses = new List<Verse>();
        var normalizedKeyword = keyword.ToLower();

        foreach (var book in books)
        {
            if (book.Chapters == null)
                continue;

            for (int chapterIndex = 0; chapterIndex < book.Chapters.Count; chapterIndex++)
            {
                var chapter = book.Chapters[chapterIndex];
                for (int verseIndex = 0; verseIndex < chapter.Count; verseIndex++)
                {
                    var verseText = chapter[verseIndex];
                    if (verseText?.ToLower().Contains(normalizedKeyword) == true)
                    {
                        verses.Add(new Verse
                        {
                            BookName = book.Name ?? "",
                            BookAbbrev = book.Abbrev ?? "",
                            Author = GetBookAuthor(book.Name ?? ""),
                            Group = GetBookGroup(books.IndexOf(book)),
                            Testament = books.IndexOf(book) < 39 ? "VT" : "NT",
                            Chapter = chapterIndex + 1,
                            Number = verseIndex + 1,
                            Text = verseText,
                            Version = version
                        });
                    }
                }
            }
        }

        return verses;
    }

    // ═══════════════════════════════════════════════════════════════════════════
    // MÉTODOS AUXILIARES
    // ═══════════════════════════════════════════════════════════════════════════

    private string GetBookAuthor(string bookName)
    {
        var authors = new Dictionary<string, string>
        {
            ["Gênesis"] = "Moisés", ["Êxodo"] = "Moisés", ["Levítico"] = "Moisés",
            ["Números"] = "Moisés", ["Deuteronômio"] = "Moisés",
            ["Josué"] = "Josué", ["Juízes"] = "Samuel", ["Rute"] = "Samuel",
            ["1 Samuel"] = "Samuel", ["2 Samuel"] = "Samuel",
            ["1 Reis"] = "Jeremias", ["2 Reis"] = "Jeremias",
            ["1 Crônicas"] = "Esdras", ["2 Crônicas"] = "Esdras",
            ["Esdras"] = "Esdras", ["Neemias"] = "Neemias", ["Ester"] = "Mardoqueu",
            ["Jó"] = "Desconhecido", ["Salmos"] = "Diversos", ["Provérbios"] = "Salomão",
            ["Eclesiastes"] = "Salomão", ["Cantares"] = "Salomão",
            ["Isaías"] = "Isaías", ["Jeremias"] = "Jeremias", ["Lamentações"] = "Jeremias",
            ["Ezequiel"] = "Ezequiel", ["Daniel"] = "Daniel",
            ["Oséias"] = "Oséias", ["Joel"] = "Joel", ["Amós"] = "Amós",
            ["Obadias"] = "Obadias", ["Jonas"] = "Jonas", ["Miqueias"] = "Miqueias",
            ["Naum"] = "Naum", ["Habacuque"] = "Habacuque", ["Sofonias"] = "Sofonias",
            ["Ageu"] = "Ageu", ["Zacarias"] = "Zacarias", ["Malaquias"] = "Malaquias",
            ["Mateus"] = "Mateus", ["Marcos"] = "Marcos", ["Lucas"] = "Lucas",
            ["João"] = "João", ["Atos"] = "Lucas", ["Romanos"] = "Paulo",
            ["1 Coríntios"] = "Paulo", ["2 Coríntios"] = "Paulo", ["Gálatas"] = "Paulo",
            ["Efésios"] = "Paulo", ["Filipenses"] = "Paulo", ["Colossenses"] = "Paulo",
            ["1 Tessalonicenses"] = "Paulo", ["2 Tessalonicenses"] = "Paulo",
            ["1 Timóteo"] = "Paulo", ["2 Timóteo"] = "Paulo", ["Tito"] = "Paulo",
            ["Filemom"] = "Paulo", ["Hebreus"] = "Desconhecido", ["Tiago"] = "Tiago",
            ["1 Pedro"] = "Pedro", ["2 Pedro"] = "Pedro", ["1 João"] = "João",
            ["2 João"] = "João", ["3 João"] = "João", ["Judas"] = "Judas",
            ["Apocalipse"] = "João"
        };

        return authors.TryGetValue(bookName, out var author) ? author : "Desconhecido";
    }

    private string GetBookGroup(int bookIndex)
    {
        // Velho Testamento (0-38)
        if (bookIndex < 5) return "Pentateuco";
        if (bookIndex < 17) return "Históricos";
        if (bookIndex < 22) return "Poéticos";
        if (bookIndex < 39) return bookIndex < 27 ? "Profetas Maiores" : "Profetas Menores";
        
        // Novo Testamento (39-65)
        if (bookIndex < 43) return "Evangelhos";
        if (bookIndex < 44) return "Históricos";
        if (bookIndex < 57) return "Cartas Paulinas";
        if (bookIndex < 65) return "Cartas Gerais";
        return "Proféticos";
    }

    private List<BookInfo> GetDefaultBooksList()
    {
        return new List<BookInfo>
        {
            // VELHO TESTAMENTO
            new() { Abbrev = "gn", Name = "Gênesis", Author = "Moisés", Group = "Pentateuco", Testament = "VT", Chapters = 50 },
            new() { Abbrev = "ex", Name = "Êxodo", Author = "Moisés", Group = "Pentateuco", Testament = "VT", Chapters = 40 },
            new() { Abbrev = "lv", Name = "Levítico", Author = "Moisés", Group = "Pentateuco", Testament = "VT", Chapters = 27 },
            new() { Abbrev = "nm", Name = "Números", Author = "Moisés", Group = "Pentateuco", Testament = "VT", Chapters = 36 },
            new() { Abbrev = "dt", Name = "Deuteronômio", Author = "Moisés", Group = "Pentateuco", Testament = "VT", Chapters = 34 },
            new() { Abbrev = "js", Name = "Josué", Author = "Josué", Group = "Históricos", Testament = "VT", Chapters = 24 },
            new() { Abbrev = "jz", Name = "Juízes", Author = "Samuel", Group = "Históricos", Testament = "VT", Chapters = 21 },
            new() { Abbrev = "rt", Name = "Rute", Author = "Samuel", Group = "Históricos", Testament = "VT", Chapters = 4 },
            new() { Abbrev = "1sm", Name = "1 Samuel", Author = "Samuel", Group = "Históricos", Testament = "VT", Chapters = 31 },
            new() { Abbrev = "2sm", Name = "2 Samuel", Author = "Samuel", Group = "Históricos", Testament = "VT", Chapters = 24 },
            new() { Abbrev = "1rs", Name = "1 Reis", Author = "Jeremias", Group = "Históricos", Testament = "VT", Chapters = 22 },
            new() { Abbrev = "2rs", Name = "2 Reis", Author = "Jeremias", Group = "Históricos", Testament = "VT", Chapters = 25 },
            new() { Abbrev = "1cr", Name = "1 Crônicas", Author = "Esdras", Group = "Históricos", Testament = "VT", Chapters = 29 },
            new() { Abbrev = "2cr", Name = "2 Crônicas", Author = "Esdras", Group = "Históricos", Testament = "VT", Chapters = 36 },
            new() { Abbrev = "ed", Name = "Esdras", Author = "Esdras", Group = "Históricos", Testament = "VT", Chapters = 10 },
            new() { Abbrev = "ne", Name = "Neemias", Author = "Neemias", Group = "Históricos", Testament = "VT", Chapters = 13 },
            new() { Abbrev = "et", Name = "Ester", Author = "Mardoqueu", Group = "Históricos", Testament = "VT", Chapters = 10 },
            new() { Abbrev = "job", Name = "Jó", Author = "Desconhecido", Group = "Poéticos", Testament = "VT", Chapters = 42 },
            new() { Abbrev = "sl", Name = "Salmos", Author = "Diversos", Group = "Poéticos", Testament = "VT", Chapters = 150 },
            new() { Abbrev = "pv", Name = "Provérbios", Author = "Salomão", Group = "Poéticos", Testament = "VT", Chapters = 31 },
            new() { Abbrev = "ec", Name = "Eclesiastes", Author = "Salomão", Group = "Poéticos", Testament = "VT", Chapters = 12 },
            new() { Abbrev = "ct", Name = "Cantares", Author = "Salomão", Group = "Poéticos", Testament = "VT", Chapters = 8 },
            new() { Abbrev = "is", Name = "Isaías", Author = "Isaías", Group = "Profetas Maiores", Testament = "VT", Chapters = 66 },
            new() { Abbrev = "jr", Name = "Jeremias", Author = "Jeremias", Group = "Profetas Maiores", Testament = "VT", Chapters = 52 },
            new() { Abbrev = "lm", Name = "Lamentações", Author = "Jeremias", Group = "Profetas Maiores", Testament = "VT", Chapters = 5 },
            new() { Abbrev = "ez", Name = "Ezequiel", Author = "Ezequiel", Group = "Profetas Maiores", Testament = "VT", Chapters = 48 },
            new() { Abbrev = "dn", Name = "Daniel", Author = "Daniel", Group = "Profetas Maiores", Testament = "VT", Chapters = 12 },
            new() { Abbrev = "os", Name = "Oséias", Author = "Oséias", Group = "Profetas Menores", Testament = "VT", Chapters = 14 },
            new() { Abbrev = "jl", Name = "Joel", Author = "Joel", Group = "Profetas Menores", Testament = "VT", Chapters = 3 },
            new() { Abbrev = "am", Name = "Amós", Author = "Amós", Group = "Profetas Menores", Testament = "VT", Chapters = 9 },
            new() { Abbrev = "ob", Name = "Obadias", Author = "Obadias", Group = "Profetas Menores", Testament = "VT", Chapters = 1 },
            new() { Abbrev = "jn", Name = "Jonas", Author = "Jonas", Group = "Profetas Menores", Testament = "VT", Chapters = 4 },
            new() { Abbrev = "mq", Name = "Miqueias", Author = "Miqueias", Group = "Profetas Menores", Testament = "VT", Chapters = 7 },
            new() { Abbrev = "na", Name = "Naum", Author = "Naum", Group = "Profetas Menores", Testament = "VT", Chapters = 3 },
            new() { Abbrev = "hc", Name = "Habacuque", Author = "Habacuque", Group = "Profetas Menores", Testament = "VT", Chapters = 3 },
            new() { Abbrev = "sf", Name = "Sofonias", Author = "Sofonias", Group = "Profetas Menores", Testament = "VT", Chapters = 3 },
            new() { Abbrev = "ag", Name = "Ageu", Author = "Ageu", Group = "Profetas Menores", Testament = "VT", Chapters = 2 },
            new() { Abbrev = "zc", Name = "Zacarias", Author = "Zacarias", Group = "Profetas Menores", Testament = "VT", Chapters = 14 },
            new() { Abbrev = "ml", Name = "Malaquias", Author = "Malaquias", Group = "Profetas Menores", Testament = "VT", Chapters = 4 },
            
            // NOVO TESTAMENTO
            new() { Abbrev = "mt", Name = "Mateus", Author = "Mateus", Group = "Evangelhos", Testament = "NT", Chapters = 28 },
            new() { Abbrev = "mc", Name = "Marcos", Author = "Marcos", Group = "Evangelhos", Testament = "NT", Chapters = 16 },
            new() { Abbrev = "lc", Name = "Lucas", Author = "Lucas", Group = "Evangelhos", Testament = "NT", Chapters = 24 },
            new() { Abbrev = "jo", Name = "João", Author = "João", Group = "Evangelhos", Testament = "NT", Chapters = 21 },
            new() { Abbrev = "at", Name = "Atos", Author = "Lucas", Group = "Históricos", Testament = "NT", Chapters = 28 },
            new() { Abbrev = "rm", Name = "Romanos", Author = "Paulo", Group = "Cartas Paulinas", Testament = "NT", Chapters = 16 },
            new() { Abbrev = "1co", Name = "1 Coríntios", Author = "Paulo", Group = "Cartas Paulinas", Testament = "NT", Chapters = 16 },
            new() { Abbrev = "2co", Name = "2 Coríntios", Author = "Paulo", Group = "Cartas Paulinas", Testament = "NT", Chapters = 13 },
            new() { Abbrev = "gl", Name = "Gálatas", Author = "Paulo", Group = "Cartas Paulinas", Testament = "NT", Chapters = 6 },
            new() { Abbrev = "ef", Name = "Efésios", Author = "Paulo", Group = "Cartas Paulinas", Testament = "NT", Chapters = 6 },
            new() { Abbrev = "fp", Name = "Filipenses", Author = "Paulo", Group = "Cartas Paulinas", Testament = "NT", Chapters = 4 },
            new() { Abbrev = "cl", Name = "Colossenses", Author = "Paulo", Group = "Cartas Paulinas", Testament = "NT", Chapters = 4 },
            new() { Abbrev = "1ts", Name = "1 Tessalonicenses", Author = "Paulo", Group = "Cartas Paulinas", Testament = "NT", Chapters = 5 },
            new() { Abbrev = "2ts", Name = "2 Tessalonicenses", Author = "Paulo", Group = "Cartas Paulinas", Testament = "NT", Chapters = 3 },
            new() { Abbrev = "1tm", Name = "1 Timóteo", Author = "Paulo", Group = "Cartas Paulinas", Testament = "NT", Chapters = 6 },
            new() { Abbrev = "2tm", Name = "2 Timóteo", Author = "Paulo", Group = "Cartas Paulinas", Testament = "NT", Chapters = 4 },
            new() { Abbrev = "tt", Name = "Tito", Author = "Paulo", Group = "Cartas Paulinas", Testament = "NT", Chapters = 3 },
            new() { Abbrev = "fm", Name = "Filemom", Author = "Paulo", Group = "Cartas Paulinas", Testament = "NT", Chapters = 1 },
            new() { Abbrev = "hb", Name = "Hebreus", Author = "Desconhecido", Group = "Cartas Gerais", Testament = "NT", Chapters = 13 },
            new() { Abbrev = "tg", Name = "Tiago", Author = "Tiago", Group = "Cartas Gerais", Testament = "NT", Chapters = 5 },
            new() { Abbrev = "1pe", Name = "1 Pedro", Author = "Pedro", Group = "Cartas Gerais", Testament = "NT", Chapters = 5 },
            new() { Abbrev = "2pe", Name = "2 Pedro", Author = "Pedro", Group = "Cartas Gerais", Testament = "NT", Chapters = 3 },
            new() { Abbrev = "1jo", Name = "1 João", Author = "João", Group = "Cartas Gerais", Testament = "NT", Chapters = 5 },
            new() { Abbrev = "2jo", Name = "2 João", Author = "João", Group = "Cartas Gerais", Testament = "NT", Chapters = 1 },
            new() { Abbrev = "3jo", Name = "3 João", Author = "João", Group = "Cartas Gerais", Testament = "NT", Chapters = 1 },
            new() { Abbrev = "jd", Name = "Judas", Author = "Judas", Group = "Cartas Gerais", Testament = "NT", Chapters = 1 },
            new() { Abbrev = "ap", Name = "Apocalipse", Author = "João", Group = "Proféticos", Testament = "NT", Chapters = 22 }
        };
    }
}

// ═══════════════════════════════════════════════════════════════════════════
// MODELOS JSON DO REPOSITÓRIO THIAGOBODRUK/BIBLIA
// ═══════════════════════════════════════════════════════════════════════════

public class BibleBookJson
{
    public string? Abbrev { get; set; }
    public string? Name { get; set; }
    public List<List<string>>? Chapters { get; set; } // Array de arrays: chapters[capítulo][versículo]
}

