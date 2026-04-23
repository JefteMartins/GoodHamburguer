# GoodHamburguer

## Indice
1. [Visao Geral](#visao-geral)
2. [Como Rodar o Projeto](#como-rodar-o-projeto)
3. [Pre-Requisitos](#pre-requisitos)
4. [Configuracao de Variaveis de Ambiente](#configuracao-de-variaveis-de-ambiente)
5. [Executando com Docker (Recomendado)](#executando-com-docker-recomendado)
6. [Executando Localmente (Sem Docker)](#executando-localmente-sem-docker)
7. [Migrations e Seed](#migrations-e-seed)
8. [Arquitetura da Solucao](#arquitetura-da-solucao)
9. [Fluxos do Sistema em ASCII](#fluxos-do-sistema-em-ascii)
10. [Tecnologias Utilizadas e Justificativas](#tecnologias-utilizadas-e-justificativas)
11. [Endpoints da API](#endpoints-da-api)
12. [Health Checks e Observabilidade](#health-checks-e-observabilidade)
13. [Cobertura de Testes por Fase e Etapa](#cobertura-de-testes-por-fase-e-etapa)
14. [Como Rodar os Testes](#como-rodar-os-testes)
15. [Estrutura de Pastas](#estrutura-de-pastas)
16. [Tudo o que Foi Feito (Resumo por Fase)](#tudo-o-que-foi-feito-resumo-por-fase)
17. [Melhorias Futuras](#melhorias-futuras)
18. [Troubleshooting](#troubleshooting)
19. [Licenca](#licenca)

## Visao Geral
Projeto completo em .NET para a lanchonete Good Hamburger com:
- API REST versionada (`/api/v1`)
- Frontend em Blazor Web App
- Persistencia MySQL real com EF Core e migrations
- Logs operacionais persistidos (`application` e `error`)
- Health checks, Swagger e observabilidade basica
- Testes unitarios, integracao e Blazor UI

Objetivo principal: permitir que qualquer avaliador clone o repositorio e rode tudo com poucos comandos.

## Como Rodar o Projeto
Existem 2 caminhos:

1. **Docker (recomendado)**: sobe MySQL + migrations + API + Blazor de forma orquestrada.
2. **Local (sem Docker para app)**: roda API e Blazor com `dotnet run` e usa MySQL local (ou container somente do MySQL).

Para a maioria dos cenarios, use o fluxo Docker.

## Pre-Requisitos
- .NET SDK 10
- Docker Desktop (ou Docker Engine + Compose v2)
- Git
- Portas livres:
- `8080` (Blazor em Docker)
- `8081` (API em Docker)
- `3306` (MySQL)
- `5102` (Blazor local por `launchSettings`)
- `5123` (API local por `launchSettings`)

## Configuracao de Variaveis de Ambiente
### Docker
No raiz do repositorio, crie `.env` a partir de `.env.example`:

```powershell
Copy-Item .env.example .env
```

Valores esperados em `.env`:

```env
MYSQL_ROOT_PASSWORD=change-me-root-password
MYSQL_DATABASE=goodhamburguer
MYSQL_APP_USER=goodhamburguer
MYSQL_APP_PASSWORD=change-me-app-password
```

### Local (sem Docker para API/Blazor)
API usa secao `Database` em `src/GoodHamburguer.Api/appsettings.Development.json`.
Voce pode configurar de 2 formas:

1. Editando o arquivo `appsettings.Development.json`
2. Sobrescrevendo via variavel de ambiente (preferido em CI/devops):
- `Database__Host`
- `Database__Port`
- `Database__Name`
- `Database__User`
- `Database__Password`

Blazor usa `Api:BaseUrl` em `src/GoodHamburguer.Blazor/appsettings.Development.json`.
Exemplo local padrao:

```json
{
  "Api": {
    "BaseUrl": "http://localhost:8081"
  }
}
```

## Executando com Docker (Recomendado)
### 1) Validar compose
```powershell
docker compose --env-file .env -f docker/docker-compose.yml config
```

### 2) Subir ambiente completo
```powershell
docker compose --env-file .env -f docker/docker-compose.yml up --build
```

### 3) URLs principais
- Blazor: [http://localhost:8080](http://localhost:8080)
- API: [http://localhost:8081](http://localhost:8081)
- Swagger: [http://localhost:8081/swagger](http://localhost:8081/swagger)

### 4) Ordem esperada de inicializacao
1. `mysql` fica healthy
2. `migrations` aplica migration/seed e finaliza (`service_completed_successfully`)
3. `api` sobe
4. `blazor` sobe apos `api` healthy

### 5) Comandos uteis
```powershell
docker compose --env-file .env -f docker/docker-compose.yml ps
docker compose --env-file .env -f docker/docker-compose.yml logs -f mysql
docker compose --env-file .env -f docker/docker-compose.yml logs -f migrations
docker compose --env-file .env -f docker/docker-compose.yml logs -f api
docker compose --env-file .env -f docker/docker-compose.yml logs -f blazor
docker compose --env-file .env -f docker/docker-compose.yml down
```

## Executando Localmente (Sem Docker)
### Opcao A (mais simples): MySQL em container, API e Blazor locais
Suba apenas o MySQL:

```powershell
docker compose --env-file .env -f docker/docker-compose.yml up -d mysql
```

Depois rode migration/seed localmente:

```powershell
dotnet run --project src/GoodHamburguer.Api -- --migrate-only
```

Suba a API:

```powershell
dotnet run --project src/GoodHamburguer.Api
```

Em outro terminal, suba o Blazor:

```powershell
dotnet run --project src/GoodHamburguer.Blazor
```

### Opcao B: Tudo local (incluindo MySQL instalado na maquina)
Configure `Database` no `appsettings.Development.json` da API e execute os mesmos comandos acima.

### URLs locais por `launchSettings`
- API: [http://localhost:5123](http://localhost:5123)
- Blazor: [http://localhost:5102](http://localhost:5102)

## Migrations e Seed
O projeto usa EF Core com migrations em `src/GoodHamburguer.Infrastructure/Persistence/Migrations`.

Comandos uteis:

```powershell
dotnet tool restore
dotnet ef migrations list --project src/GoodHamburguer.Infrastructure --startup-project src/GoodHamburguer.Api
dotnet run --project src/GoodHamburguer.Api -- --migrate-only
```

Migration atual da fase 14:
- `AddOperationalLogs` (tabela `operational_logs`)

## Arquitetura da Solucao
Padrao: Clean Architecture + DDD pragmatico.

### Camadas
- `GoodHamburguer.Domain`: regras centrais (menu, pedido, pricing).
- `GoodHamburguer.Application`: casos de uso, validacoes e contratos.
- `GoodHamburguer.Infrastructure`: EF Core, MySQL, repositorios e migrations.
- `GoodHamburguer.Api`: controllers HTTP, exception handling, health, swagger.
- `GoodHamburguer.Blazor`: frontend consumidor da API.

### Principios aplicados
- regra de negocio no dominio (nao na controller)
- persistencia isolada na infraestrutura
- API previsivel com `ProblemDetails`
- evolucao por fases pequenas e testaveis (SDD)

## Fluxos do Sistema em ASCII
### Fluxo de criacao de pedido
```text
[Client/Blazor]
      |
      v
POST /api/v1/orders
      |
      v
[OrdersController]
      |
      v
[OrderAppService] -- valida request --> [Validators]
      |                                  |
      |                                  v
      |                             erro 422
      v
[Order Domain Model] -> calcula subtotal/desconto/total
      |
      v
[OrderRepository / EF Core]
      |
      v
[MySQL]
      |
      v
201 Created + payload do pedido
```

### Fluxo de logs operacionais (fase 14)
```text
Incoming HTTP Request
       |
       v
[OperationalLoggingMiddleware]
  - captura route/method/payload/correlationId
       |
       +-----------------------> fluxo normal
       |                           |
       |                           v
       |                    status < 500
       |                           |
       |                           v
       |                 grava log type=application
       |
       +-----------------------> fluxo com excecao
                                   |
                                   v
                         [GlobalExceptionFilter]
                         - mapeia ProblemDetails
                         - grava log type=error
```

### Fluxo de subida com Docker
```text
mysql (healthy)
   |
   v
migrations (--migrate-only)
   |
   v
api (health/live e health/ready)
   |
   v
blazor
```

## Tecnologias Utilizadas e Justificativas
- **.NET 10 / C# 14**: stack alvo do desafio e ecossistema moderno.
- **ASP.NET Core Web API**: contratos HTTP claros e maduros.
- **Blazor Web App (Interactive Server)**: frontend .NET integrado e simples para demo.
- **EF Core + Pomelo MySQL**: produtividade + migrations + modelo relacional.
- **MySQL 8.4**: banco relacional real, facil de reproduzir em Docker.
- **Swagger (Swashbuckle)**: exploracao rapida dos endpoints.
- **Asp.Versioning**: API versionada em URL (`/api/v1`).
- **Serilog**: logging estruturado.
- **OpenTelemetry (traces/metrics basicos)**: observabilidade inicial sem stack externa obrigatoria.
- **xUnit + FluentAssertions**: testes legiveis.
- **Testcontainers**: integracao com MySQL real nos testes.
- **GitHub Actions**: pipeline de build/test/coverage automatizada.

## Endpoints da API
Base: `/api/v1`

### Menu
- `GET /menu`

### Orders
- `GET /orders`
- `GET /orders/{id}`
- `POST /orders`
- `PUT /orders/{id}`
- `DELETE /orders/{id}`

### Operational Logs (fase 14)
- `GET /operational-logs?type=application|error&from=<ISO>&to=<ISO>&limit=<int>`

### System
- `GET /system/info`
- `GET /system/test/known-exception` (teste interno)
- `GET /system/test/unknown-exception` (teste interno)

### Health
- `GET /health/live`
- `GET /health/ready`

## Health Checks e Observabilidade
- `health/live`: liveness da aplicacao.
- `health/ready`: readiness com dependencia real de MySQL.
- Serilog request logging ativo.
- OpenTelemetry configurado para tracing e metrics basicos.
- Logs operacionais persistidos em MySQL:
- `type=application`: sucesso/execucao normal
- `type=error`: falhas/excecoes

## Cobertura de Testes por Fase e Etapa
Fonte: `artifacts/coverage-readme/Summary.txt` gerado em **23/04/2026**.

### Cobertura consolidada atual
- **Line coverage global:** `97.4%`
- **Branch coverage global:** `82.9%`
- **Method coverage global:** `99.1%`

### Cobertura por etapa/camada (assemblies)
- `GoodHamburguer.Domain`: `100%`
- `GoodHamburguer.Application`: `98.2%`
- `GoodHamburguer.Infrastructure`: `97.7%`
- `GoodHamburguer.Blazor`: `98.2%`
- `GoodHamburguer.Api`: `94%`

### Cobertura por fase (contexto SDD)
- Fase 11 definiu gate minimo de `70%`.
- Estado atual (apos fases 12, 13 e 14): `97.4%` global, mantendo o gate amplamente atendido.
- O projeto nao versiona historico percentual separado por fase em arquivo dedicado; o valor acima representa o estado consolidado atual.

## Como Rodar os Testes
### Suites principais
```powershell
dotnet test tests/GoodHamburguer.UnitTests/GoodHamburguer.UnitTests.csproj -v minimal
dotnet test tests/GoodHamburguer.IntegrationTests/GoodHamburguer.IntegrationTests.csproj -v minimal
dotnet test tests/GoodHamburguer.Blazor.Tests/GoodHamburguer.Blazor.Tests.csproj -v minimal
```

### Cobertura
```powershell
dotnet test tests/GoodHamburguer.UnitTests/GoodHamburguer.UnitTests.csproj --collect:"XPlat Code Coverage" --results-directory artifacts/test-results/unit -v minimal
dotnet test tests/GoodHamburguer.IntegrationTests/GoodHamburguer.IntegrationTests.csproj --collect:"XPlat Code Coverage" --results-directory artifacts/test-results/integration -v minimal
dotnet test tests/GoodHamburguer.Blazor.Tests/GoodHamburguer.Blazor.Tests.csproj --collect:"XPlat Code Coverage" --results-directory artifacts/test-results/blazor -v minimal
dotnet tool run reportgenerator -reports:"artifacts/test-results/**/coverage.cobertura.xml" -targetdir:"artifacts/coverage-report" -reporttypes:"TextSummary;MarkdownSummaryGitHub"
```

## Estrutura de Pastas
```text
.
|-- docker/
|   |-- api/Dockerfile
|   |-- blazor/Dockerfile
|   `-- docker-compose.yml
|-- specs/
|   |-- mission.md
|   |-- tech-stack.md
|   |-- roadmap.md
|   `-- 2026-04-23-phase-14-persistencia-logs-operacionais/
|-- src/
|   |-- GoodHamburguer.Api/
|   |-- GoodHamburguer.Application/
|   |-- GoodHamburguer.Domain/
|   |-- GoodHamburguer.Infrastructure/
|   `-- GoodHamburguer.Blazor/
|-- tests/
|   |-- GoodHamburguer.UnitTests/
|   |-- GoodHamburguer.IntegrationTests/
|   `-- GoodHamburguer.Blazor.Tests/
`-- GoodHamburguer.slnx
```

## Tudo o que Foi Feito (Resumo por Fase)
Resumo das fases concluidas (1 a 14):
- Fundacao da solution e arquitetura.
- Dominio de menu e modelagem de pedidos.
- Persistencia MySQL com migrations e seed.
- Validacoes + ProblemDetails.
- Calculo de subtotal/desconto/total.
- CRUD completo de pedidos.
- Infra transversal (logs, traces, health, CI).
- Integracao Docker.
- Frontend Blazor baseado em referencia Stitch.
- Expansao de testes e cobertura.
- Refinos operacionais/documentacao.
- Melhorias incrementais de UX/CI/telemetria.
- **Fase 14:** persistencia de logs operacionais + endpoint de consulta.

Roadmap detalhado: `specs/roadmap.md`.

## Melhorias Futuras
- paginacao e filtros avancados no endpoint de logs operacionais
- politicas de retencao/expurgo de logs
- autenticacao/autorizacao para endpoints sensiveis
- dashboard administrativo para operacao
- exportacao de logs para backend observability externo
- testes E2E de navegador para fluxos completos

## Troubleshooting
- **`port is already allocated`**: libere a porta ou ajuste mapeamento.
- **`Database configuration is incomplete`**: revise `Database__*` ou `appsettings`.
- **`Api:BaseUrl must be configured` (Blazor)**: configure `Api:BaseUrl` valido.
- **`/health/ready` falhando**: verifique conectividade/credenciais MySQL.
- **falha em migrations**: rode `dotnet ef migrations list` e `dotnet run --project src/GoodHamburguer.Api -- --migrate-only`.

## Licenca
Distribuido sob a licenca MIT. Veja `LICENSE`.
