# Changelog

Todas as mudancas relevantes deste projeto serao registradas aqui.

## [Unreleased]

### Added

- solution `.NET 10` criada em `GoodHamburguer.slnx`
- estrutura base de projetos em `src/` e `tests/`
- fundacao da API com `Controllers`, `v1`, Swagger versionado e `ProblemDetails`
- frontend em Blazor Web App preparado para evolucao posterior
- configuracoes iniciais por ambiente
- estrutura Docker base com `API + Blazor + MySQL`
- README inicial da fundacao
- smoke tests da fundacao com `FluentAssertions`
- especificacao da feature da Fase 2 em `specs/2026-04-21-phase-2-menu-domain/`
- dominio do cardapio com categorias como `value object`
- consulta de cardapio via abstracao preparada para persistencia futura
- endpoint `GET /api/v1/menu` com resposta agrupada por categoria
- testes unitarios e de integracao para a consulta do cardapio
- especificacao da feature da Fase 3 em `specs/2026-04-21-phase-3-order-modeling/`
- agregado `Order` com slots explicitos para `sandwich`, `side` e `drink`
- contratos iniciais de criacao e atualizacao de pedido na camada de aplicacao
- servico de modelagem para transformar contratos em entidade de dominio
- testes unitarios da modelagem de pedido e dos contratos de aplicacao

## [2026-04-20]

### Added

- constituicao inicial do projeto em `specs/mission.md`, `specs/tech-stack.md`, `specs/roadmap.md` e `specs/publico-alvo.md`
- especificacao da feature da Fase 1 em `specs/2026-04-20-phase-1-foundation/`
