using Bunit;
using FluentAssertions;
using GoodHamburguer.Blazor.Services.Api.Orders;
using Microsoft.Extensions.DependencyInjection;

namespace GoodHamburguer.Blazor.Tests.Orders;

public class OrdersDashboardPageTests : TestContext
{
    [Fact]
    public void OrdersDashboard_LoadsAndRendersOrders()
    {
        Services.AddSingleton<IOrderApiClient>(new StubOrderApiClient
        {
            List = () => Task.FromResult<IReadOnlyList<OrderSummaryDto>>(
            [
                Order("11111111-1111-1111-1111-111111111111", "sandwich-x-burger", 12m),
                Order("22222222-2222-2222-2222-222222222222", "sandwich-x-egg", 10m)
            ])
        });

        var cut = RenderComponent<GoodHamburguer.Blazor.Components.Pages.OrdersDashboard>();

        cut.WaitForAssertion(() =>
        {
            cut.Markup.Should().Contain("Orders dashboard");
            cut.Markup.Should().Contain("11111111-1111-1111-1111-111111111111");
            cut.Markup.Should().Contain("sandwich-x-egg");
        });
    }

    [Fact]
    public void OrdersDashboard_ShowsEmptyStateWhenNoOrdersExist()
    {
        Services.AddSingleton<IOrderApiClient>(new StubOrderApiClient
        {
            List = () => Task.FromResult<IReadOnlyList<OrderSummaryDto>>([])
        });

        var cut = RenderComponent<GoodHamburguer.Blazor.Components.Pages.OrdersDashboard>();

        cut.WaitForAssertion(() =>
            cut.Markup.Should().Contain("No orders have been created yet"));
    }

    [Fact]
    public void OrdersDashboard_ProvidesLinkToOrderDetails()
    {
        Services.AddSingleton<IOrderApiClient>(new StubOrderApiClient
        {
            List = () => Task.FromResult<IReadOnlyList<OrderSummaryDto>>(
            [
                Order("33333333-3333-3333-3333-333333333333", "sandwich-x-burger", 12m),
                Order("44444444-4444-4444-4444-444444444444", "sandwich-x-egg", 10m)
            ])
        });

        var cut = RenderComponent<GoodHamburguer.Blazor.Components.Pages.OrdersDashboard>();

        cut.WaitForAssertion(() => cut.Markup.Should().Contain("44444444-4444-4444-4444-444444444444"));

        cut.WaitForAssertion(() =>
        {
            var detailsLink = cut.Find("a[href='/orders/33333333-3333-3333-3333-333333333333']");
            detailsLink.TextContent.Should().Contain("View details");
        });
    }

    [Fact]
    public void OrdersDashboard_ShowsProblemDetailsMessageWhenLoadFails()
    {
        Services.AddSingleton<IOrderApiClient>(new StubOrderApiClient
        {
            List = () => Task.FromException<IReadOnlyList<OrderSummaryDto>>(
                new OrderApiProblemException("The orders service is temporarily unavailable."))
        });

        var cut = RenderComponent<GoodHamburguer.Blazor.Components.Pages.OrdersDashboard>();

        cut.WaitForAssertion(() =>
            cut.Markup.Should().Contain("The orders service is temporarily unavailable."));
    }

    [Fact]
    public void OrdersDashboard_ShowsGenericErrorMessageWhenUnexpectedFailureOccurs()
    {
        Services.AddSingleton<IOrderApiClient>(new StubOrderApiClient
        {
            List = () => Task.FromException<IReadOnlyList<OrderSummaryDto>>(new InvalidOperationException("boom"))
        });

        var cut = RenderComponent<GoodHamburguer.Blazor.Components.Pages.OrdersDashboard>();

        cut.WaitForAssertion(() =>
            cut.Markup.Should().Contain("We couldn't load the orders right now. Please try again in a moment."));
    }

    [Fact]
    public void OrdersDashboard_ShowsLoadingStateWhileRequestIsInFlight()
    {
        var completion = new TaskCompletionSource<IReadOnlyList<OrderSummaryDto>>();
        Services.AddSingleton<IOrderApiClient>(new StubOrderApiClient
        {
            List = () => completion.Task
        });

        var cut = RenderComponent<GoodHamburguer.Blazor.Components.Pages.OrdersDashboard>();

        cut.WaitForAssertion(() => cut.Markup.Should().Contain("Loading orders..."));

        completion.SetResult([]);
        cut.WaitForAssertion(() => cut.Markup.Should().Contain("No orders have been created yet."));
    }

    private static OrderSummaryDto Order(string id, string? sandwichCode, decimal total) =>
        new()
        {
            Id = Guid.Parse(id),
            SandwichItemCode = sandwichCode,
            SideItemCode = null,
            DrinkItemCode = null,
            Subtotal = total,
            Discount = 0m,
            Total = total,
            CreatedAtUtc = DateTimeOffset.UtcNow,
            UpdatedAtUtc = DateTimeOffset.UtcNow
        };

    private sealed class StubOrderApiClient : IOrderApiClient
    {
        public Func<Task<IReadOnlyList<OrderSummaryDto>>> List { get; init; } =
            () => Task.FromException<IReadOnlyList<OrderSummaryDto>>(new NotImplementedException());

        public Func<CreateOrderRequestDto, Task<OrderSummaryDto>> Create { get; init; } =
            _ => Task.FromException<OrderSummaryDto>(new NotImplementedException());

        public Func<Guid, Task<OrderSummaryDto>> GetById { get; init; } =
            _ => Task.FromException<OrderSummaryDto>(new NotImplementedException());

        public Func<Guid, UpdateOrderRequestDto, Task<OrderSummaryDto>> Update { get; init; } =
            (_, _) => Task.FromException<OrderSummaryDto>(new NotImplementedException());

        public Func<Guid, Task> Delete { get; init; } =
            _ => Task.FromException(new NotImplementedException());

        public Task<IReadOnlyList<OrderSummaryDto>> ListOrdersAsync(CancellationToken cancellationToken = default) => List();

        public Task<OrderSummaryDto> CreateOrderAsync(CreateOrderRequestDto request, CancellationToken cancellationToken = default) => Create(request);

        public Task<OrderSummaryDto> GetOrderByIdAsync(Guid id, CancellationToken cancellationToken = default) => GetById(id);

        public Task<OrderSummaryDto> UpdateOrderAsync(Guid id, UpdateOrderRequestDto request, CancellationToken cancellationToken = default) => Update(id, request);

        public Task DeleteOrderAsync(Guid id, CancellationToken cancellationToken = default) => Delete(id);
    }
}
