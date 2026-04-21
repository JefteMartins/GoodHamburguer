using FluentAssertions;
using GoodHamburguer.Domain.Menu;
using GoodHamburguer.Domain.Orders;

namespace GoodHamburguer.UnitTests;

public class OrderTests
{
    [Fact]
    public void Create_ShouldStoreExplicitSlotsAndInitializeTimestamps()
    {
        var orderId = Guid.NewGuid();
        var now = new DateTimeOffset(2026, 4, 21, 12, 0, 0, TimeSpan.Zero);

        var order = Order.Create(
            orderId,
            sandwich: new OrderItemSelection("X Burger", MenuCategory.Sandwiches, 5.00m),
            side: new OrderItemSelection("Batata frita", MenuCategory.Sides, 2.00m),
            drink: new OrderItemSelection("Refrigerante", MenuCategory.Drinks, 2.50m),
            createdAtUtc: now);

        order.Id.Should().Be(orderId);
        order.CreatedAtUtc.Should().Be(now);
        order.UpdatedAtUtc.Should().Be(now);
        order.Sandwich!.Name.Should().Be("X Burger");
        order.Side!.Name.Should().Be("Batata frita");
        order.Drink!.Name.Should().Be("Refrigerante");
    }

    [Fact]
    public void UpdateItems_ShouldReplaceSlotsAndRefreshUpdatedAtUtc()
    {
        var createdAt = new DateTimeOffset(2026, 4, 21, 12, 0, 0, TimeSpan.Zero);
        var updatedAt = createdAt.AddMinutes(10);
        var order = Order.Create(
            Guid.NewGuid(),
            sandwich: new OrderItemSelection("X Burger", MenuCategory.Sandwiches, 5.00m),
            side: null,
            drink: null,
            createdAtUtc: createdAt);

        order.UpdateItems(
            sandwich: new OrderItemSelection("X Bacon", MenuCategory.Sandwiches, 7.00m),
            side: new OrderItemSelection("Batata frita", MenuCategory.Sides, 2.00m),
            drink: null,
            updatedAtUtc: updatedAt);

        order.Sandwich!.Name.Should().Be("X Bacon");
        order.Side!.Name.Should().Be("Batata frita");
        order.Drink.Should().BeNull();
        order.UpdatedAtUtc.Should().Be(updatedAt);
    }

    [Fact]
    public void Create_ShouldRejectSelectionAssignedToTheWrongSlot()
    {
        var act = () => Order.Create(
            Guid.NewGuid(),
            sandwich: new OrderItemSelection("Refrigerante", MenuCategory.Drinks, 2.50m),
            side: null,
            drink: null,
            createdAtUtc: DateTimeOffset.UtcNow);

        act.Should()
            .Throw<ArgumentException>()
            .WithMessage("*sandwich*");
    }
}
