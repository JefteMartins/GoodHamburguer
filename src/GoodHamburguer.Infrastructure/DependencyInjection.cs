using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using GoodHamburguer.Application.Menu.Abstractions;
using GoodHamburguer.Application.Orders.Abstractions;
using GoodHamburguer.Infrastructure.Menu;
using GoodHamburguer.Infrastructure.Orders;
using GoodHamburguer.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace GoodHamburguer.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString("DefaultConnection")
            ?? throw new InvalidOperationException("Connection string 'DefaultConnection' was not found.");

        services.AddDbContext<GoodHamburguerDbContext>(options =>
        {
            options.UseMySql(
                connectionString,
                new MySqlServerVersion(new Version(8, 4, 0)),
                mySqlOptions =>
                {
                    mySqlOptions.MigrationsAssembly(typeof(GoodHamburguerDbContext).Assembly.FullName);
                    mySqlOptions.EnableRetryOnFailure();
                });
            options.UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking);
        });

        services.AddScoped<IMenuQueryService, PersistedMenuQueryService>();
        services.AddScoped<IOrderRepository, MySqlOrderRepository>();
        services.AddScoped<IDatabaseInitializer, GoodHamburguerDbInitializer>();

        return services;
    }
}
