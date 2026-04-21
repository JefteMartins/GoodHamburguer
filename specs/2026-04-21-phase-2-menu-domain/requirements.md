# Requirements

## Escopo

Esta feature cobre a `Fase 2 - Dominio do Cardapio`, com foco em representar o cardapio do desafio de forma clara no dominio, disponibilizar sua consulta por meio da aplicacao e expor um endpoint HTTP versionado para consumo da API.

O objetivo desta fase e consolidar o conceito de cardapio antes da modelagem de pedido, mantendo coerencia com:

- `specs/mission.md`
- `specs/tech-stack.md`
- `specs/roadmap.md`

## O que Esta Incluido

- modelagem de uma estrutura generica de cardapio preparada para futura leitura do banco
- uso de `value object` para categorias
- representacao de itens com preco em `decimal`
- abstracao de consulta do cardapio voltada a evolucao futura para persistencia
- implementacao inicial com os itens e precos fixos do desafio
- endpoint `GET /api/v1/menu`
- resposta HTTP agrupada por categoria
- testes unitarios do dominio
- testes da camada de aplicacao
- teste de integracao do endpoint HTTP

## O que Fica Fora Desta Fase

- persistencia real do cardapio em MySQL
- migracoes e seed de banco
- modelagem completa de pedido
- regras de validacao de pedido
- calculo de descontos

## Decisoes Confirmadas

### Estrategia de Modelagem

O cardapio nao deve nascer como um catalogo rigidamente hardcoded no dominio. A estrutura desta fase deve ser generica o bastante para permitir futura leitura do banco sem exigir quebra de contrato relevante nas camadas superiores.

### Categorias

As categorias do cardapio devem ser representadas como `value object`, e nao como `enum`, para preservar flexibilidade de evolucao e manter a modelagem aderente a uma abordagem mais rica de dominio.

### Consulta do Cardapio

A consulta deve acontecer por meio de uma abstracao preparada para persistencia futura, como `IMenuQueryService`, em vez de acesso direto a um catalogo fixo dentro do endpoint ou da camada HTTP.

### Endpoint HTTP

O recurso de consulta deve ser exposto em:

- `/api/v1/menu`

A resposta deve ser agrupada por categoria para favorecer leitura no Swagger e consumo futuro pelo frontend em Blazor.

### Representacao de Preco

Os precos devem ser tratados como `decimal` no dominio. Qualquer preocupacao com formatacao pertence apenas a camada de apresentacao.

### Testes

Esta fase deve cobrir:

- regras e estrutura do dominio do cardapio
- comportamento da camada de aplicacao na consulta do cardapio
- integracao HTTP do endpoint de consulta

## Contexto e Restricoes

Esta fase precisa equilibrar dois objetivos:

- deixar o cardapio claro e facil de entender como parte central do dominio
- evitar acoplamento prematuro a MySQL antes da fase de persistencia real

Por isso:

- a consulta deve usar uma abstracao estavel desde agora
- a implementacao inicial pode usar uma fonte em memoria ou equivalente, desde que siga o contrato preparado para evolucao
- o endpoint deve nascer versionado e alinhado ao padrao ja estabelecido na API

## Padroes a Seguir

- manter a separacao entre `Api`, `Application`, `Domain` e `Infrastructure`
- nao colocar regra de dominio na camada HTTP
- nao introduzir persistencia real antes da `Fase 4`
- manter os contratos claros para futura integracao com o frontend e com o banco
