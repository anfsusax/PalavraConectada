# 🚀 Push para GitHub - Palavra Conectada

## 📋 Comandos para Enviar ao GitHub

### **Repositório:** https://github.com/anfsusax/PalavraConectada.git

---

## ⚡ Comandos (Execute na ordem):

```powershell
# 1. Ir para pasta do projeto
cd "G:\projetos\aulas\PalavraConectada"

# 2. Inicializar Git (se ainda não foi)
git init

# 3. Adicionar remote (seu repositório)
git remote add origin https://github.com/anfsusax/PalavraConectada.git

# 4. Verificar status
git status

# 5. Adicionar todos os arquivos
git add .

# 6. Fazer commit
git commit -m "feat: Projeto Palavra Conectada - Angular e Blazor organizados com MOCK funcionando"

# 7. Enviar para GitHub (branch main)
git push -u origin main
```

---

## 🔒 Se pedir autenticação:

Use **Personal Access Token** do GitHub:
1. GitHub.com → Settings → Developer settings → Personal access tokens
2. Generate new token (classic)
3. Use o token como senha

---

## 📝 Descrição do Commit

```
feat: Projeto Palavra Conectada - Angular e Blazor organizados

- ✅ Frontend Angular (TypeScript) funcionando
- ✅ Frontend Blazor (C#) funcionando  
- ✅ Modo MOCK ativo para desenvolvimento
- ✅ Documentação extensa (+4000 linhas)
- ✅ Estrutura organizada (frontend/backend/docs)
- ✅ Interface moderna e responsiva
- ✅ Comparações Angular vs Blazor
- ✅ Exercícios práticos

Funcionalidades:
- Busca de versículos por palavra
- Versículo aleatório
- Múltiplas versões da Bíblia
- Histórias bíblicas para ensinar

Próximo: Fase 2 - Backend API com análise de emoções
```

---

## ✅ O que será enviado:

- ✅ `frontend/angular/` - Projeto Angular
- ✅ `frontend/blazor/` - Projeto Blazor
- ✅ `backend/` - Pasta vazia (Fase 2)
- ✅ `docs/` - Documentação completa
- ✅ `README.md` - Guia principal
- ✅ `.gitignore` - Configurado

## ❌ O que NÃO será enviado:

- ❌ `referencias/` - Código de terceiros
- ❌ `scripts/` - Scripts locais
- ❌ `node_modules/` - Dependências
- ❌ `bin/`, `obj/` - Build artifacts

---

## 🎯 Verificar antes de enviar:

```powershell
# Ver o que será commitado
git status

# Ver arquivos ignorados
git status --ignored
```

---

*Pronto para o push!* 🚀

