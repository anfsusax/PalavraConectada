# 🅰️ GUIA COMPLETO DE ANGULAR - DO ZERO AO EXPERT

> *"Como o átrio do templo recebia a todos, Angular recebe todos os usuários"*

---

## 📖 ÍNDICE

1. [Fundamentos do Angular](#1-fundamentos)
2. [Components - As Peças](#2-components)
3. [Services - Os Mensageiros](#3-services)
4. [Routing - Os Caminhos](#4-routing)
5. [Forms - As Ofertas](#5-forms)
6. [HTTP - A Comunicação](#6-http)
7. [RxJS - Os Observadores](#7-rxjs)
8. [State Management](#8-state-management)
9. [Boas Práticas](#9-boas-praticas)
10. [Projeto Real: PalavraConectada](#10-projeto-real)

---

# 1. FUNDAMENTOS DO ANGULAR

## 1.1 O Que é Angular?

**Angular é um framework completo**, como o **Tabernáculo completo:**
- 🏗️ Estrutura definida
- 📦 Tudo incluído (bateria completa)
- 🎯 Opinativo (tem uma forma certa de fazer)
- 🔄 Reativo (responde a mudanças)

### Versões do Angular:

```
AngularJS (1.x)  →  Antigo Testamento
   |
   └─ Reescrito completamente
   
Angular (2+)     →  Novo Testamento
   ├─ Angular 2-16 (evolução gradual)
   └─ Angular 17+ (standalone components)
```

## 1.2 Instalação e Setup

```bash
# Instalar Node.js primeiro
# (Como preparar o terreno)

# Instalar Angular CLI (ferramentas)
npm install -g @angular/cli

# Criar novo projeto (plantar a semente)
ng new meu-projeto

# Entrar no projeto
cd meu-projeto

# Rodar aplicação (fazer crescer)
ng serve

# Abrir navegador
http://localhost:4200
```

**Analogia:** Como **plantar uma árvore:**
- Preparar terra (Node.js)
- Plantar semente (ng new)
- Regar (ng serve)
- Ver crescer (navegador)

## 1.3 Estrutura de Pastas

```
meu-projeto/
├─ src/
│  ├─ app/                    → A tenda (aplicação)
│  │  ├─ components/          → Móveis
│  │  ├─ services/            → Levitas
│  │  ├─ models/              → Definições
│  │  ├─ app.component.ts     → Componente raiz
│  │  └─ app.config.ts        → Configurações
│  │
│  ├─ assets/                 → Tesouros (imagens, etc)
│  ├─ index.html              → Porta de entrada
│  └─ main.ts                 → Ponto de partida
│
├─ angular.json               → Planta do edifício
├─ package.json               → Lista de suprimentos
└─ tsconfig.json              → Regras da linguagem
```

---

# 2. COMPONENTS: AS PEÇAS DO ÁTRIO

## 2.1 Anatomia de um Component

```typescript
// home.component.ts
import { Component, OnInit } from '@angular/core';

// Decorator = Marcação sagrada (como ungir)
@Component({
  selector: 'app-home',        // Como chamar no HTML
  templateUrl: './home.component.html',  // Aparência
  styleUrls: ['./home.component.css']    // Decoração
})
export class HomeComponent implements OnInit {
  
  // PROPRIEDADES (Estado)
  // Como os utensílios do altar
  title = 'Palavra Conectada';
  verses: Verse[] = [];
  loading = false;
  
  // CONSTRUCTOR (Construtor)
  // Como preparar o altar
  constructor(private apiService: BackendApiService) {
    console.log('Component criado!');
  }
  
  // LIFECYCLE HOOKS (Ciclo de vida)
  // Como as festas de Israel - acontecem em ordem
  
  ngOnInit(): void {
    // Quando component é iniciado
    // Como o Dia da Expiação - prepara tudo
    this.loadVerses();
  }
  
  ngOnDestroy(): void {
    // Quando component é destruído
    // Como desmontar o tabernáculo
    console.log('Component destruído');
  }
  
  // MÉTODOS (Ações)
  // Como os serviços que podem ser realizados
  loadVerses(): void {
    this.loading = true;
    
    this.apiService.getRandomVerse().subscribe({
      next: (verse) => {
        this.verses.push(verse);
        this.loading = false;
      }
    });
  }
}
```

## 2.2 Lifecycle Hooks - As Festas de Israel

```typescript
// Ordem de execução (como calendário judaico)

constructor()           → Preparação (antes da festa)
   ⬇️
ngOnChanges()          → Páscoa (mudanças detectadas)
   ⬇️
ngOnInit()             → Pentecostes (inicialização)
   ⬇️
ngDoCheck()            → Dia da Expiação (verificação)
   ⬇️
ngAfterContentInit()   → Festa dos Tabernáculos (conteúdo pronto)
   ⬇️
ngAfterViewInit()      → Dedicação (view completa)
   ⬇️
ngOnDestroy()          → Fim (desmontagem)
```

### Quando usar cada um:

```typescript
// ngOnInit - Mais usado! (Como Páscoa)
ngOnInit(): void {
  // Buscar dados iniciais
  // Configurar assinaturas
  // Setup inicial
}

// ngOnChanges - Quando @Input muda
ngOnChanges(changes: SimpleChanges): void {
  if (changes['verseId']) {
    this.loadNewVerse();
  }
}

// ngOnDestroy - Limpeza (Como guardar o tabernáculo)
ngOnDestroy(): void {
  // Cancelar assinaturas
  // Limpar timers
  // Liberar recursos
}
```

## 2.3 Component Communication - A Comunicação

### Pai → Filho (Input)
```typescript
// Pai (como Abraão passando bênção para Isaque)
<app-verse-card [verse]="myVerse"></app-verse-card>

// Filho (recebe bênção)
@Component({...})
export class VerseCardComponent {
  @Input() verse!: Verse;  // Recebe do pai
}
```

### Filho → Pai (Output)
```typescript
// Filho (como filho pedindo ao pai)
@Component({...})
export class VerseCardComponent {
  @Output() verseShared = new EventEmitter<Verse>();
  
  share(): void {
    this.verseShared.emit(this.verse);  // Avisa o pai
  }
}

// Pai (escuta o filho)
<app-verse-card 
  [verse]="myVerse"
  (verseShared)="handleShare($event)">
</app-verse-card>
```

**Analogia Completa:**
```
Deus (Pai Component)
  ↓ [revelação] (Input)
Profeta (Filho Component)
  ↑ (mensagem) Output
Povo (outro component)
```

## 2.4 Diretivas - Os Mandamentos do Template

### Estruturais (Mudam estrutura DOM)

```html
<!-- *ngIf - Condicional (SE... ENTÃO) -->
<div *ngIf="isLoggedIn">
  Bem-vindo, {{ userName }}!
</div>

<!-- Como: "SE guardares meus mandamentos, ENTÃO serás abençoado" -->

<!-- *ngFor - Repetição (PARA CADA) -->
<div *ngFor="let verse of verses; let i = index">
  {{ i + 1 }}. {{ verse.text }}
</div>

<!-- Como: "PARA CADA tribo de Israel..." -->

<!-- *ngSwitch - Múltiplas condições -->
<div [ngSwitch]="emotion">
  <p *ngSwitchCase="'alegria'">😊 Versículos de alegria</p>
  <p *ngSwitchCase="'tristeza'">😢 Versículos de consolo</p>
  <p *ngSwitchDefault>😐 Versículos gerais</p>
</div>
```

### Atributo (Mudam aparência/comportamento)

```html
<!-- [ngClass] - Adicionar classes condicionalmente -->
<div [ngClass]="{'destaque': isImportant, 'escuro': isDark}">
  Conteúdo
</div>

<!-- [ngStyle] - Estilos dinâmicos -->
<p [ngStyle]="{'color': textColor, 'font-size': fontSize + 'px'}">
  Texto personalizado
</p>

<!-- [hidden] - Esconder elemento -->
<div [hidden]="!showContent">
  Conteúdo oculto
</div>
```

---

# 3. SERVICES: OS MENSAGEIROS

## 3.1 Por Que Services?

**Services são como os mensageiros do rei:**
- 📨 Levam mensagens (HTTP requests)
- 🏃 Podem ir a vários lugares
- 🔄 Podem ser reutilizados
- 📦 Carregam informações

```typescript
// Service básico
@Injectable({
  providedIn: 'root'  // Singleton (único no reino)
})
export class BackendApiService {
  private apiUrl = 'https://api.com';
  
  constructor(private http: HttpClient) {}
  
  getVerses(): Observable<Verse[]> {
    return this.http.get<Verse[]>(`${this.apiUrl}/verses`);
  }
}
```

## 3.2 Injeção de Dependência

```typescript
// Providencia no root (disponível para todos)
@Injectable({ providedIn: 'root' })
export class BibleService { }

// Usar no component
export class HomeComponent {
  // Injetar no construtor
  constructor(private bibleService: BibleService) {
    // Pronto para usar!
  }
}
```

**Analogia:** Como **o Espírito Santo:**
- Provido por Deus (root)
- Recebido por quem precisa (constructor)
- Capacita para a obra (methods)

---

# 4. HTTP CLIENT: A COMUNICAÇÃO

## 4.1 GET Requests

```typescript
// GET - Buscar dados (como pedir pão)
getVerses(): Observable<Verse[]> {
  return this.http.get<Verse[]>(`${this.apiUrl}/verses`);
}

// Com parâmetros de query
searchVerses(keyword: string): Observable<Verse[]> {
  const params = new HttpParams().set('keyword', keyword);
  return this.http.get<Verse[]>(`${this.apiUrl}/verses/search`, { params });
}
```

## 4.2 POST Requests

```typescript
// POST - Enviar dados (como fazer oferta)
analyzeEmotion(text: string): Observable<EmotionResult> {
  const body = { text };
  return this.http.post<EmotionResult>(
    `${this.apiUrl}/emotion/analyze`,
    body
  );
}
```

## 4.3 Error Handling - Tratando Problemas

```typescript
getVerses(): Observable<Verse[]> {
  return this.http.get<Verse[]>(`${this.apiUrl}/verses`).pipe(
    // Tratar erro (como socorrer ferido)
    catchError((error: HttpErrorResponse) => {
      if (error.status === 404) {
        console.error('Não encontrado!');
      } else if (error.status === 500) {
        console.error('Erro no servidor!');
      }
      
      // Retornar valor padrão
      return of([]);  // Array vazio
    }),
    
    // Retry - Tentar novamente (como Elias orando 7x)
    retry(3),
    
    // Timeout - Desistir após tempo (como esperar 40 dias)
    timeout(5000)
  );
}
```

---

# 5. RXJS: OS OBSERVADORES

## 5.1 Observable - O Vigia

```typescript
// Observable = Vigia na torre
// Fica observando e avisa quando algo acontece

// Criar observable
const numberStream$ = new Observable<number>(observer => {
  // $ no final = convenção para observables
  
  observer.next(1);  // Avisar: "Vi número 1!"
  observer.next(2);  // Avisar: "Vi número 2!"
  observer.complete();  // "Terminei de vigiar"
});

// Subscribe = Colocar ouvinte
numberStream$.subscribe({
  next: (num) => console.log('Recebi:', num),
  error: (err) => console.log('Erro:', err),
  complete: () => console.log('Completo!')
});
```

**Analogia:** Como os **profetas vigiavam:**
- Isaías vigiava (Observable)
- Povo escutava (Subscribe)
- Profecia acontecia (next)
- Profecia completava (complete)

## 5.2 Operators - As Transformações

```typescript
import { map, filter, tap, debounceTime, distinctUntilChanged } from 'rxjs/operators';

// map - Transformar (como José interpretou sonhos)
of(1, 2, 3).pipe(
  map(x => x * 10)  // 10, 20, 30
).subscribe(console.log);

// filter - Filtrar (como separar limpo de impuro)
of(1, 2, 3, 4, 5).pipe(
  filter(x => x > 2)  // 3, 4, 5
).subscribe(console.log);

// tap - Espiar (como espias em Canaã)
of(1, 2, 3).pipe(
  tap(x => console.log('Espiando:', x)),  // Não modifica
  map(x => x * 2)
).subscribe(console.log);

// debounceTime - Esperar pausa (como esperar silêncio)
// Útil para autocomplete!
searchInput$.pipe(
  debounceTime(300),  // Espera 300ms sem digitação
  distinctUntilChanged()  // Só se mudou
).subscribe(term => this.search(term));

// combineLatest - Combinar streams (como dois testemunhos)
combineLatest([
  this.verses$,
  this.emotions$
]).subscribe(([verses, emotions]) => {
  // Quando AMBOS tiverem valor
  this.combine(verses, emotions);
});
```

**Operadores Comuns:**

```typescript
// Transformação
map()           → Transformar cada item
pluck()         → Pegar propriedade específica
scan()          → Acumular (como contar bênçãos)

// Filtragem
filter()        → Filtrar itens
first()         → Primeiro item apenas
last()          → Último item
take(n)         → Primeiros N itens
skip(n)         → Pular primeiros N

// Combinação
combineLatest() → Combinar múltiplos observables
merge()         → Mesclar em um só
concat()        → Um depois do outro
zip()           → Emparelar (como noivos)

// Controle de tempo
debounceTime()  → Esperar pausa
throttleTime()  → Limitar frequência
delay()         → Atrasar
timeout()       → Desistir após tempo

// Utilidade
tap()           → Efeito colateral (log, etc)
catchError()    → Capturar erro
retry()         → Tentar novamente
finalize()      → Executar ao final (sempre)
```

---

# 6. FORMS: AS OFERTAS DO POVO

## 6.1 Template-Driven Forms

```typescript
// Component
export class FormComponent {
  user = {
    name: '',
    email: ''
  };
  
  onSubmit(): void {
    console.log('Formulário enviado:', this.user);
  }
}
```

```html
<!-- Template -->
<form #userForm="ngForm" (ngSubmit)="onSubmit()">
  <!-- Two-way binding -->
  <input 
    [(ngModel)]="user.name" 
    name="name"
    required
    #nameInput="ngModel">
  
  <!-- Mostrar erro -->
  <div *ngIf="nameInput.invalid && nameInput.touched">
    Nome é obrigatório!
  </div>
  
  <input 
    [(ngModel)]="user.email"
    name="email"
    email
    required>
  
  <!-- Só habilita se form válido -->
  <button [disabled]="userForm.invalid">
    Enviar
  </button>
</form>
```

**Analogia:** Como **ofertas no templo:**
- Cada campo = item da oferta
- Validação = verificar se é puro
- Submit = entregar no altar

## 6.2 Reactive Forms (Mais Poderoso)

```typescript
import { FormBuilder, FormGroup, Validators } from '@angular/forms';

export class ReactiveFormComponent implements OnInit {
  userForm!: FormGroup;
  
  constructor(private fb: FormBuilder) {}
  
  ngOnInit(): void {
    // Construir formulário (como construir altar)
    this.userForm = this.fb.group({
      name: ['', [Validators.required, Validators.minLength(3)]],
      email: ['', [Validators.required, Validators.email]],
      age: [null, [Validators.min(0), Validators.max(120)]],
      
      // Nested group (grupo aninhado)
      address: this.fb.group({
        street: [''],
        city: [''],
        zipCode: ['', Validators.pattern(/^\d{5}$/)]
      })
    });
    
    // Observar mudanças (como vigia)
    this.userForm.valueChanges.subscribe(value => {
      console.log('Form mudou:', value);
    });
    
    // Observar campo específico
    this.userForm.get('name')?.valueChanges.subscribe(name => {
      console.log('Nome mudou:', name);
    });
  }
  
  onSubmit(): void {
    if (this.userForm.valid) {
      const data = this.userForm.value;
      console.log('Enviar:', data);
    }
  }
}
```

```html
<form [formGroup]="userForm" (ngSubmit)="onSubmit()">
  <input formControlName="name">
  <input formControlName="email">
  <input formControlName="age">
  
  <!-- Nested group -->
  <div formGroupName="address">
    <input formControlName="street">
    <input formControlName="city">
    <input formControlName="zipCode">
  </div>
  
  <button [disabled]="userForm.invalid">Enviar</button>
</form>
```

## 6.3 Custom Validators - Validadores Customizados

```typescript
// Validador customizado (como regras de pureza)
function versiculoValidator(): ValidatorFn {
  return (control: AbstractControl): ValidationErrors | null => {
    const value = control.value;
    
    // Formato: "João 3:16"
    const pattern = /^[A-Za-z]+ \d+:\d+$/;
    
    if (!pattern.test(value)) {
      return { invalidVerse: true };
    }
    
    return null;  // Válido!
  };
}

// Usar
this.userForm = this.fb.group({
  verse: ['', [Validators.required, versiculoValidator()]]
});
```

---

# 7. ROUTING: OS CAMINHOS DO TEMPLO

## 7.1 Configurar Rotas

```typescript
// app.routes.ts
import { Routes } from '@angular/router';

export const routes: Routes = [
  // Rota raiz (porta principal)
  { path: '', component: HomeComponent },
  
  // Rotas específicas (salas do templo)
  { path: 'verses', component: VersesComponent },
  { path: 'emotions', component: EmotionsComponent },
  
  // Rota com parâmetro (como chamar pelo nome)
  { path: 'verse/:id', component: VerseDetailComponent },
  
  // Rota 404 (perdido no deserto)
  { path: '**', component: NotFoundComponent }
];
```

## 7.2 Navegação

```typescript
import { Router } from '@angular/router';

export class MyComponent {
  constructor(private router: Router) {}
  
  // Navegar programaticamente
  goToVerses(): void {
    this.router.navigate(['/verses']);
  }
  
  // Com parâmetros
  goToVerse(id: number): void {
    this.router.navigate(['/verse', id]);
  }
  
  // Com query params
  searchVerses(term: string): void {
    this.router.navigate(['/verses'], {
      queryParams: { search: term }
    });
  }
}
```

```html
<!-- Navegar no template -->
<a routerLink="/">Home</a>
<a routerLink="/verses">Versículos</a>
<a [routerLink]="['/verse', verse.id]">Ver Detalhe</a>

<!-- Com classe quando ativo -->
<a routerLink="/verses" routerLinkActive="active">
  Versículos
</a>
```

## 7.3 Route Parameters

```typescript
// Receber parâmetros (como receber mensagem)
import { ActivatedRoute } from '@angular/router';

export class VerseDetailComponent implements OnInit {
  verseId!: number;
  
  constructor(private route: ActivatedRoute) {}
  
  ngOnInit(): void {
    // Pegar ID da URL
    this.route.params.subscribe(params => {
      this.verseId = +params['id'];  // + converte para número
      this.loadVerse(this.verseId);
    });
    
    // Ou snapshot (sem observar mudanças)
    this.verseId = +this.route.snapshot.params['id'];
  }
}
```

**Analogia:** Como **mensageiro trazendo carta:**
- Rota = Caminho percorrido
- Parâmetros = Conteúdo da carta
- Component = Destinatário

---

# 8. PROJETO REAL: PALAVRA CONECTADA ANGULAR

## 8.1 Estrutura do Nosso Projeto

```
src/app/
├─ components/
│  ├─ home/
│  │  ├─ home.component.ts
│  │  ├─ home.component.html
│  │  └─ home.component.css
│  │
│  └─ verses/
│     ├─ verses.component.ts
│     ├─ verses.component.html
│     └─ verses.component.css
│
├─ services/
│  └─ backend-api.service.ts    → Comunicação com Railway
│
├─ models/
│  └─ verse.model.ts            → Definições de tipos
│
├─ app.component.ts              → Raiz
└─ app.config.ts                → Configurações
```

## 8.2 Como Funciona

### Fluxo Completo:

```
1. Usuário digita "amor" no input
   ⬇️
2. (keyup) event dispara
   ⬇️
3. Component chama service
   this.apiService.searchVerses('amor')
   ⬇️
4. Service faz HTTP request
   POST https://palavraconectada-production.up.railway.app/api/Verses/search
   ⬇️
5. Railway processa
   BibleService busca no SQLite
   ⬇️
6. Response volta (JSON)
   { verses: [...] }
   ⬇️
7. Observable emite valor
   next(verses)
   ⬇️
8. Component recebe
   this.verses = verses
   ⬇️
9. Template atualiza
   *ngFor cria cards
   ⬇️
10. Usuário vê resultados! 🎉
```

## 8.3 Detecção Automática de Ambiente

```typescript
// backend-api.service.ts
private getApiUrl(): string {
  const hostname = window.location.hostname;
  
  if (hostname === 'localhost') {
    // Desenvolvimento
    return 'http://localhost:7000/api';
  }
  
  // Produção
  return 'https://palavraconectada-production.up.railway.app/api';
}
```

**Como funciona:**
- Vercel: hostname = `palavra-conectada.vercel.app`
- Detecta que NÃO é localhost
- Usa URL do Railway
- Conecta automaticamente! ✅

---

# 9. BOAS PRÁTICAS

## 9.1 Organização de Código

```typescript
// RUIM ❌
public class HomeComponent {
  getData() {
    this.http.get('https://api.com/data').subscribe(d => this.data = d);
  }
}

// BOM ✅
export class HomeComponent implements OnInit {
  data: MyData[] = [];
  loading = false;
  error: string | null = null;
  
  constructor(private dataService: DataService) {}
  
  ngOnInit(): void {
    this.loadData();
  }
  
  private loadData(): void {
    this.loading = true;
    this.error = null;
    
    this.dataService.getData().pipe(
      finalize(() => this.loading = false)
    ).subscribe({
      next: (data) => this.data = data,
      error: (err) => this.error = err.message
    });
  }
}
```

## 9.2 Unsubscribe - Cancelar Assinaturas

```typescript
import { Subject, takeUntil } from 'rxjs';

export class MyComponent implements OnInit, OnDestroy {
  private destroy$ = new Subject<void>();
  
  ngOnInit(): void {
    // Assinatura que será cancelada
    this.apiService.getVerses().pipe(
      takeUntil(this.destroy$)  // Cancela quando destroy$ emitir
    ).subscribe(verses => {
      this.verses = verses;
    });
  }
  
  ngOnDestroy(): void {
    // Cancelar todas assinaturas
    this.destroy$.next();
    this.destroy$.complete();
  }
}
```

**Analogia:** Como **cancelar voto nazireu:**
- Fez voto (subscribe)
- Cumpriu o tempo (component vivo)
- Liberado do voto (unsubscribe)

## 9.3 Async Pipe - O Automatizador

```typescript
// Component
export class MyComponent {
  verses$ = this.apiService.getVerses();  // Observable direto!
  
  constructor(private apiService: BackendApiService) {}
}
```

```html
<!-- Template - async pipe se inscreve E cancela automaticamente! -->
<div *ngIf="verses$ | async as verses">
  <div *ngFor="let verse of verses">
    {{ verse.text }}
  </div>
</div>
```

**Vantagens do Async Pipe:**
- ✅ Subscribe automático
- ✅ Unsubscribe automático
- ✅ Menos código
- ✅ Menos bugs

---

# 10. CHECKLIST DE DOMÍNIO

## Iniciante ⭐
- [ ] Criar projeto com `ng new`
- [ ] Criar component com `ng g c`
- [ ] Usar `*ngIf` e `*ngFor`
- [ ] Fazer data binding `{{ }}`, `[]`, `()`
- [ ] Criar service básico
- [ ] Fazer GET request
- [ ] Navegar entre páginas

## Intermediário ⭐⭐
- [ ] Reactive Forms
- [ ] Custom validators
- [ ] RxJS operators (map, filter)
- [ ] Error handling
- [ ] Loading states
- [ ] Route parameters
- [ ] Component communication (@Input, @Output)

## Avançado ⭐⭐⭐
- [ ] State management (NgRx ou Signals)
- [ ] Lazy loading
- [ ] Guards (route protection)
- [ ] Interceptors (HTTP)
- [ ] Custom directives
- [ ] Performance optimization
- [ ] Testing (Jasmine/Karma)
- [ ] Deploy (Vercel/Azure/AWS)

---

**Continue praticando! Cada linha de código é como uma pedra no templo!**

*"A sabedoria edificou a sua casa, lavrou as suas sete colunas."* - Provérbios 9:1

