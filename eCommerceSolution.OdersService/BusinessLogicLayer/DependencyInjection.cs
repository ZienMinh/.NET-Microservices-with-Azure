using BusinessLogicLayer.ServiceContracts;
using BusinessLogicLayer.Services;
using BusinessLogicLayer.Validators;
using FluentValidation;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace BusinessLogicLayer;

public static class DependencyInjection
{
    public static IServiceCollection AddBusinessLogicLayer(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        //TO DO: Add Business Logic Layer services into IoC container

        //Mappers
        services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

        //AddScoped
        services.AddScoped<IOrdersService, OrdersService>();

        // FluentValidation
        services.AddValidatorsFromAssemblyContaining<OrderAddRequestValidator>();

        return services;
    }
}
