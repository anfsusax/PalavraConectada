// Seed Data - Popula o banco com dados iniciais importantes
// Como Esdras restaurando as Escrituras
using PalavraConectada.API.Models;

namespace PalavraConectada.API.Data;

/// <summary>
/// Dados iniciais para popular o banco de dados
/// Versículos cuidadosamente selecionados por emoção
/// </summary>
public static class SeedData
{
    /// <summary>
    /// Retorna versículos importantes organizados por emoção
    /// </summary>
    public static List<Verse> GetSeedVerses()
    {
        return new List<Verse>
        {
            // ═══════════════════════════════════════════════════════════════════════
            // TRISTEZA / CONSOLO
            // ═══════════════════════════════════════════════════════════════════════
            new Verse
            {
                BookName = "Salmos",
                BookAbbrev = "sl",
                Author = "Davi",
                Group = "Poéticos",
                Testament = "VT",
                Chapter = 34,
                Number = 18,
                Text = "Perto está o Senhor dos que têm o coração quebrantado e salva os de espírito abatido.",
                Version = "nvi"
            },
            new Verse
            {
                BookName = "Mateus",
                BookAbbrev = "mt",
                Author = "Mateus",
                Group = "Evangelhos",
                Testament = "NT",
                Chapter = 5,
                Number = 4,
                Text = "Bem-aventurados os que choram, pois serão consolados.",
                Version = "nvi"
            },
            new Verse
            {
                BookName = "2 Coríntios",
                BookAbbrev = "2co",
                Author = "Paulo",
                Group = "Cartas",
                Testament = "NT",
                Chapter = 1,
                Number = 3,
                Text = "Bendito seja o Deus e Pai de nosso Senhor Jesus Cristo, o Pai das misericórdias e Deus de toda consolação.",
                Version = "nvi"
            },
            new Verse
            {
                BookName = "Salmos",
                BookAbbrev = "sl",
                Author = "Davi",
                Group = "Poéticos",
                Testament = "VT",
                Chapter = 42,
                Number = 11,
                Text = "Por que você está assim tão triste, ó minha alma? Por que está assim tão perturbada dentro de mim? Ponha a sua esperança em Deus! Pois ainda o louvarei; ele é o meu Salvador e o meu Deus.",
                Version = "nvi"
            },

            // ═══════════════════════════════════════════════════════════════════════
            // MEDO / CORAGEM
            // ═══════════════════════════════════════════════════════════════════════
            new Verse
            {
                BookName = "Josué",
                BookAbbrev = "js",
                Author = "Josué",
                Group = "História",
                Testament = "VT",
                Chapter = 1,
                Number = 9,
                Text = "Não fui eu que lhe ordenei? Seja forte e corajoso! Não se apavore, nem se desanime, pois o Senhor, o seu Deus, estará com você por onde você andar.",
                Version = "nvi"
            },
            new Verse
            {
                BookName = "Salmos",
                BookAbbrev = "sl",
                Author = "Davi",
                Group = "Poéticos",
                Testament = "VT",
                Chapter = 23,
                Number = 4,
                Text = "Mesmo quando eu andar por um vale de trevas e morte, não temerei perigo algum, pois tu estás comigo; a tua vara e o teu cajado me protegem.",
                Version = "nvi"
            },
            new Verse
            {
                BookName = "Isaías",
                BookAbbrev = "is",
                Author = "Isaías",
                Group = "Profetas",
                Testament = "VT",
                Chapter = 41,
                Number = 10,
                Text = "Não tema, pois estou com você; não tenha medo, pois sou o seu Deus. Eu o fortalecerei e o ajudarei; eu o segurarei com a minha mão direita vitoriosa.",
                Version = "nvi"
            },
            new Verse
            {
                BookName = "2 Timóteo",
                BookAbbrev = "2tm",
                Author = "Paulo",
                Group = "Cartas",
                Testament = "NT",
                Chapter = 1,
                Number = 7,
                Text = "Pois Deus não nos deu espírito de covardia, mas de poder, de amor e de equilíbrio.",
                Version = "nvi"
            },

            // ═══════════════════════════════════════════════════════════════════════
            // ANSIEDADE / PAZ
            // ═══════════════════════════════════════════════════════════════════════
            new Verse
            {
                BookName = "Filipenses",
                BookAbbrev = "fp",
                Author = "Paulo",
                Group = "Cartas",
                Testament = "NT",
                Chapter = 4,
                Number = 6,
                Text = "Não andem ansiosos por coisa alguma, mas em tudo, pela oração e súplicas, e com ação de graças, apresentem seus pedidos a Deus.",
                Version = "nvi"
            },
            new Verse
            {
                BookName = "João",
                BookAbbrev = "jo",
                Author = "João",
                Group = "Evangelhos",
                Testament = "NT",
                Chapter = 14,
                Number = 27,
                Text = "Deixo-lhes a paz; a minha paz lhes dou. Não a dou como o mundo a dá. Não se perturbe o coração de vocês, nem tenham medo.",
                Version = "nvi"
            },
            new Verse
            {
                BookName = "Mateus",
                BookAbbrev = "mt",
                Author = "Mateus",
                Group = "Evangelhos",
                Testament = "NT",
                Chapter = 6,
                Number = 34,
                Text = "Portanto, não se preocupem com o amanhã, pois o amanhã se preocupará consigo mesmo. Basta a cada dia o seu próprio mal.",
                Version = "nvi"
            },
            new Verse
            {
                BookName = "1 Pedro",
                BookAbbrev = "1pe",
                Author = "Pedro",
                Group = "Cartas",
                Testament = "NT",
                Chapter = 5,
                Number = 7,
                Text = "Lancem sobre ele toda a sua ansiedade, porque ele tem cuidado de vocês.",
                Version = "nvi"
            },

            // ═══════════════════════════════════════════════════════════════════════
            // SOLIDÃO / PRESENÇA DE DEUS
            // ═══════════════════════════════════════════════════════════════════════
            new Verse
            {
                BookName = "Deuteronômio",
                BookAbbrev = "dt",
                Author = "Moisés",
                Group = "Lei",
                Testament = "VT",
                Chapter = 31,
                Number = 6,
                Text = "Sejam fortes e corajosos. Não tenham medo nem fiquem apavorados por causa deles, pois o Senhor, o seu Deus, vai com vocês; nunca os deixará, nunca os abandonará.",
                Version = "nvi"
            },
            new Verse
            {
                BookName = "Salmos",
                BookAbbrev = "sl",
                Author = "Asafe",
                Group = "Poéticos",
                Testament = "VT",
                Chapter = 73,
                Number = 26,
                Text = "O meu corpo e o meu coração poderão fraquejar, mas Deus é a força do meu coração e a minha herança para sempre.",
                Version = "nvi"
            },
            new Verse
            {
                BookName = "Hebreus",
                BookAbbrev = "hb",
                Author = "Desconhecido",
                Group = "Cartas",
                Testament = "NT",
                Chapter = 13,
                Number = 5,
                Text = "Nunca o deixarei, nunca o abandonarei.",
                Version = "nvi"
            },

            // ═══════════════════════════════════════════════════════════════════════
            // ALEGRIA / LOUVOR
            // ═══════════════════════════════════════════════════════════════════════
            new Verse
            {
                BookName = "Salmos",
                BookAbbrev = "sl",
                Author = "Davi",
                Group = "Poéticos",
                Testament = "VT",
                Chapter = 100,
                Number = 2,
                Text = "Adorem o Senhor com alegria; entrem na sua presença com cânticos alegres.",
                Version = "nvi"
            },
            new Verse
            {
                BookName = "Neemias",
                BookAbbrev = "ne",
                Author = "Neemias",
                Group = "História",
                Testament = "VT",
                Chapter = 8,
                Number = 10,
                Text = "Não se entristeçam, pois a alegria do Senhor os fortalecerá.",
                Version = "nvi"
            },
            new Verse
            {
                BookName = "Filipenses",
                BookAbbrev = "fp",
                Author = "Paulo",
                Group = "Cartas",
                Testament = "NT",
                Chapter = 4,
                Number = 4,
                Text = "Alegrem-se sempre no Senhor. Novamente direi: Alegrem-se!",
                Version = "nvi"
            },

            // ═══════════════════════════════════════════════════════════════════════
            // RAIVA / PERDÃO
            // ═══════════════════════════════════════════════════════════════════════
            new Verse
            {
                BookName = "Efésios",
                BookAbbrev = "ef",
                Author = "Paulo",
                Group = "Cartas",
                Testament = "NT",
                Chapter = 4,
                Number = 26,
                Text = "Quando vocês ficarem irados, não pequem. Apaziguem a sua ira antes que o sol se ponha.",
                Version = "nvi"
            },
            new Verse
            {
                BookName = "Colossenses",
                BookAbbrev = "cl",
                Author = "Paulo",
                Group = "Cartas",
                Testament = "NT",
                Chapter = 3,
                Number = 13,
                Text = "Suportem-se uns aos outros e perdoem as queixas que tiverem uns contra os outros. Perdoem como o Senhor lhes perdoou.",
                Version = "nvi"
            },
            new Verse
            {
                BookName = "Provérbios",
                BookAbbrev = "pv",
                Author = "Salomão",
                Group = "Poéticos",
                Testament = "VT",
                Chapter = 15,
                Number = 1,
                Text = "A resposta calma desvia a fúria, mas a palavra ríspida desperta a ira.",
                Version = "nvi"
            },

            // ═══════════════════════════════════════════════════════════════════════
            // GRATIDÃO / AÇÃO DE GRAÇAS
            // ═══════════════════════════════════════════════════════════════════════
            new Verse
            {
                BookName = "1 Tessalonicenses",
                BookAbbrev = "1ts",
                Author = "Paulo",
                Group = "Cartas",
                Testament = "NT",
                Chapter = 5,
                Number = 18,
                Text = "Deem graças em todas as circunstâncias, pois esta é a vontade de Deus para vocês em Cristo Jesus.",
                Version = "nvi"
            },
            new Verse
            {
                BookName = "Salmos",
                BookAbbrev = "sl",
                Author = "Desconhecido",
                Group = "Poéticos",
                Testament = "VT",
                Chapter = 107,
                Number = 1,
                Text = "Deem graças ao Senhor porque ele é bom; o seu amor dura para sempre!",
                Version = "nvi"
            },
            new Verse
            {
                BookName = "Colossenses",
                BookAbbrev = "cl",
                Author = "Paulo",
                Group = "Cartas",
                Testament = "NT",
                Chapter = 3,
                Number = 17,
                Text = "E tudo o que fizerem, seja em palavra ou em ação, façam-no em nome do Senhor Jesus, dando por meio dele graças a Deus Pai.",
                Version = "nvi"
            },

            // ═══════════════════════════════════════════════════════════════════════
            // ESPERANÇA / ENCORAJAMENTO
            // ═══════════════════════════════════════════════════════════════════════
            new Verse
            {
                BookName = "Jeremias",
                BookAbbrev = "jr",
                Author = "Jeremias",
                Group = "Profetas",
                Testament = "VT",
                Chapter = 29,
                Number = 11,
                Text = "Pois eu sei os planos que tenho para vocês, diz o Senhor, planos de fazê-los prosperar e não de causar dano, planos de dar a vocês esperança e um futuro.",
                Version = "nvi"
            },
            new Verse
            {
                BookName = "Romanos",
                BookAbbrev = "rm",
                Author = "Paulo",
                Group = "Cartas",
                Testament = "NT",
                Chapter = 15,
                Number = 13,
                Text = "Que o Deus da esperança os encha de toda alegria e paz, por sua confiança nele, para que vocês transbordem de esperança, pelo poder do Espírito Santo.",
                Version = "nvi"
            },
            new Verse
            {
                BookName = "Salmos",
                BookAbbrev = "sl",
                Author = "Davi",
                Group = "Poéticos",
                Testament = "VT",
                Chapter = 27,
                Number = 14,
                Text = "Espere no Senhor. Seja forte! Coragem! Espere no Senhor.",
                Version = "nvi"
            },

            // ═══════════════════════════════════════════════════════════════════════
            // VERSÍCULOS GERAIS / AMOR
            // ═══════════════════════════════════════════════════════════════════════
            new Verse
            {
                BookName = "João",
                BookAbbrev = "jo",
                Author = "João",
                Group = "Evangelhos",
                Testament = "NT",
                Chapter = 3,
                Number = 16,
                Text = "Porque Deus tanto amou o mundo que deu o seu Filho Unigênito, para que todo o que nele crer não pereça, mas tenha a vida eterna.",
                Version = "nvi"
            },
            new Verse
            {
                BookName = "Romanos",
                BookAbbrev = "rm",
                Author = "Paulo",
                Group = "Cartas",
                Testament = "NT",
                Chapter = 8,
                Number = 28,
                Text = "Sabemos que Deus age em todas as coisas para o bem daqueles que o amam, dos que foram chamados de acordo com o seu propósito.",
                Version = "nvi"
            },
            new Verse
            {
                BookName = "Salmos",
                BookAbbrev = "sl",
                Author = "Davi",
                Group = "Poéticos",
                Testament = "VT",
                Chapter = 46,
                Number = 1,
                Text = "Deus é o nosso refúgio e a nossa fortaleza, auxílio sempre presente na adversidade.",
                Version = "nvi"
            }
        };
    }

    /// <summary>
    /// Retorna relacionamentos entre versículos e emoções
    /// </summary>
    public static List<VerseEmotion> GetVerseEmotions()
    {
        return new List<VerseEmotion>
        {
            // Tristeza (ID: 1)
            new VerseEmotion { VerseId = 1, EmotionId = 1, Relevance = 10 },  // Salmos 34:18
            new VerseEmotion { VerseId = 2, EmotionId = 1, Relevance = 10 },  // Mateus 5:4
            new VerseEmotion { VerseId = 3, EmotionId = 1, Relevance = 9 },   // 2 Coríntios 1:3
            new VerseEmotion { VerseId = 4, EmotionId = 1, Relevance = 9 },   // Salmos 42:11

            // Medo (ID: 3)
            new VerseEmotion { VerseId = 5, EmotionId = 3, Relevance = 10 },  // Josué 1:9
            new VerseEmotion { VerseId = 6, EmotionId = 3, Relevance = 10 },  // Salmos 23:4
            new VerseEmotion { VerseId = 7, EmotionId = 3, Relevance = 10 },  // Isaías 41:10
            new VerseEmotion { VerseId = 8, EmotionId = 3, Relevance = 9 },   // 2 Timóteo 1:7

            // Ansiedade (ID: 4)
            new VerseEmotion { VerseId = 9, EmotionId = 4, Relevance = 10 },  // Filipenses 4:6
            new VerseEmotion { VerseId = 10, EmotionId = 4, Relevance = 10 }, // João 14:27
            new VerseEmotion { VerseId = 11, EmotionId = 4, Relevance = 9 },  // Mateus 6:34
            new VerseEmotion { VerseId = 12, EmotionId = 4, Relevance = 10 }, // 1 Pedro 5:7

            // Solidão (ID: 5)
            new VerseEmotion { VerseId = 13, EmotionId = 5, Relevance = 10 }, // Deuteronômio 31:6
            new VerseEmotion { VerseId = 14, EmotionId = 5, Relevance = 9 },  // Salmos 73:26
            new VerseEmotion { VerseId = 15, EmotionId = 5, Relevance = 10 }, // Hebreus 13:5

            // Alegria (ID: 2)
            new VerseEmotion { VerseId = 16, EmotionId = 2, Relevance = 10 }, // Salmos 100:2
            new VerseEmotion { VerseId = 17, EmotionId = 2, Relevance = 10 }, // Neemias 8:10
            new VerseEmotion { VerseId = 18, EmotionId = 2, Relevance = 10 }, // Filipenses 4:4

            // Raiva (ID: 6)
            new VerseEmotion { VerseId = 19, EmotionId = 6, Relevance = 10 }, // Efésios 4:26
            new VerseEmotion { VerseId = 20, EmotionId = 6, Relevance = 10 }, // Colossenses 3:13
            new VerseEmotion { VerseId = 21, EmotionId = 6, Relevance = 9 },  // Provérbios 15:1

            // Gratidão (ID: 7)
            new VerseEmotion { VerseId = 22, EmotionId = 7, Relevance = 10 }, // 1 Tessalonicenses 5:18
            new VerseEmotion { VerseId = 23, EmotionId = 7, Relevance = 10 }, // Salmos 107:1
            new VerseEmotion { VerseId = 24, EmotionId = 7, Relevance = 9 },  // Colossenses 3:17

            // Esperança (ID: 8)
            new VerseEmotion { VerseId = 25, EmotionId = 8, Relevance = 10 }, // Jeremias 29:11
            new VerseEmotion { VerseId = 26, EmotionId = 8, Relevance = 10 }, // Romanos 15:13
            new VerseEmotion { VerseId = 27, EmotionId = 8, Relevance = 9 },  // Salmos 27:14

            // Versículos gerais que servem para múltiplas emoções
            new VerseEmotion { VerseId = 28, EmotionId = 2, Relevance = 8 },  // João 3:16 - Alegria
            new VerseEmotion { VerseId = 28, EmotionId = 8, Relevance = 8 },  // João 3:16 - Esperança
            new VerseEmotion { VerseId = 29, EmotionId = 8, Relevance = 10 }, // Romanos 8:28 - Esperança
            new VerseEmotion { VerseId = 29, EmotionId = 1, Relevance = 8 },  // Romanos 8:28 - Tristeza
            new VerseEmotion { VerseId = 30, EmotionId = 3, Relevance = 10 }, // Salmos 46:1 - Medo
            new VerseEmotion { VerseId = 30, EmotionId = 4, Relevance = 9 },  // Salmos 46:1 - Ansiedade
        };
    }
}

