namespace GoodHamburguer.Application.Menu.Contracts;

public sealed class MenuCategoryResponse
{
    public required string Code { get; init; }

    public required string DisplayName { get; init; }

    public required IReadOnlyCollection<MenuItemResponse> Items { get; init; }
}
