# 🔥 Palavra Conectada API - Backend

> "A palavra de Deus é viva e eficaz" - Hebreus 4:12

Backend API inteligente com análise de emoções e recomendação de versículos bíblicos.

---

## 🚀 Execução Rápida

```bash
cd backend/PalavraConectada.API
dotnet run
```

**Swagger:** https://localhost:7001  
**API:** https://localhost:7001/api  
**Health:** https://localhost:7001/health

---

## 📋 Endpoints Disponíveis

### **1. 🧠 Análise de Emoções**

#### POST `/api/emotion/analyze`
Analisa texto e detecta emoção.

**Request:**
```json
{
  "text": "Estou muito triste hoje"
}
```

**Response:**
```json
{
  "detectedEmotion": "tristeza",
  "confidence": 100,
  "message": "Detectei que você está sentindo tristeza.",
  "recommendationType": "consolo",
  "suggestions": [
    "Versículos de consolo e esperança",
    "História de Jó (superação do sofrimento)"
  ]
}
```

#### GET `/api/emotion/list`
Lista todas as emoções disponíveis.

---

### **2. 📖 Versículos**

#### GET `/api/verses/search?keyword=amor&version=nvi`
Busca versículos por palavra-chave.

**Response:**
```json
{
  "keyword": "amor",
  "version": "nvi",
  "count": 3,
  "verses": [
    {
      "bookName": "João",
      "chapter": 3,
      "number": 16,
      "text": "Porque Deus tanto amou o mundo..."
    }
  ]
}
```

#### GET `/api/verses/by-emotion/tristeza?limit=5`
Busca versículos por emoção.

#### GET `/api/verses/random?version=nvi`
Retorna versículo aleatório.

---

### **3. 💡 Recomendação Inteligente**

#### POST `/api/verses/recommend`
Análise completa + recomendação de versículo.

**Request:**
```json
{
  "text": "Me sinto sozinho e com medo",
  "version": "nvi"
}
```

**Response:**
```json
{
  "userInput": "Me sinto sozinho e com medo",
  "detectedEmotion": "medo",
  "confidence": 100,
  "recommendedVerse": {
    "bookName": "Salmos",
    "chapter": 23,
    "number": 1,
    "text": "O Senhor é o meu pastor..."
  },
  "alternativeVerses": [...],
  "suggestions": [
    "Versículos de coragem e proteção",
    "História de Davi e Golias"
  ]
}
```

---

## 🧠 Análise de Emoções

### **Emoções Detectáveis:**
- tristeza
- alegria
- medo
- ansiedade
- solidão
- raiva
- gratidão
- esperança

### **Como Funciona:**
1. Usuário escreve texto livre
2. Sistema analisa palavras-chave
3. Detecta emoção com % de confiança
4. Retorna sugestões personalizadas

---

## 🌐 Sistema de Busca Otimizado

```
1º → Cache em Memória (ultra-rápido)
2º → Banco de Dados SQLite (toda a Bíblia migrada)
```

**100% local!** Toda a Bíblia está no banco de dados, sem dependência de APIs externas.

---

## 📊 Banco de Dados

**SQLite** (`bible.db`) com tabelas:
- `Verses` - Versículos cacheados
- `Emotions` - 8 emoções pré-configuradas
- `VerseEmotions` - Relacionamento versículo ↔ emoção
- `BibleStories` - Histórias bíblicas
- `UserInteractions` - Histórico de uso

---

## 🔧 Tecnologias

- **ASP.NET Core 8.0** - Framework
- **Entity Framework Core** - ORM
- **SQLite** - Banco de dados (Bíblia completa)
- **Swagger** - Documentação automática
- **Cache em Memória** - Otimização de performance

---

## 📚 Exemplos de Uso

### **C# (Console):**
```csharp
var client = new HttpClient();
var response = await client.PostAsJsonAsync(
    "https://localhost:7001/api/emotion/analyze",
    new { text = "Estou triste" }
);
```

### **JavaScript/TypeScript:**
```typescript
const response = await fetch('https://localhost:7001/api/emotion/analyze', {
  method: 'POST',
  headers: { 'Content-Type': 'application/json' },
  body: JSON.stringify({ text: 'Estou triste' })
});
```

### **cURL:**
```bash
curl -X POST https://localhost:7001/api/emotion/analyze \
  -H "Content-Type: application/json" \
  -d '{"text":"Estou triste"}'
```

---

## 🎯 Próximos Passos

- [ ] Popular banco com mais versículos
- [ ] Implementar busca na API inglesa
- [ ] Machine Learning para melhorar detecção
- [ ] Autenticação JWT
- [ ] Rate limiting
- [ ] Cache Redis

---

*Desenvolvido com ❤️ e C# para conectar pessoas à Palavra*

