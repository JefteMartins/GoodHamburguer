# Plan

## 1. Estruturar o Dominio do Pedido

- criar a entidade `Order` como agregado central do dominio
- modelar o pedido com slots explicitos para `sandwich`, `side` e `drink`
- incluir identidade e datas basicas necessarias para a evolucao posterior do agregado
- manter a modelagem independente de persistencia e de HTTP

## 2. Modelar os Itens do Pedido

- definir uma representacao de item de pedido que referencie itens validos do cardapio
- separar com clareza o conceito de `MenuItem` do conceito de item escolhido em um pedido
- garantir que a modelagem do pedido reflita o limite de zero ou um item por categoria

## 3. Definir Contratos de Criacao e Atualizacao

- criar contratos de aplicacao para criacao de pedido
- criar contratos de aplicacao para atualizacao de pedido
- manter os contratos coerentes com a modelagem por slots explicitos
- preparar os contratos para futura integracao com validacoes e CRUD

## 4. Centralizar Regras Estruturais do Pedido

- encapsular no dominio as regras estruturais de composicao do pedido
- impedir que a modelagem aceite mais de um item por categoria
- deixar o agregado pronto para receber validacoes mais completas na fase seguinte

## 5. Cobrir com Testes

- adicionar testes unitarios do dominio para a modelagem do pedido
- cobrir cenarios de composicao valida do pedido
- cobrir cenarios estruturais invalidos relevantes
- validar os contratos de criacao e atualizacao na camada de aplicacao

## 6. Validar a Fase

- revisar a aderencia da modelagem aos documentos em `specs/`
- garantir que a entidade esteja pronta para persistencia, validacao e calculo nas proximas fases
- confirmar que a fase pode evoluir sem retrabalho para `Fase 4` e `Fase 5`
