using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using GoodHamburguer.Application.Menu.Abstractions;
using GoodHamburguer.Infrastructure.Menu;

namespace GoodHamburguer.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddScoped<IMenuQueryService, StaticMenuQueryService>();

        return services;
    }
}
