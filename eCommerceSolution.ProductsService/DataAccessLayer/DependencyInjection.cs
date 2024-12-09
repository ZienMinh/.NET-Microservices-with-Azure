﻿using DataAccessLayer.Context;
using DataAccessLayer.Repositories;
using DataAccessLayer.RepositoryContracts;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace DataAccessLayer;

public static class DependencyInjection
{
    public static IServiceCollection AddDataAccessLayer(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        //TO DO: Add Data Access Layer services into IoC container
        string connectionStringTemplate = configuration.GetConnectionString("local")!;
        string connectionString = connectionStringTemplate
            .Replace("$MYSQL_HOST", Environment.GetEnvironmentVariable("MYSQL_HOST"))
            .Replace("$MYSQL_PASSWORD", Environment.GetEnvironmentVariable("MYSQL_PASSWORD"))
            .Replace("$MYSQL_DATABASE", Environment.GetEnvironmentVariable("MYSQL_DATABASE"))
            .Replace("$MYSQL_PORT", Environment.GetEnvironmentVariable("MYSQL_PORT"))
            .Replace("$MYSQL_USER", Environment.GetEnvironmentVariable("MYSQL_USER"));

        services.AddDbContext<ApplicationDbContext>(options => {
            options.UseMySQL(connectionString);
        });
        
        services.AddScoped<IProductsRepository, ProductsRepository>();

        return services;
    }
}
