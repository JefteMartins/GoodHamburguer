using Microsoft.Extensions.DependencyInjection;
using GoodHamburguer.Application.Menu.Services;

namespace GoodHamburguer.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddScoped<IMenuAppService, MenuAppService>();

        return services;
    }
}
