using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using ECommerce.Domain.Models;

namespace ECommerce.Infrastructure.Data;

public static class DataSeeder
{
    public static async Task SeedAsync(IServiceProvider services)
    {
        using var scope = services.CreateScope();
        var db = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

        // Ensure database exists (for InMemory provider this initializes the store)
        await db.Database.EnsureCreatedAsync();

        if (!await db.Products.AnyAsync())
        {
            db.Products.AddRange(
                new Product { Sku = "SKU1", Name = "Sample Product 1", ImageUrl = "https://some.test.placeholder.com/1" },
                new Product { Sku = "SKU2", Name = "Sample Product 2", ImageUrl = "https://some.test.placeholder.com/2" }
            );
        }

        if (!await db.Prices.AnyAsync())
        {
            db.Prices.AddRange(
                new Price { ProductSku = "SKU1", Amount = 19.99m, Currency = "EUR", LastUpdated = DateTime.UtcNow },
                new Price { ProductSku = "SKU2", Amount = 29.50m, Currency = "EUR", LastUpdated = DateTime.UtcNow }
            );
        }

        if (!await db.Stocks.AnyAsync())
        {
            db.Stocks.AddRange(
                new Stock { ProductSku = "SKU1", Quantity = 5 },
                new Stock { ProductSku = "SKU2", Quantity = 0 }
            );
        }

        await db.SaveChangesAsync();
    }
}

