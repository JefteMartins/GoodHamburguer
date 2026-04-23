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

## Contratos HTTP Principais

Rotas principais da API `v1`:

- `GET /api/v1/menu`: retorna o cardapio agrupado por categoria
- `GET /api/v1/orders`: lista pedidos persistidos
- `GET /api/v1/orders/{id}`: consulta um pedido existente
- `POST /api/v1/orders`: cria um pedido e retorna `201 Created`
- `PUT /api/v1/orders/{id}`: atualiza um pedido existente
- `DELETE /api/v1/orders/{id}`: remove um pedido e retorna `204 No Content`

Erros de validacao usam `422 Unprocessable Entity` com `ValidationProblemDetails`.
Recursos inexistentes retornam `404 Not Found` com `ProblemDetails`.

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

## Decisoes de Arquitetura

- Clean Architecture com DDD pragmatico para separar dominio, aplicacao, infraestrutura e entrega HTTP sem cerimonia excessiva.
- Regras de subtotal, desconto e total permanecem no dominio para manter a camada HTTP fina e previsivel.
- MySQL real e migrations entram na primeira entrega para aproximar o projeto de um cenario de avaliacao mais realista.
- O frontend Blazor consome a API `v1`, mas o Stitch permanece apenas como referencia de design e planejamento.

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

Pre-requisitos minimos:

- Docker Desktop (ou Docker Engine + Compose v2)
- portas `8080`, `8081` e `3306` livres

Fluxo oficial:

```powershell
if (!(Test-Path .env)) { Copy-Item .env.example .env }
docker compose --env-file .env -f docker/docker-compose.yml config
docker compose --env-file .env -f docker/docker-compose.yml up --build
```

Ordem esperada de inicializacao:

1. `mysql` fica `healthy`
2. `migrations` executa migration/seed e finaliza com sucesso
3. `api` sobe e passa no healthcheck
4. `blazor` sobe apos a API saudavel

Verificacao de saude:

```powershell
Invoke-WebRequest http://localhost:8081/health/live
Invoke-WebRequest http://localhost:8081/health/ready
```

Comandos uteis:

```powershell
docker compose --env-file .env -f docker/docker-compose.yml ps
docker compose --env-file .env -f docker/docker-compose.yml logs -f api
docker compose --env-file .env -f docker/docker-compose.yml logs -f blazor
docker compose --env-file .env -f docker/docker-compose.yml logs -f migrations
docker compose --env-file .env -f docker/docker-compose.yml down
```

Observacoes importantes:

- o arquivo `.env` deve existir antes de subir o ambiente; use `.env.example` como base
- o servico `migrations` aplica a migration inicial e executa o seed idempotente
- o menu passa a ser lido do MySQL, nao mais de uma fonte em memoria
- o seed inicial inclui o catalogo do desafio e alguns pedidos de exemplo

Troubleshooting rapido:

- `port is already allocated`: liberar a porta em uso ou ajustar mapeamento no `docker-compose.yml`
- `migrations` falhou: verificar logs de `migrations` e status do `mysql` (`docker compose ... ps`)
- `api` ou `blazor` ficando `unhealthy`: verificar logs do servico e confirmar que o processo subiu na porta `8080` dentro do container
- `readiness` falhando: validar credenciais do `.env` e disponibilidade real do MySQL

## Persistencia Atual

Nesta etapa, a solution entrega:

- `DbContext` restrito a `Infrastructure`
- leitura do menu a partir do MySQL
- persistencia inicial do agregado `Order`
- migration inicial e seed idempotente
- servico dedicado de migration no `docker-compose`
- testes de integracao com MySQL real via Testcontainers

## Exemplo de Erro

Exemplo resumido de resposta para payload invalido:

```json
{
  "type": "https://httpstatuses.com/422",
  "title": "One or more validation errors occurred.",
  "status": 422,
  "detail": "The request payload contains invalid values.",
  "instance": "/api/v1/orders",
  "errors": {
    "sandwichItemCode": [
      "The informed item code does not exist in the menu."
    ]
  }
}
```

## Limites Conhecidos

- a primeira entrega nao cobre autenticacao, pagamento, estoque ou multiunidade
- o pedido continua limitado a no maximo um item por categoria principal
- a observabilidade desta versao e propositalmente basica, com logs estruturados, traces iniciais e health checks

## Roadmap Resumido

Proximas fases principais:

1. frontend funcional em Blazor
2. consolidacao de testes e cobertura
3. refinamentos operacionais e de documentacao

Os detalhes completos estao em `specs/roadmap.md`.
