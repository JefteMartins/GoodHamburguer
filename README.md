# GoodHamburguer

API e frontend para o desafio tecnico da Good Hamburger, construidos com foco em clareza arquitetural, evolucao segura e facilidade de avaliacao local.

## Visao do Projeto

O objetivo deste repositorio e entregar uma solucao completa em .NET para registrar pedidos da lanchonete Good Hamburger, cobrindo:

- API REST com `Controllers`
- versionamento `v1`
- Swagger versionado
- frontend em Blazor Web App
- persistencia planejada em MySQL
- containerizacao com Docker

Esta branch implementa a fundacao do projeto, deixando a solution preparada para a evolucao das proximas fases do roadmap.

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

Esta fase deixa a estrutura Docker pronta para evolucao. O ambiente base previsto e:

- `blazor` em `http://localhost:8080`
- `api` em `http://localhost:8081`
- `mysql` em `localhost:3306`

Comando principal:

```powershell
docker compose -f docker/docker-compose.yml up --build
```

Observacoes:

- o servico de `migration` ainda nao entra nesta fase
- a persistencia real sera consolidada na fase de MySQL

## Foundation Scope

Nesta etapa, a solution entrega:

- projetos base criados e referenciados
- API pronta para evoluir com `v1`, Swagger e `ProblemDetails`
- frontend Blazor preparado para evolucao posterior
- configuracoes iniciais por ambiente
- estrutura Docker base com `API + Blazor + MySQL`

## Roadmap Resumido

Proximas fases principais:

1. dominio do cardapio
2. modelagem de pedido
3. persistencia MySQL
4. regras de validacao
5. calculo de valores
6. CRUD de pedidos
7. infraestrutura transversal
8. frontend funcional

Os detalhes completos estao em `specs/roadmap.md`.
