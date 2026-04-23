using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using GoodHamburguer.Application.Menu.Services;
using GoodHamburguer.Application.OperationalLogs.Services;
using GoodHamburguer.Application.Orders.Services;
using GoodHamburguer.Application.Orders.Validation;

namespace GoodHamburguer.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddValidatorsFromAssemblyContaining<CreateOrderRequestValidator>();
        services.AddScoped<IMenuAppService, MenuAppService>();
        services.AddScoped<IOrderAppService, OrderAppService>();
        services.AddScoped<IOrderDraftingService, OrderDraftingService>();
        services.AddScoped<IOperationalLogService, OperationalLogService>();

        return services;
    }
}
