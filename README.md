# GoodHamburguer

API e frontend para o desafio tecnico da Good Hamburger, construidos com foco em clareza arquitetural, evolucao segura e facilidade de avaliacao local.

## Visao do Projeto

O objetivo deste repositorio e entregar uma solucao completa em .NET para registrar pedidos da lanchonete Good Hamburger, cobrindo:

- API REST com `Controllers`
- versionamento `v1`
- Swagger versionado
- frontend em Blazor Web App
- persistencia real em MySQL
- containerizacao com Docker

Nesta etapa, o projeto ja persiste catalogo e pedidos em MySQL, com migration inicial, seed idempotente e servico dedicado de migracao no ambiente Docker.

## Arquitetura

O projeto segue uma base de Clean Architecture com DDD pragmatico, organizada em:

- `src/GoodHamburguer.Api`
  Camada HTTP, configuracao da API, versionamento e Swagger.
- `src/GoodHamburguer.Application`
  Casos de uso e orquestracao da aplicacao.
- `src/GoodHamburguer.Domain`
  Regras e tipos centrais do dominio.
- `src/GoodHamburguer.Infrastructure`
  Integracoes externas e persistencia.
- `src/GoodHamburguer.Blazor`
  Interface web preparada para evolucao posterior.
- `tests/GoodHamburguer.UnitTests`
  Testes unitarios.
- `tests/GoodHamburguer.IntegrationTests`
  Testes de integracao.

## Estrutura de Pastas

```text
.
|-- docker/
|   |-- api/
|   |   `-- Dockerfile
|   |-- blazor/
|   |   `-- Dockerfile
|   `-- docker-compose.yml
|-- specs/
|-- src/
|-- tests/
`-- GoodHamburguer.slnx
```

## Como Rodar com Docker

O ambiente local previsto e:

- `blazor` em `http://localhost:8080`
- `api` em `http://localhost:8081`
- `mysql` em `localhost:3306`

Comando principal:

```powershell
Copy-Item .env.example .env
docker compose -f docker/docker-compose.yml up --build
```

Observacoes:

- o arquivo `.env` deve existir antes de subir o ambiente; use `.env.example` como base
- o servico `migrations` aplica a migration inicial e executa o seed idempotente
- o menu passa a ser lido do MySQL, nao mais de uma fonte em memoria
- o seed inicial inclui o catalogo do desafio e alguns pedidos de exemplo

## Persistencia Atual

Nesta etapa, a solution entrega:

- `DbContext` restrito a `Infrastructure`
- leitura do menu a partir do MySQL
- persistencia inicial do agregado `Order`
- migration inicial e seed idempotente
- servico dedicado de migration no `docker-compose`
- testes de integracao com MySQL real via Testcontainers

## Roadmap Resumido

Proximas fases principais:

1. regras de validacao
2. calculo de valores
3. CRUD de pedidos
4. infraestrutura transversal
5. frontend funcional

Os detalhes completos estao em `specs/roadmap.md`.
