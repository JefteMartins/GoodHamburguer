# Fase 8 - Infraestrutura Transversal Implementation Plan

**Goal:** adicionar logging estruturado, traces basicos, health checks com `liveness/readiness`, tratamento global de excecoes com `ExceptionFilter`, validacao real de readiness contra MySQL e pipeline minima de CI para build e testes.

**Architecture:** a composicao fica centralizada em `src/GoodHamburguer.Api/Program.cs`, os checks de saude usam a infraestrutura nativa de `HealthChecks` com dependencia real do `GoodHamburguerDbContext` ou da conexao MySQL, o tratamento de excecoes HTTP fica centralizado em um `ExceptionFilter` global apoiado por mapeadores polimorficos por tipo de excecao, os testes end-to-end estendem a suite de integracao existente baseada em `MySqlWebApplicationFactory`, e a automacao de CI entra em `.github/workflows/` usando os mesmos comandos de `dotnet` executados localmente.

**Tech Stack:** .NET 10, C# 14, ASP.NET Core Web API, `ExceptionFilter`, `ProblemDetails`, Serilog, OpenTelemetry, HealthChecks, xUnit, Testcontainers, GitHub Actions

---

## File Map

- Modify: `src/GoodHamburguer.Api/GoodHamburguer.Api.csproj`
- Modify: `src/GoodHamburguer.Api/Program.cs`
- Modify if needed: `src/GoodHamburguer.Api/appsettings.json`
- Modify if needed: `src/GoodHamburguer.Api/appsettings.Development.json`
- Modify or replace: `src/GoodHamburguer.Api/Controllers/SystemController.cs`
- Create: `src/GoodHamburguer.Api/Exceptions/GlobalExceptionFilter.cs`
- Create: `src/GoodHamburguer.Api/Exceptions/IExceptionProblemDetailsMapper.cs`
- Create or modify: `src/GoodHamburguer.Api/Exceptions/*`
- Create if needed: `src/GoodHamburguer.Api/Controllers/TestOnlyController.cs`
- Modify: `tests/GoodHamburguer.IntegrationTests/GoodHamburguer.IntegrationTests.csproj`
- Modify: `tests/GoodHamburguer.IntegrationTests/Infrastructure/MySqlWebApplicationFactory.cs`
- Create or modify: `tests/GoodHamburguer.IntegrationTests/SystemHealthEndpointTests.cs`
- Create or modify: `tests/GoodHamburguer.IntegrationTests/ExceptionHandlingTests.cs`
- Create: `.github/workflows/ci.yml`
- Modify if needed: `CHANGELOG.md`

## Task Group 1: Adicionar a base operacional da API

**Files:**
- Modify: `src/GoodHamburguer.Api/GoodHamburguer.Api.csproj`
- Modify: `src/GoodHamburguer.Api/Program.cs`
- Modify if needed: `src/GoodHamburguer.Api/appsettings.json`
- Modify if needed: `src/GoodHamburguer.Api/appsettings.Development.json`

1. Adicionar os pacotes necessarios para `Serilog`, `OpenTelemetry` e `HealthChecks`.
2. Configurar bootstrap de logging estruturado no startup da API.
3. Registrar traces basicos para ASP.NET Core e/ou HTTP client somente no escopo minimo desta fase.
4. Registrar o `ExceptionFilter` global e seus mapeadores especializados na DI da API.
5. Garantir que a configuracao continue funcional em ambiente local e em testes sem exigir infraestrutura externa.
6. Registrar health checks com separacao clara entre `liveness` e `readiness`.
7. Mapear explicitamente os endpoints de saude em `/health/live` e `/health/ready`.

## Task Group 2: Centralizar o tratamento de excecoes HTTP

**Files:**
- Create: `src/GoodHamburguer.Api/Exceptions/GlobalExceptionFilter.cs`
- Create: `src/GoodHamburguer.Api/Exceptions/IExceptionProblemDetailsMapper.cs`
- Create or modify: `src/GoodHamburguer.Api/Exceptions/*`
- Create if needed: `src/GoodHamburguer.Api/Controllers/TestOnlyController.cs`
- Modify: `src/GoodHamburguer.Api/Program.cs`

1. Definir o contrato de mapeamento polimorfico por tipo de excecao.
2. Implementar o filtro global para capturar excecoes nao tratadas na borda MVC.
3. Resolver o handler ou mapper mais especifico para o tipo concreto recebido.
4. Traduzir excecoes conhecidas para `ProblemDetails` consistentes.
5. Garantir que excecoes desconhecidas retornem `500` com logging estruturado.
6. Manter excecoes de `Domain` e `Application` livres de detalhes HTTP.
7. Definir um mecanismo deterministico para os testes de integracao dispararem excecoes conhecidas e desconhecidas sem contaminar os endpoints de dominio.

## Task Group 3: Expor endpoints de saude alinhados ao roadmap

**Files:**
- Modify or replace: `src/GoodHamburguer.Api/Controllers/SystemController.cs`
- Modify: `src/GoodHamburguer.Api/Program.cs`

1. Definir a superficie final dos endpoints de saude.
2. Preservar `system/info` apenas se ele ainda agregar valor; caso contrario, simplificar a camada de sistema.
3. Expor `liveness` e `readiness` em `/health/live` e `/health/ready` para avaliacao manual, Docker e testes automatizados.
4. Fazer `readiness` falhar quando a dependencia real de MySQL nao estiver pronta.
5. Implementar `readiness` com verificacao real de conectividade ao MySQL, nao apenas validacao de configuracao.

## Task Group 4: Cobrir infraestrutura transversal com testes de integracao reais

**Files:**
- Modify: `tests/GoodHamburguer.IntegrationTests/Infrastructure/MySqlWebApplicationFactory.cs`
- Create or modify: `tests/GoodHamburguer.IntegrationTests/SystemHealthEndpointTests.cs`
- Create or modify: `tests/GoodHamburguer.IntegrationTests/ExceptionHandlingTests.cs`

1. Escrever testes de integracao para `liveness` e `readiness`.
2. Reutilizar o container MySQL ja existente no factory.
3. Validar que a aplicacao sobe com os health checks registrados no host de testes.
4. Confirmar que `readiness` responde com sucesso quando o banco esta pronto.
5. Escrever testes para o filtro global cobrindo pelo menos uma excecao conhecida e uma excecao nao mapeada.
6. Se houver um caminho viavel e barato, cobrir tambem o comportamento de falha de readiness sem tornar a suite instavel.

## Task Group 5: Adicionar pipeline principal de CI

**Files:**
- Create: `.github/workflows/ci.yml`

1. Criar workflow acionado em `push` e `pull_request`.
2. Configurar `actions/setup-dotnet` com a versao do projeto.
3. Executar restore da solution.
4. Executar build da solution.
5. Executar testes unitarios.
6. Executar testes de integracao.
7. Manter o workflow simples, sem etapas fora do escopo desta fase.

## Task Group 6: Fechar validacao e preparacao para merge

**Files:**
- Modify if needed: `CHANGELOG.md`
- Modify if needed: `specs/roadmap.md`

1. Rodar a verificacao completa local:
   - `dotnet build GoodHamburguer.slnx -v minimal`
   - `dotnet test tests/GoodHamburguer.UnitTests/GoodHamburguer.UnitTests.csproj -v minimal`
   - `dotnet test tests/GoodHamburguer.IntegrationTests/GoodHamburguer.IntegrationTests.csproj -v minimal`
2. Validar manualmente os endpoints de saude.
3. Confirmar que a nova pipeline reflete os mesmos comandos aprovados localmente.
4. Atualizar `CHANGELOG.md` quando a fase estiver concluida.
5. Marcar a Fase 8 como concluida em `specs/roadmap.md` somente apos validacao real.
