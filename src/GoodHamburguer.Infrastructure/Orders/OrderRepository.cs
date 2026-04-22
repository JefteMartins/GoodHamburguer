using GoodHamburguer.Application.Orders.Abstractions;
using GoodHamburguer.Domain.Orders;
using GoodHamburguer.Infrastructure.Persistence;
using GoodHamburguer.Infrastructure.Persistence.Entities;
using Microsoft.EntityFrameworkCore;

namespace GoodHamburguer.Infrastructure.Orders;

public sealed class OrderRepository : IOrderRepository
{
    private readonly GoodHamburguerDbContext _dbContext;

    public OrderRepository(GoodHamburguerDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task AddAsync(Order order, CancellationToken cancellationToken = default)
    {
        _dbContext.Orders.Add(GoodHamburguerDbInitializer.MapOrder(order));
        await _dbContext.SaveChangesAsync(cancellationToken);
    }

    public async Task<IReadOnlyList<Order>> ListAsync(CancellationToken cancellationToken = default)
    {
        var entities = await _dbContext.Orders
            .AsNoTracking()
            .OrderBy(order => order.CreatedAtUtc)
            .ToListAsync(cancellationToken);

        return entities
            .Select(MapOrder)
            .ToArray();
    }

    public async Task UpdateAsync(Order order, CancellationToken cancellationToken = default)
    {
        var entity = await _dbContext.Orders
            .SingleAsync(existingOrder => existingOrder.Id == order.Id, cancellationToken);

        entity.UpdatedAtUtc = order.UpdatedAtUtc;
        entity.SandwichItemCode = order.Sandwich?.ItemCode;
        entity.SideItemCode = order.Side?.ItemCode;
        entity.DrinkItemCode = order.Drink?.ItemCode;

        await _dbContext.SaveChangesAsync(cancellationToken);
    }

    public async Task<Order?> GetByIdAsync(Guid orderId, CancellationToken cancellationToken = default)
    {
        var entity = await _dbContext.Orders
            .AsNoTracking()
            .SingleOrDefaultAsync(order => order.Id == orderId, cancellationToken);

        return entity is null ? null : MapOrder(entity);
    }

    public async Task<bool> DeleteAsync(Guid orderId, CancellationToken cancellationToken = default)
    {
        var affectedRows = await _dbContext.Orders
            .Where(order => order.Id == orderId)
            .ExecuteDeleteAsync(cancellationToken);

        return affectedRows > 0;
    }

    private static Order MapOrder(OrderEntity entity)
    {
        var sandwich = ToSelection(entity.SandwichItemCode);
        var side = ToSelection(entity.SideItemCode);
        var drink = ToSelection(entity.DrinkItemCode);

        var order = Order.Create(
            entity.Id,
            sandwich: sandwich,
            side: side,
            drink: drink,
            createdAtUtc: entity.CreatedAtUtc);

        if (entity.UpdatedAtUtc != entity.CreatedAtUtc)
        {
            order.UpdateItems(sandwich, side, drink, entity.UpdatedAtUtc);
        }

        return order;
    }

    private static OrderItemSelection? ToSelection(string? itemCode)
    {
        return string.IsNullOrWhiteSpace(itemCode)
            ? null
            : new OrderItemSelection(itemCode);
    }
}
