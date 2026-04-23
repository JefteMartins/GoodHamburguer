using Bunit;
using FluentAssertions;
using GoodHamburguer.Blazor.Services.Api.Menu;
using GoodHamburguer.Blazor.Services.Api.Orders;
using Microsoft.Extensions.DependencyInjection;

namespace GoodHamburguer.Blazor.Tests.Orders;

public class OrderDetailsPageTests : TestContext
{
    [Fact]
    public void OrderDetails_LoadsAndShowsExistingOrder()
    {
        var orderId = Guid.Parse("55555555-5555-5555-5555-555555555555");
        Services.AddSingleton<IMenuApiClient>(new StubMenuApiClient(() => Task.FromResult<IReadOnlyList<MenuCategoryDto>>(
        [
            Category("sandwiches", "Sanduiches", ("sandwich-x-burger", "X-Burger")),
            Category("sides", "Acompanhamentos", ("side-fries", "Batata frita")),
            Category("drinks", "Bebidas", ("drink-soft-drink", "Refrigerante"))
        ])));
        Services.AddSingleton<IOrderApiClient>(new StubOrderApiClient
        {
            GetById = _ => Task.FromResult(new OrderSummaryDto
            {
                Id = orderId,
                SandwichItemCode = "sandwich-x-burger",
                SideItemCode = "side-fries",
                DrinkItemCode = "drink-soft-drink",
                Subtotal = 21m,
                Discount = 1m,
                Total = 20m,
                CreatedAtUtc = DateTimeOffset.UtcNow,
                UpdatedAtUtc = DateTimeOffset.UtcNow
            })
        });

        var cut = RenderComponent<GoodHamburguer.Blazor.Components.Pages.OrderDetails>(
            parameters => parameters.Add(page => page.Id, orderId));

        cut.WaitForAssertion(() =>
        {
            cut.Markup.Should().Contain("Order details");
            cut.Markup.Should().Contain(orderId.ToString());
            cut.Find("#sandwich").GetAttribute("value").Should().Be("sandwich-x-burger");
        });
    }

    [Fact]
    public void OrderDetails_CanUpdateExistingOrder()
    {
        var orderId = Guid.Parse("66666666-6666-6666-6666-666666666666");
        var orderApi = new StubOrderApiClient
        {
            GetById = _ => Task.FromResult(new OrderSummaryDto
            {
                Id = orderId,
                SandwichItemCode = "sandwich-x-burger",
                SideItemCode = null,
                DrinkItemCode = null,
                Subtotal = 12m,
                Discount = 0m,
                Total = 12m,
                CreatedAtUtc = DateTimeOffset.UtcNow,
                UpdatedAtUtc = DateTimeOffset.UtcNow
            }),
            Update = (_, request) => Task.FromResult(new OrderSummaryDto
            {
                Id = orderId,
                SandwichItemCode = request.SandwichItemCode,
                SideItemCode = request.SideItemCode,
                DrinkItemCode = request.DrinkItemCode,
                Subtotal = 17m,
                Discount = 0m,
                Total = 17m,
                CreatedAtUtc = DateTimeOffset.UtcNow,
                UpdatedAtUtc = DateTimeOffset.UtcNow
            })
        };

        Services.AddSingleton<IMenuApiClient>(new StubMenuApiClient(() => Task.FromResult<IReadOnlyList<MenuCategoryDto>>(
        [
            Category("sandwiches", "Sanduiches", ("sandwich-x-burger", "X-Burger")),
            Category("sides", "Acompanhamentos", ("side-fries", "Batata frita")),
            Category("drinks", "Bebidas", ("drink-soft-drink", "Refrigerante"))
        ])));
        Services.AddSingleton<IOrderApiClient>(orderApi);

        var cut = RenderComponent<GoodHamburguer.Blazor.Components.Pages.OrderDetails>(
            parameters => parameters.Add(page => page.Id, orderId));

        cut.WaitForAssertion(() => cut.Find("#side"));
        cut.Find("#side").Change("side-fries");
        cut.Find("form").Submit();

        cut.WaitForAssertion(() =>
        {
            cut.Markup.Should().Contain("Order updated");
            cut.Markup.Should().Contain("17.00");
        });
    }

    [Fact]
    public void OrderDetails_CanDeleteOrder()
    {
        var orderId = Guid.Parse("77777777-7777-7777-7777-777777777777");
        Services.AddSingleton<IMenuApiClient>(new StubMenuApiClient(() => Task.FromResult<IReadOnlyList<MenuCategoryDto>>(
        [
            Category("sandwiches", "Sanduiches", ("sandwich-x-burger", "X-Burger"))
        ])));
        Services.AddSingleton<IOrderApiClient>(new StubOrderApiClient
        {
            GetById = _ => Task.FromResult(new OrderSummaryDto
            {
                Id = orderId,
                SandwichItemCode = "sandwich-x-burger",
                SideItemCode = null,
                DrinkItemCode = null,
                Subtotal = 12m,
                Discount = 0m,
                Total = 12m,
                CreatedAtUtc = DateTimeOffset.UtcNow,
                UpdatedAtUtc = DateTimeOffset.UtcNow
            }),
            Delete = _ => Task.CompletedTask
        });

        var cut = RenderComponent<GoodHamburguer.Blazor.Components.Pages.OrderDetails>(
            parameters => parameters.Add(page => page.Id, orderId));

        cut.WaitForAssertion(() => cut.Find("#delete-order"));
        cut.Find("#delete-order").Click();

        cut.WaitForAssertion(() =>
            cut.Markup.Should().Contain("Order deleted"));
    }

    [Fact]
    public void OrderDetails_DisablesDeleteWhileUpdateIsRunning()
    {
        var orderId = Guid.Parse("bbbbbbbb-bbbb-bbbb-bbbb-bbbbbbbbbbbb");
        var updateCompletion = new TaskCompletionSource<OrderSummaryDto>();
        var orderApi = new StubOrderApiClient
        {
            GetById = _ => Task.FromResult(new OrderSummaryDto
            {
                Id = orderId,
                SandwichItemCode = "sandwich-x-burger",
                SideItemCode = null,
                DrinkItemCode = null,
                Subtotal = 12m,
                Discount = 0m,
                Total = 12m,
                CreatedAtUtc = DateTimeOffset.UtcNow,
                UpdatedAtUtc = DateTimeOffset.UtcNow
            }),
            Update = (_, _) => updateCompletion.Task
        };

        Services.AddSingleton<IMenuApiClient>(new StubMenuApiClient(() => Task.FromResult<IReadOnlyList<MenuCategoryDto>>(
        [
            Category("sandwiches", "Sanduiches", ("sandwich-x-burger", "X-Burger"))
        ])));
        Services.AddSingleton<IOrderApiClient>(orderApi);

        var cut = RenderComponent<GoodHamburguer.Blazor.Components.Pages.OrderDetails>(
            parameters => parameters.Add(page => page.Id, orderId));

        cut.WaitForAssertion(() => cut.Find("#delete-order"));
        cut.Find("form").Submit();

        cut.WaitForAssertion(() =>
        {
            cut.Find("#delete-order").HasAttribute("disabled").Should().BeTrue();
            cut.Find("button[type='submit']").HasAttribute("disabled").Should().BeTrue();
        });
    }

    [Fact]
    public void OrderDetails_ReloadsLatestRouteAfterUpdateCompletes()
    {
        var firstOrderId = Guid.Parse("cccccccc-cccc-cccc-cccc-cccccccccccc");
        var secondOrderId = Guid.Parse("dddddddd-dddd-dddd-dddd-dddddddddddd");
        var updateCompletion = new TaskCompletionSource<OrderSummaryDto>();

        Services.AddSingleton<IMenuApiClient>(new StubMenuApiClient(() => Task.FromResult<IReadOnlyList<MenuCategoryDto>>(
        [
            Category("sandwiches", "Sanduiches", ("sandwich-x-burger", "X-Burger"))
        ])));
        Services.AddSingleton<IOrderApiClient>(new StubOrderApiClient
        {
            GetById = id => Task.FromResult(new OrderSummaryDto
            {
                Id = id,
                SandwichItemCode = id == firstOrderId ? "sandwich-x-burger" : null,
                SideItemCode = null,
                DrinkItemCode = null,
                Subtotal = 12m,
                Discount = 0m,
                Total = 12m,
                CreatedAtUtc = DateTimeOffset.UtcNow,
                UpdatedAtUtc = DateTimeOffset.UtcNow
            }),
            Update = (_, _) => updateCompletion.Task
        });

        var cut = RenderComponent<GoodHamburguer.Blazor.Components.Pages.OrderDetails>(
            parameters => parameters.Add(page => page.Id, firstOrderId));

        cut.WaitForAssertion(() => cut.Markup.Should().Contain(firstOrderId.ToString()));
        cut.Find("form").Submit();

        cut.SetParametersAndRender(parameters => parameters.Add(page => page.Id, secondOrderId));
        updateCompletion.SetResult(new OrderSummaryDto
        {
            Id = firstOrderId,
            SandwichItemCode = "sandwich-x-burger",
            SideItemCode = null,
            DrinkItemCode = null,
            Subtotal = 12m,
            Discount = 0m,
            Total = 12m,
            CreatedAtUtc = DateTimeOffset.UtcNow,
            UpdatedAtUtc = DateTimeOffset.UtcNow
        });

        cut.WaitForAssertion(() =>
        {
            cut.Markup.Should().Contain(secondOrderId.ToString());
            cut.Markup.Should().NotContain(firstOrderId.ToString());
        });
    }

    private static MenuCategoryDto Category(string code, string displayName, params (string Code, string Name)[] items) =>
        new()
        {
            Code = code,
            DisplayName = displayName,
            Items = items.Select(item => new MenuItemDto
            {
                Code = item.Code,
                Name = item.Name,
                Price = 10m
            }).ToArray()
        };

    private sealed class StubMenuApiClient(Func<Task<IReadOnlyList<MenuCategoryDto>>> getMenu)
        : IMenuApiClient
    {
        public Task<IReadOnlyList<MenuCategoryDto>> GetMenuAsync(CancellationToken cancellationToken = default) => getMenu();
    }

    private sealed class StubOrderApiClient : IOrderApiClient
    {
        public Func<Guid, Task<OrderSummaryDto>> GetById { get; init; } =
            _ => Task.FromException<OrderSummaryDto>(new NotImplementedException());

        public Func<CreateOrderRequestDto, Task<OrderSummaryDto>> Create { get; init; } =
            _ => Task.FromException<OrderSummaryDto>(new NotImplementedException());

        public Func<Task<IReadOnlyList<OrderSummaryDto>>> List { get; init; } =
            () => Task.FromException<IReadOnlyList<OrderSummaryDto>>(new NotImplementedException());

        public Func<Guid, UpdateOrderRequestDto, Task<OrderSummaryDto>> Update { get; init; } =
            (_, _) => Task.FromException<OrderSummaryDto>(new NotImplementedException());

        public Func<Guid, Task> Delete { get; init; } =
            _ => Task.FromException(new NotImplementedException());

        public Task<OrderSummaryDto> CreateOrderAsync(CreateOrderRequestDto request, CancellationToken cancellationToken = default) => Create(request);

        public Task<IReadOnlyList<OrderSummaryDto>> ListOrdersAsync(CancellationToken cancellationToken = default) => List();

        public Task DeleteOrderAsync(Guid id, CancellationToken cancellationToken = default) => Delete(id);

        public Task<OrderSummaryDto> GetOrderByIdAsync(Guid id, CancellationToken cancellationToken = default) => GetById(id);

        public Task<OrderSummaryDto> UpdateOrderAsync(Guid id, UpdateOrderRequestDto request, CancellationToken cancellationToken = default) => Update(id, request);
    }
}
