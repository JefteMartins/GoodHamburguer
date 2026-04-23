# Phase 11 Testes e Cobertura Plan

## Objetivo da Fase

Consolidar a confianca tecnica da entrega por meio da ampliacao planejada da malha de testes e da formalizacao da cobertura como evidencia objetiva.

## Estrategia

A fase sera executada em frentes pequenas e reviewables:

1. mapear lacunas reais por suite de testes
2. ampliar testes unitarios de dominio e aplicacao
3. ampliar testes de integracao da API e da persistencia
4. ampliar testes do frontend Blazor
5. consolidar coleta, relatorio e gate de cobertura na CI

## File Map

- Create: `specs/2026-04-23-phase-11-testes-e-cobertura/requirements.md`
- Create: `specs/2026-04-23-phase-11-testes-e-cobertura/validation.md`
- Create: `docs/superpowers/plans/2026-04-23-phase-11-testes-e-cobertura.md`
- Modify (quando implementacao iniciar): `.github/workflows/ci.yml`
- Modify/Create (quando implementacao iniciar): `tests/GoodHamburguer.UnitTests/*`
- Modify/Create (quando implementacao iniciar): `tests/GoodHamburguer.IntegrationTests/*`
- Modify/Create (quando implementacao iniciar): `tests/GoodHamburguer.Blazor.Tests/*`
- Modify (se necessario na implementacao): `dotnet-tools.json`

## Task 1: Congelar escopo da fase por suite

**Files:**
- Create: `specs/2026-04-23-phase-11-testes-e-cobertura/requirements.md`

- [ ] **Step 1: Inventariar a cobertura atual por suite**

Registrar o papel atual de:

- `GoodHamburguer.UnitTests`
- `GoodHamburguer.IntegrationTests`
- `GoodHamburguer.Blazor.Tests`

e apontar lacunas provaveis sem depender ainda de percentual final.

- [ ] **Step 2: Definir prioridade por risco**

Priorizar:

1. regras de negocio e validacao
2. contratos HTTP e `ProblemDetails`
3. fluxos principais do Blazor
4. automacao de cobertura na CI

- [ ] **Step 3: Explicitar o que entra e o que fica fora**

Garantir que:

- testes E2E completos de navegador nao entram por padrao
- refatoracoes amplas sem impacto em confianca nao entram
- percentual de cobertura nao substitui qualidade de cenarios

## Task 2: Definir a frente de testes unitarios

**Files:**
- Create: `specs/2026-04-23-phase-11-testes-e-cobertura/requirements.md`
- Create: `specs/2026-04-23-phase-11-testes-e-cobertura/validation.md`

- [ ] **Step 1: Definir alvos unitarios obrigatorios**

Cobrir explicitamente:

- `MenuCatalog`
- `Order`
- `OrderDraftingService`
- `OrderAppService`
- validadores de request

- [ ] **Step 2: Definir tipo de lacuna que deve ser fechada**

Exigir cenarios de:

- entrada vazia ou nula quando fizer sentido
- combinacoes limite
- erros esperados
- preservacao de contratos e propriedades calculadas

- [ ] **Step 3: Declarar que testes unitarios devem permanecer rapidos e isolados**

Evitar dependencias externas e usar doubles simples quando necessario.

## Task 3: Definir a frente de testes de integracao

**Files:**
- Create: `specs/2026-04-23-phase-11-testes-e-cobertura/requirements.md`
- Create: `specs/2026-04-23-phase-11-testes-e-cobertura/validation.md`

- [ ] **Step 1: Definir alvos HTTP e persistencia obrigatorios**

Cobrir explicitamente:

- `GET /api/v1/menu`
- `POST/GET/PUT/DELETE /api/v1/orders`
- health checks
- tratamento de excecoes e `ProblemDetails`
- persistencia real via MySQL de teste

- [ ] **Step 2: Exigir cenarios de falha relevantes**

Incluir:

- `404`
- `400` ou `422` quando aplicavel
- falhas conhecidas com `ProblemDetails`
- consistencia dos contratos serializados

- [ ] **Step 3: Declarar a regressao de infraestrutura como gate**

Os testes de integracao devem continuar funcionando com dependencias reais da stack de teste.

## Task 4: Definir a frente de testes Blazor

**Files:**
- Create: `specs/2026-04-23-phase-11-testes-e-cobertura/requirements.md`
- Create: `specs/2026-04-23-phase-11-testes-e-cobertura/validation.md`

- [ ] **Step 1: Definir alvos obrigatorios no frontend**

Cobrir explicitamente:

- clients HTTP de menu e pedidos
- `/menu`
- `/orders/new`
- `/orders/{id}`
- `/dashboard/orders`

- [ ] **Step 2: Exigir cobertura de estados de tela**

Incluir:

- loading
- empty
- error
- success
- interacao principal por fluxo

- [ ] **Step 3: Limitar escopo de UI ao que reforca confianca**

Evitar ampliar a fase com testes visuais ou browser E2E completos se bUnit e testes de client ja provarem o comportamento esperado.

## Task 5: Definir cobertura e CI como evidencia objetiva

**Files:**
- Create: `specs/2026-04-23-phase-11-testes-e-cobertura/requirements.md`
- Create: `specs/2026-04-23-phase-11-testes-e-cobertura/validation.md`
- Create: `docs/superpowers/plans/2026-04-23-phase-11-testes-e-cobertura.md`

- [ ] **Step 1: Definir a meta de cobertura da fase**

Registrar meta minima global de `70%`.

- [ ] **Step 2: Definir evidencia minima de cobertura**

Exigir:

- coleta automatizada
- relatorio consolidado
- evidencia em CI

- [ ] **Step 3: Definir gate de nao conclusao**

A fase nao pode ser concluida se:

- a cobertura nao for medida de forma repetivel
- a CI nao produzir evidencia suficiente
- o percentual alvo nao for atingido ou houver risco conhecido sem registro explicito

## Task 6: Preparar handoff para implementacao

**Files:**
- Create: `docs/superpowers/plans/2026-04-23-phase-11-testes-e-cobertura.md`
- Modify: `specs/2026-04-23-phase-11-testes-e-cobertura/plan.md`

- [ ] **Step 1: Congelar baseline da feature spec**

Concluir `plan.md`, `requirements.md` e `validation.md` sem ambiguidades relevantes.

- [ ] **Step 2: Declarar o roteamento tecnico esperado**

Na implementacao, encaminhar a fase por skill .NET apropriado antes de codar, dado que o trabalho sera majoritariamente em C#, ASP.NET Core, Blazor e suites de teste .NET.

- [ ] **Step 3: Escrever plano detalhado de implementacao**

Gerar plano em `docs/superpowers/plans/2026-04-23-phase-11-testes-e-cobertura.md`.

- [ ] **Step 4: Iniciar implementacao somente apos revisao humana**

Nao executar mudancas amplas de codigo da fase antes da revisao humana dos artefatos.

