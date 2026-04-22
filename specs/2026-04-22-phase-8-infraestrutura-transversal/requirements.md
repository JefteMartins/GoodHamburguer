# Requirements

## Escopo

Esta feature cobre a `Fase 8 - Infraestrutura Transversal`, com foco em adicionar capacidades operacionais e de avaliacao tecnica ao back-end atual sem expandir ainda a superficie funcional do produto.

O objetivo desta fase e fortalecer confiabilidade, diagnostico e automacao sobre a API ja existente, mantendo alinhamento com:

- `specs/mission.md`
- `specs/tech-stack.md`
- `specs/roadmap.md`

## O que Esta Incluido

- logging estruturado da API
- traces basicos da aplicacao com OpenTelemetry
- health checks separados para `liveness` e `readiness`
- tratamento global e centralizado de excecoes HTTP com `ExceptionFilter`
- validacao real de `readiness` contra MySQL
- testes de integracao end-to-end cobrindo os endpoints de saude com persistencia real
- workflow de GitHub Actions para restore, build, testes unitarios e testes de integracao em `push` e `pull request`

## O que Fica Fora Desta Fase

- mudancas de contrato nos endpoints de dominio (`menu` e `orders`)
- metricas detalhadas, dashboards ou exporters externos
- autenticacao, autorizacao ou rate limiting
- deploy em nuvem
- frontend Blazor funcional
- validacao da subida completa por Docker, que pertence a `Fase 9`
- aumento sistematico de cobertura de testes alem do necessario para esta fase

## Decisoes Confirmadas

### Logging Estruturado

O projeto deve adotar logging estruturado com `Serilog`, em linha com `specs/tech-stack.md`.

Nesta fase:

- o bootstrap do logging deve acontecer no `Program`
- logs devem continuar legiveis localmente
- a configuracao deve funcionar em ambiente local, testes e CI sem depender de infraestrutura externa

### Observabilidade Basica

Os traces devem usar `OpenTelemetry` apenas no nivel necessario para demonstrar instrumentacao da API e da camada HTTP/ASP.NET Core.

Para manter a fase pequena e reviewable:

- nao e necessario exportar para Jaeger, Zipkin ou OTLP remoto
- basta garantir que a instrumentacao esteja registrada de forma clara e pronta para evolucao

### Health Checks

Os endpoints de saude precisam deixar explicita a diferenca entre:

- `liveness`: processo ASP.NET Core ativo
- `readiness`: aplicacao pronta para servir trafego, incluindo acesso ao MySQL

Os endpoints devem ter contrato simples e adequado para uso por Docker e avaliacao tecnica.

Para evitar ambiguidade durante implementacao e validacao, esta fase assume como contrato preferido:

- `GET /health/live` para `liveness`
- `GET /health/ready` para `readiness`

Se houver motivo tecnico forte para outro path, a spec deve ser atualizada antes de seguir.

### Tratamento Global de Excecoes

O projeto deve consolidar o tratamento de excecoes HTTP em um ponto central da camada `Api`, usando `ExceptionFilter`.

Essa abordagem deve:

- evitar `try/catch` espalhados em controllers
- permitir traducao consistente para `ProblemDetails`
- suportar polimorfismo no tratamento de tipos de excecao
- manter dominio e aplicacao desacoplados de conceitos HTTP

O desenho preferido e:

- excecoes tipadas nas camadas `Domain` e `Application`, sem conhecer `status code` ou `ProblemDetails`
- mapeadores ou handlers por tipo concreto de excecao na camada `Api`
- um filtro global que resolve o mapeador mais especifico para a excecao recebida

Nesta fase, excecoes nao mapeadas explicitamente devem continuar resultando em resposta `500` com log estruturado adequado.

Para manter os testes de integracao deterministas, a API deve expor um caminho controlado para provocar pelo menos:

- uma excecao conhecida mapeada pelo filtro
- uma excecao desconhecida, tratada como `500`

Esse caminho pode ser implementado por um endpoint tecnico restrito ao ambiente de teste ou por outra estrategia equivalente, desde que nao polua os contratos de dominio de `menu` e `orders`.

### Reuso da Infraestrutura de Testes

Os testes de saude e readiness devem reaproveitar a infraestrutura ja existente com `MySqlWebApplicationFactory` e `Testcontainers`, evitando criar uma segunda estrategia de ambiente.

O probe de `readiness` deve verificar conectividade real com MySQL em nivel suficiente para diferenciar:

- aplicacao viva sem banco pronto
- aplicacao pronta com banco acessivel

Nao basta um endpoint estatico ou um check que valide apenas configuracao carregada em memoria.

### CI Minimo Confiavel

O workflow desta fase deve cobrir o caminho principal de confianca do repositorio:

- restore
- build
- testes unitarios
- testes de integracao

Nao e necessario introduzir matriz de sistema operacional, cache sofisticado ou etapas adicionais de release nesta fase.

## Contexto e Restricoes

O projeto ja possui:

- API versionada em `v1`
- persistencia MySQL real
- testes de integracao com `Testcontainers`
- um workflow limitado a abertura automatica de PR

O projeto ainda nao possui:

- logging estruturado configurado
- traces basicos registrados
- endpoints de health check operacionais
- tratamento global e polimorfico de excecoes HTTP
- pipeline principal de validacao automatica no GitHub Actions

Por isso, esta fase deve priorizar ganhos transversais de operacao e demonstracao tecnica sem reabrir decisoes ja estabilizadas das fases 1 a 7.

## Padroes a Seguir

- concentrar a composicao de infraestrutura no `Program` e nas extensoes de DI ja existentes
- evitar logica de saude distribuida em controllers ad hoc quando `HealthChecks` do ASP.NET Core resolverem melhor
- concentrar a traducao de excecoes HTTP em filtro global e mapeadores especializados, nao nas excecoes de dominio
- manter os testes de integracao com banco real e sem mocks desnecessarios
- manter a automacao de CI simples, deterministica e alinhada aos comandos reais usados localmente
- nao introduzir dependencias operacionais que compliquem a `Fase 9 - Integracao via Docker`
