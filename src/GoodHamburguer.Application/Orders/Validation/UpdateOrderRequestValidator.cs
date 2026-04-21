using GoodHamburguer.Application.Menu.Abstractions;
using GoodHamburguer.Application.Orders.Contracts;

namespace GoodHamburguer.Application.Orders.Validation;

public sealed class UpdateOrderRequestValidator : OrderRequestValidatorBase<UpdateOrderRequest>
{
    public UpdateOrderRequestValidator(IMenuQueryService menuQueryService)
        : base(menuQueryService)
    {
    }

    protected override string? GetSandwichItemCode(UpdateOrderRequest request) => request.SandwichItemCode;

    protected override string? GetSideItemCode(UpdateOrderRequest request) => request.SideItemCode;

    protected override string? GetDrinkItemCode(UpdateOrderRequest request) => request.DrinkItemCode;
}
