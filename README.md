# 📖 Palavra Conectada AI

> "Lâmpada para os meus pés é a tua palavra e luz para o meu caminho." - Salmos 119:105

🔥 **Aplicação web com Inteligência Artificial** que conecta pessoas à Palavra de Deus através de:
- 🧠 **Busca Inteligente com IA** - Analisa emoções e recomenda versículos
- 📚 **Biblioteca Bíblica Completa** - Organizada por testamentos e temas
- 🎯 **Backend C# com SQLite** - API própria com 66 livros da Bíblia
- 🌐 **Dois Frontends** - Angular (TypeScript) e Blazor (C# WebAssembly)

---

## 🚀 Início Rápido

### **🏠 Página Home (Escolha sua Tecnologia):**
```bash
# Abra o arquivo index.html no navegador
start index.html
```
→ **Escolha entre Blazor e Angular com design moderno!**

### **1. Backend API (C#):**
```bash
cd backend/PalavraConectada.API
dotnet run
```
→ **http://localhost:7000**  
→ **Swagger:** http://localhost:7000/swagger

### **2. Blazor WebAssembly:**
```bash
cd frontend/blazor
dotnet run
```
→ **http://localhost:5292**  
🎯 **Destaque:** http://localhost:5292/bible-library

### **3. Angular:**
```bash
cd frontend/angular
npm install
npm start
```
→ **http://localhost:4200**

---

## 📚 **TODA A DOCUMENTAÇÃO:**

👉 **[`docs/INDEX.md`](docs/INDEX.md)** - Índice completo de toda documentação  
👉 **[`docs/INICIO-RAPIDO.md`](docs/INICIO-RAPIDO.md)** - Começar agora  
👉 **[`docs/HOME-PAGE-GUIDE.md`](docs/HOME-PAGE-GUIDE.md)** - Guia da página home  
👉 **[`docs/TESTE-BIBLIOTECA-BIBLICA.md`](docs/TESTE-BIBLIOTECA-BIBLICA.md)** - Testar nova feature

---

## 📁 Estrutura do Projeto

```
PalavraConectada/
├── index.html                       # 🏠 PÁGINA HOME - Escolha Blazor ou Angular
├── backend/                         # 🔥 API C# com IA
│   └── PalavraConectada.API/
│       ├── Controllers/             # AdminController, BibleLibraryController, EmotionController, VersesController
│       ├── Services/                # EmotionAnalyzerService, BibleService, BibleMigrationService
│       ├── Data/                    # BibleDbContext, SeedData
│       ├── Models/                  # Verse, DTOs
│       └── bible.db                 # SQLite Database (66 livros)
│
├── frontend/
│   ├── angular/                     # 🅰️ Angular 19 (TypeScript)
│   │   ├── components/              # VerseSearchComponent
│   │   ├── services/                # BackendApiService
│   │   └── models/                  # Verse, EmotionAnalysis, Recommendation
│   │
│   └── blazor/                      # 🔥 Blazor WebAssembly (C#)
│       ├── Components/Pages/        # VerseSearch, BibleLibrary
│       ├── Services/                # BackendApiService
│       └── Layout/                  # NavMenu
│
├── docs/                            # 📚 TODA A DOCUMENTAÇÃO (INDEX.md)
├── scripts/                         # 🔧 Scripts PowerShell (ignorado)
└── referencias/                     # 📦 Código de terceiros (ignorado)
```

---

## 🎯 Funcionalidades

### **✅ IMPLEMENTADO:**

#### **Backend API C#:**
- 🧠 **Análise de Emoções com IA** - Detecta tristeza, alegria, medo, ansiedade, etc.
- 📚 **Banco SQLite** - 30+ versículos pré-populados (66 livros disponíveis para migração)
- 🔄 **Fallback Inteligente** - APIs externas com retry automático
- 📊 **Swagger UI** - Documentação interativa da API
- 🎯 **4 Endpoints de Biblioteca:**
  - `/api/BibleLibrary/old-testament` - Velho Testamento
  - `/api/BibleLibrary/new-testament` - Novo Testamento
  - `/api/BibleLibrary/theme/prosperity` - Riqueza & Prosperidade
  - `/api/BibleLibrary/theme/salvation` - Plano de Salvação

#### **Frontend Blazor:**
- 📖 **Busca Inteligente** - Com análise de emoções e recomendações
- 📚 **Biblioteca Bíblica** - 4 categorias temáticas com design moderno
- 🙏 **Oração de Salvação Interativa** - Box especial com mensagem de boas-vindas
- 🎲 **Versículo Aleatório** - "Surpreenda-me"
- 🎨 **UI Moderna** - Cards coloridos, animações, responsivo

#### **Frontend Angular:**
- 🧠 **Mesmas funcionalidades do Blazor** em TypeScript
- 🎨 **Interface moderna** e responsiva
- 🔍 **Busca simples e inteligente**

#### **Geral:**
- ✅ Múltiplas versões da Bíblia (NVI, ACF, AA)
- ✅ Histórico de interações
- ✅ CORS configurado para todos os frontends
- ✅ Projeto totalmente organizado e profissional

---

## 🔧 Tecnologias

- **Backend:** ASP.NET Core 8.0, Entity Framework Core, SQLite
- **Frontend Blazor:** Blazor WebAssembly, C# 12
- **Frontend Angular:** Angular 19, TypeScript 5
- **Banco:** SQLite (local)
- **APIs Externas:** A Bíblia Digital (fallback)

---

## 📖 Como Começar?

1. **Clone o repositório:**
   ```bash
   git clone https://github.com/anfsusax/PalavraConectada.git
   cd PalavraConectada
   ```

2. **Leia a documentação:**
   - 📚 [`docs/INDEX.md`](docs/INDEX.md) - Índice completo
   - ⚡ [`docs/INICIO-RAPIDO.md`](docs/INICIO-RAPIDO.md) - Guia de início
   - 🎯 [`docs/TESTE-BIBLIOTECA-BIBLICA.md`](docs/TESTE-BIBLIOTECA-BIBLICA.md) - Teste a nova feature

3. **Execute:**
   - Backend: `cd backend/PalavraConectada.API && dotnet run`
   - Blazor: `cd frontend/blazor && dotnet run`
   - Angular: `cd frontend/angular && npm start`

---

## 🎯 Próximas Funcionalidades

- [ ] Busca por livro específico
- [ ] Histórico de leituras salvo
- [ ] Versículos favoritos
- [ ] Compartilhamento social
- [ ] Plano de leitura anual
- [ ] Notas pessoais por versículo

---

## 📝 Licença

**MIT License** - Use para a glória de Deus! 🙏

---

## 🤝 Contribuindo

Contribuições são bem-vindas! Leia [`docs/INDEX.md`](docs/INDEX.md) para começar.

---

**✨ Repositório:** https://github.com/anfsusax/PalavraConectada  
**🎓 Projeto Educacional:** Angular vs Blazor com IA e Bíblia  
**❤️ Desenvolvido com:** C#, TypeScript, SQLite e muita oração ☕

---

*"A palavra de Deus é viva e eficaz" - Hebreus 4:12*
