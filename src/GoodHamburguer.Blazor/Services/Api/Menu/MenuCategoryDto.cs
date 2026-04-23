namespace GoodHamburguer.Blazor.Services.Api.Menu;

public sealed class MenuCategoryDto
{
    public required string Code { get; init; }

    public required string DisplayName { get; init; }

    public required IReadOnlyList<MenuItemDto> Items { get; init; }
}
