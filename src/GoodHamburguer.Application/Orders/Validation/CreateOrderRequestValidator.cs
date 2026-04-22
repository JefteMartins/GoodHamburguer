using GoodHamburguer.Application.Menu.Abstractions;
using GoodHamburguer.Application.Orders.Contracts;

namespace GoodHamburguer.Application.Orders.Validation;

public sealed class CreateOrderRequestValidator : OrderRequestValidatorBase<CreateOrderRequest>
{
    public CreateOrderRequestValidator(IMenuQueryService menuQueryService)
        : base(menuQueryService)
    {
    }

    protected override string? GetSandwichItemCode(CreateOrderRequest request) => request.SandwichItemCode;

    protected override string? GetSideItemCode(CreateOrderRequest request) => request.SideItemCode;

    protected override string? GetDrinkItemCode(CreateOrderRequest request) => request.DrinkItemCode;
}
