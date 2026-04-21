# Requirements

## Escopo

Esta feature cobre a `Fase 6 - Calculo de Valores`, com foco em calcular corretamente `subtotal`, `desconto` e `total final` para pedidos validos, sem antecipar ainda o CRUD completo de pedidos da `Fase 7`.

O objetivo desta fase e consolidar a regra de preco no dominio, mantendo coerencia com:

- `specs/mission.md`
- `specs/tech-stack.md`
- `specs/roadmap.md`

## O que Esta Incluido

- calculo de `subtotal`
- calculo de `desconto` por combinacao
- calculo de `total final`
- exposicao dos valores calculados nas respostas de criacao e atualizacao de pedido
- testes unitarios do dominio cobrindo happy path, edge cases e cenarios sem desconto
- testes da camada de aplicacao cobrindo o mapeamento dos valores calculados
- testes de integracao HTTP confirmando o contrato retornado pela API

## O que Fica Fora Desta Fase

- listagem completa de pedidos
- consulta de pedido por identificador
- remocao de pedido
- persistencia materializada de subtotal, desconto e total no banco
- historico de preco por item
- regras promocionais configuraveis por banco ou painel administrativo
- health checks, logging estruturado e observabilidade
- frontend funcional em Blazor

## Decisoes Confirmadas

### Regra Central no Dominio

O calculo de valores deve ficar no dominio do pedido, e nao em `Controller`, `DbContext`, repositório ou camada HTTP.

Isso segue a constituicao tecnica do projeto:

- `tech-stack.md` define o calculo como regra central do agregado
- `mission.md` exige regras explicitas e testaveis

### Fonte de Verdade de Preco

Os precos devem continuar vindo do catalogo atual (`MenuCatalog`), lido pela aplicacao a partir da infraestrutura existente.

Nesta fase, o pedido continua persistindo apenas:

- `sandwichItemCode`
- `sideItemCode`
- `drinkItemCode`

Os valores calculados devem ser derivados em tempo de resposta, sem duplicar subtotal, desconto e total na persistencia.

### Contrato de Resposta

As respostas de criacao e atualizacao de pedido devem passar a incluir:

- `subtotal`
- `discount`
- `total`

Esses campos devem acompanhar os codigos dos itens e os metadados ja retornados pelo contrato atual.

### Precisao Monetaria

Os calculos monetarios devem usar `decimal` de ponta a ponta.

Nao deve haver uso de:

- `double`
- `float`
- formatacao textual como parte da regra de negocio

### Hipotese de Trabalho Para a Regra Promocional

Os artefatos atuais do projeto mencionam desconto por combinacao, mas nao documentam a matriz promocional exata do desafio.

Para permitir a abertura desta spec sem bloquear o fluxo de SDD, esta fase assume como hipotese inicial de trabalho que:

- um pedido com `sandwich` + `side` + `drink` recebe `20%` de desconto sobre o subtotal
- pedidos sem a combinacao completa nao recebem desconto

Essa hipotese deve ser revisada pelo humano antes da implementacao. Se a regra oficial do desafio for diferente, `requirements.md`, `validation.md` e `plan.md` devem ser corrigidos antes de qualquer codigo ser escrito.

## Contexto e Restricoes

Esta fase precisa equilibrar dois objetivos:

- tornar o calculo de valores verificavel e facil de localizar
- evitar persistencia derivada e acoplamento desnecessario cedo demais

Por isso:

- o pedido continua modelado por `3 slots`
- o catalogo segue como fonte de preco e categoria
- a API nao deve recalcular preco por conta propria
- a infraestrutura nao deve virar dona da regra promocional
- a implementacao deve permitir reuso futuro na `Fase 7`, quando listagem e consulta de pedido forem adicionadas

## Padroes a Seguir

- manter a separacao entre `Api`, `Application`, `Domain` e `Infrastructure`
- concentrar a logica de calculo no agregado `Order` ou em tipo de dominio imediatamente adjacente a ele
- usar `decimal` para todos os valores monetarios
- manter as respostas HTTP simples, previsiveis e versionadas em `v1`
- nao persistir campos derivados nesta fase
- nao introduzir configuracao promocional dinamica antes de existir necessidade real
