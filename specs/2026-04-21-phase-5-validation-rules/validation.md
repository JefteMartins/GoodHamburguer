# Validation

## Criterios de Sucesso

A `Fase 5 - Regras de Validacao` sera considerada bem-sucedida quando:

- criacao de pedido rejeitar codigos inexistentes
- atualizacao de pedido rejeitar codigos inexistentes
- criacao de pedido rejeitar codigos em categoria errada
- atualizacao de pedido rejeitar codigos em categoria errada
- erros forem retornados como `422 Unprocessable Entity`
- erros forem retornados em `ProblemDetails`
- erros vierem separados por campo
- existir pelo menos um teste positivo de criacao
- existir pelo menos um teste positivo de atualizacao

## Validacao Automatizada

Deve haver evidencias automatizadas em tres niveis:

### Dominio

- testes unitarios cobrindo a coerencia estrutural do pedido
- testes confirmando que a refatoracao para armazenamento por `codigo` nao quebrou a composicao do agregado

### Aplicacao

- testes cobrindo validacao de criacao
- testes cobrindo validacao de atualizacao
- testes para codigo inexistente
- testes para categoria errada no slot
- testes positivos para entradas validas

### Integracao HTTP

- teste de criacao invalida retornando `422`
- teste de atualizacao invalida retornando `422`
- validacao do corpo de `ProblemDetails`
- confirmacao de erros separados por campo

## Validacao Manual

No Swagger, deve ser possivel confirmar manualmente:

- contrato de criacao com `sandwichItemCode`, `sideItemCode` e `drinkItemCode`
- contrato de atualizacao com a mesma estrutura
- resposta `422` para payload invalido
- mensagens claras por campo

## Evidencias Esperadas Antes do Merge

Antes do merge, deve existir evidencia de:

- build da solution sem erros
- testes unitarios passando
- testes de integracao passando
- comportamento coerente com `specs/mission.md`
- alinhamento com `specs/tech-stack.md`
- alinhamento com `specs/roadmap.md`

## Nao Concluir Esta Fase Se

Nao considerar esta fase pronta se:

- a validacao estiver apenas na camada HTTP
- o pedido aceitar codigo inexistente no catalogo
- o pedido aceitar item valido em categoria errada
- a API retornar erro generico sem `ProblemDetails`
- a criacao funcionar, mas a atualizacao nao seguir a mesma regra
- nao houver pelo menos um fluxo valido comprovado para criacao e atualizacao
