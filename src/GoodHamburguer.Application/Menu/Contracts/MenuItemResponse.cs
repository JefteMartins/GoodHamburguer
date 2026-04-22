namespace GoodHamburguer.Application.Menu.Contracts;

public sealed class MenuItemResponse
{
    public required string Code { get; init; }

    public required string Name { get; init; }

    public decimal Price { get; init; }
}
