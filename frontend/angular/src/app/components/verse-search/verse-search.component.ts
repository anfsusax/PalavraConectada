import { Component } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { BibleApiService } from '../../services/bible-api.service';
import { SearchResult, Verse } from '../../models/verse.model';

// Componente de busca - o coração da aplicação
@Component({
  selector: 'app-verse-search',
  standalone: true,
  imports: [CommonModule, FormsModule],
  templateUrl: './verse-search.component.html',
  styleUrls: ['./verse-search.component.css']
})
export class VerseSearchComponent {
  searchTerm: string = '';
  selectedVersion: string = 'nvi';
  searchResult: SearchResult | null = null;
  isLoading: boolean = false;
  errorMessage: string = '';
  
  // Versões disponíveis da Bíblia
  availableVersions = [
    { code: 'nvi', name: 'Nova Versão Internacional' },
    { code: 'acf', name: 'Almeida Corrigida Fiel' },
    { code: 'aa', name: 'Almeida Revista e Atualizada' }
  ];

  // Exemplos de buscas para ajudar o usuário
  exampleSearches = [
    { term: 'amor', description: 'Versículos sobre amor' },
    { term: 'fé', description: 'Versículos sobre fé' },
    { term: 'esperança', description: 'Versículos sobre esperança' },
    { term: 'paz', description: 'Versículos sobre paz' },
    { term: 'sabedoria', description: 'Versículos sobre sabedoria' }
  ];

  constructor(private bibleApiService: BibleApiService) {}

  // Busca versículos baseado no termo inserido
  searchVerses(): void {
    if (!this.searchTerm.trim()) {
      this.errorMessage = 'Por favor, insira uma palavra ou frase para buscar.';
      return;
    }

    this.isLoading = true;
    this.errorMessage = '';
    this.searchResult = null;

    this.bibleApiService.searchVerses(this.searchTerm, this.selectedVersion)
      .subscribe({
        next: (result) => {
          this.searchResult = result;
          this.isLoading = false;
          
          if (result.occurrence === 0) {
            this.errorMessage = 'Nenhum versículo encontrado com este termo.';
          }
        },
        error: (error) => {
          console.error('Erro na busca:', error);
          this.errorMessage = 'Erro ao buscar versículos. Tente novamente.';
          this.isLoading = false;
        }
      });
  }

  // Busca versículo aleatório - deixar Deus surpreender
  getRandomVerse(): void {
    this.isLoading = true;
    this.errorMessage = '';
    this.searchResult = null;

    this.bibleApiService.getRandomVerse(this.selectedVersion)
      .subscribe({
        next: (verse) => {
          // Adaptar o formato para SearchResult
          this.searchResult = {
            occurrence: 1,
            version: this.selectedVersion,
            verses: [verse]
          };
          this.isLoading = false;
        },
        error: (error) => {
          console.error('Erro ao buscar versículo aleatório:', error);
          this.errorMessage = 'Erro ao buscar versículo. Tente novamente.';
          this.isLoading = false;
        }
      });
  }

  // Usar um dos exemplos de busca
  useExample(term: string): void {
    this.searchTerm = term;
    this.searchVerses();
  }

  // Formatar referência do versículo
  getVerseReference(verse: Verse): string {
    return `${verse.book.name} ${verse.chapter}:${verse.number}`;
  }
}

