using BusinessLogicLayer;
using BusinessLogicLayer.HttpClients;
using BusinessLogicLayer.Policies;
using DataAcessLayer;
using FluentValidation.AspNetCore;
using OrdersMicroservice.API.Middlewares;

var builder = WebApplication.CreateBuilder(args);

// Add DAL and BLL services
builder.Services.AddDataAccessLayer(builder.Configuration);
builder.Services.AddBusinessLogicLayer(builder.Configuration);

builder.Services.AddControllers();

//FluentValidations
builder.Services.AddFluentValidationAutoValidation();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

//Add Cors
builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(builder =>
    {
        builder.WithOrigins("http://localhost:4200")
            .AllowAnyMethod()
            .AllowAnyHeader();
    });
});

builder.Services.AddTransient<IUsersMicroservicePolicies, UsersMicroservicePolicies>();
builder.Services.AddTransient<IProductsMicroservicePolicies, ProductsMicroservicePolicies>();

// UsersMicroservice HTTPClients and Fault Tolerance Configuration
builder.Services.AddHttpClient<UsersMicroserviceClient>(client =>
    {
        client.BaseAddress = new Uri($"http://" +
                                     $"{builder.Configuration["UsersMicroserviceName"]}:" +
                                     $"{builder.Configuration["UsersMicroservicePort"]}");
    })
    .AddPolicyHandler(
        builder.Services.BuildServiceProvider()
            .GetRequiredService<IUsersMicroservicePolicies>()
            .GetRetryPolicy())
    .AddPolicyHandler(
        builder.Services.BuildServiceProvider()
            .GetRequiredService<IUsersMicroservicePolicies>()
            .GetCircuitBreakerPolicy()
    )
    .AddPolicyHandler(
        builder.Services.BuildServiceProvider()
            .GetRequiredService<IUsersMicroservicePolicies>()
            .GetTimeoutPolicy()
    );

// ProductsMicroservice HTTPClients Configuration
builder.Services.AddHttpClient<ProductsMicroserviceClient>(client =>
    {
        client.BaseAddress = new Uri($"http://" +
                                     $"{builder.Configuration["ProductsMicroserviceName"]}:" +
                                     $"{builder.Configuration["ProductsMicroservicePort"]}");
    })
    .AddPolicyHandler(
        builder.Services.BuildServiceProvider()
            .GetRequiredService<IProductsMicroservicePolicies>()
            .GetFallbackPolicy()
    )
    .AddPolicyHandler(
        builder.Services.BuildServiceProvider()
            .GetRequiredService<IProductsMicroservicePolicies>()
            .GetBulkheadIsolationPolicy()
    );

var app = builder.Build();

app.UseExceptionHandlingMiddleware();

app.UseRouting();

//Cors
app.UseCors();

app.UseSwagger(); //Adds endpoint that can serve the swagger.json
app.UseSwaggerUI(); //Adds swagger UI (interactive page to explore and test API endpoints)

//Auth
app.UseHttpsRedirection();
app.UseAuthorization();
app.UseAuthentication();

app.MapControllers();

app.Run();