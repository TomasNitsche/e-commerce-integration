using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
namespace ECommerce.Infrastructure;
public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructureServices(this IServiceCollection services, IConfiguration configuration)
    {
        // Using InMemory database for development 
        services.AddDbContext<ApplicationDbContext>(options =>
            options.UseInMemoryDatabase("ECommerce"));
        return services;
    }
}
