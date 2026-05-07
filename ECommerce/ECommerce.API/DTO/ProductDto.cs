namespace ECommerce.API.DTO;

public record ProductDto(
	string Sku,
	string Name,
	string? ImageUrl,
	PriceDto? Price,
	StockDto? Stock);
