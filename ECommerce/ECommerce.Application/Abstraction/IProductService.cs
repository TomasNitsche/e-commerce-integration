using System;

namespace ECommerce.Application.Abstraction;

public record PriceDto(decimal Amount, string Currency);
public record StockDto(int Quantity, bool Available);
public record ProductDto(string Sku, string Name, string? ImageUrl, PriceDto? Price, StockDto? Stock);

public interface IProductService
{
    Task<ProductDto?> GetProductAsync(string sku, CancellationToken cancellationToken = default);
    Task AddProductAsync(string sku, string name, string? imageUrl, CancellationToken cancellationToken = default);
}