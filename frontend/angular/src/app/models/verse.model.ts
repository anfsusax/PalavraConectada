// Modelo que representa um versículo bíblico
// Como as tábuas da lei: estrutura clara e definida

// ═══════════════════════════════════════════════════════════════════════════
// MODELOS DA NOSSA API BACKEND
// ═══════════════════════════════════════════════════════════════════════════

/**
 * Versículo da nossa API backend
 */
export interface Verse {
  id: number;
  bookName: string;
  bookAbbrev: string;
  author: string;
  group: string;
  testament: string; // VT ou NT
  chapter: number;
  number: number;
  text: string;
  version: string;
  createdAt: string;
}

/**
 * Resposta da busca de versículos
 */
export interface SearchVerseResponse {
  keyword: string;
  version: string;
  count: number;
  verses: Verse[];
}

/**
 * Emoção detectada
 */
export interface Emotion {
  id: number;
  name: string;
  keywords: string;
  description: string;
  recommendationType: string;
}

/**
 * Resposta da análise de emoção
 */
export interface EmotionAnalysisResponse {
  detectedEmotion: string;
  confidence: number;
  message: string;
  recommendationType: string;
  suggestions: string[];
  interactionId: number;
}

/**
 * Recomendação inteligente completa
 */
export interface RecommendationResponse {
  userInput: string;
  detectedEmotion: string;
  confidence: number;
  message: string;
  recommendedVerse: Verse | null;
  alternativeVerses: Verse[];
  suggestions: string[];
}

// ═══════════════════════════════════════════════════════════════════════════
// MODELOS LEGADOS (API Brasileira)
// ═══════════════════════════════════════════════════════════════════════════

export interface Book {
  abbrev: { pt: string; en: string };
  name: string;
  author: string;
  group: string;
  version: string;
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
  verses: any[];
}

