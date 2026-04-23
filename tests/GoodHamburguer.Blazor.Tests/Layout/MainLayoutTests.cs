using Bunit;
using FluentAssertions;
using Microsoft.AspNetCore.Components;

namespace GoodHamburguer.Blazor.Tests.Layout;

public class MainLayoutTests : TestContext
{
    [Fact]
    public void MainLayout_RendersTopBarAndBodyContent()
    {
        var cut = RenderComponent<GoodHamburguer.Blazor.Components.Layout.MainLayout>(parameters =>
            parameters.Add(layout => layout.Body, (RenderFragment)(builder => builder.AddContent(0, "Body content"))));

        cut.Markup.Should().Contain("GoodHamburguer Operations");
        cut.Markup.Should().Contain("API v1 online");
        cut.Markup.Should().Contain("Body content");
    }
}
