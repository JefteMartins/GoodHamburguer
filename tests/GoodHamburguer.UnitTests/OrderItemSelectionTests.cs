using FluentAssertions;
using GoodHamburguer.Domain.Orders;

namespace GoodHamburguer.UnitTests;

public class OrderItemSelectionTests
{
    [Fact]
    public void Constructor_ShouldTrimItemCode()
    {
        var selection = new OrderItemSelection("  sandwich-x-burger  ");

        selection.ItemCode.Should().Be("sandwich-x-burger");
    }

    [Fact]
    public void Constructor_ShouldThrow_WhenItemCodeIsEmpty()
    {
        var act = () => new OrderItemSelection("   ");

        act.Should().Throw<ArgumentException>()
            .WithParameterName("itemCode");
    }

    [Fact]
    public void Constructor_ShouldThrow_WhenItemCodeIsNull()
    {
        var act = () => new OrderItemSelection(null!);

        act.Should().Throw<ArgumentException>()
            .WithParameterName("itemCode");
    }
}
