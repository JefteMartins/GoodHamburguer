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
        var repository = new RecordingOrderRepository([existingOrder]);
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

    [Fact]
    public async Task GetByIdAsync_ShouldReturnCalculatedPricingFromMenuCatalog()
    {
        var menuCatalog = CreateMenuCatalog();
        var existingOrder = Order.Create(
            Guid.NewGuid(),
            sandwich: new OrderItemSelection("sandwich-x-burger"),
            side: new OrderItemSelection("side-fries"),
            drink: new OrderItemSelection("drink-soft-drink"),
            createdAtUtc: new DateTimeOffset(2026, 4, 21, 12, 0, 0, TimeSpan.Zero));

        var service = CreateService(new RecordingOrderRepository([existingOrder]), menuCatalog);

        var response = await service.GetByIdAsync(existingOrder.Id);

        response.Id.Should().Be(existingOrder.Id);
        response.Subtotal.Should().Be(9.50m);
        response.Discount.Should().Be(1.90m);
        response.Total.Should().Be(7.60m);
    }

    [Fact]
    public async Task GetByIdAsync_ShouldThrowWhenOrderDoesNotExist()
    {
        var service = CreateService(new RecordingOrderRepository(), CreateMenuCatalog());

        var action = () => service.GetByIdAsync(Guid.NewGuid());

        await action.Should().ThrowAsync<KeyNotFoundException>();
    }

    [Fact]
    public async Task ListAsync_ShouldReturnOrdersWithCalculatedPricing()
    {
        var menuCatalog = CreateMenuCatalog();
        var firstOrder = Order.Create(
            Guid.NewGuid(),
            sandwich: new OrderItemSelection("sandwich-x-burger"),
            side: null,
            drink: new OrderItemSelection("drink-soft-drink"),
            createdAtUtc: new DateTimeOffset(2026, 4, 21, 12, 0, 0, TimeSpan.Zero));
        var secondOrder = Order.Create(
            Guid.NewGuid(),
            sandwich: null,
            side: new OrderItemSelection("side-fries"),
            drink: null,
            createdAtUtc: new DateTimeOffset(2026, 4, 21, 13, 0, 0, TimeSpan.Zero));

        var service = CreateService(new RecordingOrderRepository([firstOrder, secondOrder]), menuCatalog);

        var response = await service.ListAsync();

        response.Should().HaveCount(2);
        response[0].Id.Should().Be(firstOrder.Id);
        response[0].Subtotal.Should().Be(7.50m);
        response[0].Discount.Should().Be(0m);
        response[0].Total.Should().Be(7.50m);
        response[1].Id.Should().Be(secondOrder.Id);
        response[1].Subtotal.Should().Be(2.00m);
        response[1].Discount.Should().Be(0m);
        response[1].Total.Should().Be(2.00m);
    }

    [Fact]
    public async Task DeleteAsync_ShouldRemoveExistingOrder()
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

        repository.DeletedOrderIds.Should().Contain(existingOrder.Id);
    }

    [Fact]
    public async Task DeleteAsync_ShouldThrowWhenOrderDoesNotExist()
    {
        var service = CreateService(new RecordingOrderRepository(), CreateMenuCatalog());

        var action = () => service.DeleteAsync(Guid.NewGuid());

        await action.Should().ThrowAsync<KeyNotFoundException>();
    }

    [Fact]
    public async Task DeleteAsync_ShouldThrowWhenRepositoryDoesNotDeleteAnyRow()
    {
        var existingOrder = Order.Create(
            Guid.NewGuid(),
            sandwich: new OrderItemSelection("sandwich-x-burger"),
            side: null,
            drink: null,
            createdAtUtc: new DateTimeOffset(2026, 4, 21, 12, 0, 0, TimeSpan.Zero));
        var repository = new RecordingOrderRepository([existingOrder])
        {
            ForceDeleteMiss = true
        };
        var service = CreateService(repository, CreateMenuCatalog());

        var action = () => service.DeleteAsync(existingOrder.Id);

        await action.Should().ThrowAsync<KeyNotFoundException>();
    }

    private static OrderAppService CreateService(IOrderRepository repository, MenuCatalog menuCatalog)
    {
        var menuQueryService = new FakeMenuQueryService(menuCatalog);

        return new OrderAppService(
            repository,
            new OrderDraftingService(),
            menuQueryService,
            new CreateOrderRequestValidator(menuQueryService),
            new UpdateOrderRequestValidator(menuQueryService));
    }

    private static MenuCatalog CreateMenuCatalog()
    {
        return new MenuCatalog(
        [
            new MenuItem("sandwich-x-burger", "X Burger", MenuCategory.Sandwiches, 5.00m),
            new MenuItem("side-fries", "Batata frita", MenuCategory.Sides, 2.00m),
            new MenuItem("drink-soft-drink", "Refrigerante", MenuCategory.Drinks, 2.50m)
        ]);
    }

    private sealed class FakeMenuQueryService(MenuCatalog menuCatalog) : IMenuQueryService
    {
        public Task<MenuCatalog> GetMenuCatalogAsync(CancellationToken cancellationToken = default)
        {
            return Task.FromResult(menuCatalog);
        }
    }

    private sealed class RecordingOrderRepository(IEnumerable<Order>? storedOrders = null) : IOrderRepository
    {
        private readonly List<Order> _storedOrders = storedOrders?.ToList() ?? [];

        public List<Guid> DeletedOrderIds { get; } = [];

        public bool ForceDeleteMiss { get; init; }

        public Task AddAsync(Order order, CancellationToken cancellationToken = default)
        {
            _storedOrders.Add(order);
            return Task.CompletedTask;
        }

        public Task UpdateAsync(Order order, CancellationToken cancellationToken = default)
        {
            return Task.CompletedTask;
        }

        public Task<Order?> GetByIdAsync(Guid orderId, CancellationToken cancellationToken = default)
        {
            return Task.FromResult(_storedOrders.SingleOrDefault(order => order.Id == orderId));
        }

        public Task<IReadOnlyList<Order>> ListAsync(CancellationToken cancellationToken = default)
        {
            return Task.FromResult<IReadOnlyList<Order>>(_storedOrders);
        }

        public Task<bool> DeleteAsync(Guid orderId, CancellationToken cancellationToken = default)
        {
            if (ForceDeleteMiss)
            {
                return Task.FromResult(false);
            }

            var removed = _storedOrders.RemoveAll(order => order.Id == orderId);
            if (removed is 0)
            {
                return Task.FromResult(false);
            }

            DeletedOrderIds.Add(orderId);
            return Task.FromResult(true);
        }
    }
}
