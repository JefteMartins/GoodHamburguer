using Microsoft.Extensions.DependencyInjection;
using GoodHamburguer.Application.Menu.Services;
using GoodHamburguer.Application.Orders.Services;

namespace GoodHamburguer.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddScoped<IMenuAppService, MenuAppService>();
        services.AddScoped<IOrderDraftingService, OrderDraftingService>();

        return services;
    }
}
