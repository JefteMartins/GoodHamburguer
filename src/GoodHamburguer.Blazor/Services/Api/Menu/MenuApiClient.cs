using System.Net.Http.Json;

namespace GoodHamburguer.Blazor.Services.Api.Menu;

public sealed class MenuApiClient(IHttpClientFactory httpClientFactory) : IMenuApiClient
{
    public async Task<IReadOnlyList<MenuCategoryDto>> GetMenuAsync(CancellationToken cancellationToken = default)
    {
        var client = httpClientFactory.CreateClient(Program.ApiHttpClientName);
        using var response = await client.GetAsync("menu", cancellationToken);
        response.EnsureSuccessStatusCode();

        var categories = await response.Content.ReadFromJsonAsync<IReadOnlyList<MenuCategoryDto>>(
            cancellationToken: cancellationToken);

        return categories ?? [];
    }
}
