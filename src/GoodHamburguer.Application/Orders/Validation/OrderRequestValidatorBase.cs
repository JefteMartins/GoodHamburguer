using FluentValidation;
using GoodHamburguer.Application.Menu.Abstractions;
using GoodHamburguer.Domain.Menu;

namespace GoodHamburguer.Application.Orders.Validation;

public abstract class OrderRequestValidatorBase<T> : AbstractValidator<T>
{
    protected OrderRequestValidatorBase(IMenuQueryService menuQueryService)
    {
        RuleFor(request => request)
            .CustomAsync(async (request, context, cancellationToken) =>
            {
                var menuCatalog = await menuQueryService.GetMenuCatalogAsync(cancellationToken);

                ValidateSlot(GetSandwichItemCode(request), MenuCategory.Sandwiches, "sandwichItemCode", menuCatalog, context);
                ValidateSlot(GetSideItemCode(request), MenuCategory.Sides, "sideItemCode", menuCatalog, context);
                ValidateSlot(GetDrinkItemCode(request), MenuCategory.Drinks, "drinkItemCode", menuCatalog, context);
            });
    }

    protected abstract string? GetSandwichItemCode(T request);

    protected abstract string? GetSideItemCode(T request);

    protected abstract string? GetDrinkItemCode(T request);

    private static void ValidateSlot(
        string? itemCode,
        MenuCategory expectedCategory,
        string propertyName,
        MenuCatalog menuCatalog,
        ValidationContext<T> context)
    {
        if (itemCode is null)
        {
            return;
        }

        if (string.IsNullOrWhiteSpace(itemCode))
        {
            context.AddFailure(propertyName, "The item code cannot be empty.");
            return;
        }

        var menuItem = menuCatalog.FindByCode(itemCode);
        if (menuItem is null)
        {
            context.AddFailure(propertyName, $"The item code '{itemCode}' was not found in the menu.");
            return;
        }

        if (!menuItem.Category.Equals(expectedCategory))
        {
            context.AddFailure(
                propertyName,
                $"The item code '{itemCode}' does not belong to category '{expectedCategory.Code}'.");
        }
    }
}
