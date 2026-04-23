# Phase 14 Persistencia de Logs Operacionais Plan

## Objetivo da Fase

Registrar historico operacional da API em MySQL para auditoria basica e rastreabilidade de falhas, cobrindo logs de execucao normal e logs de erro com contexto minimo para investigacao.

## Estrategia

A fase sera executada em blocos pequenos, sem desmontar a arquitetura atual:

1. definir modelo unico de log operacional e persistencia
2. registrar logs `application` para execucoes normais com parametros de entrada
3. registrar logs `error` para excecoes tratadas pela API
4. criar estrategia de consulta por periodo e por tipo
5. fechar cobertura automatizada para persistencia e consulta

## File Map

- Create: `specs/2026-04-23-phase-14-persistencia-logs-operacionais/requirements.md`
- Create: `specs/2026-04-23-phase-14-persistencia-logs-operacionais/validation.md`
- Modify (quando implementacao iniciar): `src/GoodHamburguer.Infrastructure/Persistence/GoodHamburguerDbContext.cs`
- Create (quando implementacao iniciar): `src/GoodHamburguer.Infrastructure/Persistence/Entities/OperationalLogEntity.cs`
- Create (quando implementacao iniciar): `src/GoodHamburguer.Infrastructure/Persistence/Configurations/OperationalLogEntityTypeConfiguration.cs`
- Create (quando implementacao iniciar): `src/GoodHamburguer.Application/OperationalLogs/Abstractions/IOperationalLogRepository.cs`
- Create (quando implementacao iniciar): `src/GoodHamburguer.Application/OperationalLogs/Contracts/OperationalLogType.cs`
- Create (quando implementacao iniciar): `src/GoodHamburguer.Application/OperationalLogs/Contracts/OperationalLogRecordRequest.cs`
- Create (quando implementacao iniciar): `src/GoodHamburguer.Application/OperationalLogs/Contracts/OperationalLogQueryFilter.cs`
- Create (quando implementacao iniciar): `src/GoodHamburguer.Application/OperationalLogs/Services/IOperationalLogService.cs`
- Create (quando implementacao iniciar): `src/GoodHamburguer.Application/OperationalLogs/Services/OperationalLogService.cs`
- Modify (quando implementacao iniciar): `src/GoodHamburguer.Application/DependencyInjection.cs`
- Create (quando implementacao iniciar): `src/GoodHamburguer.Infrastructure/OperationalLogs/OperationalLogRepository.cs`
- Modify (quando implementacao iniciar): `src/GoodHamburguer.Infrastructure/DependencyInjection.cs`
- Modify (quando implementacao iniciar): `src/GoodHamburguer.Api/Program.cs`
- Modify (quando implementacao iniciar): `src/GoodHamburguer.Api/Exceptions/GlobalExceptionFilter.cs`
- Create (quando implementacao iniciar): `src/GoodHamburguer.Api/OperationalLogs/OperationalLoggingMiddleware.cs`
- Create (quando implementacao iniciar): `src/GoodHamburguer.Api/Controllers/OperationalLogsController.cs`
- Create (quando implementacao iniciar): `src/GoodHamburguer.Infrastructure/Persistence/Migrations/*Phase14OperationalLogs*.cs`
- Modify/Create (quando implementacao iniciar): `tests/GoodHamburguer.UnitTests/OperationalLogs/*`
- Modify/Create (quando implementacao iniciar): `tests/GoodHamburguer.IntegrationTests/OperationalLogsEndpointTests.cs`
- Modify/Create (quando implementacao iniciar): `tests/GoodHamburguer.IntegrationTests/OperationalLogRepositoryTests.cs`
- Keep as detailed execution artifact: `docs/superpowers/plans/2026-04-23-phase-14-persistencia-logs-operacionais.md`

## Task 1: Congelar o contrato funcional da fase

**Files:**
- Create: `specs/2026-04-23-phase-14-persistencia-logs-operacionais/requirements.md`

- [ ] **Step 1: Definir os dois tipos obrigatorios de log**

Registrar explicitamente:

- `application` para execucoes normais com parametros de entrada
- `error` para falhas e excecoes tratadas

- [ ] **Step 2: Definir contexto minimo obrigatorio por registro**

Cada log deve incluir, no minimo:

- data/hora UTC do evento
- rota/endereco chamado
- metodo HTTP
- correlation id
- payload estruturado de parametros de entrada

- [ ] **Step 3: Declarar limites da fase**

Esta fase nao inclui:

- plataforma externa de observabilidade
- dashboard dedicado
- retencao/arquivamento avancado
- analytics de negocio

## Task 2: Definir persistencia e modelagem de dados

**Files:**
- Create: `specs/2026-04-23-phase-14-persistencia-logs-operacionais/requirements.md`
- Create: `specs/2026-04-23-phase-14-persistencia-logs-operacionais/validation.md`

- [ ] **Step 1: Definir tabela unica de logs**

Modelar uma unica tabela MySQL para logs operacionais com coluna `type` para distinguir `application` e `error`.

- [ ] **Step 2: Definir payload estruturado de entrada**

Persistir parametros de entrada em formato estruturado (`json`) para manter rastreabilidade sem fragmentar o esquema em colunas por endpoint.

- [ ] **Step 3: Definir migracao versionada**

A implementacao deve criar migracao EF Core dedicada da fase, mantendo historico evolutivo do schema.

## Task 3: Definir captura de logs de aplicacao e erro

**Files:**
- Create: `specs/2026-04-23-phase-14-persistencia-logs-operacionais/requirements.md`
- Create: `specs/2026-04-23-phase-14-persistencia-logs-operacionais/validation.md`

- [ ] **Step 1: Definir ponto de captura para logs `application`**

Capturar logs de execucao normal em middleware da API, com foco inicial nos endpoints versionados de negocio.

- [ ] **Step 2: Definir ponto de captura para logs `error`**

Aproveitar o fluxo de tratamento de excecao global para gravar eventos de erro sem duplicar log para a mesma falha.

- [ ] **Step 3: Preservar resiliencia do request**

Falha de escrita do log operacional nao deve derrubar a resposta principal da API em fluxo normal.

## Task 4: Definir estrategia de consulta operacional

**Files:**
- Create: `specs/2026-04-23-phase-14-persistencia-logs-operacionais/requirements.md`
- Create: `specs/2026-04-23-phase-14-persistencia-logs-operacionais/validation.md`

- [ ] **Step 1: Definir filtros obrigatorios**

Consultar logs por:

- intervalo de tempo (`from`/`to`)
- tipo (`application`/`error`)

- [ ] **Step 2: Definir superficie de consulta**

A fase deve expor estrategia de consulta por meio de endpoint operacional versionado, mantendo contrato simples para investigacao.

- [ ] **Step 3: Definir requisitos de ordenacao e limite**

Consulta deve retornar logs ordenados por data/hora desc e com limite de itens para evitar respostas descontroladas.

## Task 5: Definir cobertura de testes da fase

**Files:**
- Create: `specs/2026-04-23-phase-14-persistencia-logs-operacionais/requirements.md`
- Create: `specs/2026-04-23-phase-14-persistencia-logs-operacionais/validation.md`

- [ ] **Step 1: Cobrir persistencia de log normal**

Adicionar testes que comprovem escrita de log `application` com contexto e payload estruturado.

- [ ] **Step 2: Cobrir persistencia de log de erro**

Adicionar testes que comprovem escrita de log `error` quando ocorrer excecao no pipeline da API.

- [ ] **Step 3: Cobrir consulta por periodo e tipo**

Adicionar testes de integracao para filtros de consulta e ordenacao da listagem de logs.

## Task 6: Preparar handoff para implementacao

**Files:**
- Modify: `specs/2026-04-23-phase-14-persistencia-logs-operacionais/plan.md`
- Keep as detailed execution artifact: `docs/superpowers/plans/2026-04-23-phase-14-persistencia-logs-operacionais.md`

- [ ] **Step 1: Congelar a feature spec**

Concluir `plan.md`, `requirements.md` e `validation.md` sem ambiguidades relevantes.

- [ ] **Step 2: Declarar roteamento tecnico esperado**

Na implementacao, encaminhar pelo skill .NET apropriado antes de codar, porque o trabalho e majoritariamente em C#, ASP.NET Core, EF Core e testes .NET.

- [ ] **Step 3: Exigir revisao humana antes do codigo**

Nao iniciar implementacao da fase sem revisao humana da spec e do plano detalhado em `docs/superpowers/plans/`.
