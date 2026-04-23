using FluentAssertions;
using GoodHamburguer.Application.Menu.Abstractions;
using GoodHamburguer.Application.Orders.Contracts;
using GoodHamburguer.Application.Orders.Validation;
using GoodHamburguer.Domain.Menu;

namespace GoodHamburguer.UnitTests;

public class OrderRequestValidatorTests
{
    private static readonly MenuCatalog Catalog = new(
    [
        new MenuItem("sandwich-x-burger", "X Burger", MenuCategory.Sandwiches, 5.00m),
        new MenuItem("side-fries", "Batata frita", MenuCategory.Sides, 2.00m),
        new MenuItem("drink-soft-drink", "Refrigerante", MenuCategory.Drinks, 2.50m)
    ]);

    [Fact]
    public async Task CreateValidator_ShouldRejectUnknownItemCode()
    {
        var validator = new CreateOrderRequestValidator(new StubMenuQueryService(Catalog));

        var result = await validator.ValidateAsync(new CreateOrderRequest
        {
            SandwichItemCode = "sandwich-does-not-exist"
        });

        result.IsValid.Should().BeFalse();
        result.Errors.Should().ContainSingle(error => error.PropertyName == "sandwichItemCode");
    }

    [Fact]
    public async Task UpdateValidator_ShouldRejectItemCodeInWrongCategory()
    {
        var validator = new UpdateOrderRequestValidator(new StubMenuQueryService(Catalog));

        var result = await validator.ValidateAsync(new UpdateOrderRequest
        {
            SideItemCode = "sandwich-x-burger"
        });

        result.IsValid.Should().BeFalse();
        result.Errors.Should().ContainSingle(error => error.PropertyName == "sideItemCode");
    }

    [Fact]
    public async Task CreateValidator_ShouldRejectDuplicatedSelectionsAcrossCategories()
    {
        var validator = new CreateOrderRequestValidator(new StubMenuQueryService(Catalog));

        var result = await validator.ValidateAsync(new CreateOrderRequest
        {
            SandwichItemCode = "sandwich-x-burger",
            SideItemCode = "sandwich-x-burger"
        });

        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(error => error.ErrorMessage.Contains("repeat", StringComparison.OrdinalIgnoreCase));
    }

    private sealed class StubMenuQueryService(MenuCatalog catalog) : IMenuQueryService
    {
        public Task<MenuCatalog> GetMenuCatalogAsync(CancellationToken cancellationToken = default)
        {
            return Task.FromResult(catalog);
        }
    }
}
