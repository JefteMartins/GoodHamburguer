using FluentAssertions;
using GoodHamburguer.Application.Orders.Contracts;
using GoodHamburguer.Application.Orders.Services;

namespace GoodHamburguer.UnitTests;

public class OrderDraftingServiceTests
{
    [Fact]
    public void CreateOrder_ShouldMapContractsIntoExplicitOrderSlots()
    {
        var service = new OrderDraftingService();
        var orderId = Guid.NewGuid();
        var now = new DateTimeOffset(2026, 4, 21, 12, 0, 0, TimeSpan.Zero);
        var request = new CreateOrderRequest
        {
            SandwichItemCode = "sandwich-x-egg",
            DrinkItemCode = "drink-soft-drink"
        };

        var order = service.CreateOrder(orderId, request, now);

        order.Id.Should().Be(orderId);
        order.Sandwich!.ItemCode.Should().Be("sandwich-x-egg");
        order.Side.Should().BeNull();
        order.Drink!.ItemCode.Should().Be("drink-soft-drink");
    }

    [Fact]
    public void UpdateOrder_ShouldReplaceExistingSlotsUsingTheUpdateContract()
    {
        var service = new OrderDraftingService();
        var createdAt = new DateTimeOffset(2026, 4, 21, 12, 0, 0, TimeSpan.Zero);
        var order = service.CreateOrder(
            Guid.NewGuid(),
            new CreateOrderRequest
            {
                SandwichItemCode = "sandwich-x-burger"
            },
            createdAt);

        service.UpdateOrder(
            order,
            new UpdateOrderRequest
            {
                SideItemCode = "side-fries",
                DrinkItemCode = "drink-soft-drink"
            },
            createdAt.AddMinutes(30));

        order.Sandwich.Should().BeNull();
        order.Side!.ItemCode.Should().Be("side-fries");
        order.Drink!.ItemCode.Should().Be("drink-soft-drink");
        order.UpdatedAtUtc.Should().Be(createdAt.AddMinutes(30));
    }
}
