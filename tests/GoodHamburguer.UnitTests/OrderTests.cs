using FluentAssertions;
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
            sandwich: new OrderItemSelection("sandwich-x-burger"),
            side: new OrderItemSelection("side-fries"),
            drink: new OrderItemSelection("drink-soft-drink"),
            createdAtUtc: now);

        order.Id.Should().Be(orderId);
        order.CreatedAtUtc.Should().Be(now);
        order.UpdatedAtUtc.Should().Be(now);
        order.Sandwich!.ItemCode.Should().Be("sandwich-x-burger");
        order.Side!.ItemCode.Should().Be("side-fries");
        order.Drink!.ItemCode.Should().Be("drink-soft-drink");
    }

    [Fact]
    public void UpdateItems_ShouldReplaceSlotsAndRefreshUpdatedAtUtc()
    {
        var createdAt = new DateTimeOffset(2026, 4, 21, 12, 0, 0, TimeSpan.Zero);
        var updatedAt = createdAt.AddMinutes(10);
        var order = Order.Create(
            Guid.NewGuid(),
            sandwich: new OrderItemSelection("sandwich-x-burger"),
            side: null,
            drink: null,
            createdAtUtc: createdAt);

        order.UpdateItems(
            sandwich: new OrderItemSelection("sandwich-x-bacon"),
            side: new OrderItemSelection("side-fries"),
            drink: null,
            updatedAtUtc: updatedAt);

        order.Sandwich!.ItemCode.Should().Be("sandwich-x-bacon");
        order.Side!.ItemCode.Should().Be("side-fries");
        order.Drink.Should().BeNull();
        order.UpdatedAtUtc.Should().Be(updatedAt);
    }

    [Fact]
    public void Create_ShouldAllowNullSlotsWithoutBreakingStructuralShape()
    {
        var order = Order.Create(
            Guid.NewGuid(),
            sandwich: null,
            side: new OrderItemSelection("side-fries"),
            drink: null,
            createdAtUtc: DateTimeOffset.UtcNow);

        order.Sandwich.Should().BeNull();
        order.Side!.ItemCode.Should().Be("side-fries");
        order.Drink.Should().BeNull();
    }
}
