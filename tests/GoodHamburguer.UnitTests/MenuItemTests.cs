using FluentAssertions;
using GoodHamburguer.Domain.Menu;

namespace GoodHamburguer.UnitTests;

public class MenuItemTests
{
    [Fact]
    public void Constructor_ShouldTrimCodeAndName()
    {
        var item = new MenuItem("  sandwich-x-burger  ", "  X Burger  ", MenuCategory.Sandwiches, 5.00m);

        item.Code.Should().Be("sandwich-x-burger");
        item.Name.Should().Be("X Burger");
        item.Category.Should().Be(MenuCategory.Sandwiches);
        item.Price.Should().Be(5.00m);
    }

    [Fact]
    public void Constructor_ShouldThrow_WhenCodeIsEmpty()
    {
        var act = () => new MenuItem("   ", "X Burger", MenuCategory.Sandwiches, 5.00m);

        act.Should().Throw<ArgumentException>()
            .WithParameterName("code");
    }

    [Fact]
    public void Constructor_ShouldThrow_WhenNameIsEmpty()
    {
        var act = () => new MenuItem("sandwich-x-burger", "   ", MenuCategory.Sandwiches, 5.00m);

        act.Should().Throw<ArgumentException>()
            .WithParameterName("name");
    }

    [Fact]
    public void Constructor_ShouldThrow_WhenCategoryIsNull()
    {
        var act = () => new MenuItem("sandwich-x-burger", "X Burger", null!, 5.00m);

        act.Should().Throw<ArgumentNullException>()
            .WithParameterName("category");
    }

    [Fact]
    public void Constructor_ShouldThrow_WhenPriceIsNegative()
    {
        var act = () => new MenuItem("sandwich-x-burger", "X Burger", MenuCategory.Sandwiches, -0.01m);

        act.Should().Throw<ArgumentOutOfRangeException>()
            .WithParameterName("price");
    }

    [Fact]
    public void Constructor_ShouldAllowZeroPrice()
    {
        var item = new MenuItem("promo-water", "Promo Water", MenuCategory.Drinks, 0m);

        item.Price.Should().Be(0m);
    }
}
