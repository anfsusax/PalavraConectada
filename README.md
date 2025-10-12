# 📖 Palavra Conectada - Dois Caminhos, Um Propósito

> **"Há diversidade de dons, mas o Espírito é o mesmo"** (1 Coríntios 12:4)

---

## 🌟 Visão Geral do Projeto

**Palavra Conectada** é uma aplicação web que conecta pessoas à Palavra de Deus através da tecnologia. Digite uma palavra ou sentimento e encontre versículos bíblicos relacionados!

Este repositório contém **DUAS implementações completas**:
- 🅰️ **Angular** (TypeScript/JavaScript)
- 🔷 **Blazor** (C#/.NET)

Ambas consomem a mesma API e oferecem a mesma experiência, mas com paradigmas diferentes!

---

## 📚 História Bíblica: Os Dois Caminhos para Jerusalém

### A Parábola dos Dois Caminhos

Imagine dois peregrinos indo para Jerusalém:

**Peregrino Angular (via costa do mar)**
- Passa por muitas cidades (frameworks JS, TypeScript)
- Vê muitas pessoas (grande comunidade)
- Caminho bem estabelecido (maduro, estável)
- Muitas paradas (muitos pacotes npm)

**Peregrino Blazor (via montanhas)**
- Caminho mais direto (C# puro)
- Menos multidão (comunidade menor, mas crescente)
- Trilha mais nova (tecnologia recente)
- Mochila mais leve (WebAssembly)

**Ambos chegam a Jerusalém** (aplicação funcional), mas por rotas diferentes!

---

## 🎯 O Que Cada Projeto Ensina

### Angular - O Templo de Salomão
*Complexo, grandioso, com muitas salas especializadas*

**Aprenda sobre:**
- TypeScript e tipagem gradual
- RxJS e programação reativa (Observables)
- Decoradores (@Component, @Injectable)
- Dependency Injection
- Módulos e componentes standalone
- Data binding bidirecional

### Blazor - A Tenda do Encontro
*Mais simples, direto, focado no essencial*

**Aprenda sobre:**
- C# no navegador com WebAssembly
- Razor syntax (HTML + C#)
- async/await para operações assíncronas
- Dependency Injection no .NET
- Componentes Razor
- Data binding com @bind

---

## 📊 Comparação Técnica - Lado a Lado

| Aspecto | Angular | Blazor |
|---------|---------|--------|
| **Linguagem** | TypeScript | C# |
| **Sintaxe de Template** | HTML + `{{ }}` | Razor + `@` |
| **Data Binding** | `[(ngModel)]` | `@bind` |
| **Eventos** | `(click)="method()"` | `@onclick="Method"` |
| **Loops** | `*ngFor` | `@foreach` |
| **Condicionais** | `*ngIf` | `@if` |
| **Serviços** | Classes com @Injectable | Classes registradas no DI |
| **HTTP** | HttpClient do Angular | HttpClient do .NET |
| **Async** | Observables (subscribe) | Task/async-await |

---

## 🔍 Comparação de Código

### Exemplo 1: Data Binding (Input de Texto)

**Angular:**
```typescript
// TypeScript
searchTerm: string = '';

// HTML
<input [(ngModel)]="searchTerm" />
```

**Blazor:**
```csharp
// C#
private string searchTerm = string.Empty;

// Razor
<input @bind="searchTerm" />
```

### Exemplo 2: Buscar Versículos

**Angular:**
```typescript
searchVerses(): void {
  this.bibleApiService.searchVerses(this.searchTerm, this.version)
    .subscribe({
      next: (result) => {
        this.searchResult = result;
      },
      error: (error) => {
        console.error('Erro:', error);
      }
    });
}
```

**Blazor:**
```csharp
private async Task SearchVerses()
{
    try
    {
        var result = await BibleApi.SearchVersesAsync(searchTerm, version);
        searchResult = result;
    }
    catch (Exception ex)
    {
        Console.WriteLine($"Erro: {ex.Message}");
    }
}
```

### Exemplo 3: Loop de Versículos

**Angular:**
```html
<div *ngFor="let verse of searchResult.verses; let i = index" 
     class="verse-card">
  <div class="verse-reference">
    {{ getVerseReference(verse) }}
  </div>
  <div class="verse-text">
    "{{ verse.text }}"
  </div>
</div>
```

**Blazor:**
```razor
@foreach (var verse in searchResult.Verses)
{
    <div class="verse-card">
        <div class="verse-reference">
            @GetVerseReference(verse)
        </div>
        <div class="verse-text">
            "@verse.Text"
        </div>
    </div>
}
```

---

## 🏗️ Estrutura dos Projetos

### Angular
```
palavra-conectada-angular/
├── src/
│   ├── app/
│   │   ├── components/
│   │   │   └── verse-search/
│   │   │       ├── verse-search.component.ts    # Lógica
│   │   │       ├── verse-search.component.html  # Template
│   │   │       └── verse-search.component.css   # Estilos
│   │   ├── services/
│   │   │   └── bible-api.service.ts             # API
│   │   ├── models/
│   │   │   └── verse.model.ts                   # Tipos
│   │   ├── app.ts                               # App raiz
│   │   └── app.config.ts                        # Configuração
│   └── styles.css                               # Estilos globais
└── package.json                                 # Dependências
```

### Blazor
```
palavra-conectada-blazor/
├── Components/
│   └── Pages/
│       ├── VerseSearch.razor                    # Componente completo
│       └── VerseSearch.razor.css                # Estilos
├── Services/
│   └── BibleApiService.cs                       # API
├── Models/
│   └── VerseModels.cs                           # Classes
├── Layout/
│   ├── MainLayout.razor                         # Layout principal
│   └── NavMenu.razor                            # Navegação
├── Program.cs                                   # Configuração
└── palavra-conectada-blazor.csproj              # Projeto
```

---

## 🚀 Como Executar

### Angular

```bash
cd palavra-conectada-angular

# Instalar dependências
npm install

# Executar em desenvolvimento
npm start
# ou
ng serve

# Abrir no navegador
http://localhost:4200
```

### Blazor

```bash
cd palavra-conectada-blazor

# Executar em desenvolvimento
dotnet run
# ou com hot reload
dotnet watch

# Abrir no navegador
https://localhost:5001
```

---

## 📖 Lições Espirituais e Técnicas

### Lição 1: Unidade na Diversidade
> **"Há um só corpo e um só Espírito"** (Efésios 4:4)

Assim como o corpo de Cristo tem muitos membros com funções diferentes, temos **Angular e Blazor** - tecnologias diferentes, mas servindo ao mesmo propósito!

### Lição 2: Use Suas Habilidades
> **"Cada um exerça o dom que recebeu"** (1 Pedro 4:10)

- Conhece JavaScript? → Use Angular!
- Conhece C#? → Use Blazor!
- Quer aprender ambos? → Estude os dois!

### Lição 3: O Fruto é o Mesmo
> **"Pelos seus frutos os conhecereis"** (Mateus 7:16)

Ambas as aplicações produzem o **mesmo resultado**: conectar pessoas à Palavra de Deus!

### Lição 4: Separação de Responsabilidades
> **"Tudo, porém, seja feito com decência e ordem"** (1 Coríntios 14:40)

Ambos os projetos seguem princípios de:
- **Componentes** = Interface
- **Serviços** = Lógica de negócios
- **Modelos** = Estrutura de dados

### Lição 5: Async é Como Oração
> **"Perseverai na oração"** (Colossenses 4:2)

**Angular (Observables):**
- Como uma oração contínua (stream de dados)
- Você se "inscreve" (subscribe) e aguarda respostas

**Blazor (async/await):**
- Como uma oração específica (Task)
- Você faz a requisição e aguarda (await) a resposta

---

## 🎓 O Que Você Vai Aprender

### Conceitos de Angular
1. **TypeScript** - JavaScript com tipos
2. **Decorators** - Metadados (@Component, @Injectable)
3. **RxJS** - Programação reativa com Observables
4. **NgModules** - Organização em módulos
5. **Standalone Components** - Componentes independentes
6. **Two-way Binding** - Sincronização automática

### Conceitos de Blazor
1. **C# no navegador** - WebAssembly em ação
2. **Razor Syntax** - Mistura de HTML e C#
3. **Component Model** - Componentes reutilizáveis
4. **Dependency Injection** - Injeção de dependências .NET
5. **async/await** - Programação assíncrona moderna
6. **Strongly Typed** - Tipagem forte em tudo

---

## 🌐 API Utilizada

**A Bíblia Digital**
- 🌍 Website: https://www.abibliadigital.com.br/
- 📘 Documentação: https://github.com/omarciovsena/abibliadigital
- 🆓 Gratuita e open source
- 📚 7 versões bíblicas
- 🌍 4 idiomas

**Endpoints usados:**
```
GET /api/verses/{version}/search/{term}  - Buscar versículos
GET /api/verses/{version}/random          - Versículo aleatório
GET /api/versions                         - Listar versões
```

---

## 🎨 Features Implementadas

✅ Busca de versículos por palavra-chave  
✅ Busca em múltiplas versões (NVI, ACF, AA)  
✅ Versículo aleatório  
✅ Interface responsiva e moderna  
✅ Animações suaves  
✅ Sugestões de busca  
✅ Tratamento de erros  
✅ Loading states  
✅ Design gradient bonito  
✅ Compatível com mobile  

---

## 💡 Próximos Passos

### Funcionalidades Futuras
- [ ] Favoritar versículos
- [ ] Histórico de buscas
- [ ] Compartilhar versículos
- [ ] Versículo do dia
- [ ] Comparar versões lado a lado
- [ ] Modo escuro
- [ ] Exportar versículos (PDF, imagem)
- [ ] Notas pessoais
- [ ] Plano de leitura

### Melhorias Técnicas
- [ ] Testes unitários (Angular: Jasmine/Karma, Blazor: xUnit)
- [ ] Testes E2E (Cypress para Angular, Playwright para Blazor)
- [ ] CI/CD (GitHub Actions)
- [ ] PWA (Progressive Web App)
- [ ] Deploy (Netlify/Vercel para Angular, Azure para Blazor)

---

## 🎯 Para Quem é Este Projeto?

### 👨‍🎓 Estudantes
Aprenda comparando duas abordagens diferentes para o mesmo problema!

### 👨‍💻 Desenvolvedores
- **Frontend JS** → Veja como C# se compara
- **Backend C#** → Veja como fazer frontend com C#
- **Full-stack** → Aprenda ambas as stacks!

### 🙏 Cristãos Desenvolvedores
Combine sua fé com sua profissão criando ferramentas que glorificam a Deus!

### 🏫 Professores
Use como material didático para ensinar frameworks modernos!

---

## 📚 Recursos de Aprendizado

### Angular
- [Documentação Oficial](https://angular.io/)
- [Angular University](https://angular-university.io/)
- [Angular Brasil](https://github.com/angular/angular-pt)

### Blazor
- [Documentação Oficial](https://dotnet.microsoft.com/apps/aspnet/web-apps/blazor)
- [Blazor School](https://blazorschool.com/)
- [Awesome Blazor](https://github.com/AdrienTorris/awesome-blazor)

---

## 🙏 Versículo Final

> **"Lâmpada para os meus pés é a tua palavra e luz para o meu caminho."**
> 
> *Salmos 119:105*

Este projeto existe para ser uma **lâmpada digital** que ilumina pessoas com a Palavra de Deus, usando as ferramentas modernas que Ele nos deu!

---

## 🤝 Contribuindo

Contribuições são bem-vindas! Seja você um desenvolvedor Angular, Blazor ou ambos!

**Como contribuir:**
1. Fork o projeto
2. Crie uma branch (`git checkout -b feature/NovaFuncionalidade`)
3. Commit suas mudanças (`git commit -m 'Adiciona nova funcionalidade'`)
4. Push para a branch (`git push origin feature/NovaFuncionalidade`)
5. Abra um Pull Request

---

## 📄 Licença

Este projeto é de domínio público. Use, modifique e compartilhe para a glória de Deus!

---

## 👨‍💻 Autor

Desenvolvido com ❤️ e ☕ para ensinar e conectar pessoas à Palavra.

---

## 🌟 Agradecimentos

- **A Bíblia Digital** pela API gratuita e abençoada
- **Angular Team** pela framework incrível
- **Microsoft** pelo Blazor revolucionário
- **Deus** pela inspiração e sabedoria

---

*"Portanto, quer comais, quer bebais ou façais outra coisa qualquer, fazei tudo para a glória de Deus."* - 1 Coríntios 10:31

**Que este código glorifique ao Senhor! 🙏**

