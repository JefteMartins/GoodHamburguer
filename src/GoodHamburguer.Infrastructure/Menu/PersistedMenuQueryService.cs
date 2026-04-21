using GoodHamburguer.Application.Menu.Abstractions;
using GoodHamburguer.Domain.Menu;
using GoodHamburguer.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace GoodHamburguer.Infrastructure.Menu;

public sealed class PersistedMenuQueryService : IMenuQueryService
{
    private readonly GoodHamburguerDbContext _dbContext;

    public PersistedMenuQueryService(GoodHamburguerDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<MenuCatalog> GetMenuCatalogAsync(CancellationToken cancellationToken = default)
    {
        var items = await _dbContext.MenuItems
            .AsNoTracking()
            .OrderBy(menuItem => menuItem.CategoryCode)
            .ThenBy(menuItem => menuItem.Name)
            .Select(menuItem => new MenuItem(
                menuItem.Name,
                new MenuCategory(menuItem.CategoryCode, menuItem.CategoryDisplayName),
                menuItem.Price))
            .ToArrayAsync(cancellationToken);

        return new MenuCatalog(items);
    }
}
