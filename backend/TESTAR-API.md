# 🧪 Testar Backend API - Palavra Conectada

## ✅ API Rodando em: https://localhost:7001

---

## 🎯 TESTE RÁPIDO (30 segundos)

### **1. Health Check**
Abra no navegador: https://localhost:7001/health

Deve retornar:
```json
{
  "status": "healthy",
  "message": "Palavra Conectada API funcionando! 📖"
}
```

### **2. Swagger (Documentação Interativa)**
Abra: https://localhost:7001

Você verá a documentação completa e poderá testar todos os endpoints!

---

## 🧠 TESTE 1: Análise de Emoção

### **Endpoint:** `POST /api/emotion/analyze`

### **Teste 1.1: Tristeza**
```json
POST https://localhost:7001/api/emotion/analyze
Content-Type: application/json

{
  "text": "Estou muito triste hoje"
}
```

**Resultado Esperado:**
```json
{
  "detectedEmotion": "tristeza",
  "confidence": 100,
  "message": "Detectei que você está sentindo tristeza.",
  "recommendations": "consolo",
  "suggestions": [
    "Versículos de consolo e esperança",
    "História de Jó (superação do sofrimento)",
    "Palavras de encorajamento",
    "Oração de conforto"
  ]
}
```

### **Teste 1.2: Alegria**
```json
{
  "text": "Estou muito feliz e animado!"
}
```

### **Teste 1.3: Medo**
```json
{
  "text": "Tenho medo de falhar"
}
```

---

## 📖 TESTE 2: Busca de Versículos

### **Endpoint:** `GET /api/verses/search`

### **Teste 2.1: Buscar "amor"**
```
GET https://localhost:7001/api/verses/search?keyword=amor&version=nvi
```

### **Teste 2.2: Buscar "paz"**
```
GET https://localhost:7001/api/verses/search?keyword=paz&version=nvi
```

---

## 💡 TESTE 3: Recomendação Inteligente

### **Endpoint:** `POST /api/verses/recommend`

### **Teste 3.1: Texto completo**
```json
POST https://localhost:7001/api/verses/recommend
Content-Type: application/json

{
  "text": "Estou sozinho e com medo do futuro",
  "version": "nvi"
}
```

**Resultado:** Analisa emoção + retorna versículo recomendado!

---

## 🎲 TESTE 4: Versículo Aleatório

### **Endpoint:** `GET /api/verses/random`

```
GET https://localhost:7001/api/verses/random?version=nvi
```

---

## 🛠️ FERRAMENTAS DE TESTE

### **1. Swagger UI** ⭐ (Mais Fácil)
- Abra: https://localhost:7001
- Clique em cada endpoint
- Clique em "Try it out"
- Execute e veja o resultado!

### **2. Postman**
- Importe a collection
- Teste cada endpoint
- Salve os testes

### **3. cURL**
```bash
curl -X POST https://localhost:7001/api/emotion/analyze \
  -H "Content-Type: application/json" \
  -d '{"text":"Estou triste"}'
```

### **4. PowerShell**
```powershell
$body = @{ text = "Estou triste" } | ConvertTo-Json

Invoke-RestMethod `
  -Uri "https://localhost:7001/api/emotion/analyze" `
  -Method Post `
  -Body $body `
  -ContentType "application/json"
```

---

## 📊 TESTE DE ESTATÍSTICAS

### **Endpoint:** `GET /api/emotion/stats`

Mostra quais emoções foram mais buscadas:

```json
[
  {
    "emotion": "tristeza",
    "count": 5,
    "lastUsed": "2025-10-12T02:30:00Z"
  },
  {
    "emotion": "alegria",
    "count": 3,
    "lastUsed": "2025-10-12T02:25:00Z"
  }
]
```

---

## ✅ CHECKLIST DE TESTES

- [ ] Health check funcionou
- [ ] Swagger abre corretamente
- [ ] Análise de "Estou triste" detecta tristeza
- [ ] Busca por "amor" retorna versículos
- [ ] Versículo aleatório funciona
- [ ] Recomendação inteligente funciona
- [ ] Estatísticas retornam dados

---

## 🐛 RESOLUÇÃO DE PROBLEMAS

### **API não inicia:**
```bash
dotnet clean
dotnet build
dotnet run
```

### **Erro de banco de dados:**
```bash
# Deletar e recriar
rm bible.db
dotnet run
```

### **Porta em uso:**
Edite `appsettings.json` e mude as portas.

---

*API pronta para testar!* 🚀

**Abra:** https://localhost:7001

