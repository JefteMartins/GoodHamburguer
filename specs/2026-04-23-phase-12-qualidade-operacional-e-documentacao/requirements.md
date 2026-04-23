# Requirements

## Escopo

Esta feature cobre a `Fase 12 - Qualidade Operacional e Documentacao`, com foco em deixar a primeira entrega mais robusta, previsivel e apresentavel apos a conclusao da Fase 11.

A fase deve refinar a superficie externa e operacional da solucao em quatro frentes:

- contratos HTTP e codigos de status
- mensagens e payloads de erro
- health endpoints e metadados operacionais basicos
- documentacao principal e clareza da pipeline de CI

## O que Esta Incluido

- revisao dos contratos HTTP principais da API de pedidos
- ajuste de codigos HTTP para maior previsibilidade externa quando fizer sentido
- reforco da padronizacao de `ProblemDetails` e `ValidationProblemDetails`
- payloads mais informativos para endpoints de health
- atualizacao do endpoint de informacao do sistema para refletir melhor o estado da entrega
- ampliacao do `README.md` com foco em arquitetura, avaliacao tecnica, contratos e limites da solucao
- refinamento da pipeline de CI para tornar os sinais operacionais e os artefatos mais claros
- registro da conclusao da fase em `CHANGELOG.md` quando a implementacao estiver validada

## API e Contratos Externos

Os contratos HTTP mais sensiveis desta fase sao:

- `GET /api/v1/menu`
- `GET /api/v1/orders`
- `GET /api/v1/orders/{id}`
- `POST /api/v1/orders`
- `PUT /api/v1/orders/{id}`
- `DELETE /api/v1/orders/{id}`

Esta fase deve priorizar previsibilidade externa para quem consome a API. Isso inclui:

- respostas coerentes com a semantica da operacao
- payloads de erro mais completos e consistentes
- documentacao suficiente para que a leitura do repositório explique rapidamente o comportamento esperado

## Erros e ProblemDetails

As respostas de erro devem continuar padronizadas com `ProblemDetails` do ASP.NET Core.

A fase deve reforcar especialmente:

- `ValidationProblemDetails` para entradas invalidas
- `ProblemDetails` para recurso inexistente
- consistencia de campos como `type`, `title`, `status`, `detail` e `instance` quando aplicavel
- mensagens claras para facilitar integracao e troubleshooting

## Superficie Operacional

A fase deve melhorar a superficie operacional basica da entrega sem transformar o projeto em uma iniciativa de observabilidade avancada.

Estao incluidos:

- refinamento dos payloads de `/health/live` e `/health/ready`
- manutencao de checks distintos para liveness e readiness
- alinhamento do endpoint `/api/v1/system/info` ao estado atual da entrega

Nao faz parte do escopo obrigatorio desta fase:

- adicionar dashboards externos
- exportadores novos de telemetria
- metricas avancadas
- tracing aprofundado

## Documentacao

O `README.md` deve ficar mais forte para avaliacao tecnica, cobrindo de forma objetiva:

- contexto do projeto
- resumo arquitetural
- contratos HTTP principais
- exemplos de erro
- limites conhecidos da primeira entrega
- fluxo de execucao local
- como API, frontend, banco e CI se encaixam na demonstracao

A documentacao deve continuar direta e util para leitura rapida. O objetivo nao e produzir um manual extenso, e sim uma base de avaliacao confiavel.

## Pipeline de CI

A pipeline principal deve continuar sustentando build, testes e cobertura, mas com sinais mais claros para humanos avaliadores. Esta fase deve favorecer:

- artefatos mais uteis para inspecao
- resumo mais legivel da cobertura
- guardrails simples como timeout e concorrencia quando isso melhorar legibilidade e confiabilidade operacional

## Decisoes Confirmadas

### Refinar sem redirecionar o projeto

Esta fase existe para fortalecer a entrega atual, nao para mudar o que a entrega e. O dominio, os fluxos principais e a arquitetura base devem permanecer os mesmos.

### README como parte do produto avaliado

Como a primeira entrega depende de boa impressao tecnica, o `README.md` faz parte do valor entregue e deve explicar o suficiente para uma avaliacao rapida e justa.

### CI como evidencia operacional

A pipeline nao deve apenas passar. Ela deve ajudar a mostrar que a solucao e verificavel, reproduzivel e compreensivel.

## O que Fica Fora Desta Fase

- novas funcionalidades de negocio
- redesign amplo de arquitetura
- redesign visual significativo do Blazor
- ampliacao da observabilidade para metricas e traces ricos
- nova suite E2E de navegador
- alteracoes de persistencia sem necessidade direta para os objetivos da fase

## Contexto e Restricoes

O projeto ja possui:

- API funcional em `v1`
- frontend Blazor integrado
- persistencia MySQL real
- Docker Compose funcional
- testes e cobertura consolidados na Fase 11

A Fase 12 deve trabalhar sobre essa base, preservando os padroes do repositorio e evitando complexidade acidental.

## Padroes a Seguir

- manter alinhamento com `specs/mission.md`, `specs/tech-stack.md` e `specs/roadmap.md`
- preferir refinamentos pequenos e reviewables
- priorizar contratos externos, clareza e avaliabilidade
- evitar misturar esta fase com melhorias incrementais tipicas da Fase 13
- manter a spec da fase como fonte de verdade, usando o plano detalhado apenas como guia de execucao
