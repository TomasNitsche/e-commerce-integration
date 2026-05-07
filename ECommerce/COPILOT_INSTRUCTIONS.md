# Copilot / Developer instruction context

Project structure
- `ECommerce.API` — ASP.NET controllers, simulators, Swagger
- `ECommerce.Application` — application interfaces and DTOs
- `ECommerce.Domain` — domain models (Product, Price, Stock)
- `ECommerce.Infrastructure` — EF DbContext, HTTP clients, ProductService, DI
- `ECommerce.Tests` — unit tests

Key design decisions
- Parallel downstream calls (Task.WhenAll) to minimize latency.
- Resilience: Polly (retry + circuit breaker) registered on HttpClient.
- Fallback: IMemoryCache for last-known-good values when downstreams fail.

Where to find important files
- `ECommerce.API/Controllers/ProductsController.cs` — aggregated product endpoint
- `ECommerce.API/Controllers/SimulatorsController.cs` — price/stock simulators
- `ECommerce.API/Mapping/ProductMapping.cs` — static mapping for DTOs
- `ECommerce.Infrastructure/DependencyInjection.cs` — DI and HttpClient registration
- `ECommerce.Infrastructure/Clients/*` — PriceClient, StockClient
- `ECommerce.Infrastructure/Services/ProductService.cs` — orchestrator combining downstream responses

Development rules and guidelines
- Controllers depend on application interfaces from `ECommerce.Application`; implementations live in `ECommerce.Infrastructure`.
- Mapping between domain entities and API DTOs is done via static mapping classes (as requested).
- Follow SOLID and Clean Architecture: separate interfaces and implementations, keep layers decoupled.

How to add new functionality
1. Add interface into `ECommerce.Application.Abstraction`
2. Implement it in `ECommerce.Infrastructure/Services` or `ECommerce.Infrastructure/Clients`
3. Register the implementation in `ECommerce.Infrastructure/DependencyInjection.cs`
4. Add tests in `ECommerce.Tests` (xUnit)

Notes
- The simulator is part of the API to make local testing easy. In production the simulator would be a separate service.
- `README.md` contains design decisions and failure scenarios.


