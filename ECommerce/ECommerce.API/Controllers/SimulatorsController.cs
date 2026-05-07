using Microsoft.AspNetCore.Mvc;

namespace ECommerce.API.Controllers;

[ApiController]
[Route("simulator")]
public class SimulatorsController : ControllerBase
{
    private static readonly Random _rng = new();

    [HttpGet("price")]
    public async Task<IActionResult> Price([FromQuery] string sku, [FromQuery] int minMs = 500, [FromQuery] int maxMs = 800, [FromQuery] int errorRate = 10)
    {
        var latency = _rng.Next(minMs, maxMs + 1);
        await Task.Delay(latency);

        if (_rng.Next(0, 100) < errorRate)
        {
            return StatusCode(500, new { error = "simulated price failure" });
        }

        var amount = Math.Round(5m + (decimal)(_rng.NextDouble() * 100), 2);
        var price = new
        {
            sku,
            amount,
            currency = "EUR"
        };
        return Ok(price);
    }

    [HttpGet("stock")]
    public async Task<IActionResult> Stock([FromQuery] string sku, [FromQuery] int minMs = 50, [FromQuery] int maxMs = 150, [FromQuery] int errorRate = 5)
    {
        var latency = _rng.Next(minMs, maxMs + 1);
        await Task.Delay(latency);

        if (_rng.Next(0, 100) < errorRate)
        {
            return StatusCode(500, new { error = "simulated stock failure" });
        }

        var qty = _rng.Next(0, 50);
        var stock = new
        {
            sku,
            quantity = qty
        };
        return Ok(stock);
    }
}

