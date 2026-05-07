using ECommerce.Infrastructure;
using ECommerce.Application;
using ECommerce.API.Middleware;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddControllers();
builder.Services.AddMemoryCache();

// Register project services
builder.Services.AddInfrastructureServices(builder.Configuration);
builder.Services.AddApplicationServices();

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "ECommerce API V1");
    c.RoutePrefix = "swagger";
});

app.UseMiddleware<CorrelationIdMiddleware>();
app.UseHttpsRedirection();
app.MapControllers();

// Ensure database is created and seed sample data
await ECommerce.Infrastructure.Data.DataSeeder.SeedAsync(app.Services);

app.Run();
