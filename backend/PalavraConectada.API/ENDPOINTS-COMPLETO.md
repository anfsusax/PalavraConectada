# 📖 Documentação Completa de Endpoints - Palavra Conectada API

## 🎯 Visão Geral

Esta API fornece acesso completo à Bíblia Sagrada em português, com funcionalidades de busca, análise de emoções e recomendações inteligentes de versículos.

**Base URL:** `http://localhost:8080/api`

---

## 📊 ADMIN - Endpoints Administrativos

### 1. GET /api/Admin/stats

**O que faz:** Retorna estatísticas completas do banco de dados (versículos, livros, versões, testamentos).

**Método:** `GET`

**Parâmetros:** Nenhum

**Exemplo de Requisição:**
```http
GET http://localhost:8080/api/Admin/stats
```

**Exemplo de Resposta:**
```json
{
  "summary": "Banco com 93315 versículos de 66 livros",
  "verses": 93315,
  "books": 66,
  "emotions": 8,
  "relationships": 0,
  "byVersion": {
    "nvi": 31105,
    "aa": 31104,
    "acf": 31106
  },
  "byTestament": {
    "VT": 69442,
    "NT": 23873
  },
  "databaseSize": "N/A"
}
```

**Campos da Resposta:**
- `summary`: Resumo textual das estatísticas
- `verses`: Total de versículos no banco
- `books`: Total de livros (sempre 66)
- `emotions`: Total de emoções cadastradas
- `relationships`: Total de relacionamentos versículo-emoção
- `byVersion`: Quantidade de versículos por versão (nvi, aa, acf)
- `byTestament`: Quantidade de versículos por testamento (VT, NT)
- `databaseSize`: Tamanho do arquivo do banco

---

### 2. POST /api/Admin/migrate

**O que faz:** Migra toda a Bíblia de uma versão específica para o banco de dados. Importa todos os ~31.102 versículos automaticamente.

**Método:** `POST`

**Body (JSON):**
```json
{
  "version": "nvi",
  "forceReimport": false
}
```

**Parâmetros:**
- `version` (string, obrigatório): Versão da Bíblia (`nvi`, `aa`, `acf`)
- `forceReimport` (boolean, opcional): Se `true`, reimporta versículos já existentes

**Exemplo de Requisição:**
```http
POST http://localhost:8080/api/Admin/migrate
Content-Type: application/json

{
  "version": "nvi",
  "forceReimport": false
}
```

**Exemplo de Resposta (Sucesso):**
```json
{
  "success": true,
  "message": "✅ Migração concluída! 31105 versículos importados.",
  "versesImported": 31105,
  "booksImported": 66,
  "version": "nvi",
  "duration": "00:05:23"
}
```

**Exemplo de Resposta (Erro):**
```json
{
  "success": false,
  "error": "Não foi possível carregar os livros. Verifique se os arquivos JSON estão em biblia-master/json/"
}
```

**Notas:**
- A migração pode levar vários minutos
- Versículos já existentes são ignorados (a menos que `forceReimport` seja `true`)
- Cada versão é armazenada separadamente no banco

---

### 3. POST /api/Admin/migrate-book

**O que faz:** Migra um livro específico da Bíblia. Útil para migração controlada livro por livro.

**Método:** `POST`

**Body (JSON):**
```json
{
  "bookAbbrev": "jo",
  "bookName": "João",
  "chapters": 21,
  "author": "João",
  "group": "Evangelhos",
  "testament": "NT",
  "version": "nvi"
}
```

**Parâmetros:**
- `bookAbbrev` (string, obrigatório): Abreviação do livro (ex: "jo", "gn", "sl")
- `bookName` (string, obrigatório): Nome completo do livro
- `chapters` (int, obrigatório): Número de capítulos do livro
- `author` (string, opcional): Autor do livro
- `group` (string, opcional): Grupo do livro (Pentateuco, Evangelhos, etc.)
- `testament` (string, obrigatório): "VT" ou "NT"
- `version` (string, obrigatório): Versão da Bíblia

**Exemplo de Requisição:**
```http
POST http://localhost:8080/api/Admin/migrate-book
Content-Type: application/json

{
  "bookAbbrev": "jo",
  "bookName": "João",
  "chapters": 21,
  "author": "João",
  "group": "Evangelhos",
  "testament": "NT",
  "version": "nvi"
}
```

**Exemplo de Resposta:**
```json
{
  "success": true,
  "book": "João",
  "versesAdded": 879,
  "versesSkipped": 0,
  "message": "✅ João migrado com sucesso! 879 versículos adicionados."
}
```

---

### 4. DELETE /api/Admin/clear-verses

**O que faz:** Remove TODOS os versículos do banco de dados. Use com cuidado! Emoções e relacionamentos são preservados.

**Método:** `DELETE`

**Parâmetros:** Nenhum

**Exemplo de Requisição:**
```http
DELETE http://localhost:8080/api/Admin/clear-verses
```

**Exemplo de Resposta:**
```json
{
  "success": true,
  "message": "✅ 93315 versículos removidos com sucesso!",
  "versesDeleted": 93315,
  "duration": "00:00:01.234",
  "statsBefore": {
    "totalVerses": 93315,
    "books": 66,
    "byVersion": {
      "nvi": 31105,
      "aa": 31104,
      "acf": 31106
    }
  },
  "warning": "⚠️ Todos os versículos foram removidos. Execute a migração novamente para popular o banco."
}
```

**⚠️ Atenção:** Esta operação é irreversível!

---

### 5. DELETE /api/Admin/clear-verses/{version}

**O que faz:** Remove apenas os versículos de uma versão específica do banco de dados.

**Método:** `DELETE`

**Parâmetros na URL:**
- `version` (string): Versão a ser removida (`nvi`, `aa`, `acf`)

**Exemplo de Requisição:**
```http
DELETE http://localhost:8080/api/Admin/clear-verses/nvi
```

**Exemplo de Resposta:**
```json
{
  "success": true,
  "message": "✅ 31105 versículos da versão 'nvi' removidos com sucesso!",
  "version": "nvi",
  "versesDeleted": 31105,
  "duration": "00:00:00.987",
  "statsBefore": {
    "totalVerses": 93315,
    "versesInVersion": 31105
  },
  "warning": "⚠️ Versículos da versão 'nvi' foram removidos. Execute a migração novamente para popular."
}
```

---

## 📖 VERSES - Busca e Recomendação de Versículos

### 6. GET /api/Verses/search

**O que faz:** Busca versículos que contêm uma palavra-chave específica no texto.

**Método:** `GET`

**Parâmetros de Query:**
- `keyword` (string, obrigatório): Palavra a buscar (ex: "amor", "fé", "paz")
- `version` (string, opcional): Versão da Bíblia (padrão: "nvi")

**Exemplo de Requisição:**
```http
GET http://localhost:8080/api/Verses/search?keyword=amor&version=nvi
```

**Exemplo de Resposta:**
```json
{
  "keyword": "amor",
  "version": "nvi",
  "count": 10,
  "verses": [
    {
      "id": 12345,
      "bookName": "João",
      "bookAbbrev": "jo",
      "chapter": 3,
      "number": 16,
      "text": "Porque Deus tanto amou o mundo que deu o seu Filho Unigênito, para que todo o que nele crer não pereça, mas tenha a vida eterna.",
      "version": "nvi",
      "author": "João",
      "group": "Evangelhos",
      "testament": "NT"
    },
    {
      "id": 12346,
      "bookName": "1 Coríntios",
      "bookAbbrev": "1co",
      "chapter": 13,
      "number": 4,
      "text": "O amor é paciente, o amor é bondoso. Não inveja, não se vangloria, não se orgulha.",
      "version": "nvi",
      "author": "Paulo",
      "group": "Cartas Paulinas",
      "testament": "NT"
    }
  ]
}
```

**Notas:**
- Retorna até 10 versículos por padrão
- A busca é case-insensitive (não diferencia maiúsculas/minúsculas)
- Busca primeiro no banco local, depois nos arquivos JSON

---

### 7. GET /api/Verses/by-emotion/{emotionName}

**O que faz:** Retorna versículos relacionados a uma emoção específica, ordenados por relevância.

**Método:** `GET`

**Parâmetros na URL:**
- `emotionName` (string): Nome da emoção (ex: "tristeza", "alegria", "medo")

**Parâmetros de Query:**
- `version` (string, opcional): Versão da Bíblia (padrão: "nvi")
- `limit` (int, opcional): Quantidade máxima de versículos (padrão: 10)

**Exemplo de Requisição:**
```http
GET http://localhost:8080/api/Verses/by-emotion/tristeza?version=nvi&limit=5
```

**Exemplo de Resposta:**
```json
[
  {
    "id": 789,
    "bookName": "Salmos",
    "bookAbbrev": "sl",
    "chapter": 34,
    "number": 18,
    "text": "O Senhor está perto dos que têm o coração quebrantado e salva os de espírito abatido.",
    "version": "nvi",
    "author": "Diversos",
    "group": "Poéticos",
    "testament": "VT"
  },
  {
    "id": 790,
    "bookName": "Salmos",
    "bookAbbrev": "sl",
    "chapter": 23,
    "number": 4,
    "text": "Mesmo quando eu andar por um vale de trevas e morte, não temerei perigo algum, pois tu estás comigo; a tua vara e o teu cajado me protegem.",
    "version": "nvi",
    "author": "Diversos",
    "group": "Poéticos",
    "testament": "VT"
  }
]
```

**Emoções Disponíveis:**
- tristeza
- alegria
- medo
- ansiedade
- solidão
- raiva
- gratidão
- esperança

---

### 8. GET /api/Verses/random

**O que faz:** Retorna um versículo aleatório do banco de dados.

**Método:** `GET`

**Parâmetros de Query:**
- `version` (string, opcional): Versão da Bíblia (padrão: "nvi")

**Exemplo de Requisição:**
```http
GET http://localhost:8080/api/Verses/random?version=nvi
```

**Exemplo de Resposta:**
```json
{
  "id": 4567,
  "bookName": "Filipenses",
  "bookAbbrev": "fp",
  "chapter": 4,
  "number": 13,
  "text": "Tudo posso naquele que me fortalece.",
  "version": "nvi",
  "author": "Paulo",
  "group": "Cartas Paulinas",
  "testament": "NT"
}
```

**Notas:**
- Se não houver versículos no banco, retorna dados MOCK
- A seleção é verdadeiramente aleatória

---

### 9. POST /api/Verses/recommend

**O que faz:** Sistema inteligente que analisa o texto do usuário, detecta a emoção e recomenda versículos apropriados.

**Método:** `POST`

**Body (JSON):**
```json
{
  "text": "Estou muito triste hoje",
  "version": "nvi"
}
```

**Parâmetros:**
- `text` (string, obrigatório): Texto do usuário para análise
- `version` (string, opcional): Versão da Bíblia (padrão: "nvi")

**Exemplo de Requisição:**
```http
POST http://localhost:8080/api/Verses/recommend
Content-Type: application/json

{
  "text": "Estou muito triste hoje",
  "version": "nvi"
}
```

**Exemplo de Resposta:**
```json
{
  "userInput": "Estou muito triste hoje",
  "detectedEmotion": "tristeza",
  "confidence": 0.95,
  "recommendedVerse": {
    "id": 789,
    "bookName": "Salmos",
    "bookAbbrev": "sl",
    "chapter": 34,
    "number": 18,
    "text": "O Senhor está perto dos que têm o coração quebrantado e salva os de espírito abatido.",
    "version": "nvi",
    "author": "Diversos",
    "group": "Poéticos",
    "testament": "VT"
  },
  "alternativeVerses": [
    {
      "id": 790,
      "bookName": "Salmos",
      "chapter": 23,
      "number": 4,
      "text": "Mesmo quando eu andar por um vale de trevas e morte..."
    }
  ],
  "suggestions": [
    "Ore pedindo consolo",
    "Leia Salmos 23",
    "Busque apoio na comunidade"
  ],
  "message": "Deus está perto de você neste momento difícil."
}
```

**Campos da Resposta:**
- `userInput`: Texto original do usuário
- `detectedEmotion`: Emoção detectada pelo sistema
- `confidence`: Nível de confiança (0.0 a 1.0)
- `recommendedVerse`: Versículo principal recomendado
- `alternativeVerses`: Versículos alternativos (até 3)
- `suggestions`: Sugestões de ações
- `message`: Mensagem personalizada

---

### 10. GET /api/Verses/history

**O que faz:** Retorna o histórico de interações do usuário com o sistema.

**Método:** `GET`

**Parâmetros de Query:**
- `limit` (int, opcional): Quantidade de registros (padrão: 10)

**Exemplo de Requisição:**
```http
GET http://localhost:8080/api/Verses/history?limit=20
```

**Exemplo de Resposta:**
```json
[
  {
    "id": 1,
    "userInput": "Estou triste",
    "detectedEmotion": "tristeza",
    "recommendedVerseReference": "Salmos 34:18",
    "createdAt": "2024-01-15T10:30:00Z"
  },
  {
    "id": 2,
    "userInput": "Estou muito feliz!",
    "detectedEmotion": "alegria",
    "recommendedVerseReference": "Salmos 100:1",
    "createdAt": "2024-01-15T11:15:00Z"
  }
]
```

---

### 11. POST /api/Verses/search-all

**O que faz:** Busca completa de todas as ocorrências de uma palavra na Bíblia, agrupadas por livro.

**Método:** `POST`

**Body (JSON):**
```json
{
  "keyword": "amor",
  "version": "nvi"
}
```

**Parâmetros:**
- `keyword` (string, obrigatório): Palavra a buscar
- `version` (string, obrigatório): Versão da Bíblia

**Exemplo de Requisição:**
```http
POST http://localhost:8080/api/Verses/search-all
Content-Type: application/json

{
  "keyword": "amor",
  "version": "nvi"
}
```

**Exemplo de Resposta:**
```json
{
  "keyword": "amor",
  "version": "nvi",
  "totalOccurrences": 245,
  "booksFound": 45,
  "books": [
    {
      "book": "João",
      "testament": "NT",
      "occurrences": 12,
      "verses": [
        {
          "chapter": 3,
          "verse": 16,
          "text": "Porque Deus tanto amou o mundo que deu o seu Filho Unigênito...",
          "reference": "João 3:16"
        },
        {
          "chapter": 13,
          "verse": 34,
          "text": "Um novo mandamento lhes dou: Amem-se uns aos outros...",
          "reference": "João 13:34"
        }
      ]
    },
    {
      "book": "1 Coríntios",
      "testament": "NT",
      "occurrences": 8,
      "verses": [
        {
          "chapter": 13,
          "verse": 4,
          "text": "O amor é paciente, o amor é bondoso...",
          "reference": "1 Coríntios 13:4"
        }
      ]
    }
  ],
  "summary": "Encontrado 'amor' em 245 versículo(s) de 45 livro(s) da Bíblia"
}
```

**Notas:**
- Retorna TODAS as ocorrências encontradas
- Agrupa por livro para facilitar navegação
- Ordena por livro, capítulo e versículo

---

### 12. POST /api/Verses/generate-motivational

**O que faz:** Gera uma frase motivacional personalizada baseada no texto do usuário e emoção detectada.

**Método:** `POST`

**Body (JSON):**
```json
{
  "text": "Estou passando por um momento difícil",
  "version": "nvi"
}
```

**Exemplo de Requisição:**
```http
POST http://localhost:8080/api/Verses/generate-motivational
Content-Type: application/json

{
  "text": "Estou passando por um momento difícil",
  "version": "nvi"
}
```

**Exemplo de Resposta:**
```json
{
  "userInput": "Estou passando por um momento difícil",
  "detectedEmotion": "tristeza",
  "confidence": 0.88,
  "motivationalPhrase": "Lembre-se: 'O Senhor está perto dos que têm o coração quebrantado.' (Salmos 34:18). Deus está perto de você neste momento difícil.",
  "versesUsed": [
    {
      "reference": "Salmos 34:18",
      "text": "O Senhor está perto dos que têm o coração quebrantado e salva os de espírito abatido.",
      "author": "Diversos"
    }
  ],
  "suggestions": [
    "Ore pedindo consolo",
    "Leia Salmos 23",
    "Busque apoio na comunidade"
  ]
}
```

**Notas:**
- A frase motivacional é gerada dinamicamente baseada na emoção
- Combina versículos bíblicos com mensagens encorajadoras
- Cada emoção tem frases específicas pré-configuradas

---

## 📚 BIBLE LIBRARY - Biblioteca Bíblica Organizada

### 13. GET /api/BibleLibrary/old-testament

**O que faz:** Lista todos os 39 livros do Velho Testamento com suas informações.

**Método:** `GET`

**Exemplo de Requisição:**
```http
GET http://localhost:8080/api/BibleLibrary/old-testament
```

**Exemplo de Resposta:**
```json
{
  "testament": "Velho Testamento",
  "totalBooks": 39,
  "books": [
    {
      "bookName": "Gênesis",
      "bookAbbrev": "gn",
      "author": "Moisés",
      "group": "Pentateuco"
    },
    {
      "bookName": "Êxodo",
      "bookAbbrev": "ex",
      "author": "Moisés",
      "group": "Pentateuco"
    },
    {
      "bookName": "Levítico",
      "bookAbbrev": "lv",
      "author": "Moisés",
      "group": "Pentateuco"
    }
  ]
}
```

---

### 14. GET /api/BibleLibrary/new-testament

**O que faz:** Lista todos os 27 livros do Novo Testamento com suas informações.

**Método:** `GET`

**Exemplo de Requisição:**
```http
GET http://localhost:8080/api/BibleLibrary/new-testament
```

**Exemplo de Resposta:**
```json
{
  "testament": "Novo Testamento",
  "totalBooks": 27,
  "books": [
    {
      "bookName": "Mateus",
      "bookAbbrev": "mt",
      "author": "Mateus",
      "group": "Evangelhos"
    },
    {
      "bookName": "Marcos",
      "bookAbbrev": "mc",
      "author": "Marcos",
      "group": "Evangelhos"
    }
  ]
}
```

---

### 15. GET /api/BibleLibrary/theme/prosperity

**O que faz:** Retorna 8 versículos aleatórios sobre riqueza e prosperidade.

**Método:** `GET`

**Exemplo de Requisição:**
```http
GET http://localhost:8080/api/BibleLibrary/theme/prosperity
```

**Exemplo de Resposta:**
```json
{
  "theme": "Riqueza & Prosperidade",
  "description": "Versículos sobre bênçãos, prosperidade e abundância em Deus (Aleatórios)",
  "count": 8,
  "totalAvailable": 245,
  "verses": [
    {
      "id": 1234,
      "bookName": "Malaquias",
      "bookAbbrev": "ml",
      "chapter": 3,
      "number": 10,
      "text": "Tragam o dízimo todo ao depósito do templo, para que haja alimento em minha casa. Ponham-me à prova", diz o Senhor dos Exércitos, "e vejam se não vou abrir as comportas dos céus e derramar sobre vocês tantas bênçãos que nem terão onde guardá-las.",
      "version": "nvi",
      "author": "Malaquias",
      "group": "Profetas Menores",
      "testament": "VT"
    },
    {
      "id": 1235,
      "bookName": "Deuteronômio",
      "bookAbbrev": "dt",
      "chapter": 28,
      "number": 8,
      "text": "O Senhor enviará bênçãos sobre os seus celeiros e sobre tudo o que você fizer. O Senhor, o seu Deus, os abençoará na terra que está dando a você.",
      "version": "nvi",
      "author": "Moisés",
      "group": "Pentateuco",
      "testament": "VT"
    }
  ]
}
```

**Notas:**
- Os versículos são aleatórios a cada requisição
- Busca por palavras: riqueza, prosperar, abundância, bênção, multiplicar, fartura

---

### 16. GET /api/BibleLibrary/theme/salvation

**O que faz:** Retorna 8 versículos aleatórios sobre salvação em Jesus Cristo, incluindo os passos do plano de salvação.

**Método:** `GET`

**Exemplo de Requisição:**
```http
GET http://localhost:8080/api/BibleLibrary/theme/salvation
```

**Exemplo de Resposta:**
```json
{
  "theme": "Salvação em Jesus Cristo",
  "description": "O caminho da salvação explicado através das Escrituras (Aleatórios)",
  "steps": [
    "1. Deus ama você (João 3:16)",
    "2. Todos pecaram (Romanos 3:23)",
    "3. O salário do pecado é a morte (Romanos 6:23)",
    "4. Cristo morreu por você (Romanos 5:8)",
    "5. Confesse e creia (Romanos 10:9)",
    "6. Salvação pela graça (Efésios 2:8-9)"
  ],
  "count": 8,
  "totalAvailable": 156,
  "verses": [
    {
      "id": 2345,
      "bookName": "João",
      "bookAbbrev": "jo",
      "chapter": 3,
      "number": 16,
      "text": "Porque Deus tanto amou o mundo que deu o seu Filho Unigênito, para que todo o que nele crer não pereça, mas tenha a vida eterna.",
      "version": "nvi",
      "author": "João",
      "group": "Evangelhos",
      "testament": "NT"
    }
  ]
}
```

---

### 17. GET /api/BibleLibrary/book/{bookAbbrev}/chapters

**O que faz:** Lista todos os capítulos disponíveis de um livro específico.

**Método:** `GET`

**Parâmetros na URL:**
- `bookAbbrev` (string): Abreviação do livro (ex: "jo", "gn", "sl")

**Exemplo de Requisição:**
```http
GET http://localhost:8080/api/BibleLibrary/book/jo/chapters
```

**Exemplo de Resposta:**
```json
{
  "bookAbbrev": "jo",
  "bookName": "João",
  "totalChapters": 21,
  "chapters": [1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16, 17, 18, 19, 20, 21]
}
```

**Exemplo de Resposta (Livro não encontrado):**
```json
{
  "message": "Livro 'xyz' não encontrado no banco"
}
```

**Status Code:** 404 Not Found

---

### 18. GET /api/BibleLibrary/book/{bookAbbrev}/chapter/{chapterNumber}

**O que faz:** Retorna todos os versículos de um capítulo específico, ordenados por número.

**Método:** `GET`

**Parâmetros na URL:**
- `bookAbbrev` (string): Abreviação do livro
- `chapterNumber` (int): Número do capítulo

**Exemplo de Requisição:**
```http
GET http://localhost:8080/api/BibleLibrary/book/jo/chapter/3
```

**Exemplo de Resposta:**
```json
{
  "bookAbbrev": "jo",
  "bookName": "João",
  "chapter": 3,
  "count": 36,
  "verses": [
    {
      "id": 123,
      "bookName": "João",
      "bookAbbrev": "jo",
      "chapter": 3,
      "number": 1,
      "text": "Havia um fariseu chamado Nicodemos, uma autoridade entre os judeus.",
      "version": "nvi",
      "author": "João",
      "group": "Evangelhos",
      "testament": "NT"
    },
    {
      "id": 124,
      "bookName": "João",
      "bookAbbrev": "jo",
      "chapter": 3,
      "number": 2,
      "text": "Ele veio a Jesus, à noite, e disse: 'Mestre, sabemos que ensinas da parte de Deus, pois ninguém pode realizar os sinais milagrosos que estás fazendo, se Deus não estiver com ele.'",
      "version": "nvi",
      "author": "João",
      "group": "Evangelhos",
      "testament": "NT"
    },
    {
      "id": 125,
      "bookName": "João",
      "bookAbbrev": "jo",
      "chapter": 3,
      "number": 3,
      "text": "Em resposta, Jesus declarou: 'Digo a verdade: Ninguém pode ver o Reino de Deus, se não nascer de novo.'",
      "version": "nvi",
      "author": "João",
      "group": "Evangelhos",
      "testament": "NT"
    }
  ]
}
```

**Notas:**
- Retorna todos os versículos do capítulo
- Ordenados por número do versículo
- Pode retornar múltiplas versões se estiverem no banco

---

### 19. GET /api/BibleLibrary/search

**O que faz:** Busca versículos por palavra-chave na biblioteca, limitado a 20 resultados.

**Método:** `GET`

**Parâmetros de Query:**
- `keyword` (string, obrigatório): Palavra-chave a buscar

**Exemplo de Requisição:**
```http
GET http://localhost:8080/api/BibleLibrary/search?keyword=amor
```

**Exemplo de Resposta:**
```json
{
  "keyword": "amor",
  "count": 20,
  "verses": [
    {
      "id": 456,
      "bookName": "João",
      "bookAbbrev": "jo",
      "chapter": 3,
      "number": 16,
      "text": "Porque Deus tanto amou o mundo que deu o seu Filho Unigênito, para que todo o que nele crer não pereça, mas tenha a vida eterna.",
      "version": "nvi",
      "author": "João",
      "group": "Evangelhos",
      "testament": "NT"
    }
  ]
}
```

**Exemplo de Resposta (Erro):**
```json
{
  "message": "Palavra-chave não pode ser vazia"
}
```

**Status Code:** 400 Bad Request

---

## 😊 EMOTION - Análise de Emoções

### 20. POST /api/Emotion/analyze

**O que faz:** Analisa o texto do usuário e detecta a emoção predominante usando análise de palavras-chave.

**Método:** `POST`

**Body (JSON):**
```json
{
  "text": "Estou muito feliz hoje!"
}
```

**Parâmetros:**
- `text` (string, obrigatório): Texto a ser analisado

**Exemplo de Requisição:**
```http
POST http://localhost:8080/api/Emotion/analyze
Content-Type: application/json

{
  "text": "Estou muito feliz hoje!"
}
```

**Exemplo de Resposta:**
```json
{
  "detectedEmotion": "alegria",
  "confidence": 0.92,
  "message": "Sua emoção foi identificada como alegria!",
  "recommendationType": "encouragement",
  "suggestions": [
    "Compartilhe sua alegria com outros",
    "Leia Salmos de louvor",
    "Agradeça a Deus pela sua felicidade"
  ],
  "interactionId": 123
}
```

**Campos da Resposta:**
- `detectedEmotion`: Emoção detectada
- `confidence`: Nível de confiança (0.0 a 1.0)
- `message`: Mensagem personalizada
- `recommendationType`: Tipo de recomendação (encouragement, comfort, etc.)
- `suggestions`: Lista de sugestões de ações
- `interactionId`: ID da interação salva no banco

**Emoções Detectáveis:**
- tristeza
- alegria
- medo
- ansiedade
- solidão
- raiva
- gratidão
- esperança

---

### 21. GET /api/Emotion/list

**O que faz:** Lista todas as emoções disponíveis no sistema com suas descrições.

**Método:** `GET`

**Exemplo de Requisição:**
```http
GET http://localhost:8080/api/Emotion/list
```

**Exemplo de Resposta:**
```json
[
  {
    "id": 1,
    "name": "tristeza",
    "description": "Sentimento de melancolia ou desânimo"
  },
  {
    "id": 2,
    "name": "alegria",
    "description": "Sentimento de felicidade e contentamento"
  },
  {
    "id": 3,
    "name": "medo",
    "description": "Sentimento de temor ou ansiedade"
  },
  {
    "id": 4,
    "name": "ansiedade",
    "description": "Estado de preocupação ou inquietação"
  },
  {
    "id": 5,
    "name": "solidão",
    "description": "Sentimento de isolamento ou falta de companhia"
  },
  {
    "id": 6,
    "name": "raiva",
    "description": "Sentimento de irritação ou fúria"
  },
  {
    "id": 7,
    "name": "gratidão",
    "description": "Sentimento de agradecimento e reconhecimento"
  },
  {
    "id": 8,
    "name": "esperança",
    "description": "Sentimento de expectativa positiva"
  }
]
```

---

### 22. GET /api/Emotion/{emotionName}/suggestions

**O que faz:** Retorna sugestões específicas para uma emoção.

**Método:** `GET`

**Parâmetros na URL:**
- `emotionName` (string): Nome da emoção

**Exemplo de Requisição:**
```http
GET http://localhost:8080/api/Emotion/tristeza/suggestions
```

**Exemplo de Resposta:**
```json
[
  "Ore pedindo consolo",
  "Leia Salmos 23",
  "Busque apoio na comunidade",
  "Lembre-se que Deus está perto"
]
```

**Exemplo de Resposta (Emoção não encontrada):**
```json
{
  "error": "Emoção 'xyz' não encontrada"
}
```

**Status Code:** 404 Not Found

---

### 23. GET /api/Emotion/stats

**O que faz:** Retorna estatísticas de uso das emoções (quais são mais buscadas).

**Método:** `GET`

**Exemplo de Requisição:**
```http
GET http://localhost:8080/api/Emotion/stats
```

**Exemplo de Resposta:**
```json
[
  {
    "emotion": "tristeza",
    "count": 45,
    "lastUsed": "2024-01-15T14:30:00Z"
  },
  {
    "emotion": "alegria",
    "count": 32,
    "lastUsed": "2024-01-15T12:15:00Z"
  },
  {
    "emotion": "ansiedade",
    "count": 28,
    "lastUsed": "2024-01-15T10:20:00Z"
  },
  {
    "emotion": "medo",
    "count": 15,
    "lastUsed": "2024-01-14T18:45:00Z"
  }
]
```

**Campos da Resposta:**
- `emotion`: Nome da emoção
- `count`: Quantidade de vezes que foi detectada
- `lastUsed`: Data/hora da última detecção

---

## 📝 Notas Importantes

### Versões Disponíveis
- **nvi**: Nova Versão Internacional
- **aa**: Almeida Atualizada
- **acf**: Almeida Corrigida e Fiel

### Abreviações de Livros Comuns
- **gn**: Gênesis
- **ex**: Êxodo
- **sl**: Salmos
- **jo**: João
- **rm**: Romanos
- **1co**: 1 Coríntios
- **2co**: 2 Coríntios
- **fp**: Filipenses

### Códigos de Status HTTP
- **200 OK**: Requisição bem-sucedida
- **400 Bad Request**: Parâmetros inválidos ou faltando
- **404 Not Found**: Recurso não encontrado
- **500 Internal Server Error**: Erro no servidor

### Limites e Performance
- Buscas retornam até 10-20 resultados por padrão
- Migrações podem levar vários minutos
- Versículos são armazenados por versão separadamente
- Cache automático de buscas frequentes

---

## 🔗 Links Úteis

- **Swagger UI**: `http://localhost:8080`
- **Health Check**: `http://localhost:8080/health`
- **Base URL**: `http://localhost:8080/api`

---

**Última atualização:** Janeiro 2024  
**Versão da API:** 1.1

