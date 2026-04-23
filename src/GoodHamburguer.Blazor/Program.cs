using GoodHamburguer.Blazor.Components;
using GoodHamburguer.Blazor.Services.Api.Menu;
using GoodHamburguer.Blazor.Services.Api.Orders;
using System.Diagnostics.CodeAnalysis;

namespace GoodHamburguer.Blazor;

public static class Program
{
    public const string ApiHttpClientName = "GoodHamburguer.Api";

    [ExcludeFromCodeCoverage]
    public static void Main(string[] args)
    {
        var app = BuildApp(args);
        app.Run();
    }

    public static WebApplication BuildApp(
        string[] args,
        string? environmentName = null,
        bool mapStaticAssets = true,
        string? staticAssetsManifestPath = null)
    {
        var builder = environmentName is null
            ? WebApplication.CreateBuilder(args)
            : WebApplication.CreateBuilder(new WebApplicationOptions
            {
                Args = args,
                EnvironmentName = environmentName
            });

        builder.Services.AddRazorComponents()
            .AddInteractiveServerComponents();

        builder.Services.AddApiIntegration(builder.Configuration);
        builder.Services.AddScoped<IMenuApiClient, MenuApiClient>();
        builder.Services.AddScoped<IOrderApiClient, OrderApiClient>();

        var app = builder.Build();

        if (!app.Environment.IsDevelopment())
        {
            app.UseExceptionHandler("/Error");
        }

        app.UseStatusCodePagesWithReExecute("/not-found", createScopeForStatusCodePages: true);
        app.UseAntiforgery();

        if (mapStaticAssets)
        {
            if (string.IsNullOrWhiteSpace(staticAssetsManifestPath))
            {
                app.MapStaticAssets();
            }
            else
            {
                app.MapStaticAssets(staticAssetsManifestPath);
            }
        }

        app.MapRazorComponents<App>()
            .AddInteractiveServerRenderMode();

        return app;
    }
}
