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

                var sandwichItemCode = GetSandwichItemCode(request);
                var sideItemCode = GetSideItemCode(request);
                var drinkItemCode = GetDrinkItemCode(request);

                ValidateSlot(sandwichItemCode, MenuCategory.Sandwiches, "sandwichItemCode", menuCatalog, context);
                ValidateSlot(sideItemCode, MenuCategory.Sides, "sideItemCode", menuCatalog, context);
                ValidateSlot(drinkItemCode, MenuCategory.Drinks, "drinkItemCode", menuCatalog, context);
                ValidateDistinctSelections(sandwichItemCode, sideItemCode, drinkItemCode, context);
            });
    }

    protected abstract string? GetSandwichItemCode(T request);

    protected abstract string? GetSideItemCode(T request);

    protected abstract string? GetDrinkItemCode(T request);

    private static void ValidateDistinctSelections(
        string? sandwichItemCode,
        string? sideItemCode,
        string? drinkItemCode,
        ValidationContext<T> context)
    {
        var selections = new[] { sandwichItemCode, sideItemCode, drinkItemCode }
            .Where(itemCode => !string.IsNullOrWhiteSpace(itemCode))
            .Select(itemCode => itemCode!.Trim())
            .ToArray();

        if (selections.Distinct(StringComparer.OrdinalIgnoreCase).Count() != selections.Length)
        {
            context.AddFailure("Order selections cannot repeat the same menu item across categories.");
        }
    }

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
