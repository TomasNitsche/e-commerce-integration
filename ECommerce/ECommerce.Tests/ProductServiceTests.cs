
using System.Threading.Tasks;
using Xunit;
using Moq;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging.Abstractions;
using ECommerce.Domain.Models;

namespace ECommerce.Tests;

public class ProductServiceTests
{
	[Fact]
	public async Task GetProduct_ReturnsAggregatedDto_WhenDownStreamsOk()
	{
		var options = new DbContextOptionsBuilder<Infrastructure.ApplicationDbContext>()
			.UseInMemoryDatabase(databaseName: "testdb_ok")
			.Options;

		await using var db = new Infrastructure.ApplicationDbContext(options);
		db.Products.Add(new Product { Sku = "SKU1", Name = "Test product" });
		await db.SaveChangesAsync();

		// Seed price/stock into DB for the test
		db.Prices.Add(new Price { ProductSku = "SKU1", Amount = 12.5m, Currency = "EUR", LastUpdated = DateTime.UtcNow });
		db.Stocks.Add(new Stock { ProductSku = "SKU1", Quantity = 3 });
		await db.SaveChangesAsync();

		var cache = new MemoryCache(new MemoryCacheOptions());
		var logger = new NullLogger<Infrastructure.Services.ProductService>();

		var mockPrice = new Mock<Application.Abstraction.IPriceClient>();
		mockPrice.Setup(p => p.GetPriceAsync("SKU1", CancellationToken.None)).ReturnsAsync(new ECommerce.Application.Abstraction.PriceDto(12.5m, "EUR"));
		var mockStock = new Mock<Application.Abstraction.IStockClient>();
		mockStock.Setup(s => s.GetStockAsync("SKU1", CancellationToken.None)).ReturnsAsync(new ECommerce.Application.Abstraction.StockDto(3, true));

		var svc = new Infrastructure.Services.ProductService(db, mockPrice.Object, mockStock.Object, cache, logger);

		var result = await svc.GetProductAsync("SKU1");

		Assert.NotNull(result);
		Assert.Equal("SKU1", result!.Sku);
		Assert.NotNull(result.Price);
		Assert.NotNull(result.Stock);
		Assert.Equal(12.5m, result.Price!.Amount);
		Assert.Equal(3, result.Stock!.Quantity);
	}

	[Fact]
	public async Task GetProduct_ReturnsNull_WhenProductNotFound()
	{
		var options = new DbContextOptionsBuilder<Infrastructure.ApplicationDbContext>()
			.UseInMemoryDatabase(databaseName: "testdb_notfound")
			.Options;

		await using var db = new Infrastructure.ApplicationDbContext(options);

		var cache = new MemoryCache(new MemoryCacheOptions());
		var logger = new NullLogger<Infrastructure.Services.ProductService>();

		var mockPrice = new Mock<Application.Abstraction.IPriceClient>();
		var mockStock = new Mock<Application.Abstraction.IStockClient>();

		var svc = new Infrastructure.Services.ProductService(db, mockPrice.Object, mockStock.Object, cache, logger);

		var result = await svc.GetProductAsync("NON_EXISTENT");

		Assert.Null(result);
	}
}

