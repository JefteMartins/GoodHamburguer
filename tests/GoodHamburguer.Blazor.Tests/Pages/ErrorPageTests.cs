using System.Diagnostics;
using Bunit;
using FluentAssertions;

namespace GoodHamburguer.Blazor.Tests.Pages;

public class ErrorPageTests : TestContext
{
    [Fact]
    public void ErrorPage_HidesRequestId_WhenUnavailable()
    {
        var cut = RenderComponent<GoodHamburguer.Blazor.Components.Pages.Error>();

        cut.Markup.Should().Contain("An error occurred while processing your request.");
        cut.Markup.Should().NotContain("Request ID:");
    }

    [Fact]
    public void ErrorPage_ShowsRequestId_WhenActivityIsAvailable()
    {
        using var activity = new Activity("error-test");
        activity.Start();

        var cut = RenderComponent<GoodHamburguer.Blazor.Components.Pages.Error>();

        cut.Markup.Should().Contain("Request ID:");
        cut.Markup.Should().Contain(activity.Id);

        activity.Stop();
    }
}
