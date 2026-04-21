using GoodHamburguer.Application.Menu.Abstractions;
using GoodHamburguer.Domain.Menu;

namespace GoodHamburguer.Infrastructure.Menu;

internal sealed class StaticMenuQueryService : IMenuQueryService
{
    public Task<MenuCatalog> GetMenuCatalogAsync(CancellationToken cancellationToken = default)
    {
        MenuItem[] items =
        [
            new("X Burger", MenuCategory.Sandwiches, 5.00m),
            new("X Egg", MenuCategory.Sandwiches, 4.50m),
            new("X Bacon", MenuCategory.Sandwiches, 7.00m),
            new("Batata frita", MenuCategory.Sides, 2.00m),
            new("Refrigerante", MenuCategory.Drinks, 2.50m)
        ];

        return Task.FromResult(new MenuCatalog(items));
    }
}
