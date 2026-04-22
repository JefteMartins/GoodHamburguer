using GoodHamburguer.Application.Menu.Abstractions;
using GoodHamburguer.Application.Menu.Contracts;

namespace GoodHamburguer.Application.Menu.Services;

public sealed class MenuAppService(IMenuQueryService menuQueryService) : IMenuAppService
{
    public async Task<IReadOnlyCollection<MenuCategoryResponse>> GetMenuAsync(CancellationToken cancellationToken = default)
    {
        var catalog = await menuQueryService.GetMenuCatalogAsync(cancellationToken);

        return catalog.GroupByCategory()
            .Select(group => new MenuCategoryResponse
            {
                Code = group.Category.Code,
                DisplayName = group.Category.DisplayName,
                Items = group
                    .Items
                    .Select(item => new MenuItemResponse
                    {
                        Code = item.Code,
                        Name = item.Name,
                        Price = item.Price
                    })
                    .ToArray()
            })
            .ToArray();
    }
}
