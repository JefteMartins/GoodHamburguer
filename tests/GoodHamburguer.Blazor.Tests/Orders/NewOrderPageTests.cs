using Bunit;
using FluentAssertions;
using GoodHamburguer.Blazor.Services.Api.Menu;
using GoodHamburguer.Blazor.Services.Api.Orders;
using Microsoft.Extensions.DependencyInjection;

namespace GoodHamburguer.Blazor.Tests.Orders;

public class NewOrderPageTests : TestContext
{
    [Fact]
    public void NewOrder_LoadsMenuAndCreatesOrder()
    {
        Services.AddSingleton<IMenuApiClient>(new StubMenuApiClient(() => Task.FromResult<IReadOnlyList<MenuCategoryDto>>(
        [
            new MenuCategoryDto
            {
                Code = "sandwiches",
                DisplayName = "Sanduiches",
                Items =
                [
                    new MenuItemDto
                    {
                        Code = "sandwich-x-burger",
                        Name = "X-Burger",
                        Price = 12m
                    }
                ]
            },
            new MenuCategoryDto
            {
                Code = "sides",
                DisplayName = "Acompanhamentos",
                Items =
                [
                    new MenuItemDto
                    {
                        Code = "side-fries",
                        Name = "Batata frita",
                        Price = 5m
                    }
                ]
            },
            new MenuCategoryDto
            {
                Code = "drinks",
                DisplayName = "Bebidas",
                Items =
                [
                    new MenuItemDto
                    {
                        Code = "drink-soft-drink",
                        Name = "Refrigerante",
                        Price = 4m
                    }
                ]
            }
        ])));
        Services.AddSingleton<IOrderApiClient>(new StubOrderApiClient(_ => Task.FromResult(new OrderSummaryDto
        {
            Id = Guid.Parse("11111111-1111-1111-1111-111111111111"),
            SandwichItemCode = "sandwich-x-burger",
            SideItemCode = "side-fries",
            DrinkItemCode = "drink-soft-drink",
            Subtotal = 21m,
            Discount = 1m,
            Total = 20m,
            CreatedAtUtc = DateTimeOffset.UtcNow,
            UpdatedAtUtc = DateTimeOffset.UtcNow
        })));

        var cut = RenderComponent<GoodHamburguer.Blazor.Components.Pages.NewOrder>();

        cut.WaitForAssertion(() => cut.Markup.Should().Contain("Create order"));
        cut.Find("#sandwich").Change("sandwich-x-burger");
        cut.Find("#side").Change("side-fries");
        cut.Find("#drink").Change("drink-soft-drink");
        cut.Find("form").Submit();

        cut.WaitForAssertion(() =>
        {
            cut.Markup.Should().Contain("Order created");
            cut.Markup.Should().Contain("11111111-1111-1111-1111-111111111111");
            cut.Markup.Should().Contain("20.00");
        });
    }

    [Fact]
    public void NewOrder_ShowsValidationErrorsFromApi()
    {
        Services.AddSingleton<IMenuApiClient>(new StubMenuApiClient(() => Task.FromResult<IReadOnlyList<MenuCategoryDto>>(
        [
            new MenuCategoryDto
            {
                Code = "sandwiches",
                DisplayName = "Sanduiches",
                Items = []
            }
        ])));
        Services.AddSingleton<IOrderApiClient>(new StubOrderApiClient(_ =>
            Task.FromException<OrderSummaryDto>(new OrderApiValidationException(
                new Dictionary<string, string[]>
                {
                    ["sandwichItemCode"] = ["The item code 'bad-item' was not found in the menu."]
                },
                "The request payload contains invalid values."))));

        var cut = RenderComponent<GoodHamburguer.Blazor.Components.Pages.NewOrder>();

        cut.WaitForAssertion(() => cut.Markup.Should().Contain("Create order"));
        cut.Find("form").Submit();

        cut.WaitForAssertion(() =>
        {
            cut.Markup.Should().Contain("We couldn't create the order");
            cut.Markup.Should().Contain("sandwichItemCode: The item code 'bad-item' was not found in the menu.");
        });
    }

    [Fact]
    public void NewOrder_ShowsRetryWhenMenuCannotBeLoaded()
    {
        Services.AddSingleton<IMenuApiClient>(new StubMenuApiClient(() =>
            Task.FromException<IReadOnlyList<MenuCategoryDto>>(new InvalidOperationException("menu failed"))));
        Services.AddSingleton<IOrderApiClient>(new StubOrderApiClient(_ =>
            Task.FromException<OrderSummaryDto>(new InvalidOperationException("should not be called"))));

        var cut = RenderComponent<GoodHamburguer.Blazor.Components.Pages.NewOrder>();

        cut.WaitForAssertion(() =>
        {
            cut.Markup.Should().Contain("Menu unavailable");
            cut.Markup.Should().Contain("Try again");
        });
    }

    [Fact]
    public void NewOrder_ShowsGenericErrorWhenSubmitFailsUnexpectedly()
    {
        Services.AddSingleton<IMenuApiClient>(new StubMenuApiClient(() => Task.FromResult<IReadOnlyList<MenuCategoryDto>>(
        [
            new MenuCategoryDto
            {
                Code = "sandwiches",
                DisplayName = "Sanduiches",
                Items = []
            }
        ])));
        Services.AddSingleton<IOrderApiClient>(new StubOrderApiClient(_ =>
            Task.FromException<OrderSummaryDto>(new HttpRequestException("network"))));

        var cut = RenderComponent<GoodHamburguer.Blazor.Components.Pages.NewOrder>();

        cut.WaitForAssertion(() => cut.Markup.Should().Contain("Create order"));
        cut.Find("form").Submit();

        cut.WaitForAssertion(() =>
            cut.Markup.Should().Contain("We couldn't create the order right now"));
    }

    private sealed class StubMenuApiClient(Func<Task<IReadOnlyList<MenuCategoryDto>>> getMenu)
        : IMenuApiClient
    {
        public Task<IReadOnlyList<MenuCategoryDto>> GetMenuAsync(CancellationToken cancellationToken = default) =>
            getMenu();
    }

    private sealed class StubOrderApiClient(Func<CreateOrderRequestDto, Task<OrderSummaryDto>> createOrder)
        : IOrderApiClient
    {
        public Task<OrderSummaryDto> CreateOrderAsync(
            CreateOrderRequestDto request,
            CancellationToken cancellationToken = default) => createOrder(request);

        public Task<IReadOnlyList<OrderSummaryDto>> ListOrdersAsync(CancellationToken cancellationToken = default) =>
            Task.FromException<IReadOnlyList<OrderSummaryDto>>(new NotImplementedException());

        public Task DeleteOrderAsync(Guid id, CancellationToken cancellationToken = default) =>
            Task.FromException(new NotImplementedException());

        public Task<OrderSummaryDto> GetOrderByIdAsync(Guid id, CancellationToken cancellationToken = default) =>
            Task.FromException<OrderSummaryDto>(new NotImplementedException());

        public Task<OrderSummaryDto> UpdateOrderAsync(
            Guid id,
            UpdateOrderRequestDto request,
            CancellationToken cancellationToken = default) =>
            Task.FromException<OrderSummaryDto>(new NotImplementedException());
    }
}
