# Requirements

## Escopo

Esta feature cobre a `Fase 10 - Frontend Blazor`, com foco em entregar a demonstracao visual integrada ao backend ja consolidado.

O frontend deve consumir a API versionada `v1` para os fluxos principais de:

- consulta de cardapio
- operacao de pedidos (listar, criar, atualizar e remover)

A implementacao deve usar como referencia visual as telas do projeto Stitch `9060755920845036151`.

## Telas de Referencia (Stitch)

Projeto: `https://stitch.withgoogle.com/projects/9060755920845036151`

Mapeamento de telas para rotas de frontend:

- `Product Menu` (`screenId: 041c078595b6422d8e8c526e61c38001`) -> `/menu`
- `New Order` (`screenId: 6732efbf243e4ca7bebde8c079fc4e35`) -> `/orders/new`
- `Order Details` (`screenId: 9b4183d37aee41298e42272047ce50a4`) -> `/orders/{id}`
- `Orders Dashboard` (`screenId: 9f654c9429324241b72f0fb9082f5a52`) -> `/dashboard/orders`

## O que Esta Incluido

- pagina de cardapio com dados reais de `GET /api/v1/menu`
- pagina de novo pedido com criacao via `POST /api/v1/orders`
- pagina de detalhes de pedido com consulta via `GET /api/v1/orders/{id}`
- pagina de dashboard de pedidos com listagem via `GET /api/v1/orders`
- operacoes de atualizar e remover pedido via `/api/v1/orders/{id}`
- integracao HTTP por servicos dedicados na camada Blazor
- navegacao para os quatro fluxos principais (`menu`, `new order`, `order details`, `orders dashboard`)
- extracao dos principais elementos visuais do Stitch para componentes reutilizaveis locais
- exibicao de estados de carregamento, vazio e erro
- tratamento de erro HTTP com base em `ProblemDetails`
- validacao manual da integracao Blazor + API em ambiente Docker

## O que Fica Fora Desta Fase

- redesign visual amplo da aplicacao
- autenticacao/autorizacao
- pagina de administracao
- filtros avancados, ordenacao e paginacao de pedidos
- internacionalizacao
- cache offline
- telemetria de frontend detalhada
- testes E2E browser completos (se houver custo alto para a fase)
- sincronizacao em tempo real com Stitch apos desenvolvimento
- qualquer dependencia de runtime do frontend em MCP Stitch

## Decisoes Confirmadas

### Integracao via API v1

As chamadas do frontend devem usar explicitamente os endpoints versionados:

- `/api/v1/menu`
- `/api/v1/orders`

A fase nao deve introduzir camada paralela de backend-for-frontend.

### Separacao de Responsabilidades

As paginas Razor nao devem concentrar regras de acesso HTTP.

A comunicacao com API deve ficar em clients/servicos dedicados para:

- facilitar teste
- evitar duplicacao de chamadas
- manter UI focada em estado e renderizacao

### Uso do Stitch

O Stitch e fonte de referencia visual e estrutural, e nao backend da aplicacao.

As evidencias de design (screenshots, html de referencia e mapeamento de rotas) devem ficar versionadas dentro de `specs/2026-04-22-phase-10-frontend-blazor/`.

### Contratos e Erros

O frontend deve respeitar os contratos atuais da API, incluindo:

- respostas de sucesso para leitura e escrita
- erros `ProblemDetails` em cenarios invalidos

O objetivo e fornecer mensagens claras ao usuario sem acoplar a UI aos detalhes internos do backend.

### Evolucao Segura

A implementacao deve ser incremental e reviewable, priorizando fluxos funcionais antes de refinamentos visuais.

## Contexto e Restricoes

O projeto ja possui:

- backend funcional em `v1`
- CRUD de pedidos pronto
- persistencia MySQL real
- ambiente Docker integrado
- Blazor base ja configurado com `Api:BaseUrl`

A fase 10 deve transformar o frontend base (atualmente placeholder) em uma demonstracao funcional conectada ao backend real.

## Padroes a Seguir

- manter simplicidade da interface sem sacrificar clareza
- priorizar componentes e paginas pequenas e objetivas
- manter alinhamento com `specs/mission.md`, `specs/tech-stack.md` e `specs/roadmap.md`
- evitar adicionar complexidade de front-end que nao contribua para demonstracao tecnica da solucao
- usar aderencia visual ao Stitch como guia, sem copiar markup literal quando isso comprometer manutenibilidade em Blazor