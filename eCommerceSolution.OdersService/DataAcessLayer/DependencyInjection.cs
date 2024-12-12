using DataAcessLayer.Repositories;
using DataAcessLayer.RepositoryContracts;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MongoDB.Driver;

namespace DataAcessLayer;

public static class DependencyInjection
{
    public static IServiceCollection AddDataAccessLayer(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        //TO DO: Add Data Access Layer services into IoC container
        // URI connected mongodb compass "mongodb://root:mystrongpassword123@localhost:27018/"
        string connectionString;
        var environment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");

        if (environment == "Development")
        {
            // Sử dụng connection string từ appsettings.Development.json
            connectionString = configuration.GetConnectionString("MongoDB")!;
        }
        else
        {
            // Sử dụng template và environment variables cho production
            string connectionStringTemplate = configuration.GetConnectionString("MongoDB")!;
            connectionString = connectionStringTemplate
                .Replace("$MONGO_HOST", Environment.GetEnvironmentVariable("MONGODB_HOST"))
                .Replace("$MONGO_PORT", Environment.GetEnvironmentVariable("MONGODB_PORT"));
        }
        
        services.AddSingleton<IMongoClient>(new MongoClient(connectionString));

        services.AddScoped<IMongoDatabase>(provider =>
        {
            IMongoClient client = provider.GetRequiredService<IMongoClient>();
            return client.GetDatabase("OrdersDatabase");
        });

        services.AddScoped<IOrdersRepository, OrdersRepository>();

        return services;
    }
}
