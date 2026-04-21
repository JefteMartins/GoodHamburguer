# Requirements

## Escopo

Esta feature cobre a `Fase 3 - Modelagem de Pedido`, com foco em representar corretamente o pedido no dominio antes da introducao de persistencia real, regras completas de validacao e calculo de valores.

O objetivo desta fase e consolidar a estrutura do agregado de pedido para sustentar as proximas fases, mantendo coerencia com:

- `specs/mission.md`
- `specs/tech-stack.md`
- `specs/roadmap.md`

## O que Esta Incluido

- entidade de pedido no dominio
- slots explicitos para `sandwich`, `side` e `drink`
- representacao de item selecionado em pedido
- identidade basica do pedido
- datas basicas do agregado, preparadas para evolucao futura
- contratos de criacao de pedido
- contratos de atualizacao de pedido
- testes unitarios iniciais da modelagem do pedido

## O que Fica Fora Desta Fase

- persistencia real em MySQL
- migrations e seed
- validacao completa de pertencimento ao cardapio
- mensagens finais de erro com `ProblemDetails`
- calculo de subtotal, desconto e total
- endpoints completos de CRUD

## Decisoes Confirmadas

### Estrategia de Modelagem

O pedido deve nascer como um agregado com slots explicitos para as tres categorias do desafio:

- `sandwich`
- `side`
- `drink`

Essa escolha segue a constituicao tecnica do projeto e favorece clareza de leitura, aderencia ao desafio e facilidade de evolucao das validacoes.

### Item de Pedido

O item armazenado dentro do pedido nao deve ser o mesmo tipo usado para consulta de cardapio. A modelagem deve deixar claro que:

- `MenuItem` representa o catalogo disponivel
- item de pedido representa uma escolha feita dentro do agregado `Order`

Isso reduz acoplamento e evita que a modelagem do pedido dependa diretamente do formato de consulta do cardapio.

### Contratos de Aplicacao

Os contratos de criacao e atualizacao devem refletir a modelagem por slots, e nao listas genericas. Eles devem preparar a aplicacao para a futura exposicao via API sem introduzir ainda comportamento de CRUD completo.

### Identidade e Datas

O pedido deve incluir identificador e datas basicas desde agora, desde que isso sirva para a evolucao futura do agregado e da persistencia. A modelagem deve ficar pronta para uso posterior com repositório e banco.

### Regra Estrutural

Mesmo antes da fase de validacao completa, a estrutura do agregado ja deve refletir o limite de zero ou um item por categoria. A modelagem nao deve sugerir suporte a multiplos itens da mesma categoria.

## Contexto e Restricoes

Esta fase precisa equilibrar dois objetivos:

- manter a modelagem do pedido extremamente clara para avaliacao tecnica
- evitar antecipar regras completas de validacao e calculo antes das fases corretas

Por isso:

- a composicao do pedido deve ficar explicita no dominio
- o agregado deve nascer preparado para evolucao, mas sem inflar comportamento cedo demais
- validacoes estruturais podem existir se forem necessarias para preservar a coerencia do agregado
- regras promocionais e verificacoes completas de cardapio ainda ficam fora desta fase

## Padroes a Seguir

- manter a separacao entre `Api`, `Application`, `Domain` e `Infrastructure`
- manter a regra estrutural do pedido no dominio
- nao mover regra de composicao para a camada HTTP
- nao introduzir persistencia antes da `Fase 4`
- manter contratos e tipos claros para futura integracao com CRUD, validacao e calculo
