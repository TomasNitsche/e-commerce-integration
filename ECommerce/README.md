# E-commerce Integration Service

Overview

This project implements an aggregation backend for a simple e-commerce scenario. The service combines product metadata, pricing and stock information from multiple sources and exposes a single REST API that returns aggregated product data.

Tech stack

- .NET (C#), latest LTS
- EF Core (InMemory used for demo)
- MediatR for application layer requests
- Swashbuckle (Swagger) for API documentation
- Polly for resilience policies (HttpClient)
- Serilog for structured logging

Quick start

1. dotnet restore
2. dotnet build
3. dotnet run --project ECommerce.API

API / Swagger

- Swagger UI is available at /swagger. Locally the UI is typically at:
  - https://localhost:7255/swagger/index.html

Implemented features

- REST endpoint GET /api/products/{sku} returning aggregated product data:
  - product metadata (name, imageUrl)
  - price (amount, currency)
  - availability/stock (quantity, available boolean)
- Simulated downstream services (included in API for local testing):
  - /simulator/price — configurable latency (default 500–800 ms) and error rate
  - /simulator/stock — configurable latency (default 50–150 ms) and error rate
- Orchestration:
  - ProductService orchestrates calls to downstream clients (price & stock) in parallel using Task.WhenAll
  - Uses Polly policies on HttpClient for retry and circuit-breaker
  - Fallback: if downstream calls fail, reads seeded values from local DB (in-memory) and returns partial results
  - Caching: successful downstream responses are stored in IMemoryCache for short TTL

Why this orchestration approach

- Parallel calls reduce end-to-end latency by waiting only for the slowest downstream.
- Using clients + HttpClient ensures we can replace simulators with real services later without changing orchestration logic.
- Polly policies (retry + circuit-breaker) help tolerate transient failures while preventing overload on failing services.

Trade-offs

- Availability over strict consistency: the service prefers returning partial data (fallback to cached or seeded values) over failing the whole request.
- Simplicity vs realism: the demo uses seeded DB values and built-in simulators to demonstrate behavior; production would use real downstream services and persistent storage.

What would change under 10× load

- Move cache to a distributed store (Redis) to share cached values across instances
- Scale API horizontally behind a load balancer and use autoscaling rules
- Offload non-critical work to background processors (message queue) to reduce request latency
- Harden resilience policies and add circuit-breaker metrics + alerting
- Add more aggressive rate limiting and API gateway protections (WAF)

Intentional simplifications

- Authentication/authorization (OAuth) is design-only and not implemented
- OpenTelemetry/metric export is not fully configured (Serilog is present for logs)
- Simulators are embedded into the API project for ease of local testing rather than separate services

Failure scenarios and handling

1) Pricing service high latency and intermittent failures
   - Symptoms: calls to /simulator/price time out or return 500
   - Handling: HttpClient configured with Polly retry; circuit-breaker opens after repeated failures; fallback reads price from local seeded DB and returns partial response; alerting/monitoring would raise incidents.

2) Circuit-breaker open for pricing service
   - Symptoms: many recent failures cause the breaker to open and short-circuit calls
   - Handling: API continues to return product metadata + stock if available; price field is null or substituted with cached/seeded value; user-facing UI can show "price unavailable" and offer retry or notification options.

3) Downstream inconsistency (price and stock return conflicting data)
   - Handling: service returns current best-effort aggregation and logs discrepancies. In production we would implement reconciliation processes and event-driven updates.

## Agents folder (design / implementation / testing templates)

This repository contains an `agents/` folder with role descriptions and templates to coordinate feature work:

- `agents/ARCHITECT.md` — Architect role and design template (high-level design, constraints, acceptance criteria).
- `agents/DEVELOPER.md` — Developer guidelines and PR checklist.
- `agents/TESTER.md` — Tester responsibilities and test-plan template.
- `agents/FEATURE_TEMPLATE.md` — Copyable issue template to start new feature requests.

Quick workflow
1. Create a new issue with the `feature` label and paste `agents/FEATURE_TEMPLATE.md` into the issue body.
2. Architect fills the design and acceptance criteria (see `agents/ARCHITECT.md`) and requests review.
3. After approval, Developer implements the feature following `agents/DEVELOPER.md` and opens a PR.
4. Tester prepares a test plan (see `agents/TESTER.md`) and validates the PR before merge.

Keeping templates up-to-date
- To change templates, edit the files in `agents/` and open a PR describing the change.
