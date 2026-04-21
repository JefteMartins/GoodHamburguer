using GoodHamburguer.Domain.Menu;
using GoodHamburguer.Domain.Orders;
using GoodHamburguer.Infrastructure.Persistence.Entities;
using Microsoft.EntityFrameworkCore;

namespace GoodHamburguer.Infrastructure.Persistence;

public sealed class GoodHamburguerDbInitializer : IDatabaseInitializer
{
    private static readonly MenuItemEntity[] SeedMenuItems =
    [
        new MenuItemEntity
        {
            Code = "drink-soft-drink",
            Name = "Refrigerante",
            CategoryCode = MenuCategory.Drinks.Code,
            CategoryDisplayName = MenuCategory.Drinks.DisplayName,
            Price = 2.50m
        },
        new MenuItemEntity
        {
            Code = "sandwich-x-bacon",
            Name = "X Bacon",
            CategoryCode = MenuCategory.Sandwiches.Code,
            CategoryDisplayName = MenuCategory.Sandwiches.DisplayName,
            Price = 7.00m
        },
        new MenuItemEntity
        {
            Code = "sandwich-x-burger",
            Name = "X Burger",
            CategoryCode = MenuCategory.Sandwiches.Code,
            CategoryDisplayName = MenuCategory.Sandwiches.DisplayName,
            Price = 5.00m
        },
        new MenuItemEntity
        {
            Code = "sandwich-x-egg",
            Name = "X Egg",
            CategoryCode = MenuCategory.Sandwiches.Code,
            CategoryDisplayName = MenuCategory.Sandwiches.DisplayName,
            Price = 4.50m
        },
        new MenuItemEntity
        {
            Code = "side-fries",
            Name = "Batata frita",
            CategoryCode = MenuCategory.Sides.Code,
            CategoryDisplayName = MenuCategory.Sides.DisplayName,
            Price = 2.00m
        }
    ];

    private static readonly Order[] SeedOrders =
    [
        Order.Create(
            Guid.Parse("11111111-1111-1111-1111-111111111111"),
            sandwich: new OrderItemSelection("sandwich-x-burger"),
            side: new OrderItemSelection("side-fries"),
            drink: new OrderItemSelection("drink-soft-drink"),
            createdAtUtc: new DateTimeOffset(2026, 4, 21, 9, 0, 0, TimeSpan.Zero)),
        Order.Create(
            Guid.Parse("22222222-2222-2222-2222-222222222222"),
            sandwich: new OrderItemSelection("sandwich-x-egg"),
            side: null,
            drink: new OrderItemSelection("drink-soft-drink"),
            createdAtUtc: new DateTimeOffset(2026, 4, 21, 9, 5, 0, TimeSpan.Zero))
    ];

    private readonly GoodHamburguerDbContext _dbContext;

    public GoodHamburguerDbInitializer(GoodHamburguerDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task InitializeAsync(CancellationToken cancellationToken = default)
    {
        var executionStrategy = _dbContext.Database.CreateExecutionStrategy();

        await executionStrategy.ExecuteAsync(async () =>
        {
            await _dbContext.Database.MigrateAsync(cancellationToken);
            await SeedMenuAsync(cancellationToken);
            await SeedOrdersAsync(cancellationToken);
        });
    }

    private async Task SeedMenuAsync(CancellationToken cancellationToken)
    {
        var existingItems = await _dbContext.MenuItems
            .AsTracking()
            .ToDictionaryAsync(item => item.Code, cancellationToken);

        foreach (var seedItem in SeedMenuItems)
        {
            if (existingItems.TryGetValue(seedItem.Code, out var existingItem))
            {
                existingItem.Name = seedItem.Name;
                existingItem.CategoryCode = seedItem.CategoryCode;
                existingItem.CategoryDisplayName = seedItem.CategoryDisplayName;
                existingItem.Price = seedItem.Price;
                continue;
            }

            _dbContext.MenuItems.Add(new MenuItemEntity
            {
                Code = seedItem.Code,
                Name = seedItem.Name,
                CategoryCode = seedItem.CategoryCode,
                CategoryDisplayName = seedItem.CategoryDisplayName,
                Price = seedItem.Price
            });
        }

        await _dbContext.SaveChangesAsync(cancellationToken);
    }

    private async Task SeedOrdersAsync(CancellationToken cancellationToken)
    {
        var existingOrders = await _dbContext.Orders
            .AsTracking()
            .ToDictionaryAsync(order => order.Id, cancellationToken);

        foreach (var seedOrder in SeedOrders)
        {
            if (existingOrders.ContainsKey(seedOrder.Id))
            {
                continue;
            }

            _dbContext.Orders.Add(MapOrder(seedOrder));
        }

        await _dbContext.SaveChangesAsync(cancellationToken);
    }

    internal static OrderEntity MapOrder(Order order)
    {
        return new OrderEntity
        {
            Id = order.Id,
            CreatedAtUtc = order.CreatedAtUtc,
            UpdatedAtUtc = order.UpdatedAtUtc,
            SandwichItemCode = order.Sandwich?.ItemCode,
            SideItemCode = order.Side?.ItemCode,
            DrinkItemCode = order.Drink?.ItemCode
        };
    }
}
