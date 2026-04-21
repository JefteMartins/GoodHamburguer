# Requirements

## Escopo

Esta feature cobre a `Fase 5 - Regras de Validacao`, com foco em impedir pedidos invalidos logo no inicio do fluxo, sem antecipar ainda o calculo de subtotal, desconto, total e o CRUD completo de pedidos.

O objetivo desta fase e consolidar a validacao da criacao e da atualizacao de pedido de forma coerente com:

- `specs/mission.md`
- `specs/tech-stack.md`
- `specs/roadmap.md`

## O que Esta Incluido

- validacao para criacao de pedido
- validacao para atualizacao de pedido
- refatoracao da selecao de item do pedido para usar apenas `codigo`
- validacao de existencia do item no catalogo persistido
- validacao de categoria esperada por slot
- respostas `422` com `ProblemDetails`
- separacao de erros por campo
- testes de dominio, aplicacao e integracao HTTP

## O que Fica Fora Desta Fase

- calculo de subtotal, desconto e total
- CRUD completo de pedidos
- health checks, logging estruturado e observabilidade
- refinamentos de frontend
- novas categorias alem das previstas no desafio

## Decisoes Confirmadas

### Estrutura do Pedido

O pedido deve continuar refletindo diretamente a regra atual do desafio:

- zero ou um sanduiche
- zero ou uma batata
- zero ou um refrigerante

Isso significa que a modelagem continua com `3 slots`, pois essa estrutura comunica a regra de negocio com mais clareza para o contexto atual do projeto.

### Representacao Interna do Item Selecionado

Cada slot do pedido deve armazenar apenas o `codigo` do item selecionado.

O pedido nao deve persistir neste momento:

- nome do item
- preco do item
- categoria duplicada no valor armazenado

A identidade principal da selecao passa a ser o `codigo` do item de cardapio.

### Divisao de Responsabilidades

As validacoes devem seguir a divisao abaixo:

- `Application`
  - consultar o catalogo persistido
  - verificar se o codigo existe
  - verificar se o codigo pertence a categoria esperada do slot
- `Domain`
  - preservar a coerencia estrutural do pedido
  - proteger as invariantes do agregado
- `Api`
  - traduzir falhas para `ProblemDetails`

### Contratos de Criacao e Atualizacao

Os contratos de entrada para criacao e atualizacao devem usar explicitamente:

- `sandwichItemCode`
- `sideItemCode`
- `drinkItemCode`

Essa forma deixa o contrato previsivel para Swagger, API versionada e futuro consumo pelo Blazor.

### Categoria Esperada por Slot

Cada slot do pedido tem uma categoria esperada:

- `sandwichItemCode` aceita apenas itens de `sandwiches`
- `sideItemCode` aceita apenas itens de `sides`
- `drinkItemCode` aceita apenas itens de `drinks`

Um codigo valido no catalogo, mas pertencente a categoria errada, deve ser rejeitado.

### Tratamento de Erro

Falhas semanticamente invalidas desta fase devem retornar:

- `422 Unprocessable Entity`
- `ProblemDetails`

Os detalhes de erro devem ser separados por campo para facilitar leitura em Swagger e consumo pelo frontend:

- `sandwichItemCode`
- `sideItemCode`
- `drinkItemCode`

## Contexto e Restricoes

Esta fase deve equilibrar dois objetivos:

- deixar as regras de validacao explicitas e confiaveis
- manter a modelagem aderente ao desafio sem generalizacao desnecessaria

Por isso:

- o pedido continua com `3 slots`
- o catalogo permanece como fonte de verdade para existencia e categoria do item
- a aplicacao nao deve assumir que o codigo recebido e valido sem consultar o menu persistido
- o dominio nao deve depender da camada HTTP para proteger a integridade estrutural do agregado

## Padroes a Seguir

- manter a separacao entre `Api`, `Application`, `Domain` e `Infrastructure`
- usar `FluentValidation` para validacoes de entrada e aplicacao
- retornar `ProblemDetails` para erros de validacao
- manter o `DbContext` restrito a `Infrastructure`
- nao antecipar calculo de valores nesta fase
- nao transformar o pedido em colecao generica quando a regra atual do desafio ainda e fixa por categoria
