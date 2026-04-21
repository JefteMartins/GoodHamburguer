namespace GoodHamburguer.Infrastructure.Persistence.Entities;

public sealed class OrderEntity
{
    public Guid Id { get; set; }

    public DateTimeOffset CreatedAtUtc { get; set; }

    public DateTimeOffset UpdatedAtUtc { get; set; }

    public string? SandwichName { get; set; }

    public decimal? SandwichUnitPrice { get; set; }

    public string? SideName { get; set; }

    public decimal? SideUnitPrice { get; set; }

    public string? DrinkName { get; set; }

    public decimal? DrinkUnitPrice { get; set; }
}
