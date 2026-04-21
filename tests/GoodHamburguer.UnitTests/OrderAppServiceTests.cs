using FluentAssertions;
using GoodHamburguer.Application.Menu.Abstractions;
using GoodHamburguer.Application.Orders.Abstractions;
using GoodHamburguer.Application.Orders.Contracts;
using GoodHamburguer.Application.Orders.Services;
using GoodHamburguer.Application.Orders.Validation;
using GoodHamburguer.Domain.Menu;
using GoodHamburguer.Domain.Orders;

namespace GoodHamburguer.UnitTests;

public sealed class OrderAppServiceTests
{
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
        var service = new OrderAppService(
            repository,
            new OrderDraftingService(),
            menuQueryService,
            new CreateOrderRequestValidator(menuQueryService),
            new UpdateOrderRequestValidator(menuQueryService));

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

    [Fact]
    public async Task UpdateAsync_ShouldRecalculatePricingWhenOrderCompositionChanges()
    {
        var menuCatalog = new MenuCatalog(
        [
            new MenuItem("sandwich-x-burger", "X Burger", MenuCategory.Sandwiches, 5.00m),
            new MenuItem("side-fries", "Batata frita", MenuCategory.Sides, 2.00m),
            new MenuItem("drink-soft-drink", "Refrigerante", MenuCategory.Drinks, 2.50m)
        ]);

        var existingOrder = Order.Create(
            Guid.NewGuid(),
            sandwich: new OrderItemSelection("sandwich-x-burger"),
            side: null,
            drink: null,
            createdAtUtc: new DateTimeOffset(2026, 4, 21, 12, 0, 0, TimeSpan.Zero));

        var menuQueryService = new FakeMenuQueryService(menuCatalog);
        var repository = new RecordingOrderRepository(existingOrder);
        var service = new OrderAppService(
            repository,
            new OrderDraftingService(),
            menuQueryService,
            new CreateOrderRequestValidator(menuQueryService),
            new UpdateOrderRequestValidator(menuQueryService));

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

    private sealed class FakeMenuQueryService(MenuCatalog menuCatalog) : IMenuQueryService
    {
        public Task<MenuCatalog> GetMenuCatalogAsync(CancellationToken cancellationToken = default)
        {
            return Task.FromResult(menuCatalog);
        }
    }

    private sealed class RecordingOrderRepository(Order? storedOrder = null) : IOrderRepository
    {
        public Task AddAsync(Order order, CancellationToken cancellationToken = default)
        {
            return Task.CompletedTask;
        }

        public Task UpdateAsync(Order order, CancellationToken cancellationToken = default)
        {
            return Task.CompletedTask;
        }

        public Task<Order?> GetByIdAsync(Guid orderId, CancellationToken cancellationToken = default)
        {
            return Task.FromResult(storedOrder);
        }
    }
}
