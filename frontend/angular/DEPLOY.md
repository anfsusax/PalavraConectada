# 🚀 Deploy do PalavraConectada - Frontend Angular

## Deploy na Vercel

### Opção 1: Deploy via Interface Web (Recomendado)

1. Acesse [vercel.com](https://vercel.com)
2. Clique em "Add New" > "Project"
3. Importe o repositório: `https://github.com/anfsusax/PalavraConectada`
4. Configure o projeto:
   - **Framework Preset**: Angular
   - **Root Directory**: `frontend/angular`
   - **Build Command**: `npm run vercel-build`
   - **Output Directory**: `dist/palavra-conectada-angular/browser`
5. Adicione as variáveis de ambiente:
   - `API_BASE_URL`: URL da sua API backend (por enquanto: `http://localhost:7000/api`)
6. Clique em "Deploy"

### Opção 2: Deploy via CLI

```bash
# Instalar Vercel CLI
npm install -g vercel

# Na pasta frontend/angular
cd frontend/angular

# Login na Vercel
vercel login

# Deploy
vercel --prod
```

## ⚙️ Configuração da API Backend

**IMPORTANTE**: O frontend precisa da API backend funcionando para trabalhar corretamente.

### Opções para o Backend:

1. **Railway** (Recomendado para .NET)
   - Suporta .NET nativamente
   - Deploy automático via GitHub
   - Free tier disponível

2. **Azure App Service**
   - Perfeito para .NET
   - Integração com GitHub Actions

3. **Render**
   - Suporta Docker
   - Free tier disponível

## 📝 Próximos Passos

1. ✅ Deploy do Frontend Angular na Vercel
2. ⏳ Deploy do Backend .NET (Railway/Azure/Render)
3. ⏳ Atualizar a URL da API no service
4. ⏳ Testar integração completa

## 🔧 Configuração Manual da URL da API

Edite o arquivo `src/app/services/backend-api.service.ts`:

```typescript
// Trocar de:
private readonly API_BASE_URL = 'http://localhost:7000/api';

// Para:
private readonly API_BASE_URL = 'https://sua-api-backend.railway.app/api';
```

## 📊 Status

- ✅ Configuração Vercel criada
- ✅ Script de build configurado
- ⏳ Backend precisa de deploy
- ⏳ URLs precisam ser atualizadas

---

**Desenvolvido por Alex Feitoza** 💻

