using Bunit;
using FluentAssertions;
using GoodHamburguer.Blazor.Components.Pages;
using GoodHamburguer.Blazor.Services.Api.Menu;
using Microsoft.Extensions.DependencyInjection;

namespace GoodHamburguer.Blazor.Tests.Menu;

public class MenuPageTests : TestContext
{
    [Fact]
    public void Menu_ShowsLoadingThenRendersMenuSections()
    {
        var completionSource = new TaskCompletionSource<IReadOnlyList<MenuCategoryDto>>();
        Services.AddSingleton<IMenuApiClient>(new StubMenuApiClient(() => completionSource.Task));

        var cut = RenderComponent<GoodHamburguer.Blazor.Components.Pages.Menu>();

        cut.Markup.Should().Contain("Loading menu");

        completionSource.SetResult(
        [
            new MenuCategoryDto
            {
                Code = "sandwiches",
                DisplayName = "Sandwiches",
                Items =
                [
                    new MenuItemDto
                    {
                        Code = "x-burger",
                        Name = "X-Burger",
                        Price = 12.5m
                    }
                ]
            }
        ]);

        cut.WaitForAssertion(() =>
        {
            cut.Markup.Should().Contain("Sandwiches");
            cut.Markup.Should().Contain("X-Burger");
            cut.Markup.Should().Contain("12.50");
        });
    }

    [Fact]
    public void Menu_ShowsEmptyStateWhenNoItemsAreAvailable()
    {
        Services.AddSingleton<IMenuApiClient>(
            new StubMenuApiClient(() => Task.FromResult<IReadOnlyList<MenuCategoryDto>>([])));

        var cut = RenderComponent<GoodHamburguer.Blazor.Components.Pages.Menu>();

        cut.WaitForAssertion(() =>
            cut.Markup.Should().Contain("No menu items are available right now"));
    }

    [Fact]
    public void Menu_ShowsFriendlyErrorWhenMenuCannotBeLoaded()
    {
        Services.AddSingleton<IMenuApiClient>(
            new StubMenuApiClient(() => Task.FromException<IReadOnlyList<MenuCategoryDto>>(
                new InvalidOperationException("boom"))));

        var cut = RenderComponent<GoodHamburguer.Blazor.Components.Pages.Menu>();

        cut.WaitForAssertion(() =>
            cut.Markup.Should().Contain("We couldn't load the menu right now"));
    }

    private sealed class StubMenuApiClient(Func<Task<IReadOnlyList<MenuCategoryDto>>> getMenu)
        : IMenuApiClient
    {
        public Task<IReadOnlyList<MenuCategoryDto>> GetMenuAsync(CancellationToken cancellationToken = default) =>
            getMenu();
    }
}
