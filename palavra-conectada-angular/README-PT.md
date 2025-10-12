# 📖 Palavra Conectada - Angular

## O que é este projeto?

**Palavra Conectada** é uma aplicação que permite buscar versículos bíblicos relacionados a palavras ou frases que você digita. É como ter um amigo que conhece toda a Bíblia e pode te ajudar a encontrar exatamente o que você precisa!

---

## 🌟 História Bíblica: A Torre de Babel ao Contrário

Você lembra da história da **Torre de Babel** (Gênesis 11:1-9)? Deus confundiu as línguas e as pessoas não conseguiam mais se comunicar. 

Este projeto é o **oposto disso**! Aqui, usamos a tecnologia para:
- **Conectar** pessoas à Palavra de Deus
- **Facilitar** a comunicação entre você e as Escrituras
- **Unir** diferentes versões da Bíblia em um só lugar

Assim como Deus usou diferentes línguas, temos diferentes versões da Bíblia (NVI, ACF, AA) todas acessíveis!

---

## 🏗️ Arquitetura - Como o Templo de Salomão

### 1. **Componentes** (As Salas do Templo)

Cada componente é como uma sala específica do templo:

```
verse-search.component.ts    → A sala principal onde acontece a adoração
verse-search.component.html  → A decoração e os móveis da sala
verse-search.component.css   → As cores e a beleza visual
```

**O que cada arquivo faz:**

- **`.ts` (TypeScript)**: O cérebro - toda a lógica e inteligência
- **`.html` (Template)**: O corpo - a estrutura visual
- **`.css` (Estilos)**: As roupas - a aparência bonita

### 2. **Serviços** (Os Levitas que Servem)

```typescript
// bible-api.service.ts
export class BibleApiService {
  // Busca versículos por palavra
  searchVerses(searchTerm: string)
  
  // Busca versículo aleatório
  getRandomVerse()
}
```

Os serviços são como os **levitas no templo** - eles fazem o trabalho pesado:
- Buscam os dados da API
- Organizam as informações
- Servem os componentes

### 3. **Modelos** (As Tábuas da Lei)

```typescript
// verse.model.ts
export interface Verse {
  book: Book;
  chapter: number;
  number: number;
  text: string;
}
```

Os modelos definem a **estrutura clara** dos dados, como as tábuas da lei definiam as regras.

---

## 🔄 Como Funciona - O Fluxo de Dados

### Parábola dos Talentos (Mateus 25:14-30)

Assim como na parábola, cada parte do código recebe uma **responsabilidade**:

1. **Usuário digita uma palavra** 
   → O primeiro servo recebe o talento

2. **Componente recebe a entrada**
   → O servo usa o talento com sabedoria

3. **Serviço busca na API**
   → O servo investe e multiplica

4. **API retorna os dados**
   → O talento rende frutos

5. **Componente exibe os resultados**
   → O senhor se alegra com os resultados!

---

## 🎨 Conceitos Importantes do Angular

### 1. **Data Binding** - A Comunicação Divina

```html
<input [(ngModel)]="searchTerm" />
```

O `[(ngModel)]` é uma **comunicação bidirecional**:
- Quando você digita → atualiza a variável
- Quando a variável muda → atualiza o input

É como a **oração e resposta de Deus** - uma conversa de duas vias!

### 2. **Observables** - A Promessa de Deus

```typescript
this.bibleApiService.searchVerses(term)
  .subscribe({
    next: (result) => { /* Promessa cumprida! */ }
  });
```

Observables são como **promessas de Deus**:
- Você faz a requisição (ora)
- Aguarda a resposta (espera com fé)
- Recebe o resultado (a promessa se cumpre)

### 3. **Dependency Injection** - O Espírito Santo

```typescript
constructor(private bibleApiService: BibleApiService) {}
```

O Angular **injeta automaticamente** os serviços necessários, assim como o Espírito Santo nos **capacita** com o que precisamos!

---

## 🚀 Como Executar

### Pré-requisitos
- Node.js instalado
- Angular CLI instalado

### Comandos

```bash
# Instalar dependências
npm install

# Executar em modo desenvolvimento
npm start
# ou
ng serve

# Abrir no navegador
http://localhost:4200
```

---

## 📚 Funcionalidades

### 1. **Busca por Palavra-chave**
Digite qualquer palavra (amor, fé, paz) e encontre versículos relacionados.

### 2. **Versículo Aleatório**
Deixe Deus surpreender você com uma palavra específica para o momento!

### 3. **Múltiplas Versões**
- NVI (Nova Versão Internacional)
- ACF (Almeida Corrigida Fiel)
- AA (Almeida Revista e Atualizada)

### 4. **Interface Bonita**
Design moderno e responsivo, funcionando em qualquer dispositivo.

---

## 🎓 Aprendizados - Lições Espirituais e Técnicas

### Lição 1: Separação de Responsabilidades
> "A cada um segundo a sua capacidade" (Mateus 25:15)

Cada arquivo tem sua função específica - não misture responsabilidades!

### Lição 2: Reutilização de Código
> "Não há nada novo debaixo do sol" (Eclesiastes 1:9)

Os serviços podem ser usados em múltiplos componentes - não repita código!

### Lição 3: Reatividade
> "Estai sempre preparados" (1 Pedro 3:15)

O código reage automaticamente às mudanças - esteja sempre pronto!

---

## 🌐 API Utilizada

**A Bíblia Digital**: https://www.abibliadigital.com.br/

Uma API RESTful gratuita com:
- 7 versões da Bíblia
- 4 idiomas
- Busca por palavras-chave
- Versículos aleatórios
- Estatísticas

---

## 🙏 Versículo Final

> **"Lâmpada para os meus pés é a tua palavra e luz para o meu caminho."**
> 
> *Salmos 119:105*

Que este projeto seja uma lâmpada que ilumina o caminho de muitos para encontrar a Palavra de Deus!

---

## 💡 Próximos Passos

- [ ] Adicionar favoritos
- [ ] Compartilhar versículos
- [ ] Histórico de buscas
- [ ] Notas pessoais
- [ ] Plano de leitura

---

*Desenvolvido com ❤️ e fé*

