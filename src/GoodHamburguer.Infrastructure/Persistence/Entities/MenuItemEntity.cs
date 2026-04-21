namespace GoodHamburguer.Infrastructure.Persistence.Entities;

public sealed class MenuItemEntity
{
    public string Code { get; set; } = string.Empty;

    public string Name { get; set; } = string.Empty;

    public string CategoryCode { get; set; } = string.Empty;

    public string CategoryDisplayName { get; set; } = string.Empty;

    public decimal Price { get; set; }
}
