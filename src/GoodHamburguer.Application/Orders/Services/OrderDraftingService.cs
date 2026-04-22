using GoodHamburguer.Application.Orders.Contracts;
using GoodHamburguer.Domain.Orders;

namespace GoodHamburguer.Application.Orders.Services;

public sealed class OrderDraftingService : IOrderDraftingService
{
    public Order CreateOrder(Guid orderId, CreateOrderRequest request, DateTimeOffset createdAtUtc)
    {
        ArgumentNullException.ThrowIfNull(request);

        return Order.Create(
            orderId,
            sandwich: ToSelection(request.SandwichItemCode),
            side: ToSelection(request.SideItemCode),
            drink: ToSelection(request.DrinkItemCode),
            createdAtUtc: createdAtUtc);
    }

    public void UpdateOrder(Order order, UpdateOrderRequest request, DateTimeOffset updatedAtUtc)
    {
        ArgumentNullException.ThrowIfNull(order);
        ArgumentNullException.ThrowIfNull(request);

        order.UpdateItems(
            sandwich: ToSelection(request.SandwichItemCode),
            side: ToSelection(request.SideItemCode),
            drink: ToSelection(request.DrinkItemCode),
            updatedAtUtc: updatedAtUtc);
    }

    private static OrderItemSelection? ToSelection(string? itemCode)
    {
        return string.IsNullOrWhiteSpace(itemCode)
            ? null
            : new OrderItemSelection(itemCode);
    }
}
