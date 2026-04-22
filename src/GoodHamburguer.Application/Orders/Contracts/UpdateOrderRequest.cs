namespace GoodHamburguer.Application.Orders.Contracts;

public sealed class UpdateOrderRequest
{
    public string? SandwichItemCode { get; init; }

    public string? SideItemCode { get; init; }

    public string? DrinkItemCode { get; init; }
}
