import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable, catchError, of } from 'rxjs';

// Serviço para consumir NOSSA API backend (localhost:7001)
// Agora temos controle total e inteligência de emoções!
@Injectable({
  providedIn: 'root'
})
export class BackendApiService {
  private readonly API_BASE_URL = 'https://localhost:7001/api';
  
  constructor(private http: HttpClient) {
    console.log('🔥 BackendApiService inicializado - Usando API própria!');
  }

  // ═══════════════════════════════════════════════════════════════════════════
  // ANÁLISE DE EMOÇÕES
  // ═══════════════════════════════════════════════════════════════════════════

  /**
   * Analisa o texto e detecta a emoção
   * Ex: "Estou triste" → { emotion: "tristeza", confidence: 100 }
   */
  analyzeEmotion(text: string): Observable<EmotionAnalysisResponse> {
    const url = `${this.API_BASE_URL}/emotion/analyze`;
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
    const url = `${this.API_BASE_URL}/emotion/list`;
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
    const url = `${this.API_BASE_URL}/emotion/${emotionName}/suggestions`;
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
    const url = `${this.API_BASE_URL}/verses/search?keyword=${encodeURIComponent(keyword)}&version=${version}`;
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
  getVersesByEmotion(emotionName: string, version: string = 'nvi', limit: number = 10): Observable<any[]> {
    const url = `${this.API_BASE_URL}/verses/by-emotion/${emotionName}?version=${version}&limit=${limit}`;
    return this.http.get<any[]>(url).pipe(
      catchError(() => of([]))
    );
  }

  /**
   * Versículo aleatório
   */
  getRandomVerse(version: string = 'nvi'): Observable<any> {
    const url = `${this.API_BASE_URL}/verses/random?version=${version}`;
    return this.http.get(url).pipe(
      catchError(() => of(null))
    );
  }

  /**
   * RECOMENDAÇÃO INTELIGENTE - A ESTRELA DO SHOW! ⭐
   * Combina análise de emoção + busca de versículos
   */
  getIntelligentRecommendation(text: string, version: string = 'nvi'): Observable<RecommendationResponse> {
    const url = `${this.API_BASE_URL}/verses/recommend`;
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
}

// ═══════════════════════════════════════════════════════════════════════════
// INTERFACES - Tipagem forte
// ═══════════════════════════════════════════════════════════════════════════

export interface EmotionAnalysisResponse {
  detectedEmotion: string;
  confidence: number;
  message: string;
  recommendationType: string;
  suggestions: string[];
  interactionId: number;
}

export interface Emotion {
  id: number;
  name: string;
  keywords: string;
  description: string;
  recommendationType: string;
}

export interface SearchVerseResponse {
  keyword: string;
  version: string;
  count: number;
  verses: any[];
}

export interface RecommendationResponse {
  userInput: string;
  detectedEmotion: string;
  confidence: number;
  message: string;
  recommendedVerse: any | null;
  alternativeVerses: any[];
  suggestions: string[];
}

