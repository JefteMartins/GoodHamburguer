using GoodHamburguer.Application.Orders.Contracts;
using GoodHamburguer.Domain.Orders;

namespace GoodHamburguer.Application.Orders.Services;

public interface IOrderDraftingService
{
    Order CreateOrder(Guid orderId, CreateOrderRequest request, DateTimeOffset createdAtUtc);

    void UpdateOrder(Order order, UpdateOrderRequest request, DateTimeOffset updatedAtUtc);
}
