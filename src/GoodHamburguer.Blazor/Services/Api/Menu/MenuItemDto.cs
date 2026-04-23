namespace GoodHamburguer.Blazor.Services.Api.Menu;

public sealed class MenuItemDto
{
    public required string Code { get; init; }

    public required string Name { get; init; }

    public decimal Price { get; init; }
}
