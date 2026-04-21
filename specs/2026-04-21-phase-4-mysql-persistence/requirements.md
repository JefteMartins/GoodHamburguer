# Requirements

## Escopo

Esta feature cobre a `Fase 4 - Persistencia MySQL`, com foco em introduzir persistencia real para o catalogo e para o agregado `Order`, logo apos a consolidacao do dominio e antes das fases de validacao completa e calculo.

O objetivo desta fase e fazer a aplicacao deixar de depender de fontes em memoria para esses dados centrais, mantendo coerencia com:

- `specs/mission.md`
- `specs/tech-stack.md`
- `specs/roadmap.md`

## O que Esta Incluido

- interfaces de repositorio para os agregados principais
- `DbContext` na camada `Infrastructure`
- persistencia real em MySQL 8.4 para catalogo e pedidos
- mapeamentos de EF Core para as entidades persistidas
- migration inicial
- seed inicial com catalogo e alguns pedidos de exemplo
- servico de migration no `docker-compose`
- testes de integracao basicos da persistencia

## O que Fica Fora Desta Fase

- validacoes completas de pedido com mensagens finais
- calculo de subtotal, desconto e total
- CRUD HTTP completo de pedidos
- health checks, observabilidade e logging estruturado
- frontend integrado com persistencia de pedidos

## Decisoes Confirmadas

### Escopo da Persistencia

Esta fase deve persistir os dois conjuntos de dados centrais:

- `Order`
- catalogo / menu

Nao devemos persistir apenas pedidos e manter o menu em memoria, porque isso criaria uma fase hibrida e deixaria a demonstracao tecnica inconsistente.

### Ordem da Fase

A ordem de implementacao desta fase deve ser:

1. modelagem de persistencia e interfaces
2. `DbContext` e mapeamentos
3. repositorios
4. migration inicial
5. seed inicial
6. servico de migration no Docker
7. testes de integracao basicos

### Fronteira Arquitetural

O `DbContext` deve continuar restrito a `Infrastructure`. As camadas `Application` e `Api` nao devem conhecer `DbContext`, `DbSet` ou tipos especificos de EF Core.

### Persistencia do Catalogo

O fluxo de consulta do menu deve passar a ler a partir da persistencia real, sem perder a regra ja consolidada de usar o dominio como fonte de verdade para agrupamento e representacao.

### Persistencia de Pedidos

O agregado `Order` deve ser materializado no banco sem perder a clareza da modelagem por slots explicitos (`sandwich`, `side`, `drink`). A estrutura de persistencia pode usar entidades proprias, desde que a modelagem do dominio continue clara e desacoplada.

### Migration Inicial

A migration inicial deve refletir o estado atual do projeto:

- catalogo persistido
- pedidos persistidos
- identificadores e datas basicas
- colunas monetarias com precisao adequada

Nao deve antecipar campos ou tabelas que ainda nao pertencem as proximas fases.

### Seed Inicial

O seed deve ser de demonstracao e precisa incluir:

- o catalogo fixo do desafio
- alguns pedidos coerentes com a modelagem atual

O seed precisa ser idempotente para evitar duplicacao em reexecucoes do fluxo local.

### Migration Service no Docker

O `docker-compose` deve passar a incluir um servico dedicado para aplicar migrations e rodar o seed usando a imagem da aplicacao. Esse servico deve ser tratado como parte oficial da experiencia local do projeto.

## Contexto e Restricoes

Esta fase precisa equilibrar dois objetivos:

- aproximar o projeto de uma entrega real e demonstravel
- preservar a clareza arquitetural, sem contaminar o dominio com EF Core

Por isso:

- entidades de persistencia podem existir separadas do dominio
- repositórios devem traduzir entre persistencia e modelo de dominio
- o seed precisa servir a demonstracao, mas sem virar uma camada de comportamento escondido
- a API deve continuar funcionando sobre a persistencia real sem mudar o contrato HTTP do menu

## Padroes a Seguir

- manter repositórios orientados a interface para os agregados principais
- manter o dominio sem referencias a EF Core
- manter a leitura do menu alinhada ao dominio, mesmo usando dados persistidos
- manter o projeto preparado para `Fase 5 - Regras de Validacao` e `Fase 7 - CRUD de Pedidos`
