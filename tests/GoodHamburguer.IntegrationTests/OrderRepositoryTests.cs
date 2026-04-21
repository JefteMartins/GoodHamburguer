using FluentAssertions;
using GoodHamburguer.Application.Orders.Abstractions;
using GoodHamburguer.Domain.Menu;
using GoodHamburguer.Domain.Orders;
using GoodHamburguer.IntegrationTests.Infrastructure;
using Microsoft.Extensions.DependencyInjection;

namespace GoodHamburguer.IntegrationTests;

public sealed class OrderRepositoryTests : IClassFixture<MySqlWebApplicationFactory>
{
    private readonly MySqlWebApplicationFactory _factory;

    public OrderRepositoryTests(MySqlWebApplicationFactory factory)
    {
        _factory = factory;
    }

    [Fact]
    public async Task AddAndGetById_ShouldPersistOrderSlots()
    {
        var order = Order.Create(
            Guid.NewGuid(),
            sandwich: new OrderItemSelection("X Burger", MenuCategory.Sandwiches, 5.00m),
            side: new OrderItemSelection("Batata frita", MenuCategory.Sides, 2.00m),
            drink: null,
            createdAtUtc: new DateTimeOffset(2026, 4, 21, 18, 0, 0, TimeSpan.Zero));

        await using var writeScope = _factory.Services.CreateAsyncScope();
        var repository = writeScope.ServiceProvider.GetRequiredService<IOrderRepository>();

        await repository.AddAsync(order);

        await using var readScope = _factory.Services.CreateAsyncScope();
        var reloadedRepository = readScope.ServiceProvider.GetRequiredService<IOrderRepository>();
        var persistedOrder = await reloadedRepository.GetByIdAsync(order.Id);

        persistedOrder.Should().NotBeNull();
        persistedOrder!.Sandwich!.Name.Should().Be("X Burger");
        persistedOrder.Side!.Name.Should().Be("Batata frita");
        persistedOrder.Drink.Should().BeNull();
        persistedOrder.CreatedAtUtc.Should().Be(order.CreatedAtUtc);
    }
}
