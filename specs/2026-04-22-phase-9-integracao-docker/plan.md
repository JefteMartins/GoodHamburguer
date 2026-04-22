# Fase 9 - Integracao via Docker Implementation Plan

> **For agentic workers:** REQUIRED SUB-SKILL: Use superpowers:subagent-driven-development (recommended) or superpowers:executing-plans to implement this plan task-by-task. Steps use checkbox (`- [ ]`) syntax for tracking.

**Goal:** consolidar a execucao completa via Docker com ajustes estruturais leves de hardening operacional, garantindo subida previsivel de `mysql`, `migrations`, `api` e `blazor`, com readiness real e documentacao objetiva para avaliacao tecnica.

**Architecture:** o orquestrador central continua em `docker/docker-compose.yml`; os servicos sao encadeados por saude real (`healthcheck` + `depends_on` com condicoes), politicas de restart sao explicitadas para resiliencia local, e os Dockerfiles de API/Blazor permanecem baseados em multi-stage build com foco em reproducibilidade e simplicidade.

**Tech Stack:** Docker Compose v2, Dockerfiles multi-stage, ASP.NET Core (.NET 10), MySQL 8.4

---

## File Map

- Modify: `docker/docker-compose.yml`
- Modify if needed: `docker/api/Dockerfile`
- Modify if needed: `docker/blazor/Dockerfile`
- Modify: `README.md`
- Modify if needed: `specs/roadmap.md`
- Modify if needed: `CHANGELOG.md`

## Task 1: Estabilizar cadeia de inicializacao por saude real

**Files:**
- Modify: `docker/docker-compose.yml`

- [ ] **Step 1: Write/adjust validation-first checks**

Definir ou ajustar validacoes de infraestrutura para confirmar, no minimo:

- `mysql` fica `healthy`
- `migrations` so roda apos `mysql` pronto e termina com sucesso
- `api` so inicia apos sucesso de `migrations`
- `blazor` so inicia quando `api` estiver pronta para trafego

- [ ] **Step 2: Add minimal compose hardening**

Aplicar hardening leve no `docker-compose.yml`:

- separar claramente `depends_on` por `condition`
- adicionar `healthcheck` no `api` usando endpoint de `liveness`/`readiness`
- adicionar `healthcheck` no `blazor` com probe HTTP local
- explicitar `restart` policy coerente por servico (`no` para `migrations`, `unless-stopped` ou equivalente para servicos de runtime)

- [ ] **Step 3: Verify compose semantics**

Executar validacao sintatica e semantica:

Run: `docker compose --env-file .env -f docker/docker-compose.yml config`

## Task 2: Revisar Dockerfiles para robustez local sem inflar complexidade

**Files:**
- Modify if needed: `docker/api/Dockerfile`
- Modify if needed: `docker/blazor/Dockerfile`

- [ ] **Step 1: Validate runtime assumptions**

Confirmar que os Dockerfiles:

- mantem build deterministico com `dotnet restore` + `dotnet publish`
- nao dependem de arquivos fora do contexto esperado
- preservam portas e entrypoints coerentes com o compose

- [ ] **Step 2: Apply only necessary structural adjustments**

Se houver gap real, ajustar apenas o minimo necessario (sem refatoracao ampla), por exemplo:

- ajustes de `WORKDIR`, `COPY` ou `EXPOSE` para coerencia operacional
- reducao de risco de quebra de build por caminho inconsistente

## Task 3: Validar subida completa e conectividade fim a fim

**Files:**
- Modify: `docker/docker-compose.yml` (se algum ajuste final for necessario)

- [ ] **Step 1: Execute full stack startup**

Run:

- `Copy-Item .env.example .env` (se `.env` nao existir)
- `docker compose --env-file .env -f docker/docker-compose.yml up --build`

- [ ] **Step 2: Confirm functional connectivity**

Validar manualmente:

- `blazor` acessivel em `http://localhost:8080`
- `api` acessivel em `http://localhost:8081`
- `api` alcança MySQL usando configuracao do compose
- readiness nao e mockado e reflete dependencia real

- [ ] **Step 3: Validate health endpoints in containerized runtime**

Checar ao menos:

- `GET /health/live`
- `GET /health/ready`

com evidencia de sucesso apos stack estabilizada.

## Task 4: Fechar documentacao operacional da fase

**Files:**
- Modify: `README.md`

- [ ] **Step 1: Document exact run flow**

Atualizar README com:

- pre-requisitos minimos
- comandos oficiais da stack Docker
- ordem esperada de inicializacao
- comandos de verificacao de saude
- troubleshooting objetivo (ex.: porta ocupada, falha de healthcheck, migrations com erro)

- [ ] **Step 2: Keep docs aligned with real commands**

Garantir que os comandos documentados sao exatamente os usados na validacao.

## Task 5: Fechar validacao e status de roadmap

**Files:**
- Modify if needed: `specs/roadmap.md`
- Modify if needed: `CHANGELOG.md`

- [ ] **Step 1: Capture objective evidence**

Registrar evidencia minima:

- compose valido (`docker compose ... config`)
- stack sobe integralmente
- health checks operacionais
- conectividade API <-> MySQL confirmada
- conectividade Blazor <-> API confirmada

- [ ] **Step 2: Close the loop**

Quando a fase estiver realmente validada:

- marcar `Fase 9 - Integracao via Docker` como concluida em `specs/roadmap.md`
- atualizar `CHANGELOG.md`
- preparar commit dedicado da fase
