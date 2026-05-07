using ECommerce.API.DTO;
using ECommerce.Domain.Models;

namespace ECommerce.API.Mapping;

public static class ProductMapping
{
    public static ProductDto MapToDto(Product product, Price? price, Stock? stock)
    {
        return new ProductDto(
            Sku: product.Sku,
            Name: product.Name,
            ImageUrl: product.ImageUrl,
            Price: price is null ? null : new PriceDto(price.Amount, price.Currency),
            Stock: stock is null ? null : new StockDto(stock.Quantity, stock.Quantity > 0));
    }

    public static ProductDto MapToDto(Application.Abstraction.ProductDto? appDto)
    {
        if (appDto is null) return null!; 

        var price = appDto.Price is null ? null : new PriceDto(appDto.Price.Amount, appDto.Price.Currency);
        var stock = appDto.Stock is null ? null : new StockDto(appDto.Stock.Quantity, appDto.Stock.Available);

        return new ProductDto(appDto.Sku, appDto.Name, appDto.ImageUrl, price, stock);
    }
}

