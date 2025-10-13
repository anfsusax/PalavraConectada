# 🔥 GUIA COMPLETO DE BLAZOR - C# NO FRONTEND!

> *"E o Espírito do SENHOR se apoderou dele..."* - Juízes 14:6  
> *(Assim como o Espírito capacitou Sansão, Blazor capacita C# no navegador!)*

---

## 📖 ÍNDICE

1. [O Milagre do Blazor](#1-o-milagre-do-blazor)
2. [Razor Components](#2-razor-components)
3. [Data Binding](#3-data-binding)
4. [Component Parameters](#4-component-parameters)
5. [Event Handling](#5-event-handling)
6. [Forms e Validation](#6-forms-e-validation)
7. [JavaScript Interop](#7-javascript-interop)
8. [State Management](#8-state-management)
9. [Ciclo de Vida](#9-ciclo-de-vida)
10. [Projeto Real](#10-projeto-real)

---

# 1. O MILAGRE DO BLAZOR

## 1.1 O Que é Blazor?

**Blazor = Blazing Fast + Razor**

É como o **milagre de Pentecostes:**
- 👥 Apóstolos falavam sua língua (C#)
- 🌍 Todos entendiam (navegador)
- 🔥 Poder do Espírito (WebAssembly)
- 🎯 Mesma mensagem, diferentes ouvintes

### Por Que Blazor é Revolucionário?

```
Antes:                     Agora com Blazor:
Frontend = JavaScript      Frontend = C#! 🤯
Backend = C#              Backend = C#
                          
Duas línguas diferentes   UMA SÓ LÍNGUA!
```

**Analogia:** Como **Paulo sendo hebreu** mas pregando em grego:
- Blazor é C# (hebraico) rodando no navegador (grego)
- WebAssembly é o tradutor

## 1.2 Tipos de Blazor

### Blazor WebAssembly (WASM)
```
Cliente (Navegador)
├─ Baixa .NET runtime (~2MB)
├─ Carrega suas DLLs
└─ Roda C# DIRETO no navegador!

Analogia: Como Paulo levando o Evangelho aos gentios
- Leva a mensagem (runtime)
- Prega na língua local (WebAssembly)
- Converte corações (executa C#)
```

### Blazor Server
```
Servidor mantém estado
├─ Cliente envia eventos
├─ Servidor processa
└─ SignalR atualiza UI

Analogia: Como Moisés no Monte
- Moisés sobe (servidor)
- Povo espera embaixo (cliente)
- Desce com tábuas (resposta)
```

## 1.3 Criar Projeto Blazor

```bash
# Criar projeto WASM
dotnet new blazorwasm -o MeuProjeto

# Ou Blazor Server
dotnet new blazorserver -o MeuProjeto

# Entrar e rodar
cd MeuProjeto
dotnet run

# Abrir navegador
https://localhost:5001
```

---

# 2. RAZOR COMPONENTS: AS CORTINAS BORDADAS

## 2.1 Estrutura Básica

```razor
@page "/verses"
@inject BackendApiService ApiService
@inject NavigationManager Navigation

<!-- HTML + Razor markup -->
<div class="container">
    <h1>Versículos da Bíblia</h1>
    
    @* Comentário Razor *@
    
    <!-- Código C# inline com @ -->
    <p>Total: @verses.Count versículos</p>
    
    <!-- Estruturas de controle -->
    @if (loading)
    {
        <p>🔍 Carregando...</p>
    }
    else if (verses.Any())
    {
        @foreach (var verse in verses)
        {
            <div class="card">
                <h3>@verse.Book @verse.Chapter:@verse.Number</h3>
                <p>@verse.Text</p>
            </div>
        }
    }
    else
    {
        <p>Nenhum versículo encontrado.</p>
    }
    
    <!-- Event handler -->
    <button @onclick="LoadMore">Carregar Mais</button>
</div>

@code {
    // BLOCO DE CÓDIGO C# PURO!
    private List<Verse> verses = new();
    private bool loading = false;
    
    // Executado quando component carrega
    protected override async Task OnInitializedAsync()
    {
        await LoadVerses();
    }
    
    private async Task LoadVerses()
    {
        loading = true;
        
        try
        {
            verses = await ApiService.GetVersesAsync();
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Erro: {ex.Message}");
        }
        finally
        {
            loading = false;
        }
    }
    
    private async Task LoadMore()
    {
        // Lógica para carregar mais
        var moreVerses = await ApiService.GetMoreVersesAsync();
        verses.AddRange(moreVerses);
    }
}
```

**Analogia:** Como **pergaminho com duas faces:**
- Frente (HTML/Razor) = Texto visível
- Verso (@code) = Notas do escriba

## 2.2 Razor Syntax - A Gramática

```razor
<!-- @ = Marca início de código C# -->

<!-- Expressão simples -->
<p>Hoje é @DateTime.Now.ToShortDateString()</p>

<!-- Expressão complexa (use parênteses) -->
<p>Nome: @(user.FirstName + " " + user.LastName)</p>

<!-- Bloco de código inline -->
@{
    var total = verses.Count;
    var message = total > 10 ? "Muitos!" : "Poucos";
}
<p>@message</p>

<!-- Estruturas de controle -->
@if (condition)
{
    <p>Verdadeiro</p>
}
else
{
    <p>Falso</p>
}

@switch (emotion)
{
    case "alegria":
        <p>😊 Alegre!</p>
        break;
    case "tristeza":
        <p>😢 Triste</p>
        break;
    default:
        <p>😐 Normal</p>
        break;
}

<!-- Loop -->
@foreach (var item in items)
{
    <div>@item.Name</div>
}

@for (int i = 0; i < 10; i++)
{
    <span>@i</span>
}

<!-- While -->
@while (count < limit)
{
    <p>Contando: @count</p>
    count++;
}
```

---

# 3. DATA BINDING: A CONEXÃO

## 3.1 One-Way Binding (Código → Template)

```razor
@code {
    private string message = "Olá!";
    private bool isVisible = true;
    private string cssClass = "highlight";
}

<!-- Interpolação -->
<p>@message</p>

<!-- Atributos -->
<div hidden="@(!isVisible)">Conteúdo</div>
<div class="@cssClass">Estilizado</div>

<!-- Condicional no atributo -->
<button disabled="@loading">Enviar</button>
```

## 3.2 Two-Way Binding (@bind)

```razor
@code {
    private string searchTerm = "";
}

<!-- Two-way binding automático! -->
<input @bind="searchTerm" />

<!-- Com evento customizado -->
<input @bind="searchTerm" @bind:event="oninput" />

<!-- Mostrar o valor -->
<p>Você digitou: @searchTerm</p>
```

**Analogia:** Como **Moisés mediando:**
- Input muda → Código atualiza (povo fala, Moisés ouve)
- Código muda → Input atualiza (Deus fala, Moisés transmite)

## 3.3 Event Binding

```razor
<!-- Click -->
<button @onclick="HandleClick">Clique</button>

<!-- Com parâmetro -->
<button @onclick="() => DeleteVerse(verse.Id)">Deletar</button>

<!-- Outros eventos -->
<input @onchange="HandleChange" />
<input @oninput="HandleInput" />
<form @onsubmit="HandleSubmit">...</form>
<div @onmouseover="HandleHover">...</div>

@code {
    private void HandleClick()
    {
        Console.WriteLine("Clicado!");
    }
    
    private void HandleChange(ChangeEventArgs e)
    {
        var value = e.Value?.ToString();
        Console.WriteLine($"Mudou para: {value}");
    }
    
    private async Task HandleSubmit()
    {
        await SaveDataAsync();
    }
}
```

---

# 4. COMPONENT PARAMETERS: PASSANDO O MANTO

## 4.1 Input Parameters (Receber do Pai)

```razor
<!-- VerseCard.razor (Componente filho) -->
<div class="card">
    <h3>@Verse.Book @Verse.Chapter:@Verse.Number</h3>
    <p>@Verse.Text</p>
</div>

@code {
    // Recebe do componente pai (como herança)
    [Parameter]
    public Verse Verse { get; set; } = new();
    
    // Com valor padrão
    [Parameter]
    public string Theme { get; set; } = "light";
    
    // Requerido (obrigatório)
    [Parameter, EditorRequired]
    public int VerseId { get; set; }
}
```

```razor
<!-- Home.razor (Componente pai) -->
<VerseCard Verse="@currentVerse" Theme="dark" />

@code {
    private Verse currentVerse = new() {
        Book = "João",
        Chapter = 3,
        Number = 16,
        Text = "Porque Deus amou..."
    };
}
```

**Analogia:** Como **Elias passar o manto para Eliseu:**
- Elias = Component pai
- Manto = Parameter
- Eliseu = Component filho
- Poder dobrado = Usar o parâmetro

## 4.2 Output Parameters (EventCallback)

```razor
<!-- VerseCard.razor -->
<div class="card">
    <h3>@Verse.Book</h3>
    <button @onclick="ShareClicked">Compartilhar</button>
    <button @onclick="DeleteClicked">Deletar</button>
</div>

@code {
    [Parameter]
    public Verse Verse { get; set; } = new();
    
    // EventCallback = Avisar o pai
    [Parameter]
    public EventCallback<Verse> OnShare { get; set; }
    
    [Parameter]
    public EventCallback<int> OnDelete { get; set; }
    
    private async Task ShareClicked()
    {
        // Invocar evento do pai
        await OnShare.InvokeAsync(Verse);
    }
    
    private async Task DeleteClicked()
    {
        await OnDelete.InvokeAsync(Verse.Id);
    }
}
```

```razor
<!-- Home.razor (Pai escuta) -->
<VerseCard 
    Verse="@verse"
    OnShare="HandleShare"
    OnDelete="HandleDelete" />

@code {
    private async Task HandleShare(Verse verse)
    {
        Console.WriteLine($"Compartilhando: {verse.Text}");
        // Lógica de compartilhamento
    }
    
    private async Task HandleDelete(int id)
    {
        Console.WriteLine($"Deletando ID: {id}");
        verses.RemoveAll(v => v.Id == id);
    }
}
```

## 4.3 Cascading Parameters (Cascata)

```razor
<!-- App.razor (Topo da hierarquia) -->
<CascadingValue Value="@currentUser">
    <Router>
        <RouteView />
    </Router>
</CascadingValue>

@code {
    private User currentUser = new User { Name = "Alex" };
}
```

```razor
<!-- QualquerComponent.razor (Em qualquer lugar da árvore!) -->
<p>Usuário logado: @CurrentUser.Name</p>

@code {
    // Recebe de QUALQUER ancestral
    [CascadingParameter]
    public User CurrentUser { get; set; } = new();
}
```

**Analogia:** Como **bênção de Abraão:**
- Abraão abençoado (CascadingValue no topo)
- Isaque recebe (filho direto)
- Jacó recebe (neto)
- Tribos recebem (descendentes)
- Todos recebem a mesma bênção!

---

# 5. FORMS E VALIDATION

## 5.1 EditForm - Formulários Blazor

```razor
<EditForm Model="@user" OnValidSubmit="HandleValidSubmit">
    <!-- DataAnnotationsValidator = Validador automático -->
    <DataAnnotationsValidator />
    
    <!-- Mostrar erros de validação -->
    <ValidationSummary />
    
    <!-- Campos -->
    <div>
        <label>Nome:</label>
        <InputText @bind-Value="user.Name" />
        <ValidationMessage For="@(() => user.Name)" />
    </div>
    
    <div>
        <label>Email:</label>
        <InputText @bind-Value="user.Email" />
        <ValidationMessage For="@(() => user.Email)" />
    </div>
    
    <div>
        <label>Idade:</label>
        <InputNumber @bind-Value="user.Age" />
        <ValidationMessage For="@(() => user.Age)" />
    </div>
    
    <div>
        <label>Emoção:</label>
        <InputSelect @bind-Value="user.Emotion">
            <option value="">Selecione...</option>
            <option value="alegria">Alegria</option>
            <option value="tristeza">Tristeza</option>
            <option value="paz">Paz</option>
        </InputSelect>
    </div>
    
    <button type="submit">Enviar</button>
</EditForm>

@code {
    private UserModel user = new();
    
    private async Task HandleValidSubmit()
    {
        // Só chega aqui se formulário válido!
        Console.WriteLine($"Enviando: {user.Name}");
        await SaveUserAsync();
    }
}

// Model com validações
public class UserModel
{
    [Required(ErrorMessage = "Nome é obrigatório")]
    [MinLength(3, ErrorMessage = "Nome deve ter no mínimo 3 caracteres")]
    public string Name { get; set; } = "";
    
    [Required]
    [EmailAddress(ErrorMessage = "Email inválido")]
    public string Email { get; set; } = "";
    
    [Range(0, 120, ErrorMessage = "Idade deve estar entre 0 e 120")]
    public int Age { get; set; }
    
    [Required]
    public string Emotion { get; set; } = "";
}
```

**Analogia:** Como **Levítico** - regras claras:
- `[Required]` = Obrigatório (como sacrifício diário)
- `[Range]` = Limites (como medidas do altar)
- `[EmailAddress]` = Formato específico (como ritual específico)

## 5.2 Validação Customizada

```csharp
// Validador de versículo (João 3:16)
public class VersiculoAttribute : ValidationAttribute
{
    protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
    {
        var versiculo = value?.ToString();
        
        if (string.IsNullOrEmpty(versiculo))
            return new ValidationResult("Versículo é obrigatório");
        
        // Validar formato: "Livro Capítulo:Versículo"
        var pattern = @"^[A-Za-z]+ \d+:\d+$";
        if (!Regex.IsMatch(versiculo, pattern))
            return new ValidationResult("Formato inválido. Use: João 3:16");
        
        return ValidationResult.Success;
    }
}

// Usar
public class SearchModel
{
    [Versiculo]
    public string Reference { get; set; } = "";
}
```

---

# 6. JAVASCRIPT INTEROP: AS DUAS LÍNGUAS

## 6.1 Chamar JavaScript do C#

```razor
@inject IJSRuntime JS

<button @onclick="ShowAlert">Mostrar Alerta</button>
<button @onclick="SaveToLocalStorage">Salvar Local</button>

@code {
    // Chamar função JavaScript
    private async Task ShowAlert()
    {
        await JS.InvokeVoidAsync("alert", "Olá do C#!");
    }
    
    // Salvar no localStorage
    private async Task SaveToLocalStorage()
    {
        await JS.InvokeVoidAsync("localStorage.setItem", "verse", "João 3:16");
    }
    
    // Obter valor do JavaScript
    private async Task<string> GetFromLocalStorage()
    {
        var value = await JS.InvokeAsync<string>("localStorage.getItem", "verse");
        return value ?? "";
    }
    
    // Chamar função customizada
    private async Task ScrollToTop()
    {
        await JS.InvokeVoidAsync("scrollToTop");
    }
}
```

```html
<!-- wwwroot/index.html -->
<script>
    // Função JavaScript que C# pode chamar
    function scrollToTop() {
        window.scrollTo({ top: 0, behavior: 'smooth' });
    }
    
    // Função mais complexa
    function shareVerse(verse) {
        if (navigator.share) {
            navigator.share({
                title: verse.book,
                text: verse.text,
                url: window.location.href
            });
        }
    }
</script>
```

## 6.2 Chamar C# do JavaScript

```csharp
// VerseHelper.cs
public class VerseHelper
{
    [JSInvokable]  // Pode ser chamado do JavaScript!
    public static string FormatVerse(string book, int chapter, int number)
    {
        return $"{book} {chapter}:{number}";
    }
    
    [JSInvokable]
    public static Task<string> GetRandomVerseAsync()
    {
        // Lógica assíncrona
        return Task.FromResult("João 3:16");
    }
}
```

```javascript
// JavaScript chamando C#
async function callCSharp() {
    const result = await DotNet.invokeMethodAsync(
        'PalavraConectada.Blazor',  // Assembly name
        'FormatVerse',               // Method name
        'João', 3, 16                // Parameters
    );
    console.log(result);  // "João 3:16"
}
```

**Analogia:** Como **Daniel interpretando sonhos:**
- Rei fala caldeu (JavaScript)
- Daniel responde hebraico (C#)
- Deus traduz (JSRuntime)

---

# 7. STATE MANAGEMENT: GUARDANDO O TESTEMUNHO

## 7.1 Estado Local (Component State)

```razor
@code {
    // Estado privado (como memória pessoal)
    private string searchTerm = "";
    private List<Verse> verses = new();
    private bool loading = false;
    private int currentPage = 1;
    
    // Propriedade computada
    private bool HasVerses => verses.Any();
    private int TotalVerses => verses.Count;
}
```

## 7.2 Estado Compartilhado (AppState Service)

```csharp
// AppState.cs - Singleton compartilhado
public class AppState
{
    // Evento para notificar mudanças
    public event Action? OnChange;
    
    // Estado global
    private List<Verse> favoriteVerses = new();
    public IReadOnlyList<Verse> FavoriteVerses => favoriteVerses.AsReadOnly();
    
    private string currentTheme = "light";
    public string CurrentTheme => currentTheme;
    
    // Métodos para modificar estado
    public void AddFavorite(Verse verse)
    {
        if (!favoriteVerses.Contains(verse))
        {
            favoriteVerses.Add(verse);
            NotifyStateChanged();
        }
    }
    
    public void RemoveFavorite(Verse verse)
    {
        favoriteVerses.Remove(verse);
        NotifyStateChanged();
    }
    
    public void ChangeTheme(string theme)
    {
        currentTheme = theme;
        NotifyStateChanged();
    }
    
    private void NotifyStateChanged()
    {
        OnChange?.Invoke();
    }
}
```

```csharp
// Program.cs - Registrar como singleton
builder.Services.AddSingleton<AppState>();
```

```razor
<!-- Usar no component -->
@inject AppState State
@implements IDisposable

<div class="favorites">
    <h3>Favoritos (@State.FavoriteVerses.Count)</h3>
    
    @foreach (var verse in State.FavoriteVerses)
    {
        <div>@verse.Text</div>
    }
</div>

<button @onclick="() => State.AddFavorite(currentVerse)">
    Adicionar aos Favoritos
</button>

@code {
    protected override void OnInitialized()
    {
        // Escutar mudanças no estado
        State.OnChange += StateHasChanged;
    }
    
    public void Dispose()
    {
        // Cancelar inscrição
        State.OnChange -= StateHasChanged;
    }
}
```

**Analogia:** Como a **Arca do Testemunho:**
- Guardada no Santíssimo (AppState centralizado)
- Acessível a todos (injetado)
- Permanece enquanto templo existe (singleton)
- Todos veem as mesmas tábuas (estado compartilhado)

---

# 8. CICLO DE VIDA: AS FESTAS

## 8.1 Lifecycle Methods

```csharp
public class MyComponent : ComponentBase
{
    // 1. Construtor (nascimento)
    public MyComponent()
    {
        Console.WriteLine("1. Construtor chamado");
    }
    
    // 2. SetParametersAsync (recebe parâmetros)
    public override async Task SetParametersAsync(ParameterView parameters)
    {
        Console.WriteLine("2. Parâmetros sendo setados");
        await base.SetParametersAsync(parameters);
    }
    
    // 3. OnInitialized (inicialização)
    protected override void OnInitialized()
    {
        Console.WriteLine("3. Component inicializado");
        // Executado UMA vez
        // Use para: setup inicial, carregar dados
    }
    
    // 3b. Versão async
    protected override async Task OnInitializedAsync()
    {
        Console.WriteLine("3b. Async init");
        await LoadDataAsync();
    }
    
    // 4. OnParametersSet (parâmetros prontos)
    protected override void OnParametersSet()
    {
        Console.WriteLine("4. Parâmetros setados");
        // Executado TODA vez que parâmetros mudam
    }
    
    protected override async Task OnParametersSetAsync()
    {
        await UpdateBasedOnParametersAsync();
    }
    
    // 5. OnAfterRender (após renderização)
    protected override void OnAfterRender(bool firstRender)
    {
        Console.WriteLine($"5. Renderizado (primeira vez: {firstRender})");
        
        if (firstRender)
        {
            // Só na primeira renderização
            // Use para: JavaScript interop, manipular DOM
        }
    }
    
    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        if (firstRender)
        {
            await JS.InvokeVoidAsync("initializeCharts");
        }
    }
    
    // StateHasChanged() - Forçar re-render
    private void ForceUpdate()
    {
        StateHasChanged();  // "Renderize novamente!"
    }
}
```

**Ordem de Execução:**

```
Constructor
   ⬇️
SetParametersAsync
   ⬇️
OnInitialized/OnInitializedAsync
   ⬇️
OnParametersSet/OnParametersSetAsync
   ⬇️
OnAfterRender/OnAfterRenderAsync
   ⬇️
(Component vivo - responde a eventos)
   ⬇️
OnParametersSet (se parâmetros mudarem)
   ⬇️
Dispose (quando destruído)
```

**Analogia das Festas de Israel:**
- Constructor → Nascimento (como circuncisão)
- OnInitialized → Bar Mitzvah (maioridade)
- OnParametersSet → Casamento (recebe responsabilidades)
- OnAfterRender → Páscoa (celebração completa)
- Dispose → Funeral (fim do ciclo)

---

# 9. RENDERIZAÇÃO CONDICIONAL

## 9.1 Técnicas Avançadas

```razor
<!-- Mostrar/Esconder (elemento existe ou não) -->
@if (showVerses)
{
    <div>Versículos aqui</div>
}

<!-- Display CSS (elemento existe mas escondido) -->
<div style="display: @(showVerses ? "block" : "none")">
    Versículos
</div>

<!-- Loading, Success, Error states -->
@if (loading)
{
    <div class="spinner">⏳ Carregando...</div>
}
else if (error != null)
{
    <div class="error">❌ Erro: @error</div>
}
else if (verses.Any())
{
    @foreach (var verse in verses)
    {
        <VerseCard Verse="@verse" />
    }
}
else
{
    <div class="empty">📭 Nenhum versículo encontrado</div>
}

<!-- Switch para múltiplos estados -->
@switch (currentState)
{
    case State.Loading:
        <LoadingComponent />
        break;
    case State.Success:
        <SuccessComponent Data="@data" />
        break;
    case State.Error:
        <ErrorComponent Message="@errorMessage" />
        break;
}
```

---

# 10. PROJETO REAL: PALAVRA CONECTADA BLAZOR

## 10.1 Estrutura do Nosso Projeto

```
frontend/blazor/
├─ Pages/                      → Páginas roteáveis
│  ├─ Home.razor              → Página inicial
│  ├─ Counter.razor           → Exemplo
│  └─ Weather.razor           → Exemplo
│
├─ Components/Pages/           → Componentes de página
│  ├─ BibleExplorer.razor     → Explorador bíblico
│  └─ EmotionAnalyzer.razor   → Analisador de emoções
│
├─ Layout/                     → Layouts
│  ├─ MainLayout.razor        → Layout principal
│  └─ NavMenu.razor           → Menu navegação
│
├─ Services/                   → Serviços
│  └─ BackendApiService.cs    → Comunicação com API
│
├─ Models/                     → Modelos
│  └─ VerseModels.cs          → DTOs
│
├─ wwwroot/                    → Arquivos estáticos
│  ├─ css/
│  ├─ images/
│  └─ index.html              → Ponto de entrada HTML
│
├─ _Imports.razor             → Imports globais
├─ App.razor                  → Componente raiz
└─ Program.cs                 → Configuração
```

## 10.2 Como o PalavraConectada Funciona

### Fluxo Completo:

```
1. Usuário abre Blazor
   https://calm-wave-0b86b2210.1.azurestaticapps.net
   ⬇️
2. Baixa runtime .NET (~2MB) - PRIMEIRA VEZ
   ⬇️
3. Carrega DLLs do projeto
   ⬇️
4. App.razor inicia
   <Router> encontra rota
   ⬇️
5. Componente da página carrega
   Ex: Home.razor
   ⬇️
6. OnInitialized executa
   Chama BackendApiService
   ⬇️
7. HttpClient faz request
   GET https://palavraconectada-production.up.railway.app/api/Verses/random
   ⬇️
8. Railway responde
   JSON com versículo
   ⬇️
9. Component recebe
   verses = response
   ⬇️
10. StateHasChanged automático
    Template re-renderiza
    ⬇️
11. Usuário vê versículo! 🎉
```

## 10.3 Detecção de Ambiente

```csharp
// BackendApiService.cs
private string GetApiUrl()
{
    var baseUri = _httpClient.BaseAddress?.ToString() ?? "";
    
    _logger.LogInformation($"🔍 BaseAddress: {baseUri}");
    
    // Localhost = desenvolvimento
    if (baseUri.Contains("localhost") || baseUri.Contains("127.0.0.1"))
    {
        _logger.LogInformation("🏠 LOCAL");
        return "http://localhost:7000/api";
    }
    
    // Produção = Railway
    _logger.LogInformation("🌐 PRODUÇÃO");
    return "https://palavraconectada-production.up.railway.app/api";
}
```

**Como Funciona:**
- **Localhost**: `baseUri = "http://localhost:5001"`
- **Azure**: `baseUri = "https://calm-wave-0b86b2210.1.azurestaticapps.net"`
- Detecta que NÃO é localhost
- Usa Railway! ✅

---

# 11. COMPARAÇÃO: ANGULAR vs BLAZOR

## O Que Usar Quando?

### Use Angular Quando:
✅ Grande ecossistema JavaScript  
✅ Equipe já conhece TypeScript  
✅ Muitas bibliotecas JS disponíveis  
✅ SEO importante (com SSR)  
✅ Projeto puramente frontend  

### Use Blazor Quando:
✅ Equipe .NET (compartilhar código)  
✅ Lógica complexa no cliente  
✅ Validações reutilizáveis  
✅ Integração forte com backend .NET  
✅ WebAssembly é aceitável (~2MB)  

## Comparação Lado a Lado:

```
ANGULAR                      BLAZOR
─────────────────────────────────────────
TypeScript                   C#
Components (.ts)             Components (.razor)
Services (Injectable)        Services (Scoped/Singleton)
RxJS (Observable)            Task/async
Template syntax              Razor syntax
npm packages                 NuGet packages
Node.js build                .NET build
@Input/@Output               [Parameter]/EventCallback
*ngIf/*ngFor                 @if/@foreach
FormControl                  EditForm
HttpClient (TS)              HttpClient (C#)
```

**Analogia Bíblica:**

```
ANGULAR              →  Paulo (apóstolo dos gentios)
├─ JavaScript        →  Língua grega
├─ TypeScript        →  Grego aperfeiçoado
└─ Flexível          →  "Tudo a todos" (1 Cor 9:22)

BLAZOR               →  Pedro (apóstolo dos judeus)
├─ C#                →  Língua hebraica
├─ .NET              →  Tradição judaica
└─ Estruturado       →  "Ordem em tudo" (1 Cor 14:40)
```

---

# 12. EXERCÍCIOS PRÁTICOS

## Nível 1: Fundamentos

```razor
<!-- Exercício 1: Contador simples -->
<button @onclick="Incrementar">Cliques: @count</button>

@code {
    private int count = 0;
    
    private void Incrementar()
    {
        count++;
    }
}

<!-- Exercício 2: Lista de nomes -->
<input @bind="novoNome" />
<button @onclick="Adicionar">Adicionar</button>

<ul>
    @foreach (var nome in nomes)
    {
        <li>@nome</li>
    }
</ul>

@code {
    private string novoNome = "";
    private List<string> nomes = new();
    
    private void Adicionar()
    {
        if (!string.IsNullOrWhiteSpace(novoNome))
        {
            nomes.Add(novoNome);
            novoNome = "";
        }
    }
}
```

## Nível 2: Intermediário

```razor
<!-- Exercício 3: Busca com API -->
@inject HttpClient Http

<input @bind="searchTerm" @bind:event="oninput" />

@if (loading)
{
    <p>Buscando...</p>
}
else
{
    @foreach (var result in results)
    {
        <div>@result.Text</div>
    }
}

@code {
    private string searchTerm = "";
    private List<SearchResult> results = new();
    private bool loading = false;
    private Timer? debounceTimer;
    
    private void OnSearchChanged()
    {
        // Debounce manual
        debounceTimer?.Dispose();
        debounceTimer = new Timer(_ => {
            InvokeAsync(async () => await SearchAsync());
        }, null, 300, Timeout.Infinite);
    }
    
    private async Task SearchAsync()
    {
        if (string.IsNullOrWhiteSpace(searchTerm)) return;
        
        loading = true;
        results = await Http.GetFromJsonAsync<List<SearchResult>>(
            $"api/search?q={searchTerm}"
        ) ?? new();
        loading = false;
        StateHasChanged();
    }
}
```

## Nível 3: Avançado

```razor
<!-- Exercício 4: Component reutilizável com generics -->
@typeparam TItem

<div class="list">
    @if (Items == null || !Items.Any())
    {
        <p>@EmptyMessage</p>
    }
    else
    {
        @foreach (var item in Items)
        {
            @ItemTemplate(item)
        }
    }
</div>

@code {
    [Parameter]
    public IEnumerable<TItem>? Items { get; set; }
    
    [Parameter]
    public RenderFragment<TItem> ItemTemplate { get; set; } = default!;
    
    [Parameter]
    public string EmptyMessage { get; set; } = "Nenhum item";
}

<!-- Usar -->
<GenericList Items="@verses" EmptyMessage="Sem versículos">
    <ItemTemplate Context="verse">
        <div class="card">
            <h3>@verse.Book</h3>
            <p>@verse.Text</p>
        </div>
    </ItemTemplate>
</GenericList>
```

---

# 13. CONCLUSÃO

## Checklist de Domínio Blazor

### Básico ⭐
- [ ] Criar projeto Blazor
- [ ] Entender Razor syntax (@)
- [ ] Criar components
- [ ] Data binding (@bind)
- [ ] Event handling (@onclick)
- [ ] Injetar services

### Intermediário ⭐⭐
- [ ] Component parameters ([Parameter])
- [ ] EventCallback
- [ ] Forms com EditForm
- [ ] Validação (DataAnnotations)
- [ ] JavaScript Interop
- [ ] Lifecycle methods
- [ ] HTTP requests

### Avançado ⭐⭐⭐
- [ ] State management global
- [ ] Generic components
- [ ] RenderFragment
- [ ] Custom validators
- [ ] Error boundaries
- [ ] Performance optimization
- [ ] Deploy no Azure

---

> *"Eis que faço novas todas as coisas."* - Apocalipse 21:5

**Com Blazor, você faz C# rodar no navegador - uma nova criação!** 🔥

*Desenvolvido por Alex Feitoza*

