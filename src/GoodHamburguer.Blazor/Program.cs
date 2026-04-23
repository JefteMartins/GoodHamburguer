using GoodHamburguer.Blazor.Components;
using GoodHamburguer.Blazor.Services.Api.Menu;
using GoodHamburguer.Blazor.Services.Api.Orders;

namespace GoodHamburguer.Blazor;

public class Program
{
    public const string ApiHttpClientName = "GoodHamburguer.Api";

    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

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

        app.MapStaticAssets();
        app.MapRazorComponents<App>()
            .AddInteractiveServerRenderMode();

        app.Run();
    }
}
