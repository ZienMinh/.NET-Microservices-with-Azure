using BusinessLogicLayer.ServiceContracts;
using BusinessLogicLayer.Services;
using BusinessLogicLayer.Validators;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;

namespace BusinessLogicLayer;

public static class DependencyInjection
{
    public static IServiceCollection AddBusinessLogicLayer(this IServiceCollection services)
    {
        //TO DO: Add Business Logic Layer services into IoC container

        //Mappers
        services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

        //AddScoped
        services.AddScoped<IProductsService, ProductsService>();

        // FluentValidation
        services.AddValidatorsFromAssemblyContaining<ProductAddRequestValidator>();

        return services;
    }
}
