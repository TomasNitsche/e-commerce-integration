namespace ECommerce.Application.Abstraction;

public interface IPriceClient
{
    Task<PriceDto?> GetPriceAsync(string sku, CancellationToken cancellationToken = default);
}
