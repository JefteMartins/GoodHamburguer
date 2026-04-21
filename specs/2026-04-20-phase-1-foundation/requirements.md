# Requirements

## Escopo

Esta feature cobre a `Fase 1 - Fundacao do Projeto`, com foco em preparar uma base executavel, organizada e pronta para sustentar a evolucao do back-end antes da implementacao das regras de negocio.

O objetivo desta fase e deixar a solution pronta para crescimento, mantendo coerencia com:

- `specs/mission.md`
- `specs/tech-stack.md`
- `specs/roadmap.md`

## Projetos que Devem Existir

Os seguintes projetos devem ser criados ja nesta fase:

- `src/GoodHamburguer.Api`
- `src/GoodHamburguer.Application`
- `src/GoodHamburguer.Domain`
- `src/GoodHamburguer.Infrastructure`
- `src/GoodHamburguer.Blazor`
- `tests/GoodHamburguer.UnitTests`
- `tests/GoodHamburguer.IntegrationTests`

## Decisoes Confirmadas

### Ordem de Implementacao

A ordem de trabalho desta fase deve ser:

1. solution e projetos
2. arquitetura e referencias entre camadas
3. versionamento e Swagger
4. configuracao por ambiente
5. Docker
6. README inicial

### Estado Esperado da Solucao

Ao final da fase, a solution deve compilar integralmente, mesmo sem regras de negocio implementadas.

### Frontend

O projeto `GoodHamburguer.Blazor` deve existir e estar pronto para evolucao posterior, mas nao precisa entregar fluxo funcional nesta fase.

### API

A API deve sair desta fase com:

- `Controllers`
- versionamento `v1`
- Swagger versionado
- configuracao basica de `ProblemDetails`

Mesmo que ainda existam poucos endpoints reais, a base da API deve refletir a arquitetura final desejada.

### Docker

Nesta fase, o ambiente Docker deve incluir:

- `API`
- `Blazor`
- `MySQL`

Artefatos esperados:

- `docker/docker-compose.yml`
- `docker/api/Dockerfile`
- `docker/blazor/Dockerfile`

O servico de `migration` nao deve ser implementado agora. A composicao deve apenas ficar pronta para recebê-lo na fase de persistencia.

### README

O README inicial deve conter:

- visao do projeto
- arquitetura
- como rodar com Docker
- estrutura das pastas
- roadmap resumido

## Contexto

Esta fase precisa equilibrar dois objetivos:

- deixar a leitura da arquitetura clara para avaliacao tecnica
- evitar adicionar pecas cenograficas ou artificiais cedo demais

Por isso:

- o frontend entra apenas como base arquitetural
- o Docker entra como estrutura real, mas sem antecipar migrations falsas
- a compilacao completa da solution e mais importante do que aparentar completude funcional
