namespace ECommerce.Application.Abstraction;

public interface IStockClient
{
	Task<StockDto?> GetStockAsync(string sku, CancellationToken cancellationToken = default);
}


