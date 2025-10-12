// Modelos que representam os dados da Bíblia
// Como as tábuas da lei: estrutura clara e definida

namespace PalavraConectada.Models
{
    public class Book
    {
        public BookAbbrev? Abbrev { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Author { get; set; } = string.Empty;
        public string Group { get; set; } = string.Empty;
        public string Version { get; set; } = string.Empty;
    }

    public class BookAbbrev
    {
        public string Pt { get; set; } = string.Empty;
        public string En { get; set; } = string.Empty;
    }

    public class Verse
    {
        public Book? Book { get; set; }
        public int Chapter { get; set; }
        public int Number { get; set; }
        public string Text { get; set; } = string.Empty;
    }

    public class VerseItem
    {
        public int Number { get; set; }
        public string Text { get; set; } = string.Empty;
    }

    public class SearchResult
    {
        public int Occurrence { get; set; }
        public string Version { get; set; } = string.Empty;
        public List<Verse> Verses { get; set; } = new();
    }

    public class BibleVersion
    {
        public string Code { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
    }
}

