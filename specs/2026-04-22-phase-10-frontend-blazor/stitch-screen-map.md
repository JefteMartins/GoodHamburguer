# Stitch Screen Map - Fase 10 Frontend Blazor

## Projeto de Referencia

- URL: `https://stitch.withgoogle.com/projects/9060755920845036151`
- projectId: `9060755920845036151`

## Telas Disponiveis

1. `Orders Dashboard`
   - screenId: `9f654c9429324241b72f0fb9082f5a52`
   - objetivo funcional: visualizar pedidos e seu estado geral
   - rota Blazor alvo: `/dashboard/orders`

2. `New Order`
   - screenId: `6732efbf243e4ca7bebde8c079fc4e35`
   - objetivo funcional: criar pedido com selecao de itens
   - rota Blazor alvo: `/orders/new`

3. `Order Details`
   - screenId: `9b4183d37aee41298e42272047ce50a4`
   - objetivo funcional: detalhar e operar sobre pedido existente
   - rota Blazor alvo: `/orders/{id}`

4. `Product Menu`
   - screenId: `041c078595b6422d8e8c526e61c38001`
   - objetivo funcional: consultar cardapio e precos
   - rota Blazor alvo: `/menu`

## Fluxo de Captura via MCP Stitch

1. Listar telas:

- tool: `list_screens`
- params: `{ "projectId": "9060755920845036151" }`

2. Obter detalhes de uma tela:

- tool: `get_screen`
- params exemplo:
  - `projectId`: `9060755920845036151`
  - `screenId`: `9f654c9429324241b72f0fb9082f5a52`

3. Usar `screenshot.downloadUrl` para comparacao visual manual.

4. Usar `htmlCode.downloadUrl` somente como referencia estrutural (nao copiar markup integral para Blazor).

## Regras de Uso

- Stitch e insumo de design e planejamento.
- Frontend nao deve depender de Stitch em runtime.
- Ajustes de UX devem priorizar manutenibilidade em componentes Razor.