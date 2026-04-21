using FluentAssertions;
using GoodHamburguer.Application;
using Microsoft.Extensions.DependencyInjection;

namespace GoodHamburguer.UnitTests;

public class FoundationSmokeTests
{
    [Fact]
    public void AddApplication_ShouldReturnTheSameServiceCollectionInstance()
    {
        var services = new ServiceCollection();

        var result = services.AddApplication();

        result.Should().BeSameAs(services);
    }
}
