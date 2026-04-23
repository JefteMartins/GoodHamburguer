# Phase 12 Qualidade Operacional e Documentacao Plan

## Objetivo da Fase

Refinar a primeira entrega para que a API, a superficie operacional e a documentacao fiquem mais consistentes para avaliacao tecnica, com contratos HTTP mais claros, erros padronizados mais completos, health endpoints mais informativos, README mais forte e CI mais legivel.

## Estrategia

A fase sera executada sem mexer na estrutura central da solucao. O fluxo previsto e:

1. endurecer os contratos HTTP externos da API
2. melhorar a padronizacao de `ProblemDetails` e dos payloads de health
3. ampliar a documentacao principal para avaliacao e handoff
4. refinar a pipeline de CI para produzir sinais operacionais mais claros

O trabalho deve preservar o desenho atual de Clean Architecture e manter a maior parte das mudancas em `Api`, `IntegrationTests`, `README.md` e `.github/workflows/ci.yml`.

## File Map

- Create: `specs/2026-04-23-phase-12-qualidade-operacional-e-documentacao/requirements.md`
- Create: `specs/2026-04-23-phase-12-qualidade-operacional-e-documentacao/validation.md`
- Modify (quando implementacao iniciar): `src/GoodHamburguer.Api/Controllers/OrdersController.cs`
- Modify (quando implementacao iniciar): `src/GoodHamburguer.Api/Controllers/SystemController.cs`
- Modify (quando implementacao iniciar): `src/GoodHamburguer.Api/Program.cs`
- Modify (quando implementacao iniciar): `src/GoodHamburguer.Api/Exceptions/FluentValidationExceptionProblemDetailsMapper.cs`
- Create (quando implementacao iniciar): `src/GoodHamburguer.Api/HealthChecks/HealthCheckJsonResponseWriter.cs`
- Modify (quando implementacao iniciar): `tests/GoodHamburguer.IntegrationTests/OrdersEndpointTests.cs`
- Modify (quando implementacao iniciar): `tests/GoodHamburguer.IntegrationTests/ExceptionHandlingTests.cs`
- Modify (quando implementacao iniciar): `tests/GoodHamburguer.IntegrationTests/SystemHealthEndpointTests.cs`
- Modify (quando implementacao iniciar): `README.md`
- Modify (quando implementacao iniciar): `.github/workflows/ci.yml`
- Modify (ao concluir a fase): `CHANGELOG.md`
- Keep as detailed execution artifact: `docs/superpowers/plans/2026-04-23-phase-12-qualidade-operacional-e-documentacao.md`

## Task 1: Congelar o escopo operacional da fase

**Files:**
- Create: `specs/2026-04-23-phase-12-qualidade-operacional-e-documentacao/requirements.md`

- [ ] **Step 1: Registrar o foco principal da fase**

Fixar que esta fase cobre:

- contratos HTTP
- codigos de status
- mensagens e payloads de erro
- health endpoints
- documentacao de avaliacao
- legibilidade operacional da CI

- [ ] **Step 2: Explicitar o que nao entra nesta fase**

Deixar claro que esta fase nao existe para:

- redesign de dominio
- novas funcionalidades de produto
- incremento visual grande do Blazor
- observabilidade avancada com metricas/traces profundos

- [ ] **Step 3: Confirmar o papel da spec**

Declarar que `plan.md`, `requirements.md` e `validation.md` sao a fonte de verdade da Fase 12, enquanto o plano detalhado em `docs/superpowers/plans/` serve como artefato de execucao.

## Task 2: Definir a frente de contratos HTTP e erros

**Files:**
- Create: `specs/2026-04-23-phase-12-qualidade-operacional-e-documentacao/requirements.md`
- Create: `specs/2026-04-23-phase-12-qualidade-operacional-e-documentacao/validation.md`

- [ ] **Step 1: Declarar os contratos HTTP a revisar**

Cobrir explicitamente:

- `POST /api/v1/orders`
- `GET /api/v1/orders/{id}`
- `DELETE /api/v1/orders/{id}`
- respostas de validacao com `ValidationProblemDetails`
- respostas de recurso inexistente com `ProblemDetails`

- [ ] **Step 2: Priorizar previsibilidade externa**

Orientar a implementacao para favorecer:

- codigos HTTP consistentes com a operacao
- headers e payloads uteis para consumidores
- exemplos claros no README

- [ ] **Step 3: Preservar o comportamento de negocio**

Garantir que a fase refine a superficie externa sem mudar as regras centrais do dominio de pedidos.

## Task 3: Definir a frente de saude e superficie operacional

**Files:**
- Create: `specs/2026-04-23-phase-12-qualidade-operacional-e-documentacao/requirements.md`
- Create: `specs/2026-04-23-phase-12-qualidade-operacional-e-documentacao/validation.md`

- [ ] **Step 1: Declarar os endpoints operacionais obrigatorios**

Cobrir explicitamente:

- `/health/live`
- `/health/ready`
- `/api/v1/system/info`

- [ ] **Step 2: Exigir payloads mais informativos**

Definir que a implementacao deve produzir respostas mais uteis para humanos e para troubleshooting basico, sem transformar esta fase em um projeto novo de observabilidade.

- [ ] **Step 3: Manter a observabilidade proporcional**

Permitir refinamentos leves de surface operacional, mas sem puxar a fase para metricas ricas, tracing avançado ou dashboards externos.

## Task 4: Definir a frente de documentacao e CI

**Files:**
- Create: `specs/2026-04-23-phase-12-qualidade-operacional-e-documentacao/requirements.md`
- Create: `specs/2026-04-23-phase-12-qualidade-operacional-e-documentacao/validation.md`

- [ ] **Step 1: Declarar os alvos de documentacao obrigatorios**

Cobrir explicitamente:

- resumo da arquitetura
- contratos HTTP principais
- exemplos de erro
- limites conhecidos da primeira entrega
- fluxo de execucao local e avaliacao

- [ ] **Step 2: Declarar os alvos operacionais de CI**

Cobrir explicitamente:

- clareza dos artefatos produzidos
- feedback mais legivel da pipeline
- manutenção do gate de qualidade ja existente

- [ ] **Step 3: Preservar simplicidade de uso**

A fase deve melhorar a apresentacao da solucao sem tornar o fluxo local ou a CI excessivamente complexos.

## Task 5: Preparar handoff para implementacao

**Files:**
- Create: `docs/superpowers/plans/2026-04-23-phase-12-qualidade-operacional-e-documentacao.md`
- Modify: `specs/2026-04-23-phase-12-qualidade-operacional-e-documentacao/plan.md`

- [ ] **Step 1: Congelar a feature spec**

Concluir `plan.md`, `requirements.md` e `validation.md` sem ambiguidades relevantes antes de iniciar codigo.

- [ ] **Step 2: Declarar o roteamento tecnico esperado**

Na implementacao, encaminhar a fase pelo skill .NET apropriado antes de codar, porque o trabalho e majoritariamente em C#, ASP.NET Core, integration tests e CI .NET.

- [ ] **Step 3: Manter o plano detalhado como guia de execucao**

Usar `docs/superpowers/plans/2026-04-23-phase-12-qualidade-operacional-e-documentacao.md` como roteiro detalhado de tarefas, sem substituir a spec da fase.

- [ ] **Step 4: Iniciar implementacao somente apos revisao humana**

Nao executar mudancas amplas da Fase 12 antes da revisao humana destes artefatos.
