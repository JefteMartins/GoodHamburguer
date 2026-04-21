# Changelog

Todas as mudancas relevantes deste projeto sao registradas aqui com data e hora no horario local do projeto.

## [Unreleased]

- [2026-04-21 17:10 -03:00] Sanitizacao da configuracao de banco para evitar `connection strings` e senhas hardcoded em `appsettings`, `docker-compose` e mensagens de erro de testes de integracao, com uso de `.env.example` para o ambiente Docker.
- [2026-04-21 16:28 -03:00] Implementacao da Fase 4 com persistencia real de catalogo e pedidos em MySQL, `DbContext` e repositórios na `Infrastructure`, migration inicial, seed idempotente, servico `migrations` no Docker Compose e testes de integracao com MySQL real via Testcontainers.
- [2026-04-21 15:24 -03:00] Modelagem inicial do agregado `Order` com slots explicitos para `sandwich`, `side` e `drink`, incluindo contratos de criacao e atualizacao na camada de aplicacao e testes unitarios da Fase 3.
- [2026-04-21 13:46 -03:00] Ajuste do workflow de PR automatico para usar o secret `AUTOMATION_PR_TOKEN` apenas na criacao do PR e pular a automacao sem falhar quando o secret nao estiver configurado.
- [2026-04-21 13:13 -03:00] Implementacao da Fase 2 com dominio do cardapio, categorias como `value object`, consulta preparada para persistencia futura e endpoint `GET /api/v1/menu` com resposta agrupada por categoria.
- [2026-04-21 12:23 -03:00] Adicao inicial do workflow no GitHub Actions para abrir automaticamente um pull request de `develop` para `main`.
- [2026-04-21 12:08 -03:00] Implementacao da Fase 1 com fundacao da solution `.NET 10`, estrutura base de projetos, API com `Controllers`, `v1`, Swagger versionado, `ProblemDetails`, base Docker e testes iniciais.

## [2026-04-20]

- [2026-04-20 20:34 -03:00] Adicao da especificacao da Fase 1 em `specs/2026-04-20-phase-1-foundation/`, incluindo `plan.md`, `requirements.md` e `validation.md`.
- [2026-04-20 16:50 -03:00] Definicao da constituicao inicial do projeto em `specs/mission.md`, `specs/tech-stack.md`, `specs/roadmap.md` e `specs/publico-alvo.md`.
