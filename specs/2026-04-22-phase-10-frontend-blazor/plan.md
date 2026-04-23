# Frontend Blazor Stitch-Aligned Implementation Plan

> **For agentic workers:** REQUIRED SUB-SKILL: Use superpowers:subagent-driven-development (recommended) or superpowers:executing-plans to implement this plan task-by-task. Steps use checkbox (`- [ ]`) syntax for tracking.

**Goal:** implementar o frontend Blazor funcional consumindo a API versionada `v1`, com estrutura e UX alinhadas as telas do Stitch `9060755920845036151`.

**Architecture:** o frontend sera organizado por rotas de fluxo (`/menu`, `/orders/new`, `/orders/{id}`, `/dashboard/orders`) e servicos HTTP dedicados para evitar chamadas diretas nas paginas. O Stitch sera usado como referencia de layout, hierarquia visual e composicao de componentes, mas sem dependencia em runtime. Contratos de API e erros `ProblemDetails` continuam encapsulados em clients e modelos proprios do Blazor.

**Tech Stack:** .NET 10, Blazor Web App (Interactive Server), HttpClientFactory, ASP.NET Core ProblemDetails, xUnit, bUnit (ou alternativa equivalente), MCP Stitch (insumo de design)

---

## File Map

- Modify: `specs/roadmap.md`
- Modify: `specs/tech-stack.md`
- Modify: `specs/2026-04-22-phase-10-frontend-blazor/requirements.md`
- Modify: `specs/2026-04-22-phase-10-frontend-blazor/validation.md`
- Create: `specs/2026-04-22-phase-10-frontend-blazor/stitch-screen-map.md`
- Modify (quando implementacao iniciar): `src/GoodHamburguer.Blazor/Program.cs`
- Modify/Create (quando implementacao iniciar): `src/GoodHamburguer.Blazor/Components/Pages/*`
- Modify/Create (quando implementacao iniciar): `src/GoodHamburguer.Blazor/Components/Shared/*`
- Modify/Create (quando implementacao iniciar): `src/GoodHamburguer.Blazor/Services/Api/*`

## Task 1: Congelar referencia Stitch para a fase

**Files:**
- Create: `specs/2026-04-22-phase-10-frontend-blazor/stitch-screen-map.md`

- [ ] **Step 1: Registrar origem das telas**

Documentar `projectId`, `screenId`, titulo e objetivo de cada tela do Stitch em arquivo versionado na pasta da fase.

- [ ] **Step 2: Definir mapeamento rota -> tela**

Mapear as quatro rotas alvo do Blazor para as quatro telas referencia do Stitch.

- [ ] **Step 3: Registrar estrategia de captura de referencia**

Incluir instrucoes de consulta via MCP (`list_screens`, `get_screen`) para facilitar reproducao em futuras sessoes.

- [ ] **Step 4: Revisar consistencia com requirements**

Garantir que o mapeamento esta refletido em `requirements.md` e sem ambiguidades.

## Task 2: Definir escopo funcional por pagina

**Files:**
- Modify: `specs/2026-04-22-phase-10-frontend-blazor/requirements.md`

- [ ] **Step 1: Especificar comportamentos de `/menu`**

Listar dados obrigatorios, estados de tela e a chamada API esperada (`GET /api/v1/menu`).

- [ ] **Step 2: Especificar comportamentos de `/orders/new`**

Definir campos minimos, validacoes de entrada e chamada API (`POST /api/v1/orders`).

- [ ] **Step 3: Especificar comportamentos de `/orders/{id}` e `/dashboard/orders`**

Definir operacoes de leitura/escrita por tela e endpoints associados.

- [ ] **Step 4: Registrar explicitamente o que nao entra**

Manter foco de fase e evitar escopo extra nao essencial para demonstracao tecnica.

## Task 3: Definir estrategia tecnica de implementacao Blazor

**Files:**
- Modify: `specs/tech-stack.md`
- Modify: `specs/roadmap.md`

- [ ] **Step 1: Consolidar papel do Stitch na stack**

Registrar que Stitch e fonte de design e nao dependencia de runtime.

- [ ] **Step 2: Consolidar objetivo e entregaveis da fase 10 no roadmap**

Atualizar fase 10 com mapeamento de telas e integracao API `v1`.

- [ ] **Step 3: Definir criterios minimos de design system local**

Registrar extracao de tokens essenciais (cores, tipografia, espacamento, estados).

- [ ] **Step 4: Revisar alinhamento da constituicao**

Verificar coerencia com `mission.md` e com prioridades da primeira entrega.

## Task 4: Definir validacao objetiva da fase

**Files:**
- Modify: `specs/2026-04-22-phase-10-frontend-blazor/validation.md`

- [ ] **Step 1: Definir criterios de sucesso por rota**

Associar evidencias funcionais para cada tela/rota principal.

- [ ] **Step 2: Definir criterios de aderencia visual**

Exigir verificacao manual comparando estrutura, hierarquia e estados com Stitch.

- [ ] **Step 3: Definir criterios de regressao**

Manter testes de backend e build da solution como gate da fase.

- [ ] **Step 4: Definir condicoes de nao conclusao**

Declarar explicitamente quando a fase deve permanecer aberta.

## Task 5: Preparar handoff para implementacao

**Files:**
- Modify: `specs/2026-04-22-phase-10-frontend-blazor/plan.md`

- [ ] **Step 1: Validar plano com stakeholder humano**

Confirmar se o mapeamento das 4 telas atende objetivo de demonstracao.

- [ ] **Step 2: Congelar baseline da fase**

Finalizar os artefatos de spec para iniciar implementacao sem ambiguidade.

- [ ] **Step 3: Escolher estrategia de execucao**

Executar por subagentes por tarefa ou inline por lotes (checkpoint por rota).

- [ ] **Step 4: Iniciar implementacao somente apos aprovacao do plano**

Nao iniciar mudancas de codigo do frontend antes da aprovacao da fase.