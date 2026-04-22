# Requirements

## Escopo

Esta feature cobre a `Fase 7 - CRUD de Pedidos`, com foco em completar os endpoints principais de pedidos sobre a persistencia MySQL ja existente.

O sistema ja possui:

- criacao de pedido
- atualizacao de pedido
- validacao por `itemCode`
- calculo de `subtotal`, `discount` e `total`

Esta fase deve completar o CRUD com leitura e remocao, além de consolidar o contrato HTTP de pedidos em `v1`.

## O que Esta Incluido

- listagem de pedidos
- consulta de pedido por identificador
- remocao de pedido
- reuso do contrato `OrderResponse` para leituras
- retorno dos valores calculados (`subtotal`, `discount`, `total`) tambem nos endpoints de leitura
- uso da persistencia MySQL real ja existente para todas as operacoes
- testes automatizados cobrindo os endpoints do CRUD em `v1`

## O que Fica Fora Desta Fase

- paginacao
- filtros por data, faixa de preco ou combinacao
- ordenacao customizavel por query string
- soft delete
- auditoria detalhada de remocao
- alteracao da regra de desconto
- persistencia materializada de subtotal, desconto e total
- revisao ampla dos codigos HTTP fora do necessario para esta fase
- frontend Blazor consumindo o CRUD

## Decisoes Confirmadas

### Persistencia Real Como Fonte do CRUD

Todas as operacoes devem usar a persistencia MySQL ja existente por meio do repositorio de pedidos atual.

Esta fase nao deve introduzir armazenamento paralelo, cache de leitura ou colecoes em memoria para sustentar `GET` e `DELETE`.

### Contrato de Leitura

Os endpoints de leitura devem retornar o mesmo formato logico de pedido que ja existe para criacao e atualizacao:

- `id`
- `sandwichItemCode`
- `sideItemCode`
- `drinkItemCode`
- `subtotal`
- `discount`
- `total`
- `createdAtUtc`
- `updatedAtUtc`

Os valores monetarios continuam derivados a partir do `MenuCatalog` no momento da resposta.

### Sem Persistir Campos Derivados

Mesmo com a ampliacao do CRUD, esta fase continua sem persistir:

- `subtotal`
- `discount`
- `total`

Esses campos permanecem derivados em tempo de resposta, seguindo a decisao da Fase 6.

### Sem Redesenhar o Contrato Existente

Como o projeto ja expoe `POST` e `PUT` com `200 OK`, esta fase deve preservar o comportamento atual e completar o CRUD sem reabrir agora a discussao de refinamento REST.

Se houver revisao de `201 Created`, `204 No Content` ou localizacao via `Location`, isso deve ficar para fase futura de refinamento de contrato.

### Not Found

Quando um pedido nao existir:

- `GET /api/v1/orders/{id}` deve retornar `404`
- `PUT /api/v1/orders/{id}` continua retornando `404`
- `DELETE /api/v1/orders/{id}` deve retornar `404`

## Contexto e Restricoes

Esta fase deve priorizar completude funcional do CRUD sem expandir escopo desnecessariamente.

Por isso:

- a listagem pode retornar uma colecao simples
- nao ha necessidade de paginação nesta etapa
- o repositorio pode ser ampliado com operacoes de `List` e `Delete`
- a camada de aplicacao continua responsavel por buscar o `MenuCatalog` e projetar o `OrderResponse`
- a regra de precificacao continua no dominio do pedido

## Padroes a Seguir

- manter separacao entre `Api`, `Application`, `Domain` e `Infrastructure`
- manter `OrderResponse` como contrato principal de saida para pedidos
- manter o calculo monetario centralizado no dominio
- usar `decimal` de ponta a ponta para valores monetarios
- usar a versao `v1` da API
- preferir testes de integracao HTTP como principal evidencia do CRUD completo
