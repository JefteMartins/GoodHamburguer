# Changelog

Todas as mudancas relevantes deste projeto sao registradas aqui com data e hora no horario local do projeto.

## [2026-04-23]

- [13:58] Fechamento da Fase 11 com validacao final da malha de testes, cobertura consolidada acima da meta definida e atualizacao do roadmap para `Fase 11 [Concluida]`.
- [09:35] Implementacao e validacao da Fase 10 com frontend Blazor interativo consumindo a API `v1`, quatro rotas principais (`/menu`, `/orders/new`, `/orders/{id}`, `/dashboard/orders`), clients HTTP dedicados, tratamento de `ProblemDetails`, testes de componente para fluxos principais, refinamento visual alinhado ao Stitch `9060755920845036151` e validacao manual em runtime via Docker.

## [2026-04-22]

- [17:09] Implementacao e validacao da Fase 9 com hardening leve do `docker-compose` (`healthcheck` em `api` e `blazor`, `depends_on` por saude e `restart policy`), ajuste dos Dockerfiles para probes HTTP com `curl`, documentacao operacional expandida no `README` e evidencia de stack completa saudavel (`mysql`, `migrations`, `api`, `blazor`) com readiness real.
- [13:05] Implementacao e validacao funcional da Fase 8 com `Serilog`, traces basicos com `OpenTelemetry`, health checks em `/health/live` e `/health/ready`, filtro global de excecoes com `ProblemDetails`, testes de integracao para saude e tratamento de erros e workflow principal de CI em `.github/workflows/ci.yml`.

## [2026-04-21]

- [21:47] Implementacao e validacao da Fase 7 com CRUD completo de pedidos em `v1`, incluindo listagem, consulta por identificador e remocao sobre persistencia MySQL real, com cobertura unitaria da aplicacao e testes de integracao HTTP do fluxo completo.
- [20:58] Implementacao e validacao da Fase 6 com calculo de `subtotal`, `discount` e `total` no dominio do pedido, exposicao dos valores nas respostas de criacao e atualizacao, ampliacao da cobertura unitaria da regra monetaria e validacao de integracao HTTP do contrato com desconto.
- [17:35] Implementacao da Fase 5 com validacao de criacao e atualizacao de pedido por `itemCode`, erros `422` com `ProblemDetails`, respostas separadas por campo, endpoint minimo de pedidos e refatoracao da persistencia para armazenar codigos por slot.
- [17:10] Sanitizacao da configuracao de banco para evitar `connection strings` e senhas hardcoded em `appsettings`, `docker-compose` e mensagens de erro de testes de integracao, com uso de `.env.example` para o ambiente Docker.
- [16:28] Implementacao da Fase 4 com persistencia real de catalogo e pedidos em MySQL, `DbContext` e repositórios na `Infrastructure`, migration inicial, seed idempotente, servico `migrations` no Docker Compose e testes de integracao com MySQL real via Testcontainers.
- [15:24] Modelagem inicial do agregado `Order` com slots explicitos para `sandwich`, `side` e `drink`, incluindo contratos de criacao e atualizacao na camada de aplicacao e testes unitarios da Fase 3.
- [13:46] Ajuste do workflow de PR automatico para usar o secret `AUTOMATION_PR_TOKEN` apenas na criacao do PR e pular a automacao sem falhar quando o secret nao estiver configurado.
- [13:13] Implementacao da Fase 2 com dominio do cardapio, categorias como `value object`, consulta preparada para persistencia futura e endpoint `GET /api/v1/menu` com resposta agrupada por categoria.
- [12:23] Adicao inicial do workflow no GitHub Actions para abrir automaticamente um pull request de `develop` para `main`.
- [12:08] Implementacao da Fase 1 com fundacao da solution `.NET 10`, estrutura base de projetos, API com `Controllers`, `v1`, Swagger versionado, `ProblemDetails`, base Docker e testes iniciais.

## [2026-04-20]

- [20:34] Adicao da especificacao da Fase 1 em `specs/2026-04-20-phase-1-foundation/`, incluindo `plan.md`, `requirements.md` e `validation.md`.
- [16:50] Definicao da constituicao inicial do projeto em `specs/mission.md`, `specs/tech-stack.md`, `specs/roadmap.md` e `specs/publico-alvo.md`.
