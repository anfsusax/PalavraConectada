# 🚂 Deploy do Backend .NET no Railway

## Passo a Passo Completo

### **PASSO 1: Criar Conta no Railway** 🎫

1. Acesse: **https://railway.app**
2. Clique em **"Login"**
3. Faça login com sua conta **GitHub**
4. Autorize o Railway a acessar seus repositórios

### **PASSO 2: Criar Novo Projeto** 🆕

1. No dashboard do Railway, clique em **"New Project"**
2. Selecione **"Deploy from GitHub repo"**
3. Procure e selecione: **`PalavraConectada`**
4. Aguarde a importação do repositório

### **PASSO 3: Configurar o Serviço** ⚙️

1. Clique no serviço criado
2. Vá em **"Settings"**
3. Configure:

```
┌─────────────────────────────────────────────────────┐
│ Root Directory: backend/PalavraConectada.API        │
│ Build Command: (automático via Dockerfile)          │
│ Start Command: (automático via Dockerfile)          │
└─────────────────────────────────────────────────────┘
```

### **PASSO 4: Variáveis de Ambiente** 🔐

Vá em **"Variables"** e adicione:

```bash
# Ambiente de execução
ASPNETCORE_ENVIRONMENT=Production

# Porta (Railway fornece automaticamente)
PORT=${{PORT}}

# Connection String (SQLite local)
ConnectionStrings__DefaultConnection=Data Source=/app/bible.db
```

### **PASSO 5: Deploy Automático** 🚀

1. O Railway vai automaticamente:
   - ✅ Detectar o Dockerfile
   - ✅ Fazer build da aplicação
   - ✅ Criar container Docker
   - ✅ Fazer deploy

2. Aguarde 3-5 minutos para o build completar

3. Você receberá uma URL como:
   ```
   https://palavraconectada-api-production.up.railway.app
   ```

### **PASSO 6: Testar a API** 🧪

1. Acesse a URL fornecida pelo Railway
2. Você verá o **Swagger UI** na raiz
3. Teste os endpoints:
   - ✅ `/health` - Verificar se está funcionando
   - ✅ `/api/Verses/random` - Buscar versículo aleatório
   - ✅ `/api/Emotion/analyze` - Testar análise de emoções

### **PASSO 7: Configurar Domínio Personalizado (Opcional)** 🌐

1. No Railway, vá em **"Settings"**
2. Clique em **"Generate Domain"**
3. Você pode usar o domínio fornecido ou conectar um domínio próprio

---

## 📊 Arquivos de Configuração Criados

- ✅ **Dockerfile** - Configuração do container
- ✅ **railway.json** - Configuração do Railway
- ✅ **.dockerignore** - Arquivos ignorados no build
- ✅ **Program.cs** - Atualizado para produção

## 🔧 Recursos do Deploy

**✅ O que está incluído:**
- Container Docker otimizado
- .NET 8 Runtime
- SQLite integrado
- CORS configurado para produção
- Swagger habilitado
- Health check endpoint
- Logs automáticos

## ⚠️ Importante - Banco de Dados

O banco SQLite (`bible.db`) precisa estar:
1. ✅ Commitado no repositório (já está)
2. ✅ Copiado para o container (Dockerfile faz isso)
3. ✅ Com dados migrados (usar endpoint `/api/Admin/migrate`)

### Migrar Dados Bíblicos em Produção:

Após o deploy, acesse:
```
POST https://sua-url.railway.app/api/Admin/migrate
Body: { "version": "nvi", "forceReimport": false }
```

---

## 🔗 Próximos Passos

Após o deploy do backend:

1. ✅ Anote a URL da API
2. ✅ Atualize o frontend Angular com a nova URL
3. ✅ Faça deploy do frontend na Vercel
4. ✅ Teste a integração completa

---

## 📈 Monitoramento

O Railway fornece:
- 📊 Logs em tempo real
- 📉 Métricas de uso
- 🔔 Alertas de erro
- 💰 Uso do plano gratuito

**Free Tier:** 500 horas/mês (suficiente para projetos pessoais)

---

## 🐛 Solução de Problemas

### Build falha:
- Verifique se o Dockerfile está na raiz do projeto
- Confirme que o .csproj existe

### API não responde:
- Verifique a variável PORT
- Confirme CORS configurado corretamente

### Banco vazio:
- Execute a migração via endpoint `/api/Admin/migrate`
- Verifique se o bible.db foi copiado

---

**Desenvolvido por Alex Feitoza** 💻
📖 Palavra Conectada - API

