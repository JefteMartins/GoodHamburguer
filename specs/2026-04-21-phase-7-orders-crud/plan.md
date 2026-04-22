# Fase 7 - CRUD de Pedidos Implementation Plan

> **For agentic workers:** REQUIRED SUB-SKILL: Use superpowers:subagent-driven-development (recommended) or superpowers:executing-plans to implement this plan task-by-task. Steps use checkbox (`- [ ]`) syntax for tracking.

**Goal:** Completar o CRUD de pedidos sobre a persistencia MySQL atual, adicionando listagem, consulta por identificador e remocao, com respostas consistentes em `v1` e valores monetarios calculados no momento da leitura.

**Architecture:** A camada `Api` expõe os novos endpoints HTTP em `OrdersController`, a camada `Application` amplia o `IOrderAppService` para orquestrar leitura e remocao, o repositorio ganha operacoes de `List` e `Delete` sobre o MySQL, e o mapeamento continua calculando `subtotal`, `discount` e `total` a partir do `MenuCatalog` antes de retornar `OrderResponse`.

**Tech Stack:** .NET 10, C# 14, ASP.NET Core Web API, FluentValidation, xUnit, FluentAssertions, EF Core, MySQL

---

## File Map

- Modify: `src/GoodHamburguer.Application/Orders/Services/IOrderAppService.cs`
- Modify: `src/GoodHamburguer.Application/Orders/Abstractions/IOrderRepository.cs`
- Modify: `src/GoodHamburguer.Application/Orders/Services/OrderAppService.cs`
- Modify: `src/GoodHamburguer.Api/Controllers/OrdersController.cs`
- Modify: `src/GoodHamburguer.Infrastructure/Orders/OrderRepository.cs`
- Create or Modify: `tests/GoodHamburguer.UnitTests/OrderAppServiceTests.cs`
- Modify: `tests/GoodHamburguer.IntegrationTests/OrdersEndpointTests.cs`

## Task 1: Ampliar contratos e aplicacao para leitura e remocao

**Files:**
- Modify: `src/GoodHamburguer.Application/Orders/Services/IOrderAppService.cs`
- Modify: `src/GoodHamburguer.Application/Orders/Abstractions/IOrderRepository.cs`
- Modify: `src/GoodHamburguer.Application/Orders/Services/OrderAppService.cs`
- Test: `tests/GoodHamburguer.UnitTests/OrderAppServiceTests.cs`

- [ ] **Step 1: Write the failing tests**

Adicionar testes unitarios cobrindo:

- consulta por identificador retornando `OrderResponse`
- listagem retornando pedidos com valores monetarios calculados
- remocao de pedido existente
- tratamento de pedido inexistente em leitura individual e remocao

- [ ] **Step 2: Run the focused unit tests and verify they fail**

Run: `dotnet test tests/GoodHamburguer.UnitTests/GoodHamburguer.UnitTests.csproj --filter "FullyQualifiedName~OrderAppServiceTests" -v minimal`

- [ ] **Step 3: Add minimal application surface**

Ampliar `IOrderAppService` com algo equivalente a:

```csharp
Task<IReadOnlyList<OrderResponse>> ListAsync(CancellationToken cancellationToken = default);
Task<OrderResponse> GetByIdAsync(Guid orderId, CancellationToken cancellationToken = default);
Task DeleteAsync(Guid orderId, CancellationToken cancellationToken = default);
```

Ampliar `IOrderRepository` com algo equivalente a:

```csharp
Task<IReadOnlyList<Order>> ListAsync(CancellationToken cancellationToken = default);
Task DeleteAsync(Guid orderId, CancellationToken cancellationToken = default);
```

Atualizar `OrderAppService` para:

- buscar pedidos no repositorio
- usar o `MenuCatalog` para projetar `OrderResponse`
- lançar `KeyNotFoundException` quando apropriado

- [ ] **Step 4: Run the focused unit tests and verify they pass**

Run: `dotnet test tests/GoodHamburguer.UnitTests/GoodHamburguer.UnitTests.csproj --filter "FullyQualifiedName~OrderAppServiceTests" -v minimal`

## Task 2: Expor endpoints HTTP do CRUD completo

**Files:**
- Modify: `src/GoodHamburguer.Api/Controllers/OrdersController.cs`
- Test: `tests/GoodHamburguer.IntegrationTests/OrdersEndpointTests.cs`

- [ ] **Step 1: Write the failing integration tests**

Adicionar testes cobrindo:

- `GET /api/v1/orders`
- `GET /api/v1/orders/{id}`
- `DELETE /api/v1/orders/{id}`
- `404` para pedido inexistente em `GET by id`
- `404` para pedido inexistente em `DELETE`

- [ ] **Step 2: Run the focused integration tests and verify they fail**

Run: `dotnet test tests/GoodHamburguer.IntegrationTests/GoodHamburguer.IntegrationTests.csproj --filter "FullyQualifiedName~OrdersEndpointTests" -v minimal`

- [ ] **Step 3: Add minimal controller surface**

Adicionar ao `OrdersController` endpoints equivalentes a:

```csharp
[HttpGet]
public async Task<ActionResult<IReadOnlyList<OrderResponse>>> List(...)

[HttpGet("{id:guid}")]
public async Task<ActionResult<OrderResponse>> GetById(...)

[HttpDelete("{id:guid}")]
public async Task<IActionResult> Delete(...)
```

Nesta fase:

- `GET` individual retorna `404` quando nao encontrado
- `DELETE` retorna `200` ou `204`, desde que a escolha fique consistente com a implementacao e os testes
- `DELETE` inexistente retorna `404`

- [ ] **Step 4: Run the focused integration tests and verify they pass**

Run: `dotnet test tests/GoodHamburguer.IntegrationTests/GoodHamburguer.IntegrationTests.csproj --filter "FullyQualifiedName~OrdersEndpointTests" -v minimal`

## Task 3: Implementar persistencia real para listagem e remocao

**Files:**
- Modify: `src/GoodHamburguer.Infrastructure/Orders/OrderRepository.cs`

- [ ] **Step 1: Add minimal repository implementation**

Implementar:

- leitura de todos os pedidos com `AsNoTracking()`
- remocao por identificador usando o `DbContext`

Preferencias:

- manter o mapeamento atual `OrderEntity -> Order`
- ordenar a listagem por `CreatedAtUtc` ou `UpdatedAtUtc` apenas se isso simplificar a previsibilidade dos testes
- lançar `KeyNotFoundException` ou permitir que a camada de aplicacao trate inexistencia de forma explicita, mas sem esconder o caso

- [ ] **Step 2: Re-run integration tests**

Run: `dotnet test tests/GoodHamburguer.IntegrationTests/GoodHamburguer.IntegrationTests.csproj --filter "FullyQualifiedName~OrdersEndpointTests" -v minimal`

## Task 4: Fechar validacao da fase

**Files:**
- Modify if needed: `CHANGELOG.md`
- Modify if needed: `specs/roadmap.md`

- [ ] **Step 1: Run full verification**

Run:

- `dotnet build GoodHamburguer.slnx -v minimal`
- `dotnet test tests/GoodHamburguer.UnitTests/GoodHamburguer.UnitTests.csproj -v minimal`
- `dotnet test tests/GoodHamburguer.IntegrationTests/GoodHamburguer.IntegrationTests.csproj -v minimal`

- [ ] **Step 2: Manual validation**

Verificar no Swagger:

- criacao
- listagem
- consulta por id
- atualizacao
- remocao

- [ ] **Step 3: Close the loop**

Quando a fase estiver validada:

- atualizar `CHANGELOG.md`
- marcar a Fase 7 como concluida em `specs/roadmap.md`
- preparar commit dedicado da fase
