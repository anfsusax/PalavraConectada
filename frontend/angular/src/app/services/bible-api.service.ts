import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable, map, catchError, of } from 'rxjs';
import { SearchResult } from '../models/verse.model';
import { BibleApiMockService } from './bible-api-mock.service';

// Serviço que busca versículos - como o profeta que busca a palavra de Deus
@Injectable({
  providedIn: 'root'
})
export class BibleApiService {
  private readonly API_BASE_URL = 'https://www.abibliadigital.com.br/api';
  
  // ⚠️ MODO: false = usa NOSSO BACKEND (localhost:7001)
  private readonly USE_MOCK = false; // AGORA temos nosso próprio backend! 🔥
  
  constructor(
    private http: HttpClient,
    private mockService: BibleApiMockService
  ) {
    if (this.USE_MOCK) {
      console.warn('🎭 MODO MOCK ATIVADO - Usando dados de exemplo');
      console.warn('Para usar a API real, altere USE_MOCK = false em bible-api.service.ts');
    }
  }

  // Busca versículos por palavra-chave (POST conforme documentação)
  searchVerses(searchTerm: string, version: string = 'nvi'): Observable<SearchResult> {
    // Se modo mock, usa dados de exemplo
    if (this.USE_MOCK) {
      return this.mockService.searchVerses(searchTerm, version);
    }
    
    // Senão, usa API real
    const url = `${this.API_BASE_URL}/verses/search`;
    const body = {
      version: version,
      search: searchTerm
    };
    
    return this.http.post<SearchResult>(url, body).pipe(
      catchError(error => {
        console.error('❌ Erro ao buscar versículos:', error);
        console.error('Dica: A API pode estar offline. Ative o modo mock!');
        return of({ occurrence: 0, version: version, verses: [] });
      })
    );
  }

  // Busca um versículo específico por referência
  getVerse(version: string, bookAbbrev: string, chapter: number, verse: number): Observable<any> {
    const url = `${this.API_BASE_URL}/verses/${version}/${bookAbbrev}/${chapter}/${verse}`;
    return this.http.get(url);
  }

  // Busca versículo aleatório - como "abrir a Bíblia e deixar Deus falar"
  getRandomVerse(version: string = 'nvi'): Observable<any> {
    if (this.USE_MOCK) {
      return this.mockService.getRandomVerse(version);
    }
    
    const url = `${this.API_BASE_URL}/verses/${version}/random`;
    return this.http.get(url).pipe(
      catchError(error => {
        console.error('❌ Erro ao buscar versículo aleatório:', error);
        return this.mockService.getRandomVerse(version);
      })
    );
  }

  // Lista todas as versões disponíveis
  getVersions(): Observable<any> {
    const url = `${this.API_BASE_URL}/versions`;
    return this.http.get(url);
  }
}

