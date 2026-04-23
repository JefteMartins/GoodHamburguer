# Phase 11 Testes e Cobertura Design

## Contexto

As fases 1 a 10 ja entregaram a base funcional completa do projeto:

- dominio de cardapio e pedidos
- persistencia MySQL real
- API versionada `v1`
- observabilidade e health checks
- integracao via Docker
- frontend Blazor funcional

O repositório ja possui tres suites de teste:

- `tests/GoodHamburguer.UnitTests`
- `tests/GoodHamburguer.IntegrationTests`
- `tests/GoodHamburguer.Blazor.Tests`

Tambem ja existe `coverlet.collector` nas suites de teste, mas ainda nao existe uma estrategia consolidada de cobertura, publicacao de relatorios ou gate objetivo na CI.

## Problema

O projeto ja inspira confianca funcional, mas essa confianca ainda nao esta formalizada por uma fase dedicada a:

- mapear lacunas relevantes de teste
- ampliar a malha automatizada de forma intencional
- medir cobertura de maneira reprodutivel
- transformar cobertura em evidencia tecnica concreta antes da fase de qualidade final

Sem isso, a Fase 12 corre o risco de virar uma revisao ampla com base em percepcao, e nao em sinais objetivos.

## Objetivo

Definir a `Fase 11 - Testes e Cobertura` como uma fase ampla de consolidacao tecnica, cobrindo:

- ampliacao de testes unitarios, de integracao e de Blazor
- fechamento de lacunas em cenarios de falha, borda e contratos
- medicao consolidada de cobertura
- evolucao da CI para produzir evidencia de cobertura e validar um minimo global

## Abordagens Consideradas

### 1. Cobertura pragmatica focada so em fluxos criticos

Vantagens:

- fase menor
- menor risco operacional
- entrega mais rapida

Desvantagens:

- deixa muitas lacunas para fases posteriores
- nao conversa tao bem com a intencao ambiciosa do roadmap

### 2. Instrumentar cobertura primeiro e expandir testes depois

Vantagens:

- baseline objetivo antes de escrever novos testes
- menor chance de meta arbitraria

Desvantagens:

- cria uma fase mais operacional do que substantiva
- pode atrasar o ganho real de confianca

### 3. Expansao ampla com gate progressivo de cobertura

Vantagens:

- combina ampliacao real de testes com evidencia objetiva
- aproveita as tres suites ja existentes
- prepara o terreno para Fase 12 e Fase 13 com menos ambiguidade

Desvantagens:

- fase mais extensa
- exige decomposicao cuidadosa para continuar revisavel

## Abordagem Escolhida

Adotar a terceira abordagem: **expansao ampla com gate progressivo de cobertura**.

Isso significa que a fase nao vai perseguir percentual de cobertura isoladamente. O trabalho sera guiado por risco e por lacunas reais:

- regras de dominio e aplicacao ainda nao exercitadas
- cenarios HTTP e `ProblemDetails` ainda pouco protegidos
- estados e fluxos importantes do Blazor ainda com espaco para ampliacao
- pipeline sem consolidacao nem publicacao objetiva de cobertura

## Arquitetura da Fase

A fase sera organizada em quatro frentes:

### 1. Cobertura de dominio e aplicacao

Expandir testes unitarios sobre:

- entidades e value objects de dominio
- servicos de aplicacao
- validadores
- casos negativos e estados limite

### 2. Cobertura HTTP e persistencia real

Expandir testes de integracao sobre:

- contratos de endpoints
- codigos HTTP e `ProblemDetails`
- operacoes completas de CRUD
- comportamento com persistencia real e dependencias ativas

### 3. Cobertura do frontend Blazor

Expandir testes Blazor sobre:

- clients HTTP e mapeamento de erros
- paginas e estados de tela
- acoes de criar, listar, atualizar e remover
- estados de loading, empty, error e success

### 4. Evidencia objetiva na CI

Evoluir a pipeline para:

- coletar cobertura nas suites
- consolidar relatorios
- publicar artefatos de cobertura
- validar a meta minima global de 70%

## Restricoes e Decisoes

- A fase deve continuar orientada por SDD, com `plan.md`, `requirements.md` e `validation.md` como contrato antes da implementacao.
- A meta de `70%` e importante, mas nao deve incentivar testes artificiais.
- O gate deve ser global o bastante para proteger a entrega, mas sem mascarar lacunas graves em camadas relevantes.
- A implementacao deve passar pelo roteamento .NET apropriado na fase de execucao, porque a maior parte do trabalho sera em C#, ASP.NET Core, Blazor e testes .NET.
- A fase nao deve introduzir E2E browser completos se isso ampliar demais o escopo.

## Resultado Esperado

Ao final da Fase 11, o projeto deve ter:

- suites mais amplas e mais intencionais
- lacunas conhecidas explicitamente reduzidas
- cobertura consolidada como evidencia repetivel
- CI reforcada para sustentar a confianca tecnica da entrega

## Artefatos Esperados

- `specs/2026-04-23-phase-11-testes-e-cobertura/plan.md`
- `specs/2026-04-23-phase-11-testes-e-cobertura/requirements.md`
- `specs/2026-04-23-phase-11-testes-e-cobertura/validation.md`
- `docs/superpowers/plans/2026-04-23-phase-11-testes-e-cobertura.md`

