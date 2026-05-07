using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using ECommerce.Application.Abstraction;
using Microsoft.EntityFrameworkCore;

namespace ECommerce.Infrastructure.Services;

public class ProductService(
    ApplicationDbContext db,
    IPriceClient priceClient,
    IStockClient stockClient,
    IMemoryCache cache,
    ILogger<ProductService> logger)
    : IProductService
{
    public async Task<ProductDto?> GetProductAsync(string sku, CancellationToken cancellationToken = default)
    {
        var product = await db.Products.FirstOrDefaultAsync(p => p.Sku == sku, cancellationToken);
        if (product is null) return null;

        // Start external calls for price and stock in parallel
        var priceTask = priceClient.GetPriceAsync(sku, cancellationToken);
        var stockTask = stockClient.GetStockAsync(sku, cancellationToken);

        await Task.WhenAll(new Task[] { priceTask, stockTask });

        var price = priceTask.IsCompletedSuccessfully ? priceTask.Result : null;
        var stock = stockTask.IsCompletedSuccessfully ? stockTask.Result : null;

        // Fallback to DB seeded values if external calls failed
        if (price is null)
        {
            var dbPrice = await db.Prices.FirstOrDefaultAsync(p => p.ProductSku == sku, cancellationToken);
            price = dbPrice is null ? null : new PriceDto(dbPrice.Amount, dbPrice.Currency);
        }
        else
        {
            // cache the successful external price
            cache.Set("price:" + sku, price, TimeSpan.FromMinutes(5));
        }

        if (stock is null)
        {
            var dbStock = await db.Stocks.FirstOrDefaultAsync(s => s.ProductSku == sku, cancellationToken);
            stock = dbStock is null ? null : new ECommerce.Application.Abstraction.StockDto(dbStock.Quantity, dbStock.Quantity > 0);
        }
        else
        {
            cache.Set("stock:" + sku, stock, TimeSpan.FromMinutes(5));
        }

        var appPrice = price;
        var appStock = stock;

        return new ProductDto(product.Sku, product.Name, product.ImageUrl, appPrice, appStock);
    }

    public async Task AddProductAsync(string sku, string name, string? imageUrl, CancellationToken cancellationToken = default)
    {
        // check if product exists
        var existing = await db.Products.FirstOrDefaultAsync(p => p.Sku == sku, cancellationToken);
        if (existing is not null)
        {
            // update existing
            existing.Name = name;
            existing.ImageUrl = imageUrl ?? existing.ImageUrl;
            db.Products.Update(existing);
        }
        else
        {
            var product = new Domain.Models.Product { Sku = sku, Name = name, ImageUrl = imageUrl ?? string.Empty };
            await db.Products.AddAsync(product, cancellationToken);
        }

        await db.SaveChangesAsync(cancellationToken);
    }
}

