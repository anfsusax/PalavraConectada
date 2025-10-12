# 📊 Comparação Prática: Angular vs Blazor

## 🎯 O Mesmo Problema, Duas Soluções

Este documento mostra **lado a lado** como cada framework resolve os mesmos problemas.

---

## 1️⃣ Declarando uma Variável

### 🅰️ Angular (TypeScript)
```typescript
// No componente .ts
export class VerseSearchComponent {
  searchTerm: string = '';
  isLoading: boolean = false;
  searchResult: SearchResult | null = null;
}
```

### 🔷 Blazor (C#)
```csharp
// No @code do .razor
@code {
    private string searchTerm = string.Empty;
    private bool isLoading = false;
    private SearchResult? searchResult = null;
}
```

**📖 Lição:** Perceba como C# usa `?` para nullable e TypeScript usa `|`

---

## 2️⃣ Vinculando Dados (Data Binding)

### 🅰️ Angular
```html
<!-- Two-way binding -->
<input [(ngModel)]="searchTerm" />

<!-- One-way binding -->
<p>Você digitou: {{ searchTerm }}</p>

<!-- Property binding -->
<button [disabled]="isLoading">Buscar</button>

<!-- Event binding -->
<button (click)="searchVerses()">Buscar</button>
```

### 🔷 Blazor
```razor
<!-- Two-way binding -->
<input @bind="searchTerm" />

<!-- One-way binding -->
<p>Você digitou: @searchTerm</p>

<!-- Property binding -->
<button disabled="@isLoading">Buscar</button>

<!-- Event binding -->
<button @onclick="SearchVerses">Buscar</button>
```

**📖 Lição:** Angular usa `[]` e `()`, Blazor usa `@`

---

## 3️⃣ Condicionais (If/Else)

### 🅰️ Angular
```html
<!-- Se tiver erro -->
<div *ngIf="errorMessage">
  ⚠️ {{ errorMessage }}
</div>

<!-- Se estiver carregando -->
<div *ngIf="isLoading">
  Carregando...
</div>

<!-- Se tiver resultado -->
<div *ngIf="searchResult && searchResult.occurrence > 0">
  Encontrados {{ searchResult.occurrence }} versículos
</div>

<!-- If/Else -->
<div *ngIf="isLoading; else showResults">
  Carregando...
</div>
<ng-template #showResults>
  Resultados aqui
</ng-template>
```

### 🔷 Blazor
```razor
<!-- Se tiver erro -->
@if (!string.IsNullOrEmpty(errorMessage))
{
    <div>
        ⚠️ @errorMessage
    </div>
}

<!-- Se estiver carregando -->
@if (isLoading)
{
    <div>
        Carregando...
    </div>
}

<!-- Se tiver resultado -->
@if (searchResult != null && searchResult.Occurrence > 0)
{
    <div>
        Encontrados @searchResult.Occurrence versículos
    </div>
}

<!-- If/Else -->
@if (isLoading)
{
    <div>Carregando...</div>
}
else
{
    <div>Resultados aqui</div>
}
```

**📖 Lição:** Blazor usa C# puro, Angular usa diretivas especiais

---

## 4️⃣ Loops (Repetições)

### 🅰️ Angular
```html
<!-- Loop básico -->
<div *ngFor="let verse of searchResult.verses">
  <p>{{ verse.text }}</p>
</div>

<!-- Loop com índice -->
<div *ngFor="let verse of searchResult.verses; let i = index">
  <span>{{ i + 1 }}. {{ verse.text }}</span>
</div>

<!-- Loop com tracking (performance) -->
<div *ngFor="let verse of searchResult.verses; trackBy: trackByVerse">
  <p>{{ verse.text }}</p>
</div>
```

```typescript
// No componente
trackByVerse(index: number, verse: Verse): number {
  return verse.number;
}
```

### 🔷 Blazor
```razor
<!-- Loop básico -->
@foreach (var verse in searchResult.Verses)
{
    <div>
        <p>@verse.Text</p>
    </div>
}

<!-- Loop com índice -->
@for (int i = 0; i < searchResult.Verses.Count; i++)
{
    var verse = searchResult.Verses[i];
    <div>
        <span>@(i + 1). @verse.Text</span>
    </div>
}

<!-- Loop com @key (performance) -->
@foreach (var verse in searchResult.Verses)
{
    <div @key="verse.Number">
        <p>@verse.Text</p>
    </div>
}
```

**📖 Lição:** Blazor usa foreach do C#, Angular usa *ngFor

---

## 5️⃣ Chamadas HTTP (API)

### 🅰️ Angular

**Serviço:**
```typescript
// bible-api.service.ts
@Injectable({ providedIn: 'root' })
export class BibleApiService {
  private readonly API_URL = 'https://www.abibliadigital.com.br/api';
  
  constructor(private http: HttpClient) {}
  
  searchVerses(term: string, version: string): Observable<SearchResult> {
    const url = `${this.API_URL}/verses/${version}/search/${term}`;
    return this.http.get<SearchResult>(url).pipe(
      catchError(error => {
        console.error('Erro:', error);
        return of({ occurrence: 0, version: version, verses: [] });
      })
    );
  }
}
```

**Componente:**
```typescript
// verse-search.component.ts
export class VerseSearchComponent {
  searchVerses(): void {
    this.bibleApiService.searchVerses(this.searchTerm, this.version)
      .subscribe({
        next: (result) => {
          this.searchResult = result;
        },
        error: (error) => {
          this.errorMessage = 'Erro ao buscar';
        },
        complete: () => {
          this.isLoading = false;
        }
      });
  }
}
```

### 🔷 Blazor

**Serviço:**
```csharp
// BibleApiService.cs
public class BibleApiService
{
    private readonly HttpClient _httpClient;
    private const string API_URL = "https://www.abibliadigital.com.br/api";
    
    public BibleApiService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }
    
    public async Task<SearchResult?> SearchVersesAsync(
        string term, string version)
    {
        try
        {
            var url = $"{API_URL}/verses/{version}/search/{term}";
            return await _httpClient.GetFromJsonAsync<SearchResult>(url);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Erro: {ex.Message}");
            return new SearchResult();
        }
    }
}
```

**Componente:**
```csharp
// VerseSearch.razor
@code {
    private async Task SearchVerses()
    {
        try
        {
            isLoading = true;
            var result = await BibleApi.SearchVersesAsync(searchTerm, version);
            searchResult = result;
        }
        catch (Exception ex)
        {
            errorMessage = "Erro ao buscar";
        }
        finally
        {
            isLoading = false;
        }
    }
}
```

**📖 Lição:** 
- Angular: Observables + subscribe
- Blazor: async/await (mais simples!)

---

## 6️⃣ Dependency Injection

### 🅰️ Angular

**Registrar serviço:**
```typescript
// app.config.ts
export const appConfig: ApplicationConfig = {
  providers: [
    provideHttpClient(),
    // Serviços com @Injectable são auto-registrados
  ]
};
```

**Injetar no componente:**
```typescript
// verse-search.component.ts
export class VerseSearchComponent {
  constructor(private bibleApiService: BibleApiService) {}
}
```

### 🔷 Blazor

**Registrar serviço:**
```csharp
// Program.cs
builder.Services.AddScoped<BibleApiService>();
builder.Services.AddScoped(sp => new HttpClient());
```

**Injetar no componente:**
```razor
<!-- VerseSearch.razor -->
@inject BibleApiService BibleApi

@code {
    // Automaticamente disponível como 'BibleApi'
}
```

**📖 Lição:** Blazor é mais direto com @inject

---

## 7️⃣ Modelos/Interfaces

### 🅰️ Angular (TypeScript)
```typescript
// verse.model.ts
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

export interface SearchResult {
  occurrence: number;
  version: string;
  verses: Verse[];
}
```

### 🔷 Blazor (C#)
```csharp
// VerseModels.cs
public class Book
{
    public BookAbbrev? Abbrev { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Author { get; set; } = string.Empty;
    public string Group { get; set; } = string.Empty;
    public string Version { get; set; } = string.Empty;
}

public class Verse
{
    public Book? Book { get; set; }
    public int Chapter { get; set; }
    public int Number { get; set; }
    public string Text { get; set; } = string.Empty;
}

public class SearchResult
{
    public int Occurrence { get; set; }
    public string Version { get; set; } = string.Empty;
    public List<Verse> Verses { get; set; } = new();
}
```

**📖 Lição:** 
- TypeScript: interface (sem implementação)
- C#: class com propriedades

---

## 8️⃣ Manipulação de Eventos

### 🅰️ Angular
```html
<!-- Click simples -->
<button (click)="searchVerses()">Buscar</button>

<!-- Com parâmetro -->
<button (click)="useExample('amor')">Amor</button>

<!-- Eventos de teclado -->
<input (keyup.enter)="searchVerses()" />

<!-- Passar evento -->
<input (input)="onInputChange($event)" />
```

```typescript
onInputChange(event: Event): void {
  const target = event.target as HTMLInputElement;
  console.log(target.value);
}
```

### 🔷 Blazor
```razor
<!-- Click simples -->
<button @onclick="SearchVerses">Buscar</button>

<!-- Com parâmetro (lambda) -->
<button @onclick="() => UseExample(\"amor\")">Amor</button>

<!-- Eventos de teclado -->
<input @onkeyup="HandleKeyPress" />

<!-- Passar evento -->
<input @oninput="OnInputChange" />
```

```csharp
@code {
    private void OnInputChange(ChangeEventArgs e)
    {
        var value = e.Value?.ToString();
        Console.WriteLine(value);
    }
    
    private async Task HandleKeyPress(KeyboardEventArgs e)
    {
        if (e.Key == "Enter")
        {
            await SearchVerses();
        }
    }
}
```

**📖 Lição:** 
- Angular: `(event)`
- Blazor: `@onevent`

---

## 9️⃣ CSS Scoped (Estilos Isolados)

### 🅰️ Angular
```typescript
// verse-search.component.ts
@Component({
  selector: 'app-verse-search',
  templateUrl: './verse-search.component.html',
  styleUrls: ['./verse-search.component.css']  // ← CSS isolado
})
```

```css
/* verse-search.component.css */
/* Estes estilos só afetam este componente! */
.verse-card {
  padding: 1rem;
  background: white;
}
```

### 🔷 Blazor
```razor
<!-- VerseSearch.razor.css -->
<!-- Arquivo separado com mesmo nome + .css -->
```

```css
/* VerseSearch.razor.css */
/* Estes estilos só afetam este componente! */
.verse-card {
    padding: 1rem;
    background: white;
}
```

**Ou inline:**
```razor
<style>
    /* Estilos inline no componente */
    .verse-card {
        padding: 1rem;
        background: white;
    }
</style>
```

**📖 Lição:** Ambos suportam CSS isolado automaticamente!

---

## 🔟 Ciclo de Vida

### 🅰️ Angular
```typescript
export class VerseSearchComponent implements OnInit, OnDestroy {
  ngOnInit(): void {
    // Quando componente é criado
    console.log('Componente iniciado');
  }
  
  ngOnDestroy(): void {
    // Quando componente é destruído
    console.log('Componente destruído');
  }
  
  ngOnChanges(changes: SimpleChanges): void {
    // Quando inputs mudam
  }
  
  ngAfterViewInit(): void {
    // Depois que a view é iniciada
  }
}
```

### 🔷 Blazor
```csharp
@code {
    protected override void OnInitialized()
    {
        // Quando componente é criado
        Console.WriteLine("Componente iniciado");
    }
    
    protected override async Task OnInitializedAsync()
    {
        // Versão assíncrona
        await LoadDataAsync();
    }
    
    protected override void OnParametersSet()
    {
        // Quando parâmetros mudam
    }
    
    protected override void OnAfterRender(bool firstRender)
    {
        // Depois que renderiza
        if (firstRender)
        {
            // Primeira renderização
        }
    }
    
    public void Dispose()
    {
        // Quando componente é destruído
        Console.WriteLine("Componente destruído");
    }
}
```

**📖 Lição:** Blazor usa override de métodos, Angular usa interfaces

---

## 📊 Resumo das Diferenças

| Recurso | Angular | Blazor |
|---------|---------|--------|
| **Linguagem** | TypeScript | C# |
| **Binding** | `[(ngModel)]` | `@bind` |
| **Eventos** | `(click)` | `@onclick` |
| **Condicionais** | `*ngIf` | `@if` |
| **Loops** | `*ngFor` | `@foreach` |
| **Interpolação** | `{{ }}` | `@` |
| **Async** | Observables | async/await |
| **DI** | Constructor | `@inject` |
| **Ciclo de vida** | Interfaces | Override |
| **Tipos** | interface | class |

---

## 🎯 Qual Escolher?

### Escolha Angular se:
✅ Você já conhece JavaScript/TypeScript  
✅ Precisa de ecosistema maduro  
✅ Quer programação reativa (RxJS)  
✅ Prefere comunidade maior  

### Escolha Blazor se:
✅ Você já conhece C#/.NET  
✅ Quer usar C# no frontend  
✅ Prefere tipagem mais forte  
✅ Quer integração com backend .NET  

### Aprenda Ambos se:
✅ Você quer ser versátil  
✅ Gosta de comparar abordagens  
✅ Quer entender paradigmas diferentes  
✅ É apaixonado por aprender! 🚀  

---

## 🙏 Versículo de Sabedoria

> **"O coração do sábio inclina-se para a direita, mas o coração do tolo, para a esquerda."**
> 
> *Eclesiastes 10:2*

**Brincadeira!** 😄 Não há "esquerda ou direita" em frameworks - use o que melhor serve seu propósito!

> **"Há tempo para tudo e um momento para cada coisa debaixo do céu."**
> 
> *Eclesiastes 3:1*

Há tempo para Angular, há tempo para Blazor! 🎯

---

*Desenvolvido com ❤️ para ensinar com clareza*

