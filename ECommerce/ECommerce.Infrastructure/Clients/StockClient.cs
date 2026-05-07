using System.Net.Http.Json;
using System.Text.Json;
using Microsoft.Extensions.Logging;
using ECommerce.Application.Abstraction;

namespace ECommerce.Infrastructure.Clients;

public class StockClient(IHttpClientFactory httpFactory, ILogger<StockClient> logger)
    : IStockClient
{
    public async Task<StockDto?> GetStockAsync(string sku, CancellationToken cancellationToken = default)
    {
        var client = httpFactory.CreateClient("Downstream");
        try
        {
            var resp = await client.GetAsync($"/simulator/stock?sku={Uri.EscapeDataString(sku)}", cancellationToken);
            if (!resp.IsSuccessStatusCode) return null;
            var content = await resp.Content.ReadAsStringAsync(cancellationToken);
            var doc = JsonDocument.Parse(content).RootElement;
            var qty = doc.GetProperty("quantity").GetInt32();
            return new StockDto(qty, qty > 0);
        }
        catch (Exception ex)
        {
            logger.LogWarning(ex, "Stock client failed for {Sku}", sku);
            return null;
        }
    }
}

