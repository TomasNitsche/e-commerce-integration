using System.Net.Http.Json;
using System.Text.Json;
using Microsoft.Extensions.Logging;
using ECommerce.Application.Abstraction;

namespace ECommerce.Infrastructure.Clients;

public class PriceClient(IHttpClientFactory httpFactory, ILogger<PriceClient> logger)
    : IPriceClient
{
    public async Task<PriceDto?> GetPriceAsync(string sku, CancellationToken cancellationToken = default)
    {
        var client = httpFactory.CreateClient("Downstream");
        try
        {
            var resp = await client.GetAsync($"/simulator/price?sku={Uri.EscapeDataString(sku)}", cancellationToken);
            if (!resp.IsSuccessStatusCode) return null;
            var content = await resp.Content.ReadAsStringAsync(cancellationToken);
            var doc = JsonDocument.Parse(content).RootElement;
            var amount = doc.GetProperty("amount").GetDecimal();
            var currency = doc.GetProperty("currency").GetString() ?? "EUR";
            return new PriceDto(amount, currency);
        }
        catch (Exception ex)
        {
            logger.LogWarning(ex, "Price client failed for {Sku}", sku);
            return null;
        }
    }
}

