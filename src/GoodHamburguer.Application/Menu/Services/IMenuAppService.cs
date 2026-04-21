using GoodHamburguer.Application.Menu.Contracts;

namespace GoodHamburguer.Application.Menu.Services;

public interface IMenuAppService
{
    Task<IReadOnlyCollection<MenuCategoryResponse>> GetMenuAsync(CancellationToken cancellationToken = default);
}
