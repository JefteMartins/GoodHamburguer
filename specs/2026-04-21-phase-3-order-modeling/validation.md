# Validation

## Como Saber se a Implementacao Foi Bem-Sucedida

A implementacao desta fase pode ser considerada bem-sucedida quando:

- a entidade `Order` estiver modelada no dominio
- a composicao do pedido refletir slots explicitos por categoria
- existirem contratos de criacao e atualizacao coerentes com essa modelagem
- a estrutura do agregado estiver pronta para evoluir para persistencia, validacao e calculo
- houver testes unitarios cobrindo os cenarios principais da modelagem

## Verificacoes Automatizadas

Devem passar:

- build da solution
- testes unitarios relacionados a modelagem do pedido
- testes da camada de aplicacao para os contratos de criacao e atualizacao, quando aplicavel

## Verificacoes Manuais

Deve ser feita uma revisao manual para confirmar:

- legibilidade da entidade de pedido
- clareza da escolha por slots explicitos
- separacao adequada entre item de cardapio e item de pedido
- aderencia da modelagem ao desafio tecnico

## O que Nao e Obrigatorio Nesta Fase

Ainda nao e obrigatorio:

- persistir pedidos em MySQL
- expor endpoints completos de pedido
- validar pertencimento ao cardapio com mensagens finais de erro
- calcular subtotal, desconto e total
- integrar o fluxo completo com Docker e banco

## Criterios de Merge

Esta fase pode ser mergeada quando atender aos seguintes criterios:

- a implementacao estiver aderente aos documentos em `specs/`
- o agregado de pedido estiver claro e coeso
- os testes definidos para a fase estiverem aprovados
- a modelagem estiver pronta para evoluir para `Fase 4 - Persistencia MySQL` e `Fase 5 - Regras de Validacao`

## Sinais de Qualidade Esperados

Alguns sinais adicionais ajudam a confirmar que a fase ficou realmente pronta:

- o agregado nao depende de HTTP nem de persistencia concreta
- a estrutura do pedido nao usa listas genericas desnecessarias
- a modelagem deixa explicita a restricao de uma escolha por categoria
- os contratos de aplicacao ja apontam para a futura exposicao do CRUD sem antecipar escopo
