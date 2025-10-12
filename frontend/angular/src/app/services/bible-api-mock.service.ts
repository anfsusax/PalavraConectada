import { Injectable } from '@angular/core';
import { Observable, of, delay } from 'rxjs';
import { SearchResult, Verse } from '../models/verse.model';

// Serviço MOCK para desenvolvimento quando a API está offline
@Injectable({
  providedIn: 'root'
})
export class BibleApiMockService {
  private readonly MOCK_DATA = {
    amor: {
      occurrence: 3,
      version: 'nvi',
      verses: [
        {
          book: {
            abbrev: { pt: 'jo', en: 'jn' },
            name: 'João',
            author: 'João',
            group: 'Evangelhos',
            version: 'nvi'
          },
          chapter: 3,
          number: 16,
          text: 'Porque Deus tanto amou o mundo que deu o seu Filho Unigênito, para que todo o que nele crer não pereça, mas tenha a vida eterna.'
        },
        {
          book: {
            abbrev: { pt: '1jo', en: '1jn' },
            name: '1 João',
            author: 'João',
            group: 'Epístolas Gerais',
            version: 'nvi'
          },
          chapter: 4,
          number: 8,
          text: 'Quem não ama não conhece a Deus, porque Deus é amor.'
        },
        {
          book: {
            abbrev: { pt: '1co', en: '1co' },
            name: '1 Coríntios',
            author: 'Paulo',
            group: 'Epístolas Paulinas',
            version: 'nvi'
          },
          chapter: 13,
          number: 13,
          text: 'Assim, permanecem agora estes três: a fé, a esperança e o amor. O maior deles, porém, é o amor.'
        }
      ]
    },
    fé: {
      occurrence: 2,
      version: 'nvi',
      verses: [
        {
          book: {
            abbrev: { pt: 'hb', en: 'heb' },
            name: 'Hebreus',
            author: 'Desconhecido',
            group: 'Epístolas Gerais',
            version: 'nvi'
          },
          chapter: 11,
          number: 1,
          text: 'Ora, a fé é a certeza daquilo que esperamos e a prova das coisas que não vemos.'
        },
        {
          book: {
            abbrev: { pt: 'rm', en: 'rom' },
            name: 'Romanos',
            author: 'Paulo',
            group: 'Epístolas Paulinas',
            version: 'nvi'
          },
          chapter: 10,
          number: 17,
          text: 'Consequentemente, a fé vem por se ouvir a mensagem, e a mensagem é ouvida mediante a palavra de Cristo.'
        }
      ]
    },
    paz: {
      occurrence: 2,
      version: 'nvi',
      verses: [
        {
          book: {
            abbrev: { pt: 'jo', en: 'jn' },
            name: 'João',
            author: 'João',
            group: 'Evangelhos',
            version: 'nvi'
          },
          chapter: 14,
          number: 27,
          text: 'Deixo-lhes a paz; a minha paz lhes dou. Não a dou como o mundo a dá. Não se perturbe o coração de vocês, nem tenham medo.'
        },
        {
          book: {
            abbrev: { pt: 'fp', en: 'php' },
            name: 'Filipenses',
            author: 'Paulo',
            group: 'Epístolas Paulinas',
            version: 'nvi'
          },
          chapter: 4,
          number: 7,
          text: 'E a paz de Deus, que excede todo o entendimento, guardará o coração e a mente de vocês em Cristo Jesus.'
        }
      ]
    },
    esperança: {
      occurrence: 1,
      version: 'nvi',
      verses: [
        {
          book: {
            abbrev: { pt: 'rm', en: 'rom' },
            name: 'Romanos',
            author: 'Paulo',
            group: 'Epístolas Paulinas',
            version: 'nvi'
          },
          chapter: 15,
          number: 13,
          text: 'Que o Deus da esperança os encha de toda alegria e paz, por sua confiança nele, para que vocês transbordem de esperança, pelo poder do Espírito Santo.'
        }
      ]
    },
    sabedoria: {
      occurrence: 2,
      version: 'nvi',
      verses: [
        {
          book: {
            abbrev: { pt: 'tg', en: 'jas' },
            name: 'Tiago',
            author: 'Tiago',
            group: 'Epístolas Gerais',
            version: 'nvi'
          },
          chapter: 1,
          number: 5,
          text: 'Se algum de vocês tem falta de sabedoria, peça-a a Deus, que a todos dá livremente, de boa vontade; e lhe será concedida.'
        },
        {
          book: {
            abbrev: { pt: 'pv', en: 'pro' },
            name: 'Provérbios',
            author: 'Salomão',
            group: 'Poéticos',
            version: 'nvi'
          },
          chapter: 3,
          number: 13,
          text: 'Como é feliz o homem que acha a sabedoria, o homem que obtém entendimento!'
        }
      ]
    }
  };

  private readonly RANDOM_VERSES: Verse[] = [
    {
      book: {
        abbrev: { pt: 'sl', en: 'ps' },
        name: 'Salmos',
        author: 'Davi',
        group: 'Poéticos',
        version: 'nvi'
      },
      chapter: 23,
      number: 1,
      text: 'O Senhor é o meu pastor; de nada terei falta.'
    },
    {
      book: {
        abbrev: { pt: 'sl', en: 'ps' },
        name: 'Salmos',
        author: 'Davi',
        group: 'Poéticos',
        version: 'nvi'
      },
      chapter: 119,
      number: 105,
      text: 'Lâmpada para os meus pés é a tua palavra e luz para o meu caminho.'
    },
    {
      book: {
        abbrev: { pt: 'fp', en: 'php' },
        name: 'Filipenses',
        author: 'Paulo',
        group: 'Epístolas Paulinas',
        version: 'nvi'
      },
      chapter: 4,
      number: 13,
      text: 'Tudo posso naquele que me fortalece.'
    }
  ];

  searchVerses(searchTerm: string, version: string = 'nvi'): Observable<SearchResult> {
    console.log('🎭 MODO MOCK: Buscando:', searchTerm);
    
    const normalizedTerm = searchTerm.toLowerCase().trim();
    const result = this.MOCK_DATA[normalizedTerm as keyof typeof this.MOCK_DATA];
    
    if (result) {
      return of(result).pipe(delay(500)); // Simula delay de rede
    }
    
    return of({
      occurrence: 0,
      version: version,
      verses: []
    });
  }

  getRandomVerse(version: string = 'nvi'): Observable<Verse> {
    console.log('🎭 MODO MOCK: Versículo aleatório');
    const randomIndex = Math.floor(Math.random() * this.RANDOM_VERSES.length);
    return of(this.RANDOM_VERSES[randomIndex]).pipe(delay(500));
  }

  getVerse(version: string, bookAbbrev: string, chapter: number, verse: number): Observable<Verse> {
    console.log('🎭 MODO MOCK: Versículo específico');
    return of(this.RANDOM_VERSES[0]).pipe(delay(500));
  }

  getVersions(): Observable<any> {
    return of([
      { version: 'nvi', verses: 31102 },
      { version: 'acf', verses: 31106 },
      { version: 'aa', verses: 31105 }
    ]).pipe(delay(500));
  }
}

