import { Component } from '@angular/core';
import { RouterOutlet } from '@angular/router';
import { VerseSearchComponent } from './components/verse-search/verse-search.component';

// Componente raiz da aplicação
@Component({
  selector: 'app-root',
  imports: [RouterOutlet, VerseSearchComponent],
  templateUrl: './app.html',
  styleUrl: './app.css'
})
export class App {
  title = 'Palavra Conectada';
}
