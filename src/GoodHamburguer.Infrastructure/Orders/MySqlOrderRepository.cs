using GoodHamburguer.Application.Orders.Abstractions;
using GoodHamburguer.Domain.Menu;
using GoodHamburguer.Domain.Orders;
using GoodHamburguer.Infrastructure.Persistence;
using GoodHamburguer.Infrastructure.Persistence.Entities;
using Microsoft.EntityFrameworkCore;

namespace GoodHamburguer.Infrastructure.Orders;

public sealed class MySqlOrderRepository : IOrderRepository
{
    private readonly GoodHamburguerDbContext _dbContext;

    public MySqlOrderRepository(GoodHamburguerDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task AddAsync(Order order, CancellationToken cancellationToken = default)
    {
        _dbContext.Orders.Add(GoodHamburguerDbInitializer.MapOrder(order));
        await _dbContext.SaveChangesAsync(cancellationToken);
    }

    public async Task<Order?> GetByIdAsync(Guid orderId, CancellationToken cancellationToken = default)
    {
        var entity = await _dbContext.Orders
            .AsNoTracking()
            .SingleOrDefaultAsync(order => order.Id == orderId, cancellationToken);

        return entity is null ? null : MapOrder(entity);
    }

    private static Order MapOrder(OrderEntity entity)
    {
        var sandwich = ToSelection(entity.SandwichName, MenuCategory.Sandwiches, entity.SandwichUnitPrice);
        var side = ToSelection(entity.SideName, MenuCategory.Sides, entity.SideUnitPrice);
        var drink = ToSelection(entity.DrinkName, MenuCategory.Drinks, entity.DrinkUnitPrice);

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

    private static OrderItemSelection? ToSelection(string? name, MenuCategory category, decimal? unitPrice)
    {
        return string.IsNullOrWhiteSpace(name) || unitPrice is null
            ? null
            : new OrderItemSelection(name, category, unitPrice.Value);
    }
}
