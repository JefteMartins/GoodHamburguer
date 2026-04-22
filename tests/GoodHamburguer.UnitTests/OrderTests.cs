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

    [Fact]
    public void CalculatePricing_ShouldReturnTwentyPercentDiscount_WhenOrderHasFullCombo()
    {
        var order = Order.Create(
            Guid.NewGuid(),
            sandwich: new OrderItemSelection("sandwich-x-burger"),
            side: new OrderItemSelection("side-fries"),
            drink: new OrderItemSelection("drink-soft-drink"),
            createdAtUtc: new DateTimeOffset(2026, 4, 21, 12, 0, 0, TimeSpan.Zero));

        var pricing = order.CalculatePricing(CreateMenuItemsByCode());

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

        var pricing = order.CalculatePricing(CreateMenuItemsByCode());

        pricing.Subtotal.Should().Be(7.00m);
        pricing.Discount.Should().Be(0m);
        pricing.Total.Should().Be(7.00m);
    }

    [Fact]
    public void CalculatePricing_ShouldReturnSubtotalForSandwichOnly_WhenOnlySandwichIsSelected()
    {
        var order = Order.Create(
            Guid.NewGuid(),
            sandwich: new OrderItemSelection("sandwich-x-burger"),
            side: null,
            drink: null,
            createdAtUtc: new DateTimeOffset(2026, 4, 21, 12, 0, 0, TimeSpan.Zero));

        var pricing = order.CalculatePricing(CreateMenuItemsByCode());

        pricing.Subtotal.Should().Be(5.00m);
        pricing.Discount.Should().Be(0m);
        pricing.Total.Should().Be(5.00m);
    }

    [Fact]
    public void CalculatePricing_ShouldReturnZeroDiscount_WhenOrderHasSandwichAndSideOnly()
    {
        var order = Order.Create(
            Guid.NewGuid(),
            sandwich: new OrderItemSelection("sandwich-x-burger"),
            side: new OrderItemSelection("side-fries"),
            drink: null,
            createdAtUtc: new DateTimeOffset(2026, 4, 21, 12, 0, 0, TimeSpan.Zero));

        var pricing = order.CalculatePricing(CreateMenuItemsByCode());

        pricing.Subtotal.Should().Be(7.00m);
        pricing.Discount.Should().Be(0m);
        pricing.Total.Should().Be(7.00m);
    }

    [Fact]
    public void CalculatePricing_ShouldReturnZeroValues_WhenOrderHasNoSelectedItems()
    {
        var order = Order.Create(
            Guid.NewGuid(),
            sandwich: null,
            side: null,
            drink: null,
            createdAtUtc: new DateTimeOffset(2026, 4, 21, 12, 0, 0, TimeSpan.Zero));

        var pricing = order.CalculatePricing(CreateMenuItemsByCode());

        pricing.Subtotal.Should().Be(0m);
        pricing.Discount.Should().Be(0m);
        pricing.Total.Should().Be(0m);
    }

    private static IReadOnlyDictionary<string, MenuItem> CreateMenuItemsByCode()
    {
        return new Dictionary<string, MenuItem>(StringComparer.OrdinalIgnoreCase)
        {
            ["sandwich-x-burger"] = new("sandwich-x-burger", "X Burger", MenuCategory.Sandwiches, 5.00m),
            ["sandwich-x-egg"] = new("sandwich-x-egg", "X Egg", MenuCategory.Sandwiches, 4.50m),
            ["side-fries"] = new("side-fries", "Batata frita", MenuCategory.Sides, 2.00m),
            ["drink-soft-drink"] = new("drink-soft-drink", "Refrigerante", MenuCategory.Drinks, 2.50m)
        };
    }
}
