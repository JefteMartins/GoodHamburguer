namespace GoodHamburguer.Application.Orders.Contracts;

public sealed class CreateOrderRequest
{
    public OrderItemInput? Sandwich { get; init; }

    public OrderItemInput? Side { get; init; }

    public OrderItemInput? Drink { get; init; }
}
