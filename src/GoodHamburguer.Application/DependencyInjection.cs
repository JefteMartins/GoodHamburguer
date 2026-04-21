using Microsoft.Extensions.DependencyInjection;

namespace GoodHamburguer.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        return services;
    }
}
