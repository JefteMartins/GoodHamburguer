# Fase 6 - Calculo de Valores Implementation Plan

> **For agentic workers:** REQUIRED SUB-SKILL: Use superpowers:subagent-driven-development (recommended) or superpowers:executing-plans to implement this plan task-by-task. Steps use checkbox (`- [ ]`) syntax for tracking.

**Goal:** Calcular `subtotal`, `discount` e `total` de pedidos validos a partir do `MenuCatalog`, retornando esses valores nas respostas de criacao e atualizacao sem persistir campos derivados.

**Architecture:** A regra monetaria entra no dominio de `Order`, recebendo os itens do catalogo para produzir um resultado de preco deterministico. A camada de aplicacao apenas obtem o `MenuCatalog`, delega o calculo ao dominio e projeta os novos campos para `OrderResponse`, enquanto a API e os repositórios permanecem sem regra promocional.

**Tech Stack:** .NET 10, C# 14, ASP.NET Core Web API, FluentValidation, xUnit, FluentAssertions, EF Core, MySQL

---

## File Map

- Create: `src/GoodHamburguer.Domain/Orders/OrderPricing.cs`
- Modify: `src/GoodHamburguer.Domain/Orders/Order.cs`
- Modify: `src/GoodHamburguer.Application/Orders/Services/OrderAppService.cs`
- Modify: `src/GoodHamburguer.Application/Orders/Contracts/OrderResponse.cs`
- Create: `tests/GoodHamburguer.UnitTests/OrderAppServiceTests.cs`
- Modify: `tests/GoodHamburguer.UnitTests/OrderTests.cs`
- Modify: `tests/GoodHamburguer.IntegrationTests/OrdersEndpointTests.cs`

## Assumption Gate

Antes de qualquer implementacao, confirmar na revisao humana da spec se a hipotese promocional continua valida:

- combo completo `sandwich + side + drink`
- desconto de `20%` sobre o subtotal

Se essa regra estiver errada, corrigir esta spec inteira antes de tocar no codigo.

### Task 1: Codificar o calculo no dominio

**Files:**
- Create: `src/GoodHamburguer.Domain/Orders/OrderPricing.cs`
- Modify: `src/GoodHamburguer.Domain/Orders/Order.cs`
- Test: `tests/GoodHamburguer.UnitTests/OrderTests.cs`

- [ ] **Step 1: Write the failing tests**

```csharp
[Fact]
public void CalculatePricing_ShouldReturnTwentyPercentDiscount_WhenOrderHasFullCombo()
{
    var order = Order.Create(
        Guid.NewGuid(),
        sandwich: new OrderItemSelection("sandwich-x-burger"),
        side: new OrderItemSelection("side-fries"),
        drink: new OrderItemSelection("drink-soft-drink"),
        createdAtUtc: new DateTimeOffset(2026, 4, 21, 12, 0, 0, TimeSpan.Zero));

    var pricing = order.CalculatePricing(new Dictionary<string, MenuItem>(StringComparer.OrdinalIgnoreCase)
    {
        ["sandwich-x-burger"] = new("sandwich-x-burger", "X Burger", MenuCategory.Sandwiches, 5.00m),
        ["side-fries"] = new("side-fries", "Batata frita", MenuCategory.Sides, 2.00m),
        ["drink-soft-drink"] = new("drink-soft-drink", "Refrigerante", MenuCategory.Drinks, 2.50m)
    });

    pricing.Subtotal.Should().Be(9.50m);
    pricing.Discount.Should().Be(1.90m);
    pricing.Total.Should().Be(7.60m);
}

[Fact]
public void CalculatePricing_ShouldReturnZeroDiscount_WhenOrderDoesNotHaveFullCombo()
{
    var order = Order.Create(
        Guid.NewGuid(),
        sandwich: new OrderItemSelection("sandwich-x-egg"),
        side: null,
        drink: new OrderItemSelection("drink-soft-drink"),
        createdAtUtc: new DateTimeOffset(2026, 4, 21, 12, 0, 0, TimeSpan.Zero));

    var pricing = order.CalculatePricing(new Dictionary<string, MenuItem>(StringComparer.OrdinalIgnoreCase)
    {
        ["sandwich-x-egg"] = new("sandwich-x-egg", "X Egg", MenuCategory.Sandwiches, 4.50m),
        ["drink-soft-drink"] = new("drink-soft-drink", "Refrigerante", MenuCategory.Drinks, 2.50m)
    });

    pricing.Subtotal.Should().Be(7.00m);
    pricing.Discount.Should().Be(0m);
    pricing.Total.Should().Be(7.00m);
}
```

- [ ] **Step 2: Run test to verify it fails**

Run: `dotnet test tests/GoodHamburguer.UnitTests/GoodHamburguer.UnitTests.csproj --filter "FullyQualifiedName~OrderTests" -v minimal`
Expected: FAIL because `Order.CalculatePricing` and `OrderPricing` do not exist yet.

- [ ] **Step 3: Write minimal implementation**

```csharp
namespace GoodHamburguer.Domain.Orders;

public sealed record OrderPricing(decimal Subtotal, decimal Discount, decimal Total);
```

```csharp
using GoodHamburguer.Domain.Menu;

namespace GoodHamburguer.Domain.Orders;

public sealed class Order
{
    public OrderPricing CalculatePricing(IReadOnlyDictionary<string, MenuItem> menuItemsByCode)
    {
        ArgumentNullException.ThrowIfNull(menuItemsByCode);

        var subtotal = SumSelection(menuItemsByCode, Sandwich)
            + SumSelection(menuItemsByCode, Side)
            + SumSelection(menuItemsByCode, Drink);

        var discount = Sandwich is not null && Side is not null && Drink is not null
            ? subtotal * 0.20m
            : 0m;

        return new OrderPricing(
            Subtotal: subtotal,
            Discount: discount,
            Total: subtotal - discount);
    }

    private static decimal SumSelection(
        IReadOnlyDictionary<string, MenuItem> menuItemsByCode,
        OrderItemSelection? selection)
    {
        if (selection is null)
        {
            return 0m;
        }

        return menuItemsByCode[selection.ItemCode].Price;
    }
}
```

- [ ] **Step 4: Run test to verify it passes**

Run: `dotnet test tests/GoodHamburguer.UnitTests/GoodHamburguer.UnitTests.csproj --filter "FullyQualifiedName~OrderTests" -v minimal`
Expected: PASS for the new pricing assertions and existing structural tests.

- [ ] **Step 5: Commit**

```bash
git add src/GoodHamburguer.Domain/Orders/Order.cs src/GoodHamburguer.Domain/Orders/OrderPricing.cs tests/GoodHamburguer.UnitTests/OrderTests.cs
git commit -m "feat: add order pricing calculation"
```

### Task 2: Expor os valores calculados no contrato da aplicacao

**Files:**
- Modify: `src/GoodHamburguer.Application/Orders/Services/OrderAppService.cs`
- Modify: `src/GoodHamburguer.Application/Orders/Contracts/OrderResponse.cs`
- Test: `tests/GoodHamburguer.UnitTests/OrderAppServiceTests.cs`

- [ ] **Step 1: Write the failing test**

```csharp
[Fact]
public async Task CreateAsync_ShouldReturnCalculatedPricingFromMenuCatalog()
{
    var menuCatalog = new MenuCatalog(
    [
        new MenuItem("sandwich-x-burger", "X Burger", MenuCategory.Sandwiches, 5.00m),
        new MenuItem("side-fries", "Batata frita", MenuCategory.Sides, 2.00m),
        new MenuItem("drink-soft-drink", "Refrigerante", MenuCategory.Drinks, 2.50m)
    ]);

    var menuQueryService = new FakeMenuQueryService(menuCatalog);
    var repository = new RecordingOrderRepository();
    var draftingService = new OrderDraftingService();
    var createValidator = new CreateOrderRequestValidator(menuQueryService);
    var updateValidator = new UpdateOrderRequestValidator(menuQueryService);
    var service = new OrderAppService(repository, draftingService, menuQueryService, createValidator, updateValidator);

    var response = await service.CreateAsync(new CreateOrderRequest
    {
        SandwichItemCode = "sandwich-x-burger",
        SideItemCode = "side-fries",
        DrinkItemCode = "drink-soft-drink"
    });

    response.Subtotal.Should().Be(9.50m);
    response.Discount.Should().Be(1.90m);
    response.Total.Should().Be(7.60m);
}

private sealed class FakeMenuQueryService(MenuCatalog catalog) : IMenuQueryService
{
    public Task<MenuCatalog> GetMenuCatalogAsync(CancellationToken cancellationToken = default)
        => Task.FromResult(catalog);
}

private sealed class RecordingOrderRepository : IOrderRepository
{
    public Order? LastAddedOrder { get; private set; }

    public Task AddAsync(Order order, CancellationToken cancellationToken = default)
    {
        LastAddedOrder = order;
        return Task.CompletedTask;
    }

    public Task UpdateAsync(Order order, CancellationToken cancellationToken = default)
        => Task.CompletedTask;

    public Task<Order?> GetByIdAsync(Guid orderId, CancellationToken cancellationToken = default)
        => Task.FromResult<Order?>(null);
}
```

- [ ] **Step 2: Run test to verify it fails**

Run: `dotnet test tests/GoodHamburguer.UnitTests/GoodHamburguer.UnitTests.csproj --filter "FullyQualifiedName~OrderAppServiceTests" -v minimal`
Expected: FAIL because `OrderResponse` does not expose monetary fields and `OrderAppService` does not obtain the `MenuCatalog` for pricing.

- [ ] **Step 3: Write minimal implementation**

```csharp
public sealed class OrderResponse
{
    public required Guid Id { get; init; }
    public string? SandwichItemCode { get; init; }
    public string? SideItemCode { get; init; }
    public string? DrinkItemCode { get; init; }
    public required decimal Subtotal { get; init; }
    public required decimal Discount { get; init; }
    public required decimal Total { get; init; }
    public required DateTimeOffset CreatedAtUtc { get; init; }
    public required DateTimeOffset UpdatedAtUtc { get; init; }
}
```

```csharp
public sealed class OrderAppService(
    IOrderRepository orderRepository,
    IOrderDraftingService orderDraftingService,
    IMenuQueryService menuQueryService,
    IValidator<CreateOrderRequest> createOrderValidator,
    IValidator<UpdateOrderRequest> updateOrderValidator) : IOrderAppService
{
    public async Task<OrderResponse> CreateAsync(CreateOrderRequest request, CancellationToken cancellationToken = default)
    {
        await createOrderValidator.ValidateAndThrowAsync(request, cancellationToken);

        var order = orderDraftingService.CreateOrder(Guid.NewGuid(), request, DateTimeOffset.UtcNow);
        await orderRepository.AddAsync(order, cancellationToken);

        var catalog = await menuQueryService.GetMenuCatalogAsync(cancellationToken);
        return Map(order, catalog);
    }
}
```

- [ ] **Step 4: Run test to verify it passes**

Run: `dotnet test tests/GoodHamburguer.UnitTests/GoodHamburguer.UnitTests.csproj --filter "FullyQualifiedName~OrderAppServiceTests" -v minimal`
Expected: PASS with correct values projected in the response.

- [ ] **Step 5: Commit**

```bash
git add src/GoodHamburguer.Application/Orders/Services/OrderAppService.cs src/GoodHamburguer.Application/Orders/Contracts/OrderResponse.cs tests/GoodHamburguer.UnitTests/OrderAppServiceTests.cs
git commit -m "feat: expose order pricing in application responses"
```

### Task 3: Cobrir recalculo na atualizacao e cenarios sem desconto

**Files:**
- Modify: `tests/GoodHamburguer.UnitTests/OrderAppServiceTests.cs`
- Modify: `tests/GoodHamburguer.UnitTests/OrderTests.cs`

- [ ] **Step 1: Write the failing tests**

```csharp
[Fact]
public async Task UpdateAsync_ShouldRecalculatePricingWhenOrderCompositionChanges()
{
    var existingOrder = Order.Create(
        Guid.NewGuid(),
        sandwich: new OrderItemSelection("sandwich-x-burger"),
        side: null,
        drink: null,
        createdAtUtc: new DateTimeOffset(2026, 4, 21, 12, 0, 0, TimeSpan.Zero));

    var repository = new RecordingOrderRepository(existingOrder);

    var response = await service.UpdateAsync(existingOrder.Id, new UpdateOrderRequest
    {
        SandwichItemCode = "sandwich-x-burger",
        SideItemCode = "side-fries",
        DrinkItemCode = "drink-soft-drink"
    });

    response.Subtotal.Should().Be(9.50m);
    response.Discount.Should().Be(1.90m);
    response.Total.Should().Be(7.60m);
}

private sealed class RecordingOrderRepository : IOrderRepository
{
    private readonly Order? _storedOrder;

    public RecordingOrderRepository(Order? storedOrder = null)
    {
        _storedOrder = storedOrder;
    }

    public Task AddAsync(Order order, CancellationToken cancellationToken = default)
        => Task.CompletedTask;

    public Task UpdateAsync(Order order, CancellationToken cancellationToken = default)
        => Task.CompletedTask;

    public Task<Order?> GetByIdAsync(Guid orderId, CancellationToken cancellationToken = default)
        => Task.FromResult(_storedOrder);
}
```

- [ ] **Step 2: Run test to verify it fails**

Run: `dotnet test tests/GoodHamburguer.UnitTests/GoodHamburguer.UnitTests.csproj --filter "FullyQualifiedName~OrderAppServiceTests|FullyQualifiedName~OrderTests" -v minimal`
Expected: FAIL until `UpdateAsync` also maps pricing using the same domain rule.

- [ ] **Step 3: Write minimal implementation**

```csharp
public async Task<OrderResponse> UpdateAsync(Guid orderId, UpdateOrderRequest request, CancellationToken cancellationToken = default)
{
    await updateOrderValidator.ValidateAndThrowAsync(request, cancellationToken);

    var order = await orderRepository.GetByIdAsync(orderId, cancellationToken)
        ?? throw new KeyNotFoundException($"Order '{orderId}' was not found.");

    orderDraftingService.UpdateOrder(order, request, DateTimeOffset.UtcNow);
    await orderRepository.UpdateAsync(order, cancellationToken);

    var catalog = await menuQueryService.GetMenuCatalogAsync(cancellationToken);
    return Map(order, catalog);
}
```

- [ ] **Step 4: Run test to verify it passes**

Run: `dotnet test tests/GoodHamburguer.UnitTests/GoodHamburguer.UnitTests.csproj -v minimal`
Expected: PASS for all unit tests, including previous validation coverage.

- [ ] **Step 5: Commit**

```bash
git add tests/GoodHamburguer.UnitTests/OrderAppServiceTests.cs tests/GoodHamburguer.UnitTests/OrderTests.cs src/GoodHamburguer.Application/Orders/Services/OrderAppService.cs
git commit -m "test: cover pricing recalculation scenarios"
```

### Task 4: Garantir o contrato HTTP da API

**Files:**
- Modify: `tests/GoodHamburguer.IntegrationTests/OrdersEndpointTests.cs`

- [ ] **Step 1: Write the failing integration assertions**

```csharp
createResponse.StatusCode.Should().Be(HttpStatusCode.OK);

var createdOrder = await createResponse.Content.ReadFromJsonAsync<OrderContract>();
createdOrder.Should().NotBeNull();
createdOrder!.Subtotal.Should().Be(9.50m);
createdOrder.Discount.Should().Be(1.90m);
createdOrder.Total.Should().Be(7.60m);
```

```csharp
public sealed class OrderContract
{
    public required Guid Id { get; init; }
    public string? SandwichItemCode { get; init; }
    public string? SideItemCode { get; init; }
    public string? DrinkItemCode { get; init; }
    public decimal Subtotal { get; init; }
    public decimal Discount { get; init; }
    public decimal Total { get; init; }
}
```

- [ ] **Step 2: Run test to verify it fails**

Run: `dotnet test tests/GoodHamburguer.IntegrationTests/GoodHamburguer.IntegrationTests.csproj --filter "FullyQualifiedName~OrdersEndpointTests" -v minimal`
Expected: FAIL because the API response contract does not yet include the pricing fields.

- [ ] **Step 3: Write minimal implementation**

```csharp
private static OrderResponse Map(Domain.Orders.Order order, MenuCatalog catalog)
{
    var pricing = order.CalculatePricing(
        catalog.Items.ToDictionary(item => item.Code, StringComparer.OrdinalIgnoreCase));

    return new OrderResponse
    {
        Id = order.Id,
        SandwichItemCode = order.Sandwich?.ItemCode,
        SideItemCode = order.Side?.ItemCode,
        DrinkItemCode = order.Drink?.ItemCode,
        Subtotal = pricing.Subtotal,
        Discount = pricing.Discount,
        Total = pricing.Total,
        CreatedAtUtc = order.CreatedAtUtc,
        UpdatedAtUtc = order.UpdatedAtUtc
    };
}
```

- [ ] **Step 4: Run test to verify it passes**

Run: `dotnet test GoodHamburguer.slnx -v minimal`
Expected: PASS for unit and integration suites, with endpoint responses exposing `subtotal`, `discount` and `total`.

- [ ] **Step 5: Commit**

```bash
git add tests/GoodHamburguer.IntegrationTests/OrdersEndpointTests.cs src/GoodHamburguer.Application/Orders/Services/OrderAppService.cs src/GoodHamburguer.Application/Orders/Contracts/OrderResponse.cs
git commit -m "feat: return calculated pricing from orders endpoints"
```

## Self-Review

**Spec coverage:** O plano cobre o calculo no dominio, a projecao dos valores pela aplicacao e a comprovacao via testes unitarios e HTTP. O unico ponto que exige revisao humana antes de executar e a hipotese da promocao de `20%` para combo completo.

**Placeholder scan:** Nao ficaram `TODO`, `TBD` ou referencias vagas a "adicionar validacao depois". Cada tarefa aponta arquivos, testes e comandos objetivos.

**Type consistency:** O plano usa `OrderPricing`, `Order.CalculatePricing`, `OrderResponse.Subtotal`, `OrderResponse.Discount` e `OrderResponse.Total` de forma consistente em dominio, aplicacao e testes.
