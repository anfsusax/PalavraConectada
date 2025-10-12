# 📖 Palavra Conectada - Blazor

## O que é este projeto?

**Palavra Conectada** é uma aplicação que permite buscar versículos bíblicos relacionados a palavras ou frases que você digita. Esta versão usa **Blazor WebAssembly** e C#!

---

## 🌟 História Bíblica: Davi e Golias - A Força do C#

Você lembra de **Davi e Golias** (1 Samuel 17)? Davi era pequeno mas poderoso!

O **Blazor** é como Davi:
- **Pequeno e eficiente** (WebAssembly compacto)
- **Poderoso** (toda a força do .NET no navegador)
- **Simples** (usa apenas C#, sem JavaScript)
- **Preciso** (tipagem forte, menos erros)

Assim como Davi venceu com uma pedra, Blazor conquista com C#!

---

## 🏗️ Arquitetura - Como as 12 Tribos

### 1. **Componentes Razor** (As Tribos Unidas)

```razor
@page "/verse-search"
@inject BibleApiService BibleApi

<div>
    <input @bind="searchTerm" />
    <button @onclick="SearchVerses">Buscar</button>
</div>

@code {
    private string searchTerm = "";
    
    private async Task SearchVerses() {
        // Lógica aqui
    }
}
```

**Estrutura de um arquivo Razor:**
- **HTML**: A parte visual (markup)
- **@code**: A lógica em C#
- **@inject**: Recebe serviços (Dependency Injection)

### 2. **Serviços** (Os Profetas que Servem)

```csharp
public class BibleApiService
{
    private readonly HttpClient _httpClient;
    
    public async Task<SearchResult?> SearchVersesAsync(string searchTerm)
    {
        var result = await _httpClient.GetFromJsonAsync<SearchResult>(url);
        return result;
    }
}
```

Os serviços são como os **profetas de Israel** - fazem o trabalho pesado e trazem a mensagem.

### 3. **Modelos** (As Leis de Moisés)

```csharp
public class Verse
{
    public Book? Book { get; set; }
    public int Chapter { get; set; }
    public int Number { get; set; }
    public string Text { get; set; } = string.Empty;
}
```

Classes fortemente tipadas - cada propriedade tem seu tipo definido!

---

## 🔄 Como Funciona - O Fluxo de Dados

### A Jornada do Maná (Êxodo 16)

Assim como Deus enviava maná do céu para o povo:

1. **Usuário faz a requisição** 
   → O povo pede pão

2. **Componente chama o serviço**
   → Moisés ora a Deus

3. **Serviço busca na API**
   → Deus envia o maná

4. **API retorna os dados**
   → O maná cai do céu

5. **Componente atualiza a UI**
   → O povo come e se alegra!

---

## 🎨 Conceitos Importantes do Blazor

### 1. **Data Binding** - A Nova Aliança

```razor
<input @bind="searchTerm" />
<input @bind="searchTerm" @bind:event="oninput" />
```

O `@bind` é uma **aliança bilateral**:
- One-way: `@bind-value="searchTerm"`
- Two-way: `@bind="searchTerm"`

### 2. **Event Handlers** - Respondendo ao Chamado

```razor
<button @onclick="SearchVerses">Buscar</button>
```

Assim como os profetas respondiam ao chamado de Deus, os eventos respondem às ações do usuário!

### 3. **Dependency Injection** - O Espírito Santo

```csharp
@inject BibleApiService BibleApi
```

O Blazor **injeta automaticamente** os serviços, assim como o Espírito nos **capacita** com dons!

### 4. **Async/Await** - A Paciência de Jó

```csharp
private async Task SearchVerses()
{
    var result = await BibleApi.SearchVersesAsync(searchTerm);
}
```

Como **Jó esperou pacientemente**, usamos `async/await` para esperar respostas sem travar a interface!

---

## 🚀 Como Executar

### Pré-requisitos
- .NET 8.0 SDK instalado

### Comandos

```bash
# Restaurar dependências
dotnet restore

# Executar em modo desenvolvimento
dotnet run
# ou
dotnet watch

# Abrir no navegador
https://localhost:5001
```

---

## 📚 Funcionalidades

### 1. **Busca por Palavra-chave**
Digite qualquer palavra (amor, fé, paz) e encontre versículos relacionados.

### 2. **Versículo Aleatório**
Deixe Deus surpreender você com uma palavra específica para o momento!

### 3. **Múltiplas Versões**
- NVI (Nova Versão Internacional)
- ACF (Almeida Corrigida Fiel)
- AA (Almeida Revista e Atualizada)

### 4. **Interface Bonita**
Design moderno e responsivo, funcionando em qualquer dispositivo.

---

## 🎓 Aprendizados - Lições Espirituais e Técnicas

### Lição 1: Tipagem Forte
> "Sim, sim; não, não" (Mateus 5:37)

C# exige tipos definidos - não há ambiguidade, assim como Jesus era direto!

### Lição 2: Async/Await
> "Os que esperam no Senhor renovam as forças" (Isaías 40:31)

Esperar assincronamente renova a interface sem travamentos!

### Lição 3: Componentes Reutilizáveis
> "Ajuntai os pedaços que sobejaram, para que nada se perca" (João 6:12)

Reutilize componentes para não desperdiçar código!

---

## 🔥 Blazor vs JavaScript

### História: Daniel na Cova dos Leões

**Blazor** está na "cova dos leões" (navegador dominado por JavaScript), mas:

| Blazor (Daniel) | JavaScript (Leões) |
|-----------------|-------------------|
| C# puro | JavaScript/TypeScript |
| WebAssembly | Engine JS nativa |
| Tipagem forte | Tipagem fraca/opcional |
| .NET no navegador | Limitado ao JS |

Assim como Deus fechou a boca dos leões, Blazor mostra que C# pode dominar o navegador!

---

## 🌐 API Utilizada

**A Bíblia Digital**: https://www.abibliadigital.com.br/

Uma API RESTful gratuita com:
- 7 versões da Bíblia
- 4 idiomas
- Busca por palavras-chave
- Versículos aleatórios

---

## 🙏 Versículo Final

> **"A tua palavra é a verdade desde o princípio"**
> 
> *Salmos 119:160*

Que este projeto seja um testemunho de como a tecnologia pode servir à Palavra!

---

## 💡 Próximos Passos

- [ ] Adicionar favoritos
- [ ] Compartilhar versículos
- [ ] Modo offline
- [ ] Notas pessoais
- [ ] Plano de leitura

---

*Desenvolvido com ❤️ e fé em C#*

