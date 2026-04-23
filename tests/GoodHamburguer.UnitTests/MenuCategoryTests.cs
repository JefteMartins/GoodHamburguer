using FluentAssertions;
using GoodHamburguer.Domain.Menu;

namespace GoodHamburguer.UnitTests;

public class MenuCategoryTests
{
    [Fact]
    public void Constructor_ShouldNormalizeCodeAndDisplayName()
    {
        var category = new MenuCategory("  Sandwiches  ", "  Signature Sandwiches  ");

        category.Code.Should().Be("sandwiches");
        category.DisplayName.Should().Be("Signature Sandwiches");
    }

    [Fact]
    public void Equals_ShouldCompareCodeIgnoringCase()
    {
        var first = new MenuCategory("drinks", "Bebidas");
        var second = new MenuCategory("DRINKS", "Drinks");

        first.Equals(second).Should().BeTrue();
        first.GetHashCode().Should().Be(second.GetHashCode());
    }

    [Fact]
    public void Constructor_ShouldThrow_WhenCodeIsEmpty()
    {
        var act = () => new MenuCategory("   ", "Bebidas");

        act.Should().Throw<ArgumentException>()
            .WithParameterName("code");
    }

    [Fact]
    public void Constructor_ShouldThrow_WhenDisplayNameIsEmpty()
    {
        var act = () => new MenuCategory("drinks", "   ");

        act.Should().Throw<ArgumentException>()
            .WithParameterName("displayName");
    }

    [Fact]
    public void Equals_ShouldReturnFalse_WhenComparedWithNullOrDifferentType()
    {
        var category = new MenuCategory("sides", "Acompanhamentos");
        object boxedCategory = category;

        category.Equals((MenuCategory?)null).Should().BeFalse();
        boxedCategory.Equals(new object()).Should().BeFalse();
    }

    [Fact]
    public void PredefinedCategories_ShouldExposeExpectedCodes()
    {
        MenuCategory.Sandwiches.Code.Should().Be("sandwiches");
        MenuCategory.Sides.Code.Should().Be("sides");
        MenuCategory.Drinks.Code.Should().Be("drinks");
    }
}
