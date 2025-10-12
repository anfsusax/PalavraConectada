// Modelo que representa um versículo bíblico
// Como as tábuas da lei: estrutura clara e definida

export interface Book {
  abbrev: { pt: string; en: string };
  name: string;
  author: string;
  group: string;
  version: string;
}

export interface Verse {
  book: Book;
  chapter: number;
  number: number;
  text: string;
}

export interface ApiResponse {
  book: string;
  chapter: number;
  verses: Array<{
    number: number;
    text: string;
  }>;
}

export interface SearchResult {
  occurrence: number;
  version: string;
  verses: Verse[];
}

