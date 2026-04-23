using System.Net.Http.Json;
using System.Text.Json;

namespace GoodHamburguer.Blazor.Services.Api.Menu;

public sealed class MenuApiClient(IHttpClientFactory httpClientFactory) : IMenuApiClient
{
    public async Task<IReadOnlyList<MenuCategoryDto>> GetMenuAsync(CancellationToken cancellationToken = default)
    {
        var client = httpClientFactory.CreateClient(Program.ApiHttpClientName);
        var response = await client.GetAsync(new Uri("menu", UriKind.Relative), cancellationToken);

        response.EnsureSuccessStatusCode();

        if (response.Content.Headers.ContentLength is 0)
        {
            return [];
        }

        try
        {
            var categories = await response.Content.ReadFromJsonAsync<IReadOnlyList<MenuCategoryDto>>(
                cancellationToken: cancellationToken);

            return categories ?? [];
        }
        catch (JsonException)
        {
            return [];
        }
    }
}
