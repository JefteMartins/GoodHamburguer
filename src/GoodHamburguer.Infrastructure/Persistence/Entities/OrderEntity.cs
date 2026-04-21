namespace GoodHamburguer.Infrastructure.Persistence.Entities;

public sealed class OrderEntity
{
    public Guid Id { get; set; }

    public DateTimeOffset CreatedAtUtc { get; set; }

    public DateTimeOffset UpdatedAtUtc { get; set; }

    public string? SandwichItemCode { get; set; }

    public string? SideItemCode { get; set; }

    public string? DrinkItemCode { get; set; }
}
