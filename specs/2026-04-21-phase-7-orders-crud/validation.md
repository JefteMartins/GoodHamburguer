# Validation

## Criterios de Sucesso

A `Fase 7 - CRUD de Pedidos` sera considerada bem-sucedida quando:

- for possivel criar, listar, consultar, atualizar e remover pedidos pela API `v1`
- a listagem retornar pedidos persistidos no MySQL real
- a consulta por identificador retornar os campos esperados do contrato
- a remocao tornar o pedido indisponivel para leituras posteriores
- os endpoints de leitura retornarem `subtotal`, `discount` e `total`
- pedidos inexistentes retornarem `404` nos endpoints de leitura individual, atualizacao e remocao
- existirem testes automatizados cobrindo o CRUD principal

## Validacao Automatizada

Deve haver evidencias automatizadas em tres niveis.

### Aplicacao

- teste para consulta por identificador retornando `OrderResponse`
- teste para listagem retornando colecao de `OrderResponse`
- teste para remocao de pedido existente
- teste para remocao de pedido inexistente resultando em falha controlada
- teste garantindo que as leituras reutilizam o `MenuCatalog` para calcular os valores monetarios

### Integracao HTTP

- teste de `POST /api/v1/orders` seguido de `GET /api/v1/orders/{id}`
- teste de `POST /api/v1/orders` seguido de `GET /api/v1/orders`
- teste de `DELETE /api/v1/orders/{id}` com confirmacao de `404` em leitura posterior
- teste de `GET /api/v1/orders/{id}` para pedido inexistente retornando `404`
- teste de `DELETE /api/v1/orders/{id}` para pedido inexistente retornando `404`
- confirmacao de que o contrato HTTP de leitura inclui os campos monetarios e os timestamps `createdAtUtc` e `updatedAtUtc`, com comparacao contra os valores persistidos
- confirmacao de que a listagem reflete o conjunto persistido esperado apos criacoes realizadas no proprio teste

## Validacao Manual

No Swagger, deve ser possivel confirmar manualmente:

- criacao de um pedido
- visualizacao do pedido criado por identificador
- visualizacao do pedido em listagem
- exclusao do pedido
- tentativa de consulta do pedido removido recebendo `404`

## Evidencias Esperadas Antes do Merge

Antes do merge, deve existir evidencia de:

- build da solution sem erros
- testes unitarios passando
- testes de integracao passando
- CRUD basico funcionando sobre persistencia real
- alinhamento com `specs/mission.md`
- alinhamento com `specs/tech-stack.md`
- alinhamento com `specs/roadmap.md`

## Nao Concluir Esta Fase Se

Nao considerar esta fase pronta se:

- a listagem depender de armazenamento em memoria em vez do MySQL
- a leitura nao retornar os valores monetarios calculados
- a remocao nao afetar a persistencia real
- pedidos inexistentes nao retornarem `404`
- o CRUD funcionar parcialmente apenas em testes unitarios, sem evidencias HTTP
- a implementacao duplicar a regra de precificacao fora do dominio
