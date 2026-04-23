# Phase 11 Testes e Cobertura Implementation Plan

> **For agentic workers:** REQUIRED SUB-SKILL: Use superpowers:subagent-driven-development (recommended) or superpowers:executing-plans to implement this plan task-by-task. Steps use checkbox (`- [ ]`) syntax for tracking.

**Goal:** ampliar as suites de teste do projeto e consolidar cobertura minima global de 70% com evidencia automatizada local e em CI.

**Architecture:** a implementacao sera conduzida por suite e por risco. Primeiro expandimos testes unitarios para regras e validacoes ainda pouco exercitadas; depois cobrimos contratos HTTP e persistencia real; em seguida reforcamos os fluxos do frontend Blazor; por fim instrumentamos a cobertura na pipeline para produzir um gate objetivo. A estrutura existente das suites sera preservada, e a implementacao deve seguir TDD sempre que houver lacuna identificada.

**Tech Stack:** .NET 10, xUnit, FluentAssertions, bUnit, ASP.NET Core integration testing, Testcontainers MySQL, coverlet.collector, GitHub Actions, eventual `dotnet-reportgenerator-globaltool` via `dotnet-tools.json`.

---

## File Map

- Modify: `tests/GoodHamburguer.UnitTests/MenuCatalogTests.cs`
- Modify: `tests/GoodHamburguer.UnitTests/OrderTests.cs`
- Modify: `tests/GoodHamburguer.UnitTests/OrderDraftingServiceTests.cs`
- Modify: `tests/GoodHamburguer.UnitTests/OrderRequestValidatorTests.cs`
- Modify: `tests/GoodHamburguer.UnitTests/OrderAppServiceTests.cs`
- Modify: `tests/GoodHamburguer.IntegrationTests/MenuEndpointTests.cs`
- Modify: `tests/GoodHamburguer.IntegrationTests/OrdersEndpointTests.cs`
- Modify: `tests/GoodHamburguer.IntegrationTests/ExceptionHandlingTests.cs`
- Modify: `tests/GoodHamburguer.IntegrationTests/SystemHealthEndpointTests.cs`
- Modify: `tests/GoodHamburguer.Blazor.Tests/Menu/MenuApiClientTests.cs`
- Modify: `tests/GoodHamburguer.Blazor.Tests/Menu/MenuPageTests.cs`
- Modify: `tests/GoodHamburguer.Blazor.Tests/Orders/OrderApiClientTests.cs`
- Modify: `tests/GoodHamburguer.Blazor.Tests/Orders/NewOrderPageTests.cs`
- Modify: `tests/GoodHamburguer.Blazor.Tests/Orders/OrderDetailsPageTests.cs`
- Modify: `tests/GoodHamburguer.Blazor.Tests/Orders/OrdersDashboardPageTests.cs`
- Modify: `.github/workflows/ci.yml`
- Modify: `dotnet-tools.json`

## Task 1: Expand Unit Coverage for Domain Pricing and Mapping

**Files:**
- Modify: `tests/GoodHamburguer.UnitTests/OrderTests.cs`
- Modify: `tests/GoodHamburguer.UnitTests/OrderDraftingServiceTests.cs`
- Modify: `tests/GoodHamburguer.UnitTests/MenuCatalogTests.cs`

- [ ] **Step 1: Write a failing test for unknown item codes in pricing**

Add to `tests/GoodHamburguer.UnitTests/OrderTests.cs`:

```csharp
[Fact]
public void CalculatePricing_ShouldThrow_WhenSelectedItemDoesNotExistInCatalog()
{
    var order = Order.Create(
        Guid.NewGuid(),
        sandwich: new OrderItemSelection("sandwich-unknown"),
        side: null,
        drink: null,
        createdAtUtc: new DateTimeOffset(2026, 4, 21, 12, 0, 0, TimeSpan.Zero));

    var act = () => order.CalculatePricing(CreateMenuItemsByCode());

    act.Should().Throw<KeyNotFoundException>();
}
```

- [ ] **Step 2: Run the unit test to verify the current behavior**

Run:
```powershell
$env:DOTNET_CLI_HOME='C:\Source\GoodHamburguer_v2\.dotnet'; $env:NUGET_PACKAGES='C:\Source\GoodHamburguer_v2\.nuget\packages'; dotnet test tests\GoodHamburguer.UnitTests\GoodHamburguer.UnitTests.csproj --filter "FullyQualifiedName~OrderTests.CalculatePricing_ShouldThrow_WhenSelectedItemDoesNotExistInCatalog" -v minimal
```

Expected:
- if the behavior is not yet protected, the test fails first
- if it already passes, keep it as explicit regression coverage

- [ ] **Step 3: Add a failing mapping test for clearing slots during drafting**

Add to `tests/GoodHamburguer.UnitTests/OrderDraftingServiceTests.cs`:

```csharp
[Fact]
public void UpdateOrder_ShouldClearAllSlots_WhenUpdateContractHasNoSelections()
{
    var service = new OrderDraftingService();
    var createdAt = new DateTimeOffset(2026, 4, 21, 12, 0, 0, TimeSpan.Zero);
    var order = service.CreateOrder(
        Guid.NewGuid(),
        new CreateOrderRequest
        {
            SandwichItemCode = "sandwich-x-burger",
            SideItemCode = "side-fries",
            DrinkItemCode = "drink-soft-drink"
        },
        createdAt);

    service.UpdateOrder(order, new UpdateOrderRequest(), createdAt.AddMinutes(5));

    order.Sandwich.Should().BeNull();
    order.Side.Should().BeNull();
    order.Drink.Should().BeNull();
}
```

- [ ] **Step 4: Add a failing catalog contract test for case-insensitive lookup or grouping behavior**

Add to `tests/GoodHamburguer.UnitTests/MenuCatalogTests.cs` a scenario matching the current API contract style, for example:

```csharp
[Fact]
public void FindItemByCode_ShouldBeCaseInsensitive()
{
    var catalog = new MenuCatalog(
    [
        new MenuItem("sandwich-x-burger", "X Burger", MenuCategory.Sandwiches, 5.00m)
    ]);

    var item = catalog.FindByCode("SANDWICH-X-BURGER");

    item.Should().NotBeNull();
    item!.Code.Should().Be("sandwich-x-burger");
}
```

- [ ] **Step 5: Run the focused unit subset**

Run:
```powershell
$env:DOTNET_CLI_HOME='C:\Source\GoodHamburguer_v2\.dotnet'; $env:NUGET_PACKAGES='C:\Source\GoodHamburguer_v2\.nuget\packages'; dotnet test tests\GoodHamburguer.UnitTests\GoodHamburguer.UnitTests.csproj --filter "FullyQualifiedName~OrderTests|FullyQualifiedName~OrderDraftingServiceTests|FullyQualifiedName~MenuCatalogTests" -v minimal
```

Expected:
- failing tests expose the exact gaps to close
- no unrelated suites are required yet

## Task 2: Expand Unit Coverage for Validation and Application Services

**Files:**
- Modify: `tests/GoodHamburguer.UnitTests/OrderRequestValidatorTests.cs`
- Modify: `tests/GoodHamburguer.UnitTests/OrderAppServiceTests.cs`

- [ ] **Step 1: Write a failing validator test for accepting empty updates**

Add to `tests/GoodHamburguer.UnitTests/OrderRequestValidatorTests.cs`:

```csharp
[Fact]
public async Task UpdateValidator_ShouldAcceptEmptyPayload_WhenClientClearsSelections()
{
    var validator = new UpdateOrderRequestValidator(new StubMenuQueryService(Catalog));

    var result = await validator.ValidateAsync(new UpdateOrderRequest());

    result.IsValid.Should().BeTrue();
}
```

- [ ] **Step 2: Write a failing validator test for multiple simultaneous invalid fields**

Add:

```csharp
[Fact]
public async Task CreateValidator_ShouldReturnErrorsForEachInvalidField()
{
    var validator = new CreateOrderRequestValidator(new StubMenuQueryService(Catalog));

    var result = await validator.ValidateAsync(new CreateOrderRequest
    {
        SandwichItemCode = "side-fries",
        DrinkItemCode = "sandwich-x-burger"
    });

    result.IsValid.Should().BeFalse();
    result.Errors.Should().Contain(error => error.PropertyName == "sandwichItemCode");
    result.Errors.Should().Contain(error => error.PropertyName == "drinkItemCode");
}
```

- [ ] **Step 3: Write a failing service test for delete after list consistency**

Add to `tests/GoodHamburguer.UnitTests/OrderAppServiceTests.cs`:

```csharp
[Fact]
public async Task DeleteAsync_ShouldRemoveOrderFromSubsequentList()
{
    var existingOrder = Order.Create(
        Guid.NewGuid(),
        sandwich: new OrderItemSelection("sandwich-x-burger"),
        side: null,
        drink: null,
        createdAtUtc: new DateTimeOffset(2026, 4, 21, 12, 0, 0, TimeSpan.Zero));
    var repository = new RecordingOrderRepository([existingOrder]);
    var service = CreateService(repository, CreateMenuCatalog());

    await service.DeleteAsync(existingOrder.Id);

    var orders = await service.ListAsync();

    orders.Should().NotContain(order => order.Id == existingOrder.Id);
}
```

- [ ] **Step 4: Write a failing service test for preserving timestamps on read-only operations**

Add:

```csharp
[Fact]
public async Task GetByIdAsync_ShouldNotMutatePersistedOrderTimestamps()
{
    var createdAt = new DateTimeOffset(2026, 4, 21, 12, 0, 0, TimeSpan.Zero);
    var existingOrder = Order.Create(
        Guid.NewGuid(),
        sandwich: new OrderItemSelection("sandwich-x-burger"),
        side: null,
        drink: null,
        createdAtUtc: createdAt);
    var service = CreateService(new RecordingOrderRepository([existingOrder]), CreateMenuCatalog());

    var response = await service.GetByIdAsync(existingOrder.Id);

    response.CreatedAtUtc.Should().Be(createdAt);
    response.UpdatedAtUtc.Should().Be(createdAt);
}
```

- [ ] **Step 5: Run the focused validation/application subset**

Run:
```powershell
$env:DOTNET_CLI_HOME='C:\Source\GoodHamburguer_v2\.dotnet'; $env:NUGET_PACKAGES='C:\Source\GoodHamburguer_v2\.nuget\packages'; dotnet test tests\GoodHamburguer.UnitTests\GoodHamburguer.UnitTests.csproj --filter "FullyQualifiedName~OrderRequestValidatorTests|FullyQualifiedName~OrderAppServiceTests" -v minimal
```

Expected:
- new tests drive any missing fixes in validation or orchestration behavior

## Task 3: Expand Integration Coverage for HTTP Contracts and Failure Paths

**Files:**
- Modify: `tests/GoodHamburguer.IntegrationTests/OrdersEndpointTests.cs`
- Modify: `tests/GoodHamburguer.IntegrationTests/MenuEndpointTests.cs`
- Modify: `tests/GoodHamburguer.IntegrationTests/ExceptionHandlingTests.cs`
- Modify: `tests/GoodHamburguer.IntegrationTests/SystemHealthEndpointTests.cs`

- [ ] **Step 1: Add a failing integration test for `ProblemDetails` payload shape on not found**

Add to `tests/GoodHamburguer.IntegrationTests/OrdersEndpointTests.cs`:

```csharp
[Fact]
public async Task GetById_ShouldReturnProblemDetailsPayload_WhenOrderDoesNotExist()
{
    var response = await _client.GetAsync($"/api/v1/orders/{Guid.NewGuid()}");

    response.StatusCode.Should().Be(HttpStatusCode.NotFound);

    var problem = await response.Content.ReadFromJsonAsync<ExceptionHandlingTests.ProblemDetailsContract>();

    problem.Should().NotBeNull();
    problem!.Status.Should().Be((int)HttpStatusCode.NotFound);
    problem.Title.Should().NotBeNullOrWhiteSpace();
}
```

- [ ] **Step 2: Add a failing integration test for update after delete**

Add:

```csharp
[Fact]
public async Task Update_ShouldReturnNotFound_WhenOrderWasDeleted()
{
    var createResponse = await _client.PostAsJsonAsync("/api/v1/orders", new
    {
        sandwichItemCode = "sandwich-x-burger"
    });
    var createdOrder = await createResponse.Content.ReadFromJsonAsync<OrderContract>();

    var deleteResponse = await _client.DeleteAsync($"/api/v1/orders/{createdOrder!.Id}");
    deleteResponse.StatusCode.Should().Be(HttpStatusCode.NoContent);

    var updateResponse = await _client.PutAsJsonAsync($"/api/v1/orders/{createdOrder.Id}", new
    {
        sideItemCode = "side-fries"
    });

    updateResponse.StatusCode.Should().Be(HttpStatusCode.NotFound);
}
```

- [ ] **Step 3: Add a failing integration test for menu contract stability**

Add to `tests/GoodHamburguer.IntegrationTests/MenuEndpointTests.cs` a structural assertion like:

```csharp
[Fact]
public async Task GetMenu_ShouldReturnCategoriesWithStableCodesAndItems()
{
    var response = await _client.GetAsync("/api/v1/menu");

    response.StatusCode.Should().Be(HttpStatusCode.OK);

    var payload = await response.Content.ReadAsStringAsync();

    payload.Should().Contain("sandwiches");
    payload.Should().Contain("sides");
    payload.Should().Contain("drinks");
    payload.Should().Contain("sandwich-x-burger");
}
```

- [ ] **Step 4: Add a failing integration test for health endpoint content**

Add to `tests/GoodHamburguer.IntegrationTests/SystemHealthEndpointTests.cs`:

```csharp
[Fact]
public async Task Readiness_ShouldReturnSuccessStatusCodeWhenDependenciesAreReady()
{
    var response = await _client.GetAsync("/health/ready");

    response.EnsureSuccessStatusCode();
}
```

- [ ] **Step 5: Run the integration suite**

Run:
```powershell
$env:DOTNET_CLI_HOME='C:\Source\GoodHamburguer_v2\.dotnet'; $env:NUGET_PACKAGES='C:\Source\GoodHamburguer_v2\.nuget\packages'; dotnet test tests\GoodHamburguer.IntegrationTests\GoodHamburguer.IntegrationTests.csproj -v minimal
```

Expected:
- all HTTP contract and failure-path assertions pass against the real test stack

## Task 4: Expand Blazor Coverage for Client and Page States

**Files:**
- Modify: `tests/GoodHamburguer.Blazor.Tests/Menu/MenuApiClientTests.cs`
- Modify: `tests/GoodHamburguer.Blazor.Tests/Menu/MenuPageTests.cs`
- Modify: `tests/GoodHamburguer.Blazor.Tests/Orders/OrderApiClientTests.cs`
- Modify: `tests/GoodHamburguer.Blazor.Tests/Orders/NewOrderPageTests.cs`
- Modify: `tests/GoodHamburguer.Blazor.Tests/Orders/OrderDetailsPageTests.cs`
- Modify: `tests/GoodHamburguer.Blazor.Tests/Orders/OrdersDashboardPageTests.cs`

- [ ] **Step 1: Add a failing API client test for empty `ProblemDetails` fallback messaging**

Add to `tests/GoodHamburguer.Blazor.Tests/Orders/OrderApiClientTests.cs`:

```csharp
[Fact]
public async Task ListOrdersAsync_UsesFallbackMessage_WhenProblemDetailsHasNoDetail()
{
    var handler = new StubHttpMessageHandler(_ =>
        Task.FromResult(new HttpResponseMessage(HttpStatusCode.ServiceUnavailable)
        {
            Content = JsonContent.Create(new Microsoft.AspNetCore.Mvc.ProblemDetails())
        }));

    var services = new ServiceCollection();
    services.AddHttpClient(Program.ApiHttpClientName, client => client.BaseAddress = new Uri("https://api.goodhamburguer.local/api/v1/"))
        .ConfigurePrimaryHttpMessageHandler(() => handler);
    services.AddScoped<IOrderApiClient, OrderApiClient>();

    await using var provider = services.BuildServiceProvider().CreateAsyncScope();
    var sut = provider.ServiceProvider.GetRequiredService<IOrderApiClient>();

    var act = () => sut.ListOrdersAsync();

    var exception = await act.Should().ThrowAsync<OrderApiProblemException>();
    exception.Which.Message.Should().NotBeNullOrWhiteSpace();
}
```

- [ ] **Step 2: Add a failing page test for loading state on dashboard**

Add to `tests/GoodHamburguer.Blazor.Tests/Orders/OrdersDashboardPageTests.cs`:

```csharp
[Fact]
public void OrdersDashboard_ShowsLoadingStateWhileRequestIsPending()
{
    var completion = new TaskCompletionSource<IReadOnlyList<OrderSummaryDto>>();
    Services.AddSingleton<IOrderApiClient>(new StubOrderApiClient
    {
        List = () => completion.Task
    });

    var cut = RenderComponent<GoodHamburguer.Blazor.Components.Pages.OrdersDashboard>();

    cut.Markup.Should().Contain("Loading");
}
```

- [ ] **Step 3: Add a failing page test for new order validation or submit error rendering**

Add to `tests/GoodHamburguer.Blazor.Tests/Orders/NewOrderPageTests.cs` a scenario in the current page idiom, for example:

```csharp
[Fact]
public void NewOrder_ShowsValidationErrorsReturnedByApi()
{
    Services.AddSingleton<IMenuApiClient>(new StubMenuApiClient(() => Task.FromResult<IReadOnlyList<MenuCategoryDto>>(
    [
        Category("sandwiches", "Sanduiches", ("sandwich-x-burger", "X-Burger"))
    ])));
    Services.AddSingleton<IOrderApiClient>(new StubOrderApiClient
    {
        Create = _ => Task.FromException<OrderSummaryDto>(
            new OrderApiValidationException(
                "Validation failed",
                new Dictionary<string, string[]>
                {
                    ["sandwichItemCode"] = ["Invalid sandwich."]
                }))
    });

    var cut = RenderComponent<GoodHamburguer.Blazor.Components.Pages.NewOrder>();

    cut.WaitForAssertion(() => cut.Find("form"));
    cut.Find("form").Submit();

    cut.WaitForAssertion(() => cut.Markup.Should().Contain("Invalid sandwich."));
}
```

- [ ] **Step 4: Add a failing details-page test for not found behavior**

Add to `tests/GoodHamburguer.Blazor.Tests/Orders/OrderDetailsPageTests.cs`:

```csharp
[Fact]
public void OrderDetails_ShowsNotFoundStateWhenOrderIsMissing()
{
    var orderId = Guid.Parse("eeeeeeee-eeee-eeee-eeee-eeeeeeeeeeee");
    Services.AddSingleton<IMenuApiClient>(new StubMenuApiClient(() => Task.FromResult<IReadOnlyList<MenuCategoryDto>>([])));
    Services.AddSingleton<IOrderApiClient>(new StubOrderApiClient
    {
        GetById = _ => Task.FromException<OrderSummaryDto>(new OrderApiNotFoundException(orderId))
    });

    var cut = RenderComponent<GoodHamburguer.Blazor.Components.Pages.OrderDetails>(
        parameters => parameters.Add(page => page.Id, orderId));

    cut.WaitForAssertion(() => cut.Markup.Should().Contain("not found"));
}
```

- [ ] **Step 5: Run the Blazor suite**

Run:
```powershell
$env:DOTNET_CLI_HOME='C:\Source\GoodHamburguer_v2\.dotnet'; $env:NUGET_PACKAGES='C:\Source\GoodHamburguer_v2\.nuget\packages'; dotnet test tests\GoodHamburguer.Blazor.Tests\GoodHamburguer.Blazor.Tests.csproj -v minimal
```

Expected:
- client error mapping and page-state regressions are explicitly covered

## Task 5: Add Coverage Collection and Gate to the Tooling and CI

**Files:**
- Modify: `dotnet-tools.json`
- Modify: `.github/workflows/ci.yml`

- [ ] **Step 1: Add ReportGenerator to the local tool manifest**

Update `dotnet-tools.json` to:

```json
{
  "version": 1,
  "isRoot": true,
  "tools": {
    "dotnet-ef": {
      "version": "9.0.0",
      "commands": [
        "dotnet-ef"
      ],
      "rollForward": false
    },
    "dotnet-reportgenerator-globaltool": {
      "version": "5.4.6",
      "commands": [
        "reportgenerator"
      ]
    }
  }
}
```

- [ ] **Step 2: Verify tools restore succeeds**

Run:
```powershell
$env:DOTNET_CLI_HOME='C:\Source\GoodHamburguer_v2\.dotnet'; $env:NUGET_PACKAGES='C:\Source\GoodHamburguer_v2\.nuget\packages'; dotnet tool restore
```

Expected:
- `reportgenerator` becomes available from the local tool manifest

- [ ] **Step 3: Update CI to collect coverage for all three suites**

Replace `.github/workflows/ci.yml` with a coverage-aware version like:

```yaml
name: CI

on:
  push:
  pull_request:

jobs:
  build-and-test:
    runs-on: ubuntu-latest

    steps:
      - name: Checkout
        uses: actions/checkout@v4

      - name: Setup .NET
        uses: actions/setup-dotnet@v4
        with:
          dotnet-version: 10.0.x

      - name: Restore
        run: dotnet restore GoodHamburguer.slnx

      - name: Restore local tools
        run: dotnet tool restore

      - name: Build
        run: dotnet build GoodHamburguer.slnx --no-restore -v minimal

      - name: Unit tests with coverage
        run: dotnet test tests/GoodHamburguer.UnitTests/GoodHamburguer.UnitTests.csproj --no-build --collect:"XPlat Code Coverage" --results-directory artifacts/test-results/unit -v minimal

      - name: Integration tests with coverage
        run: dotnet test tests/GoodHamburguer.IntegrationTests/GoodHamburguer.IntegrationTests.csproj --no-build --collect:"XPlat Code Coverage" --results-directory artifacts/test-results/integration -v minimal

      - name: Blazor tests with coverage
        run: dotnet test tests/GoodHamburguer.Blazor.Tests/GoodHamburguer.Blazor.Tests.csproj --no-build --collect:"XPlat Code Coverage" --results-directory artifacts/test-results/blazor -v minimal

      - name: Generate coverage report
        run: dotnet tool run reportgenerator -reports:"artifacts/test-results/**/coverage.cobertura.xml" -targetdir:"artifacts/coverage-report" -reporttypes:"TextSummary;HtmlInline_AzurePipelines"

      - name: Enforce coverage threshold
        run: |
          summary_file="artifacts/coverage-report/Summary.txt"
          cat "$summary_file"
          line_rate=$(grep "Line coverage" "$summary_file" | sed -E 's/.*: ([0-9]+\.[0-9]+)%.*/\1/')
          awk -v rate="$line_rate" 'BEGIN { exit !(rate >= 70.0) }'

      - name: Upload coverage artifacts
        uses: actions/upload-artifact@v4
        with:
          name: coverage-report
          path: artifacts/coverage-report
```

- [ ] **Step 4: Run the full local verification with coverage**

Run:
```powershell
$env:DOTNET_CLI_HOME='C:\Source\GoodHamburguer_v2\.dotnet'; $env:NUGET_PACKAGES='C:\Source\GoodHamburguer_v2\.nuget\packages'; dotnet build GoodHamburguer.slnx -v minimal
$env:DOTNET_CLI_HOME='C:\Source\GoodHamburguer_v2\.dotnet'; $env:NUGET_PACKAGES='C:\Source\GoodHamburguer_v2\.nuget\packages'; dotnet test tests\GoodHamburguer.UnitTests\GoodHamburguer.UnitTests.csproj --collect:\"XPlat Code Coverage\" --results-directory artifacts/test-results/unit -v minimal
$env:DOTNET_CLI_HOME='C:\Source\GoodHamburguer_v2\.dotnet'; $env:NUGET_PACKAGES='C:\Source\GoodHamburguer_v2\.nuget\packages'; dotnet test tests\GoodHamburguer.IntegrationTests\GoodHamburguer.IntegrationTests.csproj --collect:\"XPlat Code Coverage\" --results-directory artifacts/test-results/integration -v minimal
$env:DOTNET_CLI_HOME='C:\Source\GoodHamburguer_v2\.dotnet'; $env:NUGET_PACKAGES='C:\Source\GoodHamburguer_v2\.nuget\packages'; dotnet test tests\GoodHamburguer.Blazor.Tests\GoodHamburguer.Blazor.Tests.csproj --collect:\"XPlat Code Coverage\" --results-directory artifacts/test-results/blazor -v minimal
$env:DOTNET_CLI_HOME='C:\Source\GoodHamburguer_v2\.dotnet'; $env:NUGET_PACKAGES='C:\Source\GoodHamburguer_v2\.nuget\packages'; dotnet tool run reportgenerator -reports:\"artifacts/test-results/**/coverage.cobertura.xml\" -targetdir:\"artifacts/coverage-report\" -reporttypes:\"TextSummary\"
```

Expected:
- all three suites pass
- `artifacts/coverage-report/Summary.txt` is produced
- total line coverage is at least `70%`

- [ ] **Step 5: Commit the Phase 11 coverage foundation**

Run:
```bash
git add tests/GoodHamburguer.UnitTests tests/GoodHamburguer.IntegrationTests tests/GoodHamburguer.Blazor.Tests .github/workflows/ci.yml dotnet-tools.json
git commit -m "test: expand coverage and enforce threshold"
```

## Self-Review

**Spec coverage:** This plan covers unit, integration, and Blazor test expansion plus coverage tooling and CI enforcement, matching the feature spec for Phase 11.

**Placeholder scan:** No `TODO`, `TBD`, or deferred placeholders remain. Every task includes exact files, concrete test additions, exact commands, and expected outcomes.

**Type consistency:** File names, DTO names, API client names, and route names match the current codebase (`OrderSummaryDto`, `CreateOrderRequestDto`, `UpdateOrderRequestDto`, `OrderApiClient`, `MenuApiClient`, route paths under `/api/v1/...`).

## Execution Handoff

Plan complete and saved to `docs/superpowers/plans/2026-04-23-phase-11-testes-e-cobertura.md`. Two execution options:

**1. Subagent-Driven (recommended)** - I dispatch a fresh subagent per task, review between tasks, fast iteration

**2. Inline Execution** - Execute tasks in this session using executing-plans, batch execution with checkpoints

**Which approach?**
