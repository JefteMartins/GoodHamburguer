# Validation

## Como Saber se a Implementacao Foi Bem-Sucedida

A implementacao desta fase pode ser considerada bem-sucedida quando:

- o dominio do cardapio estiver modelado
- existir uma abstracao de consulta preparada para persistencia futura
- a API expuser o endpoint `GET /api/v1/menu`
- a resposta do endpoint vier agrupada por categoria
- houver testes do dominio e da aplicacao cobrindo os cenarios principais
- houver validacao de integracao do endpoint HTTP

## Verificacoes Automatizadas

Devem passar:

- build da solution
- testes unitarios relacionados ao dominio do cardapio
- testes da camada de aplicacao para consulta do cardapio
- testes de integracao do endpoint HTTP de menu

## Verificacoes Manuais

Deve ser feita uma checagem manual no Swagger para confirmar:

- exposicao do endpoint em `v1`
- caminho `/api/v1/menu`
- contrato agrupado por categoria
- legibilidade adequada para avaliacao tecnica e consumo futuro pelo Blazor

## O que Nao e Obrigatorio Nesta Fase

Ainda nao e obrigatorio:

- persistir o cardapio em MySQL
- implementar migration e seed
- modelar pedido
- aplicar validacoes de pedido
- calcular desconto

## Criterios de Merge

Esta fase pode ser mergeada quando atender aos seguintes criterios:

- a implementacao estiver aderente aos documentos em `specs/`
- o endpoint de consulta do cardapio estiver funcional
- os testes definidos para a fase estiverem aprovados
- a checagem manual no Swagger confirmar o contrato esperado
- a branch estiver pronta para evoluir para a `Fase 3 - Modelagem de Pedido`

## Sinais de Qualidade Esperados

Alguns sinais adicionais ajudam a confirmar que a fase ficou realmente pronta:

- o contrato da aplicacao nao depende de persistencia concreta
- o endpoint nao expõe estrutura acidental ou acoplada ao armazenamento atual
- a modelagem do dominio permite evolucao para persistencia real sem retrabalho excessivo
