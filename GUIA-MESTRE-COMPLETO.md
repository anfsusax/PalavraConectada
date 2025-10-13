# 📖 GUIA MESTRE COMPLETO - JORNADA DO DESENVOLVEDOR
## Do Iniciante ao Expert em Arquitetura, Angular, Blazor e .NET

**Desenvolvido por Alex Feitoza** 💻  
*"Como Salomão construiu o Templo com sabedoria, construiremos sistemas com excelência"*

---

## 🎯 ÍNDICE DA JORNADA

1. [Fundamentos: O Alicerce](#1-fundamentos-o-alicerce)
2. [Arquitetura de Software: O Projeto do Templo](#2-arquitetura-de-software)
3. [Backend .NET: As Colunas do Templo](#3-backend-net)
4. [Frontend Angular: O Átrio Exterior](#4-frontend-angular)
5. [Frontend Blazor: O Lugar Santo](#5-frontend-blazor)
6. [Banco de Dados: O Fundamento de Pedra](#6-banco-de-dados)
7. [APIs REST: Os Mensageiros](#7-apis-rest)
8. [Deploy: Levantando o Edifício](#8-deploy)
9. [Git: O Livro das Crônicas](#9-git)
10. [Exercícios Práticos: Construindo Sua Torre](#10-exercicios-praticos)

---

# 1. FUNDAMENTOS: O ALICERCE

> *"Todo aquele que ouve estas minhas palavras e as pratica é semelhante a um homem prudente que edificou a sua casa sobre a rocha."* - Mateus 7:24

## 1.1 O Que É Desenvolvimento de Software?

**Como Noé construiu a Arca**, nós construímos sistemas:
- 🎯 **Planejamento** - Deus deu as especificações (Gênesis 6:14-16)
- 🔨 **Execução** - Noé executou com precisão
- ✅ **Testes** - A arca foi testada pelo dilúvio
- 🎉 **Entrega** - Salvou vidas!

### Analogia do Projeto PalavraConectada:

```
Noé construiu a Arca              →  Nós construímos o PalavraConectada
├─ Madeira de Gofer               →  Linguagens (C#, TypeScript)
├─ Três andares                   →  Três camadas (Backend, Angular, Blazor)
├─ Porta lateral                  →  APIs REST
└─ Janela no teto                 →  Interface do usuário
```

## 1.2 As 7 Linguagens que Você Dominou

### 1. **C# (Backend)**
```csharp
// Como o Hebraico do Antigo Testamento - forte e estruturado
public class Verse
{
    public string Text { get; set; }  // Propriedade
    public string Book { get; set; }
}
```

**Analogia Bíblica:**  
C# é como os **Levitas** - organizados, com regras claras, cada um sabe seu papel.

### 2. **TypeScript (Angular)**
```typescript
// Como o Grego do Novo Testamento - preciso e expressivo
interface Verse {
  text: string;  // Tipagem forte
  book: string;
}
```

**Analogia Bíblica:**  
TypeScript é como **Paulo escrevendo cartas** - cada palavra tem tipo e significado preciso.

### 3. **HTML (Estrutura)**
```html
<!-- Como a estrutura do Tabernáculo -->
<div class="arca">
  <div class="lugar-santissimo">
    <h1>Conteúdo Sagrado</h1>
  </div>
</div>
```

### 4. **CSS (Aparência)**
```css
/* Como as cortinas bordadas do Tabernáculo */
.arca {
  background: linear-gradient(gold, white);
  border: 2px solid gold;
}
```

### 5. **SQL (Banco de Dados)**
```sql
-- Como o Livro da Lei de Moisés - guardado e consultado
SELECT text FROM Verses WHERE book = 'João';
```

### 6. **JSON (Comunicação)**
```json
// Como as mensagens dos profetas - estruturadas e claras
{
  "verse": "João 3:16",
  "text": "Porque Deus amou..."
}
```

### 7. **YAML (Configuração)**
```yaml
# Como as instruções de construção do Templo
build:
  steps:
    - restore
    - build
    - test
```

---

# 2. ARQUITETURA DE SOFTWARE: O PROJETO DO TEMPLO

> *"Vê que faças tudo segundo o modelo que te foi mostrado no monte."* - Êxodo 25:40

## 2.1 Clean Architecture (Arquitetura Limpa)

**Como o Templo de Salomão** tinha divisões claras:

```
TEMPLO DE SALOMÃO              →  NOSSA ARQUITETURA
├─ Átrio Exterior              →  Frontend (Angular/Blazor)
│  └─ Onde o povo entrava      →  Interface do usuário
│
├─ Lugar Santo                 →  API/Controllers
│  └─ Serviço dos sacerdotes   →  Processamento de requests
│
├─ Lugar Santíssimo            →  Domain/Business Logic
│  └─ A Arca da Aliança        →  Regras de negócio
│
└─ Fundamento                  →  Banco de Dados
   └─ Pedras grandes           →  Persistência
```

### 2.2 Camadas da Arquitetura

#### **Camada 1: Apresentação (UI)**
```
Como o Átrio - Onde todos veem
├─ Angular (Átrio dos Gentios)
└─ Blazor (Átrio de Israel)
```

**Responsabilidade:** Mostrar informações, receber input do usuário

#### **Camada 2: API (Controllers)**
```
Como os Sacerdotes - Mediam entre povo e Deus
├─ VersesController (Sacerdote dos Versículos)
├─ EmotionController (Sacerdote das Emoções)
└─ AdminController (Sumo Sacerdote)
```

**Responsabilidade:** Receber requisições, validar, direcionar

#### **Camada 3: Serviços (Business Logic)**
```
Como os Levitas - Fazem o trabalho especializado
├─ BibleService (Conhecedor das Escrituras)
├─ EmotionAnalyzerService (Discernidor de espíritos)
└─ BibleMigrationService (Escriba)
```

**Responsabilidade:** Lógica de negócio, processamento

#### **Camada 4: Dados (Repository)**
```
Como a Arca da Aliança - Guarda o que é sagrado
├─ BibleDbContext (Contexto das Escrituras)
└─ SQLite (As Tábuas da Lei)
```

**Responsabilidade:** Persistir e recuperar dados

### 2.3 Fluxo de Uma Requisição

**Como uma Oração chega a Deus:**

```
1. USUÁRIO faz pedido
   └─ "Preciso de versículo sobre amor"
   
2. FRONTEND (Angular/Blazor)
   └─ Como o adorador no átrio
   └─ Prepara a requisição
   
3. API CONTROLLER
   └─ Como o sacerdote
   └─ Recebe e valida
   
4. SERVICE (BibleService)
   └─ Como o levita
   └─ Processa a lógica
   
5. REPOSITORY (DbContext)
   └─ Como o guardião da arca
   └─ Busca no banco
   
6. RESPOSTA volta pelo mesmo caminho
   └─ Como a resposta de Deus
   └─ JSON com versículos
```

## 2.4 Princípios SOLID

**Como os 10 Mandamentos - Regras fundamentais:**

### S - Single Responsibility (Responsabilidade Única)
```csharp
// ERRADO - Fazendo tudo (como Moisés sozinho julgando)
public class Verse
{
    public void Save() { }
    public void SendEmail() { }
    public void Log() { }
}

// CERTO - Uma responsabilidade (como os 70 anciãos)
public class Verse { }  // Só representa dados
public class VerseRepository { }  // Só salva
public class EmailService { }  // Só envia email
```

**Analogia:** Êxodo 18 - Jetro aconselha Moisés a **delegar**

### O - Open/Closed (Aberto/Fechado)
```csharp
// Como a Lei - não muda, mas pode ser interpretada
public interface IEmotionAnalyzer
{
    string Analyze(string text);
}

// Implementações diferentes sem mudar a interface
public class SimpleAnalyzer : IEmotionAnalyzer { }
public class AdvancedAnalyzer : IEmotionAnalyzer { }
```

### L - Liskov Substitution
```csharp
// Qualquer versão da Bíblia deve funcionar
public abstract class BibleVersion
{
    public abstract Verse GetVerse(string reference);
}

public class NVI : BibleVersion { }  // Substitui sem problemas
public class ACF : BibleVersion { }  // Substitui sem problemas
```

### I - Interface Segregation
```csharp
// ERRADO - Interface gorda (como exigir que todos sejam profetas)
public interface IBibleWorker
{
    void Read();
    void Write();
    void Preach();
    void Heal();
}

// CERTO - Interfaces específicas
public interface IReader { void Read(); }
public interface IWriter { void Write(); }
public interface IPreacher { void Preach(); }
```

### D - Dependency Inversion
```csharp
// Depender de abstrações, não de implementações
// Como depender de Deus, não de ídolos

// ERRADO
public class VersesController
{
    private MySqlDatabase db = new MySqlDatabase();  // Depende de concreto
}

// CERTO
public class VersesController
{
    private IDatabase db;  // Depende de abstração
    
    public VersesController(IDatabase database)
    {
        db = database;  // Injeção de dependência
    }
}
```

**Analogia:** Não construa sobre areia (concreto), construa sobre rocha (abstração)

---

# 3. BACKEND .NET: AS COLUNAS DO TEMPLO

> *"Fez também duas colunas... uma se chamava Jaquim, e a outra Boaz."* - 1 Reis 7:21

## 3.1 O Que é .NET?

**.NET é como o Sistema do Templo:**
- 🏛️ **Framework** - A estrutura do templo
- 📚 **Libraries** - As ferramentas dos levitas
- ⚙️ **Runtime** - O sistema que mantém tudo funcionando

### 3.2 Estrutura do Projeto .NET

```
PalavraConectada.API/
├─ Controllers/          → Sacerdotes (recebem pedidos)
│  ├─ VersesController
│  ├─ EmotionController
│  └─ AdminController
│
├─ Services/            → Levitas (fazem o trabalho)
│  ├─ BibleService
│  ├─ EmotionAnalyzerService
│  └─ BibleMigrationService
│
├─ Models/              → Tábuas da Lei (definições)
│  ├─ Verse.cs
│  └─ DTOs.cs
│
├─ Data/                → Arca (persistência)
│  ├─ BibleDbContext
│  └─ SeedData
│
└─ Program.cs           → Fundação (configuração)
```

## 3.3 Entendendo o Program.cs

**Como Gênesis 1 - A Criação em ordem:**

```csharp
// DIA 1 - Haja a aplicação!
var builder = WebApplication.CreateBuilder(args);

// DIA 2 - Adicionar serviços (criar os céus)
builder.Services.AddControllers();
builder.Services.AddSwagger();

// DIA 3 - Banco de dados (separar terra e águas)
builder.Services.AddDbContext<BibleDbContext>();

// DIA 4 - CORS (criar luminares)
builder.Services.AddCors();

// DIA 5 - Serviços customizados (criar vida)
builder.Services.AddScoped<BibleService>();

// DIA 6 - Build da aplicação (criar o homem)
var app = builder.Build();

// DIA 7 - Executar (descansar e ver que era bom)
await app.RunAsync();
```

### 3.4 Controllers - Os Sacerdotes

```csharp
[ApiController]
[Route("api/[controller]")]
public class VersesController : ControllerBase
{
    // Como um sacerdote tem instrumentos
    private readonly BibleService _bibleService;
    
    // Construtor - Recebe as ferramentas (Dependency Injection)
    public VersesController(BibleService bibleService)
    {
        _bibleService = bibleService;
    }
    
    // Endpoint - Como um ritual específico
    [HttpGet("random")]
    public async Task<ActionResult<Verse>> GetRandom()
    {
        // 1. Orar (logar)
        _logger.LogInformation("Buscando versículo aleatório");
        
        // 2. Consultar as escrituras
        var verse = await _bibleService.GetRandomVerseAsync();
        
        // 3. Responder ao povo
        return Ok(verse);
    }
}
```

**Analogia:** Cada endpoint é como um **tipo de sacrifício** diferente:
- `GET /random` → Oferta de manjares (aleatória, espontânea)
- `POST /search` → Holocausto (busca intencional)
- `GET /by-emotion` → Oferta pacífica (encontrar paz)

### 3.5 Services - Os Levitas

```csharp
public class BibleService
{
    private readonly BibleDbContext _context;
    
    // Método - Como uma função dos levitas
    public async Task<List<Verse>> SearchVersesAsync(string keyword)
    {
        // 1. Ir à arca (banco de dados)
        var verses = await _context.Verses
            // 2. Procurar nas tábuas
            .Where(v => v.Text.Contains(keyword))
            // 3. Trazer os primeiros 10
            .Take(10)
            // 4. Devolver ao sacerdote
            .ToListAsync();
            
        return verses;
    }
}
```

### 3.6 Entity Framework - O Escriba

**Como Esdras organizava as Escrituras:**

```csharp
// DbContext - Como o rolo das Escrituras
public class BibleDbContext : DbContext
{
    // Cada DbSet é como um livro da Bíblia
    public DbSet<Verse> Verses { get; set; }
    public DbSet<Emotion> Emotions { get; set; }
    
    // OnModelCreating - Como definir a gramática hebraica
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // Configurar relações
        modelBuilder.Entity<VerseEmotion>()
            .HasKey(ve => new { ve.VerseId, ve.EmotionId });
    }
}
```

### 3.7 Async/Await - A Promessa

```csharp
// Síncrono - Como Moisés descendo do monte
// (O povo espera parado)
public List<Verse> GetVerses()
{
    return _context.Verses.ToList();  // Trava tudo
}

// Assíncrono - Como enviar mensageiros
// (O rei continua governando enquanto espera resposta)
public async Task<List<Verse>> GetVersesAsync()
{
    // await = "vou esperar a promessa se cumprir"
    return await _context.Verses.ToListAsync();  // Não trava!
}
```

**Analogia:** Abraão recebeu uma **promessa** (Task) que se **cumpriu** depois (await)

---

# 4. FRONTEND ANGULAR: O ÁTRIO EXTERIOR

> *"E farás o átrio do tabernáculo..."* - Êxodo 27:9

## 4.1 O Que é Angular?

**Angular é como o Átrio do Templo:**
- 👥 **Onde o povo interage**
- 🎨 **Bonito e organizado**
- 📱 **Acessível a todos**
- 🔄 **Reativo** (muda conforme necessidade)

## 4.2 Estrutura do Projeto Angular

```
src/
├─ app/
│  ├─ components/        → Cada móvel do átrio
│  │  ├─ home/          → Entrada principal
│  │  └─ verses/        → Altar dos versículos
│  │
│  ├─ services/          → Mensageiros (comunicação com API)
│  │  └─ backend-api.service.ts
│  │
│  ├─ models/            → Definições (como era feito)
│  │  └─ verse.model.ts
│  │
│  └─ app.component.ts   → Componente raiz (a estrutura toda)
│
├─ index.html            → Porta de entrada
└─ main.ts              → Ponto de partida
```

## 4.3 Components - As Peças do Átrio

```typescript
// Component = Uma peça do átrio
@Component({
  selector: 'app-verses',      // Nome no HTML
  templateUrl: './verses.component.html',  // Como é visto
  styleUrls: ['./verses.component.css']    // Como é decorado
})
export class VersesComponent implements OnInit {
  // Propriedades - Como o bronze e ouro
  verses: Verse[] = [];
  loading = false;
  
  // Construtor - Recebe ferramentas
  constructor(private apiService: BackendApiService) {}
  
  // ngOnInit - Quando o móvel é colocado no átrio
  ngOnInit(): void {
    this.loadVerses();
  }
  
  // Método - Ação que o usuário pode fazer
  loadVerses(): void {
    // Mostrar que está trabalhando
    this.loading = true;
    
    // Pedir ao mensageiro (service)
    this.apiService.getRandomVerse().subscribe({
      next: (verse) => {
        // Recebeu resposta!
        this.verses.push(verse);
        this.loading = false;
      },
      error: (err) => {
        // Algo deu errado
        console.error('Erro:', err);
        this.loading = false;
      }
    });
  }
}
```

**Analogia:** Cada component é como um **móvel do tabernáculo:**
- `HomeComponent` → Porta de entrada
- `VersesComponent` → Altar de bronze
- `EmotionComponent` → Pia de bronze (lavar emoções)

## 4.4 Services - Os Mensageiros

```typescript
// Service = Como os mensageiros do rei
@Injectable({
  providedIn: 'root'  // Disponível em todo reino
})
export class BackendApiService {
  private API_URL = 'https://api.palavraconectada.app';
  
  constructor(private http: HttpClient) {}
  
  // Método - Enviar mensageiro buscar versículo
  getRandomVerse(): Observable<Verse> {
    // Observable = Promessa que pode retornar várias vezes
    return this.http.get<Verse>(`${this.API_URL}/verses/random`);
  }
  
  // Método - Enviar mensageiro analisar emoção
  analyzeEmotion(text: string): Observable<EmotionResult> {
    return this.http.post<EmotionResult>(
      `${this.API_URL}/emotion/analyze`,
      { text }  // Corpo da mensagem
    );
  }
}
```

## 4.5 Templates (HTML) - A Aparência

```html
<!-- Template = Como o átrio é visto -->
<div class="atrio">
  <!-- Estrutura condicional - Mostra conforme estado -->
  <div *ngIf="loading">
    <p>🔍 Buscando versículo... (Como buscar nas escrituras)</p>
  </div>
  
  <div *ngIf="!loading && verses.length > 0">
    <!-- Loop - Para cada versículo -->
    <div *ngFor="let verse of verses" class="verse-card">
      <!-- Interpolação - Mostrar dados -->
      <h3>{{ verse.book }} {{ verse.chapter }}:{{ verse.verse }}</h3>
      <p>{{ verse.text }}</p>
      
      <!-- Event binding - Quando clicado -->
      <button (click)="shareVerse(verse)">
        Compartilhar
      </button>
    </div>
  </div>
</div>
```

**Diretivas Angular (Como mandamentos):**
- `*ngIf` → "SE isto for verdade, mostra" (condicional)
- `*ngFor` → "PARA CADA item, faz isto" (repetição)
- `(click)` → "QUANDO clicar, faz isto" (evento)
- `{{}}` → "MOSTRA este valor" (interpolação)

## 4.6 Data Binding - A Conexão

```typescript
// Property Binding - Passar valor do código para template
<img [src]="imagemUrl">  // [] = Uma via (código → template)

// Event Binding - Template avisa código
<button (click)="salvar()">  // () = Uma via (template → código)

// Two-Way Binding - Os dois se comunicam
<input [(ngModel)]="nome">  // [()] = Duas vias (ambos sincronizados)
```

**Analogia:** Como **Moisés mediava** entre Deus e povo:
- `[]` → Deus fala ao povo (Property)
- `()` → Povo fala a Deus (Event)
- `[()]` → Conversa contínua (Two-way)

## 4.7 RxJS e Observables - Os Vigilantes

```typescript
// Observable = Como os vigias na torre
// Ficam observando e avisam quando algo acontece

// Criar observable
const verseStream$ = this.apiService.getRandomVerse();

// Subscribe = Colocar vigia
verseStream$.subscribe({
  next: (verse) => {
    console.log('Versículo chegou!', verse);
  },
  error: (err) => {
    console.log('Problema no caminho!', err);
  },
  complete: () => {
    console.log('Mensageiro voltou!');
  }
});

// Operators - Transformar mensagens
this.apiService.searchVerses(keyword).pipe(
  map(verses => verses.slice(0, 5)),    // Pegar primeiros 5
  filter(verses => verses.length > 0),   // Só se tiver resultados
  tap(verses => console.log(verses))     // Espiar sem modificar
).subscribe(/* ... */);
```

**Analogia:** Observables são como os **atalaias** (vigias):
- Ezequiel 3:17 - "Filho do homem, eu te dei por atalaia"
- Observam e avisam quando algo acontece
- Podem ser cancelados (unsubscribe)

---

# 5. FRONTEND BLAZOR: O LUGAR SANTO

> *"E farás o tabernáculo de dez cortinas..."* - Êxodo 26:1

## 5.1 O Que é Blazor?

**Blazor é como o Lugar Santo:**
- 🕯️ **Mesa dos pães** → Components
- 🔥 **Altar de incenso** → Two-way binding
- 🕎 **Candelabro** → Razor syntax (ilumina o código)

**Diferença de Angular:**
- Angular = **JavaScript/TypeScript** (linguagem dos gentios)
- Blazor = **C#** (mesma língua do backend - língua dos sacerdotes)

## 5.2 Estrutura Blazor

```
blazor/
├─ Pages/               → As salas do lugar santo
│  ├─ Home.razor       → Entrada
│  └─ Verses.razor     → Sala dos versículos
│
├─ Components/          → Móveis sagrados
│  ├─ VerseCard.razor
│  └─ EmotionAnalyzer.razor
│
├─ Services/            → Levitas especializados
│  └─ BackendApiService.cs
│
└─ Program.cs          → Configuração do lugar santo
```

## 5.3 Razor Pages - As Cortinas Bordadas

```razor
@page "/verses"
@inject BackendApiService ApiService

<!-- HTML + C# misturados! -->
<div class="lugar-santo">
    <h1>Versículos</h1>
    
    @* Código C# dentro do HTML! *@
    @if (loading)
    {
        <p>🔍 Buscando...</p>
    }
    else if (verses.Any())
    {
        @foreach (var verse in verses)
        {
            <div class="verse-card">
                <h3>@verse.Book @verse.Chapter:@verse.Number</h3>
                <p>@verse.Text</p>
                
                <!-- Event handler direto em C# -->
                <button @onclick="() => Share(verse)">
                    Compartilhar
                </button>
            </div>
        }
    }
</div>

@code {
    // Código C# puro!
    private List<Verse> verses = new();
    private bool loading = false;
    
    // Quando componente carrega
    protected override async Task OnInitializedAsync()
    {
        await LoadVerses();
    }
    
    // Método assíncrono
    private async Task LoadVerses()
    {
        loading = true;
        verses = await ApiService.GetRandomVerseAsync();
        loading = false;
    }
    
    // Compartilhar versículo
    private void Share(Verse verse)
    {
        // Lógica em C#!
        Console.WriteLine($"Compartilhando: {verse.Text}");
    }
}
```

**Analogia:** Razor é como as **cortinas do tabernáculo:**
- HTML = Linho fino (estrutura)
- C# = Ouro bordado (funcionalidade)
- Tudo entrelaçado perfeitamente!

## 5.4 Component Parameters - Passando o Bastão

```razor
<!-- Component pai -->
<VerseCard Verse="@verseAtual" OnShare="HandleShare" />

<!-- Component filho (VerseCard.razor) -->
<div class="card">
    <h3>@Verse.Book</h3>
    <button @onclick="() => OnShare.InvokeAsync(Verse)">
        Compartilhar
    </button>
</div>

@code {
    // Recebe dados do pai (como Moisés recebe de Deus)
    [Parameter]
    public Verse Verse { get; set; } = new();
    
    // Callback para o pai (como Moisés responde a Deus)
    [Parameter]
    public EventCallback<Verse> OnShare { get; set; }
}
```

**Analogia:** Como **Elias passou o manto para Eliseu:**
- `[Parameter]` = Receber o manto (dados do pai)
- `EventCallback` = Usar o manto (avisar o pai)

## 5.5 Dependency Injection - O Suprimento

```csharp
// Program.cs - Configurar suprimentos
builder.Services.AddScoped<BackendApiService>();
builder.Services.AddScoped<AuthService>();

// Component - Receber suprimentos
@inject BackendApiService ApiService
@inject NavigationManager Navigation

@code {
    // Usar os suprimentos
    private async Task Search()
    {
        var results = await ApiService.SearchAsync(keyword);
    }
}
```

**Analogia:** Como **o maná caía do céu:**
- Deus provia (DI container)
- Povo recebia (inject)
- Usavam conforme necessidade

## 5.6 State Management - A Memória

```csharp
// Estado local - Como memória pessoal
@code {
    private string searchTerm = "";
    private List<Verse> results = new();
}

// Estado compartilhado - Como a memória coletiva
public class AppState
{
    public event Action? OnChange;
    
    private List<Verse> favorites = new();
    
    public void AddFavorite(Verse verse)
    {
        favorites.Add(verse);
        NotifyStateChanged();
    }
    
    private void NotifyStateChanged() => OnChange?.Invoke();
}
```

**Analogia:** Como o **testemunho de Israel:**
- Estado local = Memória pessoal (cada tribo)
- Estado global = Arca do testemunho (para todos)

---

# 6. BANCO DE DADOS: O FUNDAMENTO DE PEDRA

> *"Fez-se todo o trabalho do tabernáculo... conforme o SENHOR ordenara."* - Êxodo 39:42

## 6.1 O Que é Um Banco de Dados?

**Como o Lugar Santíssimo guardava a Arca:**
- 📖 **Tábuas da Lei** → Dados estruturados
- 🏺 **Maná** → Cache/dados temporários  
- 🌿 **Vara de Arão** → Logs/histórico

## 6.2 SQLite - As Tábuas de Pedra

```sql
-- Criar tabela (como esculpir nas pedras)
CREATE TABLE Verses (
    Id INTEGER PRIMARY KEY,
    Book TEXT NOT NULL,
    Chapter INTEGER,
    Number INTEGER,
    Text TEXT,
    Version TEXT
);

-- Inserir (como escrever nas tábuas)
INSERT INTO Verses (Book, Chapter, Number, Text, Version)
VALUES ('João', 3, 16, 'Porque Deus amou...', 'NVI');

-- Buscar (como ler das tábuas)
SELECT * FROM Verses 
WHERE Book = 'João' 
  AND Chapter = 3;

-- Atualizar (como revisar)
UPDATE Verses 
SET Text = 'Texto corrigido'
WHERE Id = 1;

-- Deletar (como apagar)
DELETE FROM Verses WHERE Id = 1;
```

**As 4 Operações (CRUD):**
- **C**reate → Criar (INSERT)
- **R**ead → Ler (SELECT)
- **U**pdate → Atualizar (UPDATE)
- **D**elete → Deletar (DELETE)

## 6.3 Entity Framework - O Escriba Automático

```csharp
// Modelo - Como definir o formato das tábuas
public class Verse
{
    public int Id { get; set; }
    public string Book { get; set; }
    public int Chapter { get; set; }
    public int Number { get; set; }
    public string Text { get; set; }
}

// DbContext - Como o guardião da arca
public class BibleDbContext : DbContext
{
    public DbSet<Verse> Verses { get; set; }
}

// Usar - Código C# ao invés de SQL!
var verses = await context.Verses
    .Where(v => v.Book == "João")
    .OrderBy(v => v.Chapter)
    .ToListAsync();

// Entity Framework traduz para SQL automaticamente!
// SELECT * FROM Verses WHERE Book = 'João' ORDER BY Chapter
```

**Analogia:** EF é como **Esdras, o escriba:**
- Você fala em hebraico (C#)
- Ele escreve nas tábuas (SQL)
- Traz de volta em hebraico (C#)

## 6.4 Relationships - Os Relacionamentos

```csharp
// Um para Muitos - Como 12 tribos de Israel
public class Book
{
    public int Id { get; set; }
    public string Name { get; set; }
    public List<Verse> Verses { get; set; }  // Um livro tem muitos versículos
}

public class Verse
{
    public int Id { get; set; }
    public int BookId { get; set; }
    public Book Book { get; set; }  // Cada versículo pertence a um livro
}

// Muitos para Muitos - Como os 12 apóstolos e suas missões
public class Verse
{
    public List<VerseEmotion> VerseEmotions { get; set; }
}

public class Emotion
{
    public List<VerseEmotion> VerseEmotions { get; set; }
}

public class VerseEmotion  // Tabela de junção
{
    public int VerseId { get; set; }
    public Verse Verse { get; set; }
    
    public int EmotionId { get; set; }
    public Emotion Emotion { get; set; }
}
```

**Analogia das Relações:**
- 1:N → Abraão:Descendentes (um pai, muitos filhos)
- N:M → Apóstolos:Cidades (muitos apóstolos, muitas cidades)

---

# 7. APIS REST: OS MENSAGEIROS

> *"Como as pernas de um coxo... assim é o provérbio na boca dos tolos."* - Provérbios 26:7

(Uma API mal feita é como um mensageiro coxo!)

## 7.1 O Que é uma API REST?

**API REST é como o sistema de mensageiros do rei:**
- 📨 **Request** → Mensagem enviada
- 📬 **Response** → Resposta recebida
- 🛣️ **Endpoint** → Cidade destino
- 📦 **JSON** → Formato da mensagem

## 7.2 HTTP Methods - Os Tipos de Mensagens

```
Método   | Ação                    | Analogia Bíblica
---------|-------------------------|---------------------------
GET      | Buscar (ler)           | Ler as Escrituras
POST     | Criar (adicionar)      | Escrever novo livro
PUT      | Atualizar (completar)  | Revisar todo o livro
PATCH    | Atualizar (parcial)    | Corrigir um versículo
DELETE   | Deletar (remover)      | Apagar da memória
```

**Exemplo Prático:**

```http
# GET - Buscar versículo
GET /api/verses/random
Response: { "book": "João", "text": "..." }

# POST - Criar análise de emoção
POST /api/emotion/analyze
Body: { "text": "Estou triste" }
Response: { "emotion": "tristeza", "confidence": 95 }

# PUT - Atualizar versículo completo
PUT /api/verses/1
Body: { "id": 1, "book": "João", "chapter": 3, ... }

# DELETE - Remover versículo
DELETE /api/verses/1
Response: 204 No Content
```

## 7.3 Status Codes - As Respostas

```
Código  | Significado              | Analogia
--------|--------------------------|---------------------------
200     | OK (sucesso)            | "Bem-aventurado!" (Mateus 5)
201     | Created (criado)        | "Haja luz!" (Gênesis 1)
204     | No Content (sem corpo)  | "Silêncio no céu" (Apocalipse 8:1)
400     | Bad Request (pedido ruim)| "Não tentarás o Senhor" (Mateus 4:7)
401     | Unauthorized (não autorizado)| "Não conheceis nem a mim" (João 8:19)
403     | Forbidden (proibido)    | "Não toques" (Gênesis 3:3)
404     | Not Found (não encontrado)| "Buscaram mas não acharam" (João 7:34)
500     | Server Error (erro servidor)| "Clamavam mas não respondeu" (Salmo 18:41)
```

## 7.4 REST Principles - Os Princípios

### 1. Stateless (Sem Estado)
```
Cada requisição é independente
Como orar - cada oração é completa em si
```

### 2. Client-Server (Cliente-Servidor)
```
Separação clara de responsabilidades
Como rei e profeta - cada um tem seu papel
```

### 3. Cacheable (Cacheável)
```
Respostas podem ser guardadas
Como memorizar versículos
```

### 4. Uniform Interface (Interface Uniforme)
```
Padrão consistente
Como as festas de Israel - sempre no mesmo formato
```

## 7.5 JSON - A Linguagem Universal

```json
{
  "verse": {
    "book": "João",
    "chapter": 3,
    "number": 16,
    "text": "Porque Deus amou o mundo...",
    "tags": ["salvação", "amor", "vida eterna"],
    "emotions": [
      {
        "name": "amor",
        "intensity": 10
      }
    ]
  }
}
```

**Estrutura JSON:**
- `{}` → Objeto (como um pergaminho completo)
- `[]` → Array (como lista de nomes)
- `"key": "value"` → Par chave-valor (como índice)

---

# 8. DEPLOY: LEVANTANDO O EDIFÍCIO

> *"E levantou Moisés o tabernáculo..."* - Êxodo 40:18

## 8.1 O Que é Deploy?

**Deploy é como erguer o Tabernáculo:**
- 🏗️ **Desenvolvimento** → Construir as peças (local)
- 📦 **Build** → Preparar para transporte
- 🚚 **Deploy** → Levar ao local definitivo
- ⛺ **Produção** → Montar e deixar funcionando

## 8.2 Os Três Pilares do Deploy

```
NOSSA APLICAÇÃO        →  TABERNÁCULO DE MOISÉS
├─ Backend (Railway)   →  Lugar Santíssimo
├─ Angular (Vercel)    →  Átrio dos Gentios
└─ Blazor (Azure)      →  Átrio de Israel
```

### 8.3 Railway (Backend .NET)

**Railway é como o Lugar Santíssimo - sustenta tudo:**

```yaml
# Dockerfile - Instruções de construção
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY . .
RUN dotnet build -c Release

FROM mcr.microsoft.com/dotnet/aspnet:8.0
WORKDIR /app
COPY --from=build /app/publish .
ENTRYPOINT ["dotnet", "PalavraConectada.API.dll"]
```

**Passos do Deploy Railway:**
1. Conectar GitHub
2. Escolher repositório
3. Configurar Root Directory: `backend/PalavraConectada.API`
4. Railway detecta Dockerfile
5. Build automático
6. URL gerada: `https://palavraconectada-production.up.railway.app`

**Analogia:** Como construir a Arca da Aliança:
- Madeira de acácia (código)
- Revestida de ouro (Docker)
- Colocada no Santíssimo (Railway)

### 8.4 Vercel (Frontend Angular)

**Vercel é como o Átrio - onde todos entram:**

```json
// vercel.json - Configuração
{
  "version": 2,
  "buildCommand": "npm run build",
  "outputDirectory": "dist/palavra-conectada-angular/browser",
  "rewrites": [
    { "source": "/(.*)", "destination": "/index.html" }
  ]
}
```

**Passos do Deploy Vercel:**
1. Conectar GitHub
2. Importar repositório
3. Framework: Angular
4. Root Directory: `frontend/angular`
5. Build Command: `npm run build`
6. Deploy automático
7. URL: `https://palavra-conectada-angular.vercel.app`

### 8.5 Azure Static Web Apps (Blazor)

**Azure é como o Tabernáculo completo:**

```yaml
# GitHub Actions - CI/CD
name: Azure Static Web Apps
on:
  push:
    branches: [main]

jobs:
  build_and_deploy:
    runs-on: ubuntu-latest
    steps:
      - uses: actions/checkout@v3
      - name: Build And Deploy
        uses: Azure/static-web-apps-deploy@v1
        with:
          app_location: "frontend/blazor"
          output_location: "wwwroot"
```

**Passos do Deploy Azure:**
1. Criar recurso Static Web App
2. Conectar GitHub
3. Configurar paths
4. GitHub Actions automático
5. Build e deploy
6. URL: `https://calm-wave-0b86b2210.1.azurestaticapps.net`

## 8.6 CI/CD - A Reconstrução Automática

**Como Neemias reconstruiu os muros:**
- 🏗️ **CI (Continuous Integration)** → Cada família constrói sua parte
- 🚀 **CD (Continuous Deployment)** → Quando uma parte fica pronta, já é colocada

```
FLUXO CI/CD:
1. Você faz commit (como Neemias dá ordem)
2. GitHub detecta mudança
3. Tests rodam (verificar se está bem construído)
4. Build acontece (preparar os blocos)
5. Deploy acontece (colocar no muro)
6. Aplicação atualizada! (muro restaurado)
```

## 8.7 Environment Variables - Os Segredos

```bash
# .env - Segredos guardados
DATABASE_URL=postgres://...
API_KEY=secret_key_123
JWT_SECRET=super_secret

# Como usar no código
var dbUrl = Environment.GetEnvironmentVariable("DATABASE_URL");
```

**Analogia:** Como os **urim e tumim** no peitoral:
- Segredos guardados
- Só o sumo sacerdote acessa
- Revelam informações importantes

---

# 9. GIT: O LIVRO DAS CRÔNICAS

> *"Não está isto escrito no livro das crônicas...?"* - 1 Reis 14:19

## 9.1 O Que é Git?

**Git é como o Livro das Crônicas de Israel:**
- 📖 **Registra** tudo que aconteceu
- ⏮️ **Volta** no tempo quando necessário
- 🌿 **Branches** são como diferentes reinados
- 🤝 **Merge** é como unir os reinos

## 9.2 Conceitos Fundamentais

```
WORKING DIRECTORY   →  Mesa do escriba (onde você trabalha)
       ⬇️
STAGING AREA       →  Rascunho (preparando para escrever)
       ⬇️
REPOSITORY         →  Pergaminho (escrito permanentemente)
       ⬇️
REMOTE (GitHub)    →  Biblioteca de Alexandria (armazenado para sempre)
```

## 9.3 Comandos Essenciais

```bash
# git init - Começar novo livro
git init
# "No princípio, criou Deus..." (Gênesis 1:1)

# git add - Preparar para escrever
git add .
# Como Jeremias prepara o pergaminho (Jeremias 36:2)

# git commit - Escrever no livro
git commit -m "Adiciona versículos de João"
# Como selar o livro (Daniel 12:4)

# git push - Enviar para biblioteca
git push origin main
# Como enviar carta às igrejas (Apocalipse 2-3)

# git pull - Receber atualizações
git pull origin main
# Como receber revelação (Apocalipse 1:1)

# git clone - Copiar livro
git clone https://github.com/user/repo.git
# Como copistas copiavam as Escrituras

# git status - Ver situação atual
git status
# "E disse o SENHOR a Josué..." (Josué 1:1)

# git log - Ver história
git log
# Como ler as crônicas dos reis

# git diff - Ver diferenças
git diff
# Como comparar manuscritos
```

## 9.4 Branches - Os Reinados

```bash
# Criar branch - Novo reinado
git checkout -b feature/emotion-analysis
# Como dividir reino (1 Reis 12)

# Listar branches
git branch
  main
* feature/emotion-analysis

# Mudar de branch
git checkout main
# Voltar ao reinado principal

# Merge - Unir reinos
git checkout main
git merge feature/emotion-analysis
# Como Josias unificou o reino
```

**Analogia dos Branches:**
```
main           → Reino unido (Davi e Salomão)
├─ feature/A   → Reino do Norte (Israel)
└─ feature/B   → Reino do Sul (Judá)
```

## 9.5 Workflow Real

```bash
# 1. Começar nova funcionalidade
git checkout -b feature/new-emotion

# 2. Fazer mudanças
# (editar arquivos...)

# 3. Ver o que mudou
git status
git diff

# 4. Adicionar mudanças
git add .

# 5. Commit
git commit -m "Adiciona análise de gratidão"

# 6. Enviar para GitHub
git push origin feature/new-emotion

# 7. Criar Pull Request no GitHub
# (outros revisam - como conselho de anciãos)

# 8. Aprovado? Merge para main!
git checkout main
git merge feature/new-emotion

# 9. Enviar main atualizada
git push origin main

# 10. Deletar branch antiga
git branch -d feature/new-emotion
```

## 9.6 Resolvendo Conflitos

```bash
# Quando duas pessoas editam o mesmo arquivo
# Git marca os conflitos:

<<<<<<< HEAD
public string Emotion = "alegria";
=======
public string Emotion = "felicidade";
>>>>>>> feature/new-emotion

# Você decide:
# 1. Manter HEAD (seu código)
# 2. Manter incoming (código do outro)
# 3. Manter ambos
# 4. Escrever novo código

# Depois:
git add .
git commit -m "Resolve conflito de emoções"
```

**Analogia:** Como **conselho de Jerusalém** (Atos 15):
- Paulo e Barnabé (branches diferentes)
- Conflito de ideias
- Reunião para resolver
- Decisão unificada (merge)

---

# 10. EXERCÍCIOS PRÁTICOS: CONSTRUINDO SUA TORRE

> *"Construí pois uma casa ao nome do SENHOR..."* - 1 Reis 8:20

## 10.1 Nível Iniciante - As Pedras de Fundação

### Exercício 1: Criar Model
```csharp
// Crie um modelo para Salmo
public class Psalm
{
    public int Id { get; set; }
    public int Number { get; set; }  // Salmo 23, 91, etc
    public string Title { get; set; }
    public string Author { get; set; }
    public List<string> Verses { get; set; }
}
```

### Exercício 2: Criar Endpoint Simples
```csharp
[HttpGet("psalm/{number}")]
public async Task<ActionResult<Psalm>> GetPsalm(int number)
{
    // TODO: Buscar salmo do banco
    // TODO: Retornar ou 404 se não existir
}
```

### Exercício 3: Service Básico
```csharp
public class PsalmService
{
    public async Task<Psalm?> GetPsalmByNumberAsync(int number)
    {
        // TODO: Implementar busca
    }
}
```

## 10.2 Nível Intermediário - Construindo os Muros

### Exercício 4: CRUD Completo
```csharp
// Criar controller completo para Prayers (Orações)
// - GET /prayers (listar todas)
// - GET /prayers/{id} (buscar uma)
// - POST /prayers (criar nova)
// - PUT /prayers/{id} (atualizar)
// - DELETE /prayers/{id} (deletar)
```

### Exercício 5: Relacionamento N:M
```csharp
// Criar relação entre Verses e Tags
// Um versículo pode ter várias tags
// Uma tag pode estar em vários versículos
```

### Exercício 6: Component Angular
```typescript
// Criar component que:
// 1. Lista versículos
// 2. Permite buscar por palavra
// 3. Mostra loading
// 4. Trata erros
```

## 10.3 Nível Avançado - O Templo Completo

### Exercício 7: Autenticação JWT
```csharp
// Implementar sistema de login
// - Usuário se registra
// - Recebe token JWT
// - Usa token para acessar endpoints protegidos
```

### Exercício 8: Real-time com SignalR
```csharp
// Implementar chat de estudos bíblicos
// - Usuários entram em sala
// - Mensagens em tempo real
// - Notificações
```

### Exercício 9: Microserviços
```
// Separar em serviços:
// - Serviço de Versículos
// - Serviço de Usuários
// - Serviço de Comentários
// Comunicação via HTTP ou mensageria
```

### Exercício 10: Machine Learning
```csharp
// Implementar recomendação inteligente
// - Treinar modelo com versículos e emoções
// - Recomendar baseado em histórico do usuário
// - Melhorar com feedback
```

---

# 11. ROADMAP DE ESTUDOS: A JORNADA COMPLETA

## Mês 1-2: Fundamentos (Como Moisés no Deserto)
- ✅ C# básico
- ✅ .NET básico
- ✅ SQL básico
- ✅ Git básico
- ✅ APIs REST
- 📚 **Recursos:**
  - Microsoft Learn (grátis)
  - FreeCodeCamp
  - YouTube: Balta.io

## Mês 3-4: Backend Intermediário (Construindo o Tabernáculo)
- ✅ Entity Framework
- ✅ LINQ
- ✅ Async/Await
- ✅ Dependency Injection
- ✅ Design Patterns
- 📚 **Recursos:**
  - Clean Architecture (livro)
  - Pluralsight
  - Macoratti (YouTube PT-BR)

## Mês 5-6: Frontend Angular (O Átrio)
- ✅ TypeScript
- ✅ Components
- ✅ Services
- ✅ RxJS
- ✅ HTTP Client
- 📚 **Recursos:**
  - Angular.io docs
  - Loiane Groner (YouTube PT-BR)
  - Frontend Masters

## Mês 7-8: Frontend Blazor (Lugar Santo)
- ✅ Razor syntax
- ✅ Components
- ✅ State management
- ✅ JavaScript interop
- 📚 **Recursos:**
  - Microsoft Blazor docs
  - Blazor School
  - Dev Express (tutoriais)

## Mês 9-10: DevOps (Levantando o Edifício)
- ✅ Docker
- ✅ CI/CD
- ✅ GitHub Actions
- ✅ Cloud Deploy (Azure, Railway, Vercel)
- 📚 **Recursos:**
  - Docker docs
  - GitHub Learning Lab
  - Azure Learn

## Mês 11-12: Avançado (O Templo Completo)
- ✅ Microserviços
- ✅ Event-driven
- ✅ CQRS
- ✅ DDD
- ✅ Performance
- 📚 **Recursos:**
  - Domain-Driven Design (livro)
  - Microservices Patterns (livro)
  - InfoQ
  - Martin Fowler blog

---

# 12. ANALOGIAS BÍBLICAS FINAIS

## O Desenvolvedor é Como...

### Moisés - O Líder
- Recebe instruções (requisitos)
- Guia o povo (equipe)
- Constrói conforme ordenado (desenvolvimento)

### Bezalel - O Artífice (Êxodo 31:1-5)
*"O SENHOR encheu Bezalel do Espírito de Deus, de sabedoria, de entendimento e de conhecimento em todo artifício"*

- Sabedoria → Arquitetura
- Entendimento → Lógica
- Conhecimento → Tecnologias
- Artifício → Código

### Neemias - O Construtor
- Planejou (design)
- Organizou equipes (sprints)
- Enfrentou oposição (bugs)
- Completou em 52 dias! (deadline)

## O Código é Como...

### Provérbios - Sabedoria
- Cada linha tem propósito
- Reutilizável
- Testado pelo tempo

### Salmos - Poesia
- Belo de ler
- Bem estruturado
- Expressa emoções

### Levítico - Instruções Precisas
- Cada detalhe importa
- Ordem específica
- Consequências claras

---

# 13. CONCLUSÃO: SEU TEMPLO ESTÁ PRONTO

> *"Assim se acabou toda a obra... e Moisés viu toda a obra, e eis que a tinham feito; como o SENHOR ordenara, assim a fizeram; então Moisés os abençoou."* - Êxodo 39:32,43

## O Que Você Construiu:

```
🏛️ TEMPLO PALAVRA CONECTADA
├─ Fundamento (SQLite)
│  └─ 31.102 versículos
│
├─ Colunas (Backend .NET)
│  ├─ API REST
│  ├─ Análise de Emoções
│  └─ Clean Architecture
│
├─ Átrio dos Gentios (Angular)
│  ├─ Interface moderna
│  ├─ Componentes reativos
│  └─ TypeScript tipado
│
├─ Átrio de Israel (Blazor)
│  ├─ C# no frontend
│  ├─ WebAssembly
│  └─ Razor components
│
└─ Deploy
   ├─ Railway (Backend)
   ├─ Vercel (Angular)
   └─ Azure (Blazor)
```

## Suas Conquistas:

✅ **7 Linguagens** dominadas  
✅ **3 Frameworks** implementados  
✅ **Clean Architecture** aplicada  
✅ **31.102 versículos** no banco  
✅ **3 Deploys** em produção  
✅ **GitHub** organizado  
✅ **Portfolio** profissional atualizado  

## Próximos Passos:

1. 📚 **Estudar** - Use este guia como mapa
2. 🔨 **Praticar** - Faça os exercícios
3. 🌟 **Criar** - Desenvolva seus próprios projetos
4. 🤝 **Compartilhar** - Ensine outros
5. 🚀 **Crescer** - Nunca pare de aprender

## Versículo Final:

> *"Tudo quanto te vier à mão para fazer, faze-o conforme as tuas forças."*  
> **- Eclesiastes 9:10**

---

**Você não é mais iniciante. Você é um construtor de sistemas.**

**Como Salomão construiu o Templo, você construiu o PalavraConectada.**

**Que este seja o primeiro de muitos templos que você erguerá! 🏛️**

---

*Desenvolvido com 💙 por Alex Feitoza*  
*"Instruí o sábio, e ele se fará mais sábio" - Provérbios 9:9*

