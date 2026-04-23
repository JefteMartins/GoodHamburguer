# Phase 13 Melhorias Incrementais Plan

## Objetivo da Fase

Elevar a qualidade percebida e a robustez tecnica da entrega depois das fases obrigatorias, concentrando os ganhos em quatro frentes pequenas e independentes: cobertura de borda, refinamento visual do Blazor, verificacoes adicionais de CI e telemetria mais rica.

## Estrategia

A fase nao deve ser implementada como um bloco unico. O trabalho precisa ser quebrado em lotes reviewables que preservem autonomia entre testes, frontend, pipeline e observabilidade.

Sequencia recomendada:

1. fechar primeiro as lacunas de teste de maior risco
2. refinar UX do Blazor sem romper o design system local
3. ampliar o CI com checks de apresentacao e empacotamento
4. aprofundar metricas e traces onde houver maior valor diagnostico

## File Map

- Create: `specs/2026-04-23-phase-13-melhorias-incrementais/requirements.md`
- Create: `specs/2026-04-23-phase-13-melhorias-incrementais/validation.md`
- Create: `docs/superpowers/plans/2026-04-23-phase-13-melhorias-incrementais.md`
- Modify (quando implementacao iniciar): `tests/GoodHamburguer.UnitTests/OrderTests.cs`
- Modify (quando implementacao iniciar): `tests/GoodHamburguer.UnitTests/OrderRequestValidatorTests.cs`
- Modify (quando implementacao iniciar): `tests/GoodHamburguer.IntegrationTests/OrdersEndpointTests.cs`
- Modify (quando implementacao iniciar): `tests/GoodHamburguer.Blazor.Tests/Orders/NewOrderPageTests.cs`
- Modify (quando implementacao iniciar): `tests/GoodHamburguer.Blazor.Tests/Orders/OrderDetailsPageTests.cs`
- Modify (quando implementacao iniciar): `tests/GoodHamburguer.Blazor.Tests/Orders/OrdersDashboardPageTests.cs`
- Modify (quando implementacao iniciar): `src/GoodHamburguer.Blazor/Components/Pages/NewOrder.razor`
- Modify (quando implementacao iniciar): `src/GoodHamburguer.Blazor/Components/Pages/NewOrder.razor.css`
- Modify (quando implementacao iniciar): `src/GoodHamburguer.Blazor/Components/Pages/OrderDetails.razor`
- Modify (quando implementacao iniciar): `src/GoodHamburguer.Blazor/Components/Pages/OrderDetails.razor.css`
- Modify (quando implementacao iniciar): `src/GoodHamburguer.Blazor/Components/Pages/OrdersDashboard.razor`
- Modify (quando implementacao iniciar): `src/GoodHamburguer.Blazor/Components/Pages/OrdersDashboard.razor.css`
- Modify (quando implementacao iniciar): `.github/workflows/ci.yml`
- Modify (quando implementacao iniciar): `README.md`
- Modify (quando implementacao iniciar): `src/GoodHamburguer.Api/Program.cs`
- Modify/Create (quando implementacao iniciar): `src/GoodHamburguer.Application/Orders/Services/*`
- Modify/Create (quando implementacao iniciar): `tests/GoodHamburguer.UnitTests/*Telemetry*.cs`

## Task 1: Congelar o recorte incremental da fase

**Files:**
- Create: `specs/2026-04-23-phase-13-melhorias-incrementais/requirements.md`
- Create: `specs/2026-04-23-phase-13-melhorias-incrementais/validation.md`

- [ ] **Step 1: Registrar explicitamente as quatro frentes**

As frentes obrigatorias desta fase sao:

1. cobertura de testes em cenarios de borda
2. refinamentos de UX e apresentacao no Blazor
3. verificacoes adicionais na pipeline
4. metricas e traces mais profundos

- [ ] **Step 2: Bloquear escopo fora do objetivo**

Esta fase nao deve ser usada para:

- reescrever arquitetura
- trocar stack
- introduzir E2E completos de navegador como requisito
- criar observabilidade operacional complexa demais para o contexto do desafio

- [ ] **Step 3: Declarar independencia entre frentes**

Cada frente deve poder ser implementada, revisada e validada separadamente sem exigir merge atomico da fase inteira.

## Task 2: Definir a frente de testes de borda

**Files:**
- Create: `specs/2026-04-23-phase-13-melhorias-incrementais/requirements.md`
- Create: `specs/2026-04-23-phase-13-melhorias-incrementais/validation.md`

- [ ] **Step 1: Fixar os alvos prioritarios**

Os alvos minimos desta frente sao:

- `tests/GoodHamburguer.UnitTests/OrderTests.cs`
- `tests/GoodHamburguer.UnitTests/OrderRequestValidatorTests.cs`
- `tests/GoodHamburguer.IntegrationTests/OrdersEndpointTests.cs`
- `tests/GoodHamburguer.IntegrationTests/ExceptionHandlingTests.cs`

- [ ] **Step 2: Explicitar o tipo de borda que deve entrar**

Cobrir:

- payloads parcialmente vazios
- transicoes invalidas de atualizacao
- persistencia e serializacao de valores limite
- mensagens de erro e contratos `ProblemDetails` em falhas menos triviais

- [ ] **Step 3: Manter foco em utilidade real**

Nao adicionar testes artificiais apenas para elevar percentual. Cada teste novo deve proteger um comportamento que faria diferenca numa regressao real.

## Task 3: Definir a frente de refinamento visual do Blazor

**Files:**
- Create: `specs/2026-04-23-phase-13-melhorias-incrementais/requirements.md`
- Create: `specs/2026-04-23-phase-13-melhorias-incrementais/validation.md`

- [ ] **Step 1: Fixar as telas-alvo**

As telas obrigatorias desta frente sao:

- `src/GoodHamburguer.Blazor/Components/Pages/NewOrder.razor`
- `src/GoodHamburguer.Blazor/Components/Pages/OrderDetails.razor`
- `src/GoodHamburguer.Blazor/Components/Pages/OrdersDashboard.razor`

- [ ] **Step 2: Explicitar o tipo de melhoria esperado**

Priorizar:

- hierarquia visual mais clara
- feedback de carregamento, erro e sucesso mais legivel
- estados vazios mais intencionais
- densidade de informacao melhor distribuida

- [ ] **Step 3: Prender o refinamento ao design existente**

As melhorias devem preservar o design system local do Blazor e a referencia visual consolidada na Fase 10, evitando redesign completo.

## Task 4: Definir a frente de CI adicional

**Files:**
- Create: `specs/2026-04-23-phase-13-melhorias-incrementais/requirements.md`
- Create: `specs/2026-04-23-phase-13-melhorias-incrementais/validation.md`
- Create: `docs/superpowers/plans/2026-04-23-phase-13-melhorias-incrementais.md`

- [ ] **Step 1: Registrar o ganho esperado de pipeline**

O CI desta fase deve ir alem de build + testes + cobertura e passar a validar tambem a apresentacao minima da entrega.

- [ ] **Step 2: Fixar verificacoes candidatas**

As verificacoes preferenciais sao:

- `dotnet build` em configuracao mais rigorosa
- consistencia de `docker compose ... config`
- artefatos de teste e cobertura mais claros para analise humana

- [ ] **Step 3: Evitar inflar tempo de pipeline sem retorno**

Qualquer novo check precisa justificar custo e manter a pipeline adequada ao contexto de avaliacao tecnica.

## Task 5: Definir a frente de observabilidade incremental

**Files:**
- Create: `specs/2026-04-23-phase-13-melhorias-incrementais/requirements.md`
- Create: `specs/2026-04-23-phase-13-melhorias-incrementais/validation.md`
- Create: `docs/superpowers/plans/2026-04-23-phase-13-melhorias-incrementais.md`

- [ ] **Step 1: Fixar o recorte de telemetria**

Esta frente deve aprofundar a telemetria ja existente em vez de troca-la. O foco e adicionar sinais uteis sobre operacoes de pedido.

- [ ] **Step 2: Explicitar sinais obrigatorios**

Cobrir pelo menos:

- traces com enriquecimento de spans em fluxos de pedido
- metricas de operacao de pedido
- identificacao clara de sucesso e falha nos fluxos principais

- [ ] **Step 3: Conter a complexidade**

Nao introduzir backends externos obrigatorios para a fase. A entrega precisa continuar executavel localmente sem stack adicional de observabilidade.

## Task 6: Preparar handoff para execucao incremental

**Files:**
- Create: `docs/superpowers/plans/2026-04-23-phase-13-melhorias-incrementais.md`
- Modify: `specs/2026-04-23-phase-13-melhorias-incrementais/plan.md`

- [ ] **Step 1: Congelar a feature spec sem ambiguidades relevantes**

Concluir `plan.md`, `requirements.md` e `validation.md` com os quatro lotes claramente separados.

- [ ] **Step 2: Gerar plano detalhado de implementacao**

Salvar o plano em `docs/superpowers/plans/2026-04-23-phase-13-melhorias-incrementais.md`.

- [ ] **Step 3: Exigir revisao humana antes de qualquer codigo**

Nenhuma frente deve comecar a ser implementada antes da revisao humana desta spec e do plano detalhado.
