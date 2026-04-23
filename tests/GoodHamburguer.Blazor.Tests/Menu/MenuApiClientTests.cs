using System.Net;
using System.Net.Http.Json;
using FluentAssertions;
using GoodHamburguer.Blazor.Services.Api.Menu;
using Microsoft.Extensions.DependencyInjection;

namespace GoodHamburguer.Blazor.Tests.Menu;

public class MenuApiClientTests
{
    [Fact]
    public async Task GetMenuAsync_ReturnsCategoriesFromApi()
    {
        var handler = new StubHttpMessageHandler(_ =>
        {
            var response = new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = JsonContent.Create(new[]
                {
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
                })
            };

            return Task.FromResult(response);
        });

        var services = new ServiceCollection();
        services.AddHttpClient(Program.ApiHttpClientName, client =>
            {
                client.BaseAddress = new Uri("https://api.goodhamburguer.local/api/v1/");
            })
            .ConfigurePrimaryHttpMessageHandler(() => handler);
        services.AddScoped<IMenuApiClient, MenuApiClient>();

        await using var provider = services.BuildServiceProvider().CreateAsyncScope();
        var sut = provider.ServiceProvider.GetRequiredService<IMenuApiClient>();

        var result = await sut.GetMenuAsync();

        result.Should().ContainSingle();
        result[0].DisplayName.Should().Be("Sandwiches");
        result[0].Items.Should().ContainSingle();
        result[0].Items[0].Name.Should().Be("X-Burger");
    }

    [Fact]
    public async Task GetMenuAsync_ThrowsWhenApiReturnsError()
    {
        var handler = new StubHttpMessageHandler(_ =>
            Task.FromResult(new HttpResponseMessage(HttpStatusCode.InternalServerError)));

        var services = new ServiceCollection();
        services.AddHttpClient(Program.ApiHttpClientName, client =>
            {
                client.BaseAddress = new Uri("https://api.goodhamburguer.local/api/v1/");
            })
            .ConfigurePrimaryHttpMessageHandler(() => handler);
        services.AddScoped<IMenuApiClient, MenuApiClient>();

        await using var provider = services.BuildServiceProvider().CreateAsyncScope();
        var sut = provider.ServiceProvider.GetRequiredService<IMenuApiClient>();

        var act = () => sut.GetMenuAsync();

        await act.Should().ThrowAsync<HttpRequestException>();
    }

    private sealed class StubHttpMessageHandler(Func<HttpRequestMessage, Task<HttpResponseMessage>> handler)
        : HttpMessageHandler
    {
        protected override Task<HttpResponseMessage> SendAsync(
            HttpRequestMessage request,
            CancellationToken cancellationToken) => handler(request);
    }
}
