# Validation

## Criterios de Sucesso

A `Fase 6 - Calculo de Valores` sera considerada bem-sucedida quando:

- um pedido valido retornar `subtotal`
- um pedido elegivel retornar `discount` maior que zero
- um pedido nao elegivel retornar `discount` igual a zero
- `total` for sempre igual a `subtotal - discount`
- os valores forem retornados com `decimal`
- criacao e atualizacao de pedido retornarem os mesmos calculos para a mesma combinacao de itens
- existirem testes automatizados cobrindo happy path, edge cases e ausencia de desconto

## Validacao Automatizada

Deve haver evidencias automatizadas em tres niveis:

### Dominio

- teste unitario para pedido com `sandwich + side + drink`
- teste unitario para pedido com apenas `sandwich`
- teste unitario para pedido com `sandwich + drink` sem desconto
- teste unitario para pedido com `sandwich + side` sem desconto
- teste unitario para pedido vazio ou parcialmente preenchido, quando aplicavel a regra atual
- confirmacao de que `total = subtotal - discount`

### Aplicacao

- teste cobrindo criacao de pedido com retorno de `subtotal`, `discount` e `total`
- teste cobrindo atualizacao de pedido com recalculo dos valores
- teste garantindo que a aplicacao usa o `MenuCatalog` como fonte dos precos
- teste garantindo que o contrato `OrderResponse` exponha os campos monetarios esperados

### Integracao HTTP

- teste de `POST /api/v1/orders` retornando os valores calculados
- teste de `PUT /api/v1/orders/{id}` retornando os valores recalculados
- confirmacao de que o contrato HTTP inclui os novos campos monetarios
- confirmacao de que a resposta continua funcional junto com as validacoes da fase anterior

## Validacao Manual

No Swagger, deve ser possivel confirmar manualmente:

- criacao de pedido com `sandwich`, `side` e `drink`
- visualizacao de `subtotal`, `discount` e `total` na resposta
- atualizacao de pedido alterando a combinacao e recalculando os valores
- ausencia de erro de serializacao ou inconsistencias de contrato

## Evidencias Esperadas Antes do Merge

Antes do merge, deve existir evidencia de:

- build da solution sem erros
- testes unitarios passando
- testes de integracao passando
- resposta de criacao contendo valores monetarios coerentes
- resposta de atualizacao contendo valores monetarios coerentes
- alinhamento com `specs/mission.md`
- alinhamento com `specs/tech-stack.md`
- alinhamento com `specs/roadmap.md`

## Nao Concluir Esta Fase Se

Nao considerar esta fase pronta se:

- o desconto estiver implementado na camada HTTP
- a aplicacao retornar apenas subtotal sem desconto e total
- o total nao bater com `subtotal - discount`
- houver uso de `double` ou `float` em regra monetaria
- os valores forem persistidos no banco sem necessidade
- o comportamento de criacao e atualizacao divergir para a mesma combinacao
- a hipotese promocional nao tiver sido revisada pelo humano antes da implementacao
