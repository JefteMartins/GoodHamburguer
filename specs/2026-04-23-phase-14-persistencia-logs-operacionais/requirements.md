# Requirements

## Escopo

Esta feature cobre a `Fase 14 - Persistencia de Logs Operacionais`, com objetivo de registrar historico operacional da API em MySQL para auditoria basica e rastreabilidade de falhas.

O escopo funcional minimo da fase e:

- persistir logs normais de execucao (`application`)
- persistir logs de excecao (`error`)
- armazenar payload estruturado dos parametros de entrada
- armazenar contexto minimo de rastreabilidade (rota, metodo, correlacao, data/hora)
- oferecer estrategia de consulta por periodo e por tipo
- garantir cobertura automatizada para escrita e consulta

## O que Esta Incluido

- modelagem de tabela unica de logs operacionais no banco MySQL
- criacao de coluna `type` com valores `application` e `error`
- armazenamento de payload estruturado em `json`
- registracao de logs normais durante o pipeline HTTP
- registracao de logs de erro no fluxo de tratamento de excecoes
- consulta de logs com filtro por intervalo de data/hora e por tipo
- testes de unidade e integracao cobrindo persistencia e consulta

## O que Fica Fora Desta Fase

- dashboards e visualizacoes dedicadas
- exportacao de logs para sistemas externos
- politicas avancadas de retencao, expurgo e arquivamento
- analytics de negocio em cima dos logs
- mecanismos de busca textual avancada fora do banco principal

## Decisoes Confirmadas

### Tabela unica com tipagem explicita

A fase usara uma unica tabela de logs para reduzir complexidade e manter consulta operacional simples. A diferenciacao por tipo sera feita via coluna `type` (`application`/`error`).

### Payload estruturado de entrada

Os parametros de entrada do request devem ser registrados em formato estruturado (`json`) para permitir investigacao posterior sem acoplamento ao contrato interno de cada endpoint.

### Contexto minimo obrigatorio

Cada registro deve incluir, no minimo:

- `createdAtUtc`
- `type`
- `route`
- `method`
- `correlationId`
- `payload`

Para logs de erro, tambem e obrigatorio persistir:

- `errorMessage`
- tipo da excecao (`exceptionType`)

### Captura separada para normal e erro

- logs `application` serao gravados no fluxo normal do pipeline HTTP
- logs `error` serao gravados no tratamento global de excecoes

Essa separacao evita ambiguidade na semantica dos registros e facilita filtros de investigacao.

### Consulta com foco operacional

A estrategia de consulta deve priorizar investigacao rapida:

- filtro por periodo (`from` e `to`)
- filtro por `type`
- ordenacao por data/hora decrescente
- limite de itens por requisicao

## Contexto Tecnico e Restricoes

O projeto ja possui:

- ASP.NET Core API versionada em `v1`
- EF Core com MySQL e migracoes versionadas
- tratamento global de excecoes
- testes de integracao com MySQL real
- observabilidade basica por logs estruturados e OpenTelemetry

A Fase 14 deve respeitar essa base e evitar novas dependencias de infraestrutura para subir localmente.

## Requisitos Funcionais

1. Deve existir uma tabela unica de logs operacionais em MySQL.
2. Cada log deve possuir `type` com valores suportados `application` e `error`.
3. Logs `application` devem registrar parametros de entrada de forma estruturada.
4. Logs `error` devem registrar mensagem e tipo da excecao.
5. Cada log deve registrar data/hora UTC, rota, metodo HTTP e correlation id.
6. Deve existir consulta de logs por periodo e por tipo.
7. A consulta deve retornar resultados ordenados por data/hora desc.
8. A consulta deve aplicar limite maximo de itens retornados.

## Requisitos Nao Funcionais

1. A persistencia de logs nao deve introduzir acoplamento circular entre `Api`, `Application` e `Infrastructure`.
2. Falha de gravacao de log nao deve interromper o fluxo principal de resposta em cenarios normais.
3. A implementacao deve manter o projeto executavel por Docker sem servicos extras.
4. A escrita de logs deve ser testavel por automacao.
5. A consulta de logs deve ser testavel por automacao.

## Padroes e Alinhamento

- manter alinhamento com `specs/mission.md`, `specs/tech-stack.md` e `specs/roadmap.md`
- preservar Clean Architecture pragmatica ja adotada no repositorio
- manter nomes, contratos e convencoes de testes consistentes com fases anteriores
- usar evidencias de teste para validar conclusao da fase
