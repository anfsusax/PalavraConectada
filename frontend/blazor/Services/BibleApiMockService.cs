using PalavraConectada.Models;

// Serviço MOCK para desenvolvimento quando a API está offline
namespace PalavraConectada.Services
{
    public class BibleApiMockService
    {
        private readonly Dictionary<string, SearchResult> mockData = new()
        {
            ["amor"] = new SearchResult
            {
                Occurrence = 3,
                Version = "nvi",
                Verses = new List<Verse>
                {
                    new Verse
                    {
                        Book = new Book
                        {
                            Abbrev = new BookAbbrev { Pt = "jo", En = "jn" },
                            Name = "João",
                            Author = "João",
                            Group = "Evangelhos",
                            Version = "nvi"
                        },
                        Chapter = 3,
                        Number = 16,
                        Text = "Porque Deus tanto amou o mundo que deu o seu Filho Unigênito, para que todo o que nele crer não pereça, mas tenha a vida eterna."
                    },
                    new Verse
                    {
                        Book = new Book
                        {
                            Abbrev = new BookAbbrev { Pt = "1jo", En = "1jn" },
                            Name = "1 João",
                            Author = "João",
                            Group = "Epístolas Gerais",
                            Version = "nvi"
                        },
                        Chapter = 4,
                        Number = 8,
                        Text = "Quem não ama não conhece a Deus, porque Deus é amor."
                    },
                    new Verse
                    {
                        Book = new Book
                        {
                            Abbrev = new BookAbbrev { Pt = "1co", En = "1co" },
                            Name = "1 Coríntios",
                            Author = "Paulo",
                            Group = "Epístolas Paulinas",
                            Version = "nvi"
                        },
                        Chapter = 13,
                        Number = 13,
                        Text = "Assim, permanecem agora estes três: a fé, a esperança e o amor. O maior deles, porém, é o amor."
                    }
                }
            },
            ["fé"] = new SearchResult
            {
                Occurrence = 2,
                Version = "nvi",
                Verses = new List<Verse>
                {
                    new Verse
                    {
                        Book = new Book
                        {
                            Abbrev = new BookAbbrev { Pt = "hb", En = "heb" },
                            Name = "Hebreus",
                            Author = "Desconhecido",
                            Group = "Epístolas Gerais",
                            Version = "nvi"
                        },
                        Chapter = 11,
                        Number = 1,
                        Text = "Ora, a fé é a certeza daquilo que esperamos e a prova das coisas que não vemos."
                    },
                    new Verse
                    {
                        Book = new Book
                        {
                            Abbrev = new BookAbbrev { Pt = "rm", En = "rom" },
                            Name = "Romanos",
                            Author = "Paulo",
                            Group = "Epístolas Paulinas",
                            Version = "nvi"
                        },
                        Chapter = 10,
                        Number = 17,
                        Text = "Consequentemente, a fé vem por se ouvir a mensagem, e a mensagem é ouvida mediante a palavra de Cristo."
                    }
                }
            },
            ["paz"] = new SearchResult
            {
                Occurrence = 2,
                Version = "nvi",
                Verses = new List<Verse>
                {
                    new Verse
                    {
                        Book = new Book
                        {
                            Abbrev = new BookAbbrev { Pt = "jo", En = "jn" },
                            Name = "João",
                            Author = "João",
                            Group = "Evangelhos",
                            Version = "nvi"
                        },
                        Chapter = 14,
                        Number = 27,
                        Text = "Deixo-lhes a paz; a minha paz lhes dou. Não a dou como o mundo a dá. Não se perturbe o coração de vocês, nem tenham medo."
                    },
                    new Verse
                    {
                        Book = new Book
                        {
                            Abbrev = new BookAbbrev { Pt = "fp", En = "php" },
                            Name = "Filipenses",
                            Author = "Paulo",
                            Group = "Epístolas Paulinas",
                            Version = "nvi"
                        },
                        Chapter = 4,
                        Number = 7,
                        Text = "E a paz de Deus, que excede todo o entendimento, guardará o coração e a mente de vocês em Cristo Jesus."
                    }
                }
            },
            ["esperança"] = new SearchResult
            {
                Occurrence = 1,
                Version = "nvi",
                Verses = new List<Verse>
                {
                    new Verse
                    {
                        Book = new Book
                        {
                            Abbrev = new BookAbbrev { Pt = "rm", En = "rom" },
                            Name = "Romanos",
                            Author = "Paulo",
                            Group = "Epístolas Paulinas",
                            Version = "nvi"
                        },
                        Chapter = 15,
                        Number = 13,
                        Text = "Que o Deus da esperança os encha de toda alegria e paz, por sua confiança nele, para que vocês transbordem de esperança, pelo poder do Espírito Santo."
                    }
                }
            },
            ["sabedoria"] = new SearchResult
            {
                Occurrence = 2,
                Version = "nvi",
                Verses = new List<Verse>
                {
                    new Verse
                    {
                        Book = new Book
                        {
                            Abbrev = new BookAbbrev { Pt = "tg", En = "jas" },
                            Name = "Tiago",
                            Author = "Tiago",
                            Group = "Epístolas Gerais",
                            Version = "nvi"
                        },
                        Chapter = 1,
                        Number = 5,
                        Text = "Se algum de vocês tem falta de sabedoria, peça-a a Deus, que a todos dá livremente, de boa vontade; e lhe será concedida."
                    },
                    new Verse
                    {
                        Book = new Book
                        {
                            Abbrev = new BookAbbrev { Pt = "pv", En = "pro" },
                            Name = "Provérbios",
                            Author = "Salomão",
                            Group = "Poéticos",
                            Version = "nvi"
                        },
                        Chapter = 3,
                        Number = 13,
                        Text = "Como é feliz o homem que acha a sabedoria, o homem que obtém entendimento!"
                    }
                }
            }
        };

        private readonly List<Verse> randomVerses = new()
        {
            new Verse
            {
                Book = new Book
                {
                    Abbrev = new BookAbbrev { Pt = "sl", En = "ps" },
                    Name = "Salmos",
                    Author = "Davi",
                    Group = "Poéticos",
                    Version = "nvi"
                },
                Chapter = 23,
                Number = 1,
                Text = "O Senhor é o meu pastor; de nada terei falta."
            },
            new Verse
            {
                Book = new Book
                {
                    Abbrev = new BookAbbrev { Pt = "sl", En = "ps" },
                    Name = "Salmos",
                    Author = "Davi",
                    Group = "Poéticos",
                    Version = "nvi"
                },
                Chapter = 119,
                Number = 105,
                Text = "Lâmpada para os meus pés é a tua palavra e luz para o meu caminho."
            },
            new Verse
            {
                Book = new Book
                {
                    Abbrev = new BookAbbrev { Pt = "fp", En = "php" },
                    Name = "Filipenses",
                    Author = "Paulo",
                    Group = "Epístolas Paulinas",
                    Version = "nvi"
                },
                Chapter = 4,
                Number = 13,
                Text = "Tudo posso naquele que me fortalece."
            }
        };

        public async Task<SearchResult> SearchVersesAsync(string searchTerm, string version = "nvi")
        {
            Console.WriteLine($"🎭 MODO MOCK: Buscando: {searchTerm}");
            
            // Simula delay de rede
            await Task.Delay(500);
            
            var normalizedTerm = searchTerm.ToLower().Trim();
            
            if (mockData.ContainsKey(normalizedTerm))
            {
                return mockData[normalizedTerm];
            }
            
            return new SearchResult { Occurrence = 0, Version = version, Verses = new List<Verse>() };
        }

        public async Task<Verse> GetRandomVerseAsync(string version = "nvi")
        {
            Console.WriteLine("🎭 MODO MOCK: Versículo aleatório");
            
            // Simula delay de rede
            await Task.Delay(500);
            
            var random = new Random();
            var index = random.Next(randomVerses.Count);
            return randomVerses[index];
        }
    }
}

