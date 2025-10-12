import { Component } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { BackendApiService } from '../../services/backend-api.service';
import { 
  Verse, 
  SearchVerseResponse, 
  EmotionAnalysisResponse, 
  RecommendationResponse 
} from '../../models/verse.model';

// Componente de busca inteligente - o coração da aplicação
// Agora com IA de análise de emoções!
@Component({
  selector: 'app-verse-search',
  standalone: true,
  imports: [CommonModule, FormsModule],
  templateUrl: './verse-search.component.html',
  styleUrls: ['./verse-search.component.css']
})
export class VerseSearchComponent {
  // Estado da aplicação
  searchTerm: string = '';
  selectedVersion: string = 'nvi';
  searchMode: 'simple' | 'intelligent' = 'intelligent'; // Modo de busca
  
  // Resultados
  searchResult: SearchVerseResponse | null = null;
  intelligentResult: RecommendationResponse | null = null;
  emotionAnalysis: EmotionAnalysisResponse | null = null;
  randomVerse: Verse | null = null;
  
  // UI State
  isLoading: boolean = false;
  errorMessage: string = '';
  
  // Versões disponíveis da Bíblia
  availableVersions = [
    { code: 'nvi', name: 'Nova Versão Internacional' },
    { code: 'acf', name: 'Almeida Corrigida Fiel' },
    { code: 'aa', name: 'Almeida Revista e Atualizada' }
  ];

  // Exemplos de buscas para ajudar o usuário
  exampleSearches: Array<{term: string, description: string, mode: 'simple' | 'intelligent'}> = [
    { term: 'Estou triste hoje', description: 'IA detecta tristeza e recomenda consolo', mode: 'intelligent' },
    { term: 'Estou com medo', description: 'IA detecta medo e recomenda coragem', mode: 'intelligent' },
    { term: 'Estou ansioso', description: 'IA detecta ansiedade e recomenda paz', mode: 'intelligent' },
    { term: 'amor', description: 'Busca simples por palavra', mode: 'simple' },
    { term: 'paz', description: 'Busca simples por palavra', mode: 'simple' }
  ];

  constructor(private backendApi: BackendApiService) {
    console.log('🎨 VerseSearchComponent inicializado com IA!');
  }

  // ═══════════════════════════════════════════════════════════════════════════
  // BUSCA INTELIGENTE (COM IA)
  // ═══════════════════════════════════════════════════════════════════════════

  /**
   * Busca inteligente com análise de emoção
   * Exemplo: "Estou triste" → Detecta tristeza → Recomenda versículos de consolo
   */
  intelligentSearch(): void {
    if (!this.searchTerm.trim()) {
      this.errorMessage = 'Por favor, insira como você está se sentindo.';
      return;
    }

    this.isLoading = true;
    this.errorMessage = '';
    this.clearResults();

    console.log('🧠 Iniciando busca inteligente com IA:', this.searchTerm);

    this.backendApi.getIntelligentRecommendation(this.searchTerm, this.selectedVersion)
      .subscribe({
        next: (result) => {
          this.intelligentResult = result;
          this.isLoading = false;
          
          console.log('✅ Recomendação recebida:', result);
          
          if (!result.recommendedVerse) {
            this.errorMessage = 'Não consegui encontrar versículos para este sentimento.';
          }
        },
        error: (error) => {
          console.error('❌ Erro na busca inteligente:', error);
          this.errorMessage = 'Erro ao processar sua busca. Tente novamente.';
          this.isLoading = false;
        }
      });
  }

  // ═══════════════════════════════════════════════════════════════════════════
  // BUSCA SIMPLES (POR PALAVRA)
  // ═══════════════════════════════════════════════════════════════════════════

  /**
   * Busca simples por palavra-chave
   * Exemplo: "amor" → Busca todos os versículos com "amor"
   */
  simpleSearch(): void {
    if (!this.searchTerm.trim()) {
      this.errorMessage = 'Por favor, insira uma palavra para buscar.';
      return;
    }

    this.isLoading = true;
    this.errorMessage = '';
    this.clearResults();

    console.log('🔍 Busca simples por palavra:', this.searchTerm);

    this.backendApi.searchVerses(this.searchTerm, this.selectedVersion)
      .subscribe({
        next: (result) => {
          this.searchResult = result;
          this.isLoading = false;
          
          console.log('✅ Resultado recebido:', result);
          
          if (result.count === 0) {
            this.errorMessage = 'Nenhum versículo encontrado com esta palavra.';
          }
        },
        error: (error) => {
          console.error('❌ Erro na busca simples:', error);
          this.errorMessage = 'Erro ao buscar versículos. Tente novamente.';
          this.isLoading = false;
        }
      });
  }

  // ═══════════════════════════════════════════════════════════════════════════
  // BUSCA ALEATÓRIA
  // ═══════════════════════════════════════════════════════════════════════════

  /**
   * Busca versículo aleatório - deixar Deus surpreender
   */
  getRandomVerse(): void {
    this.isLoading = true;
    this.errorMessage = '';
    this.clearResults();

    console.log('🎲 Buscando versículo aleatório');

    this.backendApi.getRandomVerse(this.selectedVersion)
      .subscribe({
        next: (verse) => {
          this.randomVerse = verse;
          this.isLoading = false;
          
          console.log('✅ Versículo aleatório recebido:', verse);
        },
        error: (error) => {
          console.error('❌ Erro ao buscar versículo aleatório:', error);
          this.errorMessage = 'Erro ao buscar versículo. Tente novamente.';
          this.isLoading = false;
        }
      });
  }

  // ═══════════════════════════════════════════════════════════════════════════
  // MÉTODOS DE APOIO
  // ═══════════════════════════════════════════════════════════════════════════

  /**
   * Executa a busca baseada no modo selecionado
   */
  search(): void {
    if (this.searchMode === 'intelligent') {
      this.intelligentSearch();
    } else {
      this.simpleSearch();
    }
  }

  /**
   * Usar um dos exemplos de busca
   */
  useExample(term: string, mode: 'simple' | 'intelligent'): void {
    this.searchTerm = term;
    this.searchMode = mode;
    this.search();
  }

  /**
   * Alterna entre modos de busca
   */
  toggleSearchMode(): void {
    this.searchMode = this.searchMode === 'simple' ? 'intelligent' : 'simple';
    this.clearResults();
  }

  /**
   * Limpa todos os resultados
   */
  clearResults(): void {
    this.searchResult = null;
    this.intelligentResult = null;
    this.emotionAnalysis = null;
    this.randomVerse = null;
  }

  /**
   * Formatar referência do versículo (novo modelo)
   */
  getVerseReference(verse: Verse): string {
    return `${verse.bookName} ${verse.chapter}:${verse.number}`;
  }

  /**
   * Obtém a cor do badge de confiança
   */
  getConfidenceBadgeClass(confidence: number): string {
    if (confidence >= 80) return 'badge-success';
    if (confidence >= 50) return 'badge-warning';
    return 'badge-secondary';
  }

  /**
   * Obtém o ícone da emoção
   */
  getEmotionIcon(emotion: string): string {
    const icons: Record<string, string> = {
      'tristeza': '😢',
      'alegria': '😊',
      'medo': '😨',
      'ansiedade': '😰',
      'solidão': '😔',
      'raiva': '😠',
      'gratidão': '🙏',
      'esperança': '🌟',
      'neutra': '😐'
    };
    return icons[emotion.toLowerCase()] || '💭';
  }
}

