namespace GoodHamburguer.Blazor.Services.Api.Orders;

public sealed class OrderSummaryDto
{
    public required Guid Id { get; init; }

    public string? SandwichItemCode { get; init; }

    public string? SideItemCode { get; init; }

    public string? DrinkItemCode { get; init; }

    public decimal Subtotal { get; init; }

    public decimal Discount { get; init; }

    public decimal Total { get; init; }

    public DateTimeOffset CreatedAtUtc { get; init; }

    public DateTimeOffset UpdatedAtUtc { get; init; }
}
