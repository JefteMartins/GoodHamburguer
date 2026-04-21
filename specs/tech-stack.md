# Tech Stack

## Direcao Tecnica

Como o desafio pede C# com .NET / ASP.NET Core, a stack deve privilegiar produtividade, clareza arquitetural, facilidade de demonstrar boas decisoes tecnicas e execucao simples para avaliacao local.

## Stack Base

- Runtime: .NET 10
- Linguagem: C# 14
- API: ASP.NET Core Web API com Controllers
- Frontend: Blazor Web App com Interactive Server
- ORM: Entity Framework Core
- Banco de dados: MySQL 8.4
- Documentacao de endpoints: Swagger / OpenAPI
- Testes: xUnit
- Assertions de teste: FluentAssertions
- Containerizacao: Docker + Docker Compose
- CI: GitHub Actions

## Estrutura Sugerida

- `src/GoodHamburguer.Api`
  API HTTP com Controllers, configuracao, middlewares e contratos externos.
- `src/GoodHamburguer.Blazor`
  Interface de demonstracao consumindo a API e cobrindo os fluxos principais.
- `src/GoodHamburguer.Application`
  Casos de uso, servicos de aplicacao e orquestracao das operacoes de pedido.
- `src/GoodHamburguer.Domain`
  Entidades, enums, regras de negocio, calculo de desconto e validacoes centrais.
- `src/GoodHamburguer.Infrastructure`
  Repositorios, persistencia com EF Core, configuracao do MySQL e adaptadores externos.
- `tests/GoodHamburguer.UnitTests`
  Testes das regras de negocio e validacoes.
- `tests/GoodHamburguer.IntegrationTests`
  Testes de integracao com persistencia real e validacao end-to-end entre camadas.
- `docker/`
  Artefatos de containerizacao centralizados, como `docker/docker-compose.yml`, `docker/api/Dockerfile` e `docker/blazor/Dockerfile`.

## Decisoes Iniciais

### Arquitetura

Adotar Clean Architecture com orientacao a DDD de forma pragmatica. O objetivo nao e inflar o projeto, mas separar com clareza dominio, aplicacao, infraestrutura e entrega HTTP para facilitar leitura, evolucao e testes, evitando complexidade cerimonial que nao agregue ao desafio.

### Persistencia

Adotar MySQL 8.4 desde o inicio, com acesso via EF Core e repositorios orientados a interface para os agregados principais. O `DbContext` deve ficar restrito a camada `Infrastructure`, sem ser referenciado diretamente por `Application` ou `Api`. Isso aproxima a solucao de um ambiente real e demonstra preocupacao com modelagem, migracoes e configuracao de infraestrutura sem abrir mao do desacoplamento do dominio.

### Infraestrutura Local

Docker deve entrar desde a primeira fase para que qualquer avaliador consiga subir a aplicacao com poucos comandos. O ambiente local ideal inclui:

- container da API
- container do frontend Blazor
- container do MySQL com imagem oficial `mysql:8.4`
- variaveis de ambiente para conexao
- comando unico para subir tudo com `docker compose up --build`
- estrutura pronta para receber um servico de migracao no `docker-compose`

O servico de migracao, reutilizando a imagem da aplicacao para executar migrations e seed, deve entrar junto com a fase de persistencia real. Nesse momento, ele sera responsavel por aplicar as migrations e preparar o seed inicial de demonstracao, incluindo cardapio e alguns pedidos de exemplo.

### Modelagem de Pedido

Cada pedido deve refletir diretamente as restricoes do desafio:

- zero ou um sanduiche
- zero ou uma batata
- zero ou um refrigerante

Essa modelagem evita listas genericas demais e torna a validacao de duplicidade mais simples e explicita.

### Calculo de Preco

O calculo de subtotal, desconto e total deve ficar no dominio do `Pedido`, como regra central da entidade ou agregado. A camada HTTP nao deve conter regra promocional.

### Tratamento de Erros

Padronizar erros com `ProblemDetails` do ASP.NET Core, incluindo mensagem clara e codigo HTTP adequado:

- `400 Bad Request` para pedido invalido
- `404 Not Found` para recurso inexistente

### Qualidade Operacional

A base tecnica deve incluir ainda durante a implementacao do back-end:

- padronizacao de erros
- logging estruturado com Serilog
- health checks com endpoints separados
- observabilidade basica com OpenTelemetry
- versionamento de API em `v1`
- cobertura de testes como indicador de confianca, com meta minima de 70%

### Versionamento de API

A API deve expor a primeira versao desde o inicio por `URL segment`, no formato `/api/v1/...`. O Swagger tambem deve refletir esse versionamento e servir como principal ponto de exploracao da API na demonstracao local.

### Observabilidade e Saude

Na primeira entrega, a observabilidade deve incluir:

- logging estruturado
- health checks para aplicacao e MySQL
- traces basicos com OpenTelemetry

Os health checks devem distinguir:

- `liveness` para a aplicacao
- `readiness` validando dependencias como o MySQL

## Pacotes Base

- `Swashbuckle.AspNetCore` para Swagger
- `FluentValidation` para validacoes de entrada e aplicacao
- `Microsoft.EntityFrameworkCore`
- provider MySQL para EF Core, como `Pomelo.EntityFrameworkCore.MySql`
- `Microsoft.EntityFrameworkCore.Design` para migracoes
- `Asp.Versioning.Http` para versionamento de API
- `OpenTelemetry` para traces basicos
- `Serilog` para logging estruturado
- pacote de health checks para MySQL
- `Testcontainers for .NET` para testes de integracao com MySQL real

## Diferenciais se Houver Tempo

- refinamentos visuais e de UX na interface Blazor
- observabilidade mais rica com metricas
- seed demonstrativo mais completo
- validacoes extras na pipeline de CI

## Criterios para Escolhas

Cada tecnologia adotada deve responder a pelo menos um destes objetivos:

- reduzir complexidade acidental
- tornar as regras mais explicitas
- facilitar testes
- melhorar a apresentacao da solucao no repositorio
