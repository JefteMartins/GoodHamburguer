# Plan

## Grupo 1 - Ajuste da Modelagem do Pedido

1. Revisar a modelagem atual do `Order` para manter os `3 slots` do desafio.
2. Refatorar a selecao de item do pedido para armazenar apenas o `codigo` do item.
3. Ajustar contratos de criacao e atualizacao para usar:
   - `sandwichItemCode`
   - `sideItemCode`
   - `drinkItemCode`
4. Atualizar testes unitarios da modelagem para refletir a nova representacao.

## Grupo 2 - Validacao na Aplicacao

1. Introduzir validacao de entrada para criacao e atualizacao com `FluentValidation`.
2. Consultar o catalogo persistido para validar que cada codigo informado existe.
3. Validar que cada codigo informado pertence a categoria esperada do slot.
4. Garantir que criacao e atualizacao compartilhem a mesma regra de consistencia.

## Grupo 3 - Validacao no Dominio

1. Preservar no dominio a coerencia estrutural do pedido.
2. Garantir que o agregado continue refletindo no maximo:
   - um sanduiche
   - uma batata
   - um refrigerante
3. Evitar que a camada HTTP ou a camada de persistencia se tornem fonte principal da regra do pedido.

## Grupo 4 - Traducao de Erros para HTTP

1. Padronizar falhas de validacao para resposta com `ProblemDetails`.
2. Retornar `422 Unprocessable Entity` para entradas semanticamente invalidas.
3. Separar os erros por campo:
   - `sandwichItemCode`
   - `sideItemCode`
   - `drinkItemCode`
4. Manter mensagens claras para consumo por Swagger e futuro frontend Blazor.

## Grupo 5 - Testes e Validacao

1. Adicionar testes unitarios do dominio cobrindo a coerencia estrutural.
2. Adicionar testes unitarios ou de aplicacao para os cenarios invalidos de criacao e atualizacao.
3. Adicionar testes de integracao HTTP cobrindo:
   - codigo inexistente
   - codigo em categoria errada
   - resposta `422` com `ProblemDetails`
4. Adicionar pelo menos:
   - um teste positivo de criacao
   - um teste positivo de atualizacao
5. Validar manualmente os contratos no Swagger.
