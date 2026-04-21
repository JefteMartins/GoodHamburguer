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
            Sandwich = new OrderItemInput
            {
                Name = "X Egg",
                UnitPrice = 4.50m
            },
            Drink = new OrderItemInput
            {
                Name = "Refrigerante",
                UnitPrice = 2.50m
            }
        };

        var order = service.CreateOrder(orderId, request, now);

        order.Id.Should().Be(orderId);
        order.Sandwich!.Name.Should().Be("X Egg");
        order.Sandwich.Category.Code.Should().Be("sandwiches");
        order.Side.Should().BeNull();
        order.Drink!.Name.Should().Be("Refrigerante");
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
                Sandwich = new OrderItemInput
                {
                    Name = "X Burger",
                    UnitPrice = 5.00m
                }
            },
            createdAt);

        service.UpdateOrder(
            order,
            new UpdateOrderRequest
            {
                Side = new OrderItemInput
                {
                    Name = "Batata frita",
                    UnitPrice = 2.00m
                },
                Drink = new OrderItemInput
                {
                    Name = "Refrigerante",
                    UnitPrice = 2.50m
                }
            },
            createdAt.AddMinutes(30));

        order.Sandwich.Should().BeNull();
        order.Side!.Category.Code.Should().Be("sides");
        order.Drink!.Category.Code.Should().Be("drinks");
        order.UpdatedAtUtc.Should().Be(createdAt.AddMinutes(30));
    }
}
