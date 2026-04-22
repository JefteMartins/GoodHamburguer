namespace GoodHamburguer.Application.Orders.Contracts;

public sealed class OrderResponse
{
    public required Guid Id { get; init; }

    public string? SandwichItemCode { get; init; }

    public string? SideItemCode { get; init; }

    public string? DrinkItemCode { get; init; }

    public required decimal Subtotal { get; init; }

    public required decimal Discount { get; init; }

    public required decimal Total { get; init; }

    public required DateTimeOffset CreatedAtUtc { get; init; }

    public required DateTimeOffset UpdatedAtUtc { get; init; }
}
