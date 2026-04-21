namespace GoodHamburguer.Application.Orders.Contracts;

public sealed class OrderItemInput
{
    public required string Name { get; init; }

    public decimal UnitPrice { get; init; }
}
