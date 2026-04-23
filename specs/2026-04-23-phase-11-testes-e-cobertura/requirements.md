# Requirements

## Escopo

Esta feature cobre a `Fase 11 - Testes e Cobertura`, com foco em consolidar a confianca tecnica do projeto apos a entrega funcional do backend, da infraestrutura e do frontend Blazor.

A fase deve ampliar e fortalecer a malha automatizada existente em tres niveis:

- testes unitarios
- testes de integracao
- testes Blazor

Tambem deve transformar cobertura em evidencia objetiva da entrega por meio de coleta, consolidacao e validacao em CI.

## O que Esta Incluido

- mapeamento das lacunas mais relevantes das suites atuais
- ampliacao de testes unitarios para dominio, aplicacao e validacao
- ampliacao de testes de integracao para API, contratos HTTP, `ProblemDetails` e persistencia real
- ampliacao de testes Blazor para clients e paginas principais
- cobertura de cenarios de happy path, edge cases e falhas relevantes
- consolidacao da coleta de cobertura em execucao automatizada
- producao de relatorio de cobertura utilizavel por humanos e CI
- gate objetivo de cobertura minima global em pipeline

## Suites e Alvos Minimos

### Unit Tests

Os testes unitarios devem continuar priorizando isolamento, velocidade e clareza. A fase deve cobrir e ampliar cenarios sobre:

- `MenuCatalog`
- `Order`
- `OrderDraftingService`
- `OrderAppService`
- `CreateOrderRequestValidator`
- `UpdateOrderRequestValidator`

### Integration Tests

Os testes de integracao devem validar comportamento real entre camadas, incluindo:

- endpoint de menu
- CRUD de pedidos
- health checks
- tratamento de excecoes
- contratos `ProblemDetails`
- persistencia com MySQL real de teste

### Blazor Tests

Os testes Blazor devem reforcar confianca sobre:

- `MenuApiClient`
- `OrderApiClient`
- `/menu`
- `/orders/new`
- `/orders/{id}`
- `/dashboard/orders`

Os testes devem cobrir especialmente estados de carregamento, vazio, erro e sucesso, alem das principais interacoes de criar, atualizar, listar e remover pedidos.

## Meta de Cobertura

A fase deve perseguir cobertura minima global de `70%` como sinal objetivo de confianca.

Essa meta nao substitui a qualidade dos testes. O trabalho nao deve introduzir casos artificiais so para elevar percentual. Sempre que houver trade-off, a prioridade deve ser:

1. proteger comportamento importante
2. fechar lacunas reais
3. usar cobertura como confirmacao, nao como fim em si

## Decisoes Confirmadas

### Cobertura como evidencia, nao como teatro

O projeto ja referencia `coverlet.collector` nas suites de teste. A fase deve aproveitar essa base e evoluir a solucao para um fluxo reproduzivel de coleta e consolidacao.

### Escopo amplo, mas ainda reviewable

A escolha para esta fase e ambiciosa. Mesmo assim, a implementacao deve ser quebrada em lotes pequenos por suite e por risco, para manter revisao e validacao viaveis.

### CI como parte do contrato da fase

Nao basta rodar cobertura localmente. A pipeline deve produzir evidencia suficiente para que a fase possa ser validada de forma repetivel por qualquer pessoa avaliadora do projeto.

### Sem ampliar a fase com E2E browser completos por padrao

O objetivo desta fase e confianca automatizada ampla dentro da stack ja existente. Testes de navegador completos podem ser considerados depois, mas nao sao requisito obrigatorio desta fase.

## O que Fica Fora Desta Fase

- redesign de arquitetura sem relacao direta com testes ou cobertura
- refatoracoes cosmeticas sem ganho claro de confianca
- telemetria de observabilidade adicional sem ligacao com validacao da fase
- testes E2E browser completos como requisito obrigatorio
- metas de cobertura por arquivo sem contexto

## Contexto e Restricoes

O projeto ja possui:

- API funcional em `v1`
- persistencia MySQL real
- frontend Blazor integrado
- Docker Compose funcional
- suites de teste separadas por natureza

A fase deve trabalhar sobre essa base, preservando os padroes existentes do repositório e evitando introduzir complexidade operacional desnecessaria.

## Padroes a Seguir

- manter alinhamento com `specs/mission.md`, `specs/tech-stack.md` e `specs/roadmap.md`
- preferir testes pequenos, legiveis e orientados a comportamento
- priorizar falhas relevantes e contratos criticos antes de buscar volume
- manter a estrutura atual das suites, evitando reorganizacoes gratuitas
- tornar os comandos de validacao claros para execucao local e em CI

