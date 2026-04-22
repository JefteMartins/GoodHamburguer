using GoodHamburguer.Domain.Orders;

namespace GoodHamburguer.Application.Orders.Abstractions;

public interface IOrderRepository
{
    Task AddAsync(Order order, CancellationToken cancellationToken = default);

    Task<IReadOnlyList<Order>> ListAsync(CancellationToken cancellationToken = default);

    Task<Order?> GetByIdAsync(Guid orderId, CancellationToken cancellationToken = default);

    Task UpdateAsync(Order order, CancellationToken cancellationToken = default);

    Task<bool> DeleteAsync(Guid orderId, CancellationToken cancellationToken = default);
}
