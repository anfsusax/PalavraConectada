# 🚀 Deploy do Blazor no Azure Static Web Apps

## ✨ Por que Azure Static Web Apps?

- ✅ **Feito para Blazor** - Suporte nativo da Microsoft
- ✅ **SSL Grátis** - HTTPS automático
- ✅ **CI/CD Automático** - Deploy via GitHub Actions
- ✅ **Free Tier Generoso** - 100 GB bandwidth/mês
- ✅ **Domínio Personalizado** - Gratuito

---

## 📋 Passo a Passo Completo

### **PASSO 1: Acessar Azure Portal** 🌐

1. Acesse: **https://portal.azure.com**
2. Faça login com sua conta Microsoft
   - Se não tiver, crie em: **https://azure.microsoft.com/free/**
   - Free tier não precisa de cartão de crédito!

---

### **PASSO 2: Criar Static Web App** ➕

1. No Azure Portal, clique em **"Create a resource"** (Criar um recurso)
2. Busque por: **"Static Web App"**
3. Clique em **"Static Web App"** → **"Create"**

---

### **PASSO 3: Configurar o Projeto** ⚙️

#### **Aba: Basics**

```
┌──────────────────────────────────────────────────┐
│ Subscription: (sua assinatura Azure)            │
│ Resource Group: [Create new] PalavraConectada   │
│ Name: palavraconectada-blazor                   │
│ Plan Type: Free                                  │
│ Region: Central US (ou mais próximo)            │
└──────────────────────────────────────────────────┘
```

#### **Aba: Deployment Details**

```
┌──────────────────────────────────────────────────┐
│ Source: GitHub                                   │
│ [Sign in with GitHub]  ← CLIQUE AQUI            │
│                                                  │
│ Organization: (seu usuário GitHub)               │
│ Repository: PalavraConectada                    │
│ Branch: main                                     │
└──────────────────────────────────────────────────┘
```

#### **Aba: Build Details**

```
┌──────────────────────────────────────────────────┐
│ Build Presets: Blazor                            │
│                                                  │
│ App location: /frontend/blazor                   │
│ Api location: (deixe vazio)                      │
│ Output location: wwwroot                         │
└──────────────────────────────────────────────────┘
```

---

### **PASSO 4: Review + Create** ✅

1. Clique em **"Review + create"**
2. Revise as configurações
3. Clique em **"Create"**
4. Aguarde 2-3 minutos

---

### **PASSO 5: Obter URL** 🌐

Após criação:

1. Vá em **"Overview"** (Visão geral)
2. Copie a **URL** gerada:
   ```
   https://palavraconectada-blazor-<hash>.azurestaticapps.net
   ```

---

## 🔄 Deploy Automático

Após configurar, **QUALQUER commit** na pasta `frontend/blazor/` vai:

1. ✅ Triggerar GitHub Actions automaticamente
2. ✅ Fazer build do Blazor
3. ✅ Deploy no Azure
4. ✅ Aplicação atualizada em ~3 minutos!

---

## 📊 Verificar Deploy

### **Via Azure Portal:**

1. No recurso criado, vá em **"GitHub Action runs"**
2. Veja os logs do último deploy
3. Status deve estar: **"Succeeded"** ✅

### **Via GitHub:**

1. No repositório, vá em **"Actions"**
2. Veja o workflow: **"Azure Static Web Apps - Blazor"**
3. Deve estar: **"Success"** ✅

---

## 🧪 Testar a Aplicação

1. Acesse a URL gerada pelo Azure
2. A aplicação Blazor deve carregar
3. Teste as funcionalidades:
   - ✅ Busca de versículos
   - ✅ Análise de emoções
   - ✅ Recomendações inteligentes

---

## 🔐 Segurança e CORS

O arquivo `staticwebapp.config.json` já está configurado com:

- ✅ Content Security Policy
- ✅ MIME types corretos para Blazor WASM
- ✅ Permissão para API no Railway
- ✅ Navegação SPA (fallback para index.html)

---

## 🎯 URLs do Projeto Completo

Após todos os deploys:

```
Backend API:
https://palavraconectada-production.up.railway.app

Frontend Angular:
https://palavra-conectada-angular.vercel.app

Frontend Blazor:
https://palavraconectada-blazor-<hash>.azurestaticapps.net
```

---

## 💡 Dicas

### **Domínio Personalizado (Opcional):**

1. No Azure, vá em **"Custom domains"**
2. Adicione seu domínio
3. Configure DNS apontando para Azure
4. SSL automático em minutos!

### **Ver Logs:**

1. Azure Portal → Seu recurso
2. **"Log stream"** para ver logs em tempo real

### **Reverter Deploy:**

1. GitHub Actions → Selecione deploy antigo
2. **"Re-run all jobs"**

---

## ❓ Solução de Problemas

### **Build Falha:**
- Verifique logs no GitHub Actions
- Confirme que .NET 8 está configurado

### **App não carrega:**
- Verifique `Output location` = `wwwroot`
- Confirme que `App location` = `/frontend/blazor`

### **CORS Error:**
- Verifique URL da API no `BackendApiService.cs`
- Confirme que Railway está aceitando requisições do Azure

---

## 📞 Suporte

- Azure Docs: https://docs.microsoft.com/azure/static-web-apps/
- GitHub Issues: Abra issue no repositório
- Documentação Blazor: https://docs.microsoft.com/aspnet/core/blazor/

---

**Desenvolvido por Alex Feitoza** 💻  
📖 Palavra Conectada - Frontend Blazor

