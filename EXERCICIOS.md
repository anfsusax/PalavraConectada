# 🎯 Exercícios Práticos - Palavra Conectada

## 📖 História: Os 12 Discípulos

Assim como Jesus treinou os 12 discípulos através da **prática**, você vai aprender fazendo!

---

## 🌱 Nível Iniciante - As Primeiras Sementes

### Exercício 1: Mudando Cores (Fácil)
**Objetivo:** Personalizar as cores do tema

**📍 Angular:**
- Arquivo: `palavra-conectada-angular/src/app/components/verse-search/verse-search.component.css`
- Linha: 16-20 (background do header)
- **Tarefa:** Mude o gradiente de roxo para verde/azul

**📍 Blazor:**
- Arquivo: `palavra-conectada-blazor/Components/Pages/VerseSearch.razor.css`
- Linha: 16-20
- **Tarefa:** Mude o gradiente de roxo para laranja/vermelho

**💡 Dica:** Use um gerador de gradientes online como [cssgradient.io](https://cssgradient.io)

---

### Exercício 2: Adicionar Nova Sugestão (Fácil)
**Objetivo:** Adicionar "perdão" nas sugestões de busca

**📍 Angular:**
```typescript
// verse-search.component.ts, linha ~29
exampleSearches = [
  // ... exemplos existentes ...
  { term: 'perdão', description: 'Versículos sobre perdão' }  // ← ADICIONE
];
```

**📍 Blazor:**
```csharp
// VerseSearch.razor, linha ~86
private List<ExampleSearch> exampleSearches = new()
{
    // ... exemplos existentes ...
    new() { Term = "perdão", Description = "Versículos sobre perdão" }  // ← ADICIONE
};
```

**✅ Teste:** Veja o novo botão aparecer e clique nele!

---

### Exercício 3: Mudar Texto do Cabeçalho (Fácil)
**Objetivo:** Personalizar a mensagem de boas-vindas

**📍 Angular:**
```html
<!-- verse-search.component.html, linha 2 -->
<h1>📖 Sua Nova Mensagem Aqui</h1>
```

**📍 Blazor:**
```razor
<!-- VerseSearch.razor, linha 9 -->
<h1>📖 Sua Nova Mensagem Aqui</h1>
```

**💡 Ideias:**
- "🙏 Bíblia em Mãos"
- "✝️ Palavra Viva"
- "📚 Buscador de Versículos"

---

## 🌿 Nível Intermediário - Crescendo em Sabedoria

### Exercício 4: Contador de Caracteres (Médio)
**Objetivo:** Mostrar quantos caracteres o usuário digitou

**📍 Angular:**
```html
<!-- verse-search.component.html, após o input -->
<p *ngIf="searchTerm">
  Você digitou {{ searchTerm.length }} caracteres
</p>
```

**📍 Blazor:**
```razor
<!-- VerseSearch.razor, após o input -->
@if (!string.IsNullOrEmpty(searchTerm))
{
    <p>Você digitou @searchTerm.Length caracteres</p>
}
```

**✅ Teste:** Digite algo e veja o contador aparecer!

---

### Exercício 5: Botão Copiar Versículo (Médio)
**Objetivo:** Adicionar botão para copiar versículo

**📍 Angular:**
```typescript
// verse-search.component.ts
copyVerse(verse: Verse): void {
  const text = `"${verse.text}" - ${this.getVerseReference(verse)}`;
  navigator.clipboard.writeText(text).then(() => {
    alert('Versículo copiado!');
  });
}
```

```html
<!-- verse-search.component.html, dentro do verse-card -->
<button (click)="copyVerse(verse)" class="copy-btn">
  📋 Copiar
</button>
```

**📍 Blazor:**
```csharp
// VerseSearch.razor
@inject IJSRuntime JS

@code {
    private async Task CopyVerse(Verse verse)
    {
        var text = $"\"{verse.Text}\" - {GetVerseReference(verse)}";
        await JS.InvokeVoidAsync("navigator.clipboard.writeText", text);
        await JS.InvokeVoidAsync("alert", "Versículo copiado!");
    }
}
```

```razor
<!-- Dentro do verse-card -->
<button @onclick="() => CopyVerse(verse)" class="copy-btn">
    📋 Copiar
</button>
```

**CSS (ambos):**
```css
.copy-btn {
    padding: 0.5rem 1rem;
    background: #4caf50;
    color: white;
    border: none;
    border-radius: 6px;
    cursor: pointer;
    margin-top: 0.5rem;
}
```

---

### Exercício 6: Modo Escuro (Médio)
**Objetivo:** Adicionar toggle de modo escuro

**📍 Angular:**
```typescript
// verse-search.component.ts
isDarkMode: boolean = false;

toggleDarkMode(): void {
  this.isDarkMode = !this.isDarkMode;
}
```

```html
<!-- verse-search.component.html, no header -->
<button (click)="toggleDarkMode()" class="dark-mode-btn">
  {{ isDarkMode ? '☀️' : '🌙' }}
</button>

<!-- No container principal -->
<div class="verse-search-container" [class.dark-mode]="isDarkMode">
```

**📍 Blazor:**
```csharp
// VerseSearch.razor
@code {
    private bool isDarkMode = false;

    private void ToggleDarkMode()
    {
        isDarkMode = !isDarkMode;
    }
}
```

```razor
<!-- No header -->
<button @onclick="ToggleDarkMode" class="dark-mode-btn">
    @(isDarkMode ? "☀️" : "🌙")
</button>

<!-- No container principal -->
<div class="verse-search-container @(isDarkMode ? "dark-mode" : "")">
```

**CSS (ambos):**
```css
.dark-mode {
    background: #1a1a1a;
    color: #e0e0e0;
}

.dark-mode .search-section {
    background: #2a2a2a;
}

.dark-mode .verse-card {
    background: #2a2a2a;
    color: #e0e0e0;
}

.dark-mode-btn {
    position: absolute;
    top: 1rem;
    right: 1rem;
    font-size: 1.5rem;
    background: none;
    border: none;
    cursor: pointer;
}
```

---

## 🌳 Nível Avançado - Frutos Maduros

### Exercício 7: Histórico de Buscas (Difícil)
**Objetivo:** Salvar últimas 5 buscas no localStorage

**📍 Angular:**
```typescript
// verse-search.component.ts
searchHistory: string[] = [];

ngOnInit(): void {
  this.loadHistory();
}

saveToHistory(term: string): void {
  this.searchHistory = [term, ...this.searchHistory.filter(t => t !== term)];
  this.searchHistory = this.searchHistory.slice(0, 5);
  localStorage.setItem('searchHistory', JSON.stringify(this.searchHistory));
}

loadHistory(): void {
  const saved = localStorage.getItem('searchHistory');
  if (saved) {
    this.searchHistory = JSON.parse(saved);
  }
}

searchVerses(): void {
  // ... código existente ...
  this.saveToHistory(this.searchTerm);
}
```

```html
<!-- verse-search.component.html -->
<div class="history-section" *ngIf="searchHistory.length > 0">
  <p>Buscas recentes:</p>
  <button *ngFor="let term of searchHistory" 
          (click)="useExample(term)"
          class="history-btn">
    {{ term }}
  </button>
</div>
```

**📍 Blazor:**
```csharp
// VerseSearch.razor
@inject IJSRuntime JS

@code {
    private List<string> searchHistory = new();

    protected override async Task OnInitializedAsync()
    {
        await LoadHistory();
    }

    private async Task SaveToHistory(string term)
    {
        searchHistory.Remove(term);
        searchHistory.Insert(0, term);
        searchHistory = searchHistory.Take(5).ToList();
        
        var json = JsonSerializer.Serialize(searchHistory);
        await JS.InvokeVoidAsync("localStorage.setItem", "searchHistory", json);
    }

    private async Task LoadHistory()
    {
        var json = await JS.InvokeAsync<string>("localStorage.getItem", "searchHistory");
        if (!string.IsNullOrEmpty(json))
        {
            searchHistory = JsonSerializer.Deserialize<List<string>>(json) ?? new();
        }
    }

    private async Task SearchVerses()
    {
        // ... código existente ...
        await SaveToHistory(searchTerm);
    }
}
```

```razor
<!-- Adicionar no componente -->
@if (searchHistory.Any())
{
    <div class="history-section">
        <p>Buscas recentes:</p>
        @foreach (var term in searchHistory)
        {
            <button @onclick="() => UseExample(term)" class="history-btn">
                @term
            </button>
        }
    </div>
}
```

**CSS (ambos):**
```css
.history-section {
    margin: 1rem 0;
    padding: 1rem;
    background: #f0f0f0;
    border-radius: 8px;
}

.history-btn {
    padding: 0.5rem 1rem;
    margin: 0.25rem;
    background: white;
    border: 1px solid #ddd;
    border-radius: 6px;
    cursor: pointer;
}

.history-btn:hover {
    background: #e0e0e0;
}
```

---

### Exercício 8: Favoritos (Difícil)
**Objetivo:** Salvar versículos favoritos

**Estrutura:**
1. Adicionar botão ⭐ em cada versículo
2. Salvar favoritos no localStorage
3. Criar aba para ver favoritos
4. Permitir remover favoritos

**💡 Dica:** Use o código do Exercício 7 como base e adapte!

---

### Exercício 9: Compartilhar no WhatsApp (Médio-Difícil)
**Objetivo:** Adicionar botão para compartilhar versículo

**📍 Ambos (Angular e Blazor):**
```typescript / csharp
shareVerse(verse: Verse): void {
  const text = `"${verse.text}" - ${this.getVerseReference(verse)}`;
  const encoded = encodeURIComponent(text);
  const url = `https://wa.me/?text=${encoded}`;
  window.open(url, '_blank');
}
```

```html / razor
<button (click)="shareVerse(verse)" class="share-btn">
  💬 WhatsApp
</button>
```

---

## 🏆 Desafio Final - O Grande Projeto

### Projeto Completo: Plano de Leitura Bíblica

**Requisitos:**
1. ✅ Criar página para plano de leitura
2. ✅ 5 planos diferentes (Novo Testamento, Salmos, etc)
3. ✅ Marcar capítulos como lidos
4. ✅ Progresso visual (barra de progresso)
5. ✅ Salvar progresso no localStorage
6. ✅ Notificação quando completar um plano

**📖 História Bíblica:** Como Neemias reconstruiu os muros de Jerusalém - um tijolo de cada vez!

---

## 📝 Checklist de Aprendizado

Marque o que você já consegue fazer:

### Angular
- [ ] Criar componente
- [ ] Usar data binding (two-way)
- [ ] Usar *ngIf e *ngFor
- [ ] Criar serviço
- [ ] Fazer requisição HTTP
- [ ] Usar Observables e subscribe
- [ ] Usar lifecycle hooks
- [ ] Criar interface/model
- [ ] Usar dependency injection

### Blazor
- [ ] Criar componente Razor
- [ ] Usar @bind
- [ ] Usar @if e @foreach
- [ ] Criar serviço C#
- [ ] Fazer requisição HTTP async
- [ ] Usar async/await
- [ ] Usar lifecycle methods
- [ ] Criar classes/models
- [ ] Usar @inject

---

## 🎓 Certificado de Conclusão

Quando completar todos os exercícios, você terá aprendido:

✅ Fundamentos de Angular  
✅ Fundamentos de Blazor  
✅ Consumir APIs REST  
✅ Gerenciar estado  
✅ Salvar dados localmente  
✅ Criar interfaces modernas  
✅ Comparar frameworks diferentes  

---

## 🙏 Versículo de Encorajamento

> **"Tudo posso naquele que me fortalece."**
> 
> *Filipenses 4:13*

Você consegue! Pratique, erre, aprenda, e cresça! 💪

---

## 💡 Dicas Finais

1. **Não tenha medo de errar** - Erros são professores
2. **Leia as mensagens de erro** - Elas te guiam
3. **Use o console do navegador** (F12) - Seu melhor amigo
4. **Compare os códigos** - Veja as diferenças
5. **Mude coisas pequenas** - Veja o que acontece
6. **Pergunte "por quê?"** - Entenda o motivo
7. **Pratique diariamente** - Consistência vence talento

---

## 📚 Próximos Passos

Depois de dominar os exercícios:

1. **Estude os READMEs completos**
2. **Leia a documentação oficial**
   - Angular: https://angular.io
   - Blazor: https://blazor.net
3. **Crie seu próprio projeto**
4. **Contribua com código open source**
5. **Ensine outros** - Ensinar é aprender duas vezes!

---

## 🎯 Meta Final

> **"Porque os que me acharam acharam a vida"** (Provérbios 8:35)

Sua meta não é apenas aprender código, mas usar o código para **conectar pessoas à Palavra de Deus**!

---

*Desenvolvido com ❤️ para transformar aprendizes em mestres*

