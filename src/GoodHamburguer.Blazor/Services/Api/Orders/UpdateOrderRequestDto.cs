namespace GoodHamburguer.Blazor.Services.Api.Orders;

public sealed class UpdateOrderRequestDto
{
    public string? SandwichItemCode { get; init; }

    public string? SideItemCode { get; init; }

    public string? DrinkItemCode { get; init; }
}
