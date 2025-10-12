import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable, map, catchError, of } from 'rxjs';
import { SearchResult } from '../models/verse.model';

// Serviço que busca versículos - como o profeta que busca a palavra de Deus
@Injectable({
  providedIn: 'root'
})
export class BibleApiService {
  private readonly API_BASE_URL = 'https://www.abibliadigital.com.br/api';
  
  constructor(private http: HttpClient) {}

  // Busca versículos por palavra-chave
  searchVerses(searchTerm: string, version: string = 'nvi'): Observable<SearchResult> {
    const url = `${this.API_BASE_URL}/verses/${version}/search/${encodeURIComponent(searchTerm)}`;
    
    return this.http.get<SearchResult>(url).pipe(
      catchError(error => {
        console.error('Erro ao buscar versículos:', error);
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
    const url = `${this.API_BASE_URL}/verses/${version}/random`;
    return this.http.get(url);
  }

  // Lista todas as versões disponíveis
  getVersions(): Observable<any> {
    const url = `${this.API_BASE_URL}/versions`;
    return this.http.get(url);
  }
}

