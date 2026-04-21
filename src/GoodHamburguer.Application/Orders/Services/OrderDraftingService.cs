using GoodHamburguer.Application.Orders.Contracts;
using GoodHamburguer.Domain.Menu;
using GoodHamburguer.Domain.Orders;

namespace GoodHamburguer.Application.Orders.Services;

public sealed class OrderDraftingService : IOrderDraftingService
{
    public Order CreateOrder(Guid orderId, CreateOrderRequest request, DateTimeOffset createdAtUtc)
    {
        ArgumentNullException.ThrowIfNull(request);

        return Order.Create(
            orderId,
            sandwich: ToSelection(request.Sandwich, MenuCategory.Sandwiches),
            side: ToSelection(request.Side, MenuCategory.Sides),
            drink: ToSelection(request.Drink, MenuCategory.Drinks),
            createdAtUtc: createdAtUtc);
    }

    public void UpdateOrder(Order order, UpdateOrderRequest request, DateTimeOffset updatedAtUtc)
    {
        ArgumentNullException.ThrowIfNull(order);
        ArgumentNullException.ThrowIfNull(request);

        order.UpdateItems(
            sandwich: ToSelection(request.Sandwich, MenuCategory.Sandwiches),
            side: ToSelection(request.Side, MenuCategory.Sides),
            drink: ToSelection(request.Drink, MenuCategory.Drinks),
            updatedAtUtc: updatedAtUtc);
    }

    private static OrderItemSelection? ToSelection(OrderItemInput? input, MenuCategory category)
    {
        return input is null
            ? null
            : new OrderItemSelection(input.Name, category, input.UnitPrice);
    }
}
