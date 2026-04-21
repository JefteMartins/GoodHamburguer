using GoodHamburguer.Domain.Menu;

namespace GoodHamburguer.Application.Menu.Abstractions;

public interface IMenuQueryService
{
    Task<MenuCatalog> GetMenuCatalogAsync(CancellationToken cancellationToken = default);
}
