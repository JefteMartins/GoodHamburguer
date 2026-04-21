namespace GoodHamburguer.Application.Orders.Contracts;

public sealed class UpdateOrderRequest
{
    public OrderItemInput? Sandwich { get; init; }

    public OrderItemInput? Side { get; init; }

    public OrderItemInput? Drink { get; init; }
}
