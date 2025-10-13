import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable, catchError, of } from 'rxjs';
import { 
  Verse, 
  SearchVerseResponse, 
  Emotion, 
  EmotionAnalysisResponse, 
  RecommendationResponse 
} from '../models/verse.model';

// Serviço para consumir NOSSA API backend
// Detecta automaticamente ambiente: desenvolvimento ou produção
@Injectable({
  providedIn: 'root'
})
export class BackendApiService {
  private readonly API_BASE_URL = this.getApiUrl();
  
  constructor(private http: HttpClient) {
    console.log('🔥 BackendApiService inicializado - Usando API própria em', this.API_BASE_URL);
  }

  /**
   * Detecta automaticamente a URL da API baseado no ambiente
   */
  private getApiUrl(): string {
    const hostname = window.location.hostname;
    
    console.log('🔍 Hostname detectado:', hostname);
    
    // Se estiver em localhost, usa API local
    if (hostname === 'localhost' || hostname === '127.0.0.1') {
      console.log('🏠 Ambiente LOCAL detectado - usando API local');
      return 'http://localhost:7000/api';
    }
    
    // Produção: usa API no Railway
    console.log('🌐 Ambiente PRODUÇÃO detectado - usando API Railway');
    return 'https://palavraconectada-production.up.railway.app/api';
  }

  // ═══════════════════════════════════════════════════════════════════════════
  // ANÁLISE DE EMOÇÕES
  // ═══════════════════════════════════════════════════════════════════════════

  /**
   * Analisa o texto e detecta a emoção
   * Ex: "Estou triste" → { emotion: "tristeza", confidence: 100 }
   */
  analyzeEmotion(text: string): Observable<EmotionAnalysisResponse> {
    const url = `${this.API_BASE_URL}/Emotion/analyze`;
    return this.http.post<EmotionAnalysisResponse>(url, { text }).pipe(
      catchError(error => {
        console.error('❌ Erro ao analisar emoção:', error);
        return of({
          detectedEmotion: 'neutra',
          confidence: 0,
          message: 'Erro ao detectar emoção',
          recommendationType: '',
          suggestions: [],
          interactionId: 0
        });
      })
    );
  }

  /**
   * Lista todas as emoções disponíveis
   */
  getEmotions(): Observable<Emotion[]> {
    const url = `${this.API_BASE_URL}/Emotion/list`;
    return this.http.get<Emotion[]>(url).pipe(
      catchError(error => {
        console.error('❌ Erro ao buscar emoções:', error);
        return of([]);
      })
    );
  }

  /**
   * Busca sugestões para uma emoção específica
   */
  getSuggestions(emotionName: string): Observable<string[]> {
    const url = `${this.API_BASE_URL}/Emotion/${emotionName}/suggestions`;
    return this.http.get<string[]>(url).pipe(
      catchError(() => of([]))
    );
  }

  // ═══════════════════════════════════════════════════════════════════════════
  // VERSÍCULOS
  // ═══════════════════════════════════════════════════════════════════════════

  /**
   * Busca versículos por palavra-chave
   */
  searchVerses(keyword: string, version: string = 'nvi'): Observable<SearchVerseResponse> {
    const url = `${this.API_BASE_URL}/Verses/search?keyword=${encodeURIComponent(keyword)}&version=${version}`;
    return this.http.get<SearchVerseResponse>(url).pipe(
      catchError(error => {
        console.error('❌ Erro ao buscar versículos:', error);
        return of({ keyword, version, count: 0, verses: [] });
      })
    );
  }

  /**
   * Busca versículos por emoção
   */
  getVersesByEmotion(emotionName: string, version: string = 'nvi', limit: number = 10): Observable<Verse[]> {
    const url = `${this.API_BASE_URL}/Verses/by-emotion/${emotionName}?version=${version}&limit=${limit}`;
    return this.http.get<Verse[]>(url).pipe(
      catchError(() => of([]))
    );
  }

  /**
   * Versículo aleatório
   */
  getRandomVerse(version: string = 'nvi'): Observable<Verse | null> {
    const url = `${this.API_BASE_URL}/Verses/random?version=${version}`;
    return this.http.get<Verse>(url).pipe(
      catchError(() => of(null))
    );
  }

  /**
   * RECOMENDAÇÃO INTELIGENTE - A ESTRELA DO SHOW! ⭐
   * Combina análise de emoção + busca de versículos
   */
  getIntelligentRecommendation(text: string, version: string = 'nvi'): Observable<RecommendationResponse> {
    const url = `${this.API_BASE_URL}/Verses/recommend`;
    return this.http.post<RecommendationResponse>(url, { text, version }).pipe(
      catchError(error => {
        console.error('❌ Erro ao gerar recomendação:', error);
        return of({
          userInput: text,
          detectedEmotion: 'neutra',
          confidence: 0,
          message: 'Erro ao gerar recomendação',
          recommendedVerse: null,
          alternativeVerses: [],
          suggestions: []
        });
      })
    );
  }

  /**
   * Busca histórico de interações
   */
  getHistory(limit: number = 10): Observable<any[]> {
    const url = `${this.API_BASE_URL}/Verses/history?limit=${limit}`;
    return this.http.get<any[]>(url).pipe(
      catchError(() => of([]))
    );
  }
}

