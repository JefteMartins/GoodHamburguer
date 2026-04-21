using GoodHamburguer.Application.Orders.Contracts;

namespace GoodHamburguer.Application.Orders.Services;

public interface IOrderAppService
{
    Task<OrderResponse> CreateAsync(CreateOrderRequest request, CancellationToken cancellationToken = default);

    Task<OrderResponse> UpdateAsync(Guid orderId, UpdateOrderRequest request, CancellationToken cancellationToken = default);
}
