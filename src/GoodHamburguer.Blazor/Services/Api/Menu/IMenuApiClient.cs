namespace GoodHamburguer.Blazor.Services.Api.Menu;

public interface IMenuApiClient
{
    Task<IReadOnlyList<MenuCategoryDto>> GetMenuAsync(CancellationToken cancellationToken = default);
}
