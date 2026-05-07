using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using ECommerce.Application.Abstraction;
using Polly;
using Polly.Extensions.Http;

namespace ECommerce.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructureServices(this IServiceCollection services, IConfiguration configuration)
    {
        // Using InMemory database for development
        services.AddDbContext<ApplicationDbContext>(options =>
            options.UseInMemoryDatabase("ECommerce"));

        // Register a shared HttpClient for downstream simulator calls
        var baseUrl = configuration.GetValue<string>("Downstream:BaseUrl") ?? "http://localhost:5000";
        services.AddHttpClient("Downstream", client =>
        {
            client.BaseAddress = new Uri(baseUrl);
            client.Timeout = TimeSpan.FromSeconds(5);
        })
        .AddPolicyHandler(HttpPolicyExtensions
            .HandleTransientHttpError()
            .WaitAndRetryAsync(3, retryAttempt => TimeSpan.FromMilliseconds(200 * Math.Pow(2, retryAttempt))))
        .AddTransientHttpErrorPolicy(policyBuilder => policyBuilder.CircuitBreakerAsync(5, TimeSpan.FromSeconds(30)));

        // Register clients and product service 
        services.AddScoped<IPriceClient, Clients.PriceClient>();
        services.AddScoped<IStockClient, Clients.StockClient>();
        services.AddScoped<IProductService, Services.ProductService>();
        
        return services;
    }
}
