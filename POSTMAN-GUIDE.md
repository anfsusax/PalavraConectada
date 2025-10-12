# 📮 Guia de Uso - Postman Collection

## 🚀 Como Importar no Postman

### **PASSO 1: Abrir o Postman**
1. Abra o **Postman** no seu computador
2. Se não tiver, baixe em: https://www.postman.com/downloads/

### **PASSO 2: Importar a Coleção**
1. Clique em **"Import"** (canto superior esquerdo)
2. Clique em **"Upload Files"**
3. Selecione o arquivo: **`PalavraConectada-API.postman_collection.json`**
4. Clique em **"Import"**

### **PASSO 3: Importar o Environment**
1. Clique novamente em **"Import"**
2. Selecione o arquivo: **`PalavraConectada-Environment.postman_environment.json`**
3. Clique em **"Import"**

### **PASSO 4: Configurar a URL**
1. No canto superior direito, selecione o environment: **"Palavra Conectada - Production"**
2. Clique no ícone de 👁️ (olho) ao lado
3. Clique em **"Edit"**
4. Altere o valor de `baseUrl` para a **URL do Railway**:
   ```
   https://sua-url-real-do-railway.up.railway.app
   ```
5. Clique em **"Save"**

---

## 🧪 Como Testar a API

### **1️⃣ Health Check**
```
📁 Health Check
  → GET Health
```
- Clique em **"Send"**
- Deve retornar: `{ "status": "healthy", ... }`

### **2️⃣ Migrar Dados Bíblicos** ⭐ **FAÇA ISSO PRIMEIRO!**
```
📁 Admin
  → POST Migrar Bíblia - NVI
```
- Clique em **"Send"**
- **Aguarde 1-2 minutos**
- Deve retornar: `{ "success": true, "versesImported": 31102, ... }`

### **3️⃣ Versículo Aleatório**
```
📁 Versículos
  → GET Versículo Aleatório
```
- Clique em **"Send"**
- Retorna um versículo aleatório da Bíblia

### **4️⃣ Buscar por Palavra**
```
📁 Versículos
  → GET Buscar por Palavra-chave
```
- Mude `amor` por outra palavra se quiser
- Clique em **"Send"**

### **5️⃣ Analisar Emoção**
```
📁 Emoções
  → POST Analisar Emoção - Alegria
```
- Veja exemplos de diferentes emoções
- Clique em **"Send"**
- Retorna a emoção detectada e recomendações

### **6️⃣ Recomendação Inteligente** 🌟
```
📁 Versículos
  → POST Recomendação Inteligente
```
- Analisa o texto E retorna versículos relacionados
- Clique em **"Send"**

---

## 📋 Endpoints Disponíveis

### **Health Check**
- ✅ `GET /health` - Verifica se API está funcionando

### **Admin**
- ✅ `POST /api/Admin/migrate` - Importa toda a Bíblia
  - Body: `{ "version": "nvi", "forceReimport": false }`

### **Versículos**
- ✅ `GET /api/Verses/random` - Versículo aleatório
- ✅ `GET /api/Verses/search?keyword=amor` - Buscar por palavra
- ✅ `GET /api/Verses/by-emotion/alegria` - Versículos por emoção
- ✅ `POST /api/Verses/recommend` - Recomendação inteligente
- ✅ `GET /api/Verses/history` - Histórico de interações

### **Emoções**
- ✅ `POST /api/Emotion/analyze` - Detectar emoção em texto
- ✅ `GET /api/Emotion/list` - Listar todas emoções
- ✅ `GET /api/Emotion/{nome}/suggestions` - Sugestões por emoção

---

## 🎯 Ordem Recomendada de Testes

1. ✅ **Health Check** → Confirmar que API está online
2. ✅ **Migrar Bíblia (NVI)** → Popular banco de dados
3. ✅ **Versículo Aleatório** → Testar versículos
4. ✅ **Analisar Emoção** → Testar detecção de emoções
5. ✅ **Recomendação Inteligente** → Testar integração completa
6. ✅ **Buscar por Palavra** → Testar busca
7. ✅ **Listar Emoções** → Ver emoções disponíveis

---

## 🔧 Variáveis de Ambiente

O environment já vem configurado com:

```
baseUrl = https://sua-url-do-railway.up.railway.app
version = nvi
```

**Lembre-se de atualizar a `baseUrl` com a URL real do Railway!**

---

## 📊 Exemplos de Respostas

### Health Check
```json
{
  "status": "healthy",
  "timestamp": "2025-10-12T15:30:00Z",
  "version": "1.0.0",
  "message": "Palavra Conectada API funcionando! 📖"
}
```

### Migração
```json
{
  "success": true,
  "message": "Migração concluída com sucesso!",
  "versesImported": 31102,
  "booksImported": 66,
  "version": "nvi"
}
```

### Versículo Aleatório
```json
{
  "book": "João",
  "chapter": 3,
  "verse": 16,
  "text": "Porque Deus tanto amou o mundo...",
  "version": "nvi"
}
```

### Análise de Emoção
```json
{
  "detectedEmotion": "alegria",
  "confidence": 95,
  "message": "Emoção detectada com sucesso",
  "suggestions": ["feliz", "alegre", "contente"]
}
```

---

**Desenvolvido por Alex Feitoza** 💻  
📖 Palavra Conectada API

