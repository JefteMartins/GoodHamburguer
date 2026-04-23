# Validation

## Criterios de Sucesso

A `Fase 10 - Frontend Blazor` sera considerada bem-sucedida quando:

- o frontend Blazor consumir a API `v1` em runtime real
- as quatro rotas principais estiverem implementadas:
  - `/menu`
  - `/orders/new`
  - `/orders/{id}`
  - `/dashboard/orders`
- for possivel consultar o cardapio pela interface
- for possivel listar, criar, atualizar e remover pedidos pela interface
- erros de operacao invalidos forem exibidos com feedback claro
- o fluxo funcionar tanto em execucao local quanto no compose Docker
- houver aderencia visual funcional as telas Stitch do projeto `9060755920845036151`

## Validacao Automatizada

Deve haver evidencias automatizadas em tres niveis.

### Build e Composicao

- `dotnet build GoodHamburguer.slnx -v minimal` sem erros
- projeto Blazor compilando sem warnings criticos novos

### Testes de Frontend

- testes para client de menu cobrindo sucesso e erro
- testes para client de pedidos cobrindo sucesso e erro
- testes de componente/pagina cobrindo estados de loading, vazio e erro
- teste para renderizacao basica de dados de menu
- teste para fluxo basico de operacao de pedidos
- teste para navegacao minima entre as quatro rotas principais

### Regressao Back-end

- testes unitarios existentes continuam passando
- testes de integracao existentes continuam passando

## Validacao Manual

Checks manuais esperados:

- subir stack com Docker (`blazor`, `api`, `mysql`, `migrations`)
- abrir Blazor em `http://localhost:8080`
- confirmar consulta de cardapio com dados reais em `/menu`
- criar um pedido valido em `/orders/new`
- abrir detalhes do pedido criado em `/orders/{id}`
- atualizar e remover pedido a partir de fluxo de detalhes/dashboard
- confirmar listagem de pedidos em `/dashboard/orders`
- provocar um cenario invalido e confirmar exibicao de erro amigavel

### Aderencia Stitch (manual)

Comparar cada tela implementada com a tela de referencia correspondente do Stitch:

- `Product Menu` <-> `/menu`
- `New Order` <-> `/orders/new`
- `Order Details` <-> `/orders/{id}`
- `Orders Dashboard` <-> `/dashboard/orders`

A comparacao deve validar:

- hierarquia visual da pagina
- distribuicao dos blocos principais
- estados de carregamento/erro/vazio
- clareza de acoes principais por fluxo

## Evidencias Esperadas Antes do Merge

Antes do merge, deve existir evidencia de:

- build da solution sem erros
- testes de frontend passando
- testes de backend (unitarios e integracao) passando
- fluxo funcional Blazor + API validado manualmente
- checklist de aderencia Stitch preenchido
- alinhamento com `specs/mission.md`
- alinhamento com `specs/tech-stack.md`
- alinhamento com `specs/roadmap.md`

## Nao Concluir Esta Fase Se

Nao considerar esta fase pronta se:

- o frontend ainda estiver em modo placeholder sem consumo real da API
- qualquer uma das quatro rotas principais nao estiver implementada
- operacoes de pedido funcionarem parcialmente sem feedback de erro
- a UI depender de dados mockados em vez do backend real
- a integracao funcionar apenas localmente fora do compose Docker
- nao houver evidencia de comparacao com as telas Stitch de referencia