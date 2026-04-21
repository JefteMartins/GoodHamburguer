using FluentAssertions;
using System.Reflection;

namespace GoodHamburguer.IntegrationTests;

public class FoundationSmokeTests
{
    [Fact]
    public void ApiAssembly_ShouldExposeTheFoundationController()
    {
        var assembly = typeof(GoodHamburguer.Api.Program).Assembly;

        assembly.GetType("GoodHamburguer.Api.Controllers.SystemController")
            .Should()
            .NotBeNull();
    }
}
