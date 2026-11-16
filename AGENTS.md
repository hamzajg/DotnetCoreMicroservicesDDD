# AGENTS.md — DotnetCoreMicroservicesDDD

Guidance for AI agents (and humans) working in this repository.

## Project

A .NET microservices reference project built around Domain-Driven Design:
four services (Customer, Product, Order, Identity) plus shared common
libraries, each service layered as Domain → Application → Infrastructure →
WebApi. Learning/reference project — structure is the lesson.
Stack: C#, `net10.0` (+ `netstandard2.1` for shared libs), ASP.NET Core,
Docker Compose, xUnit tests.

## Commands

Verified 2026-11-16 against CI (`.github/workflows/ci.yml`).

| Task | Command |
|---|---|
| Restore | `dotnet restore MicroservicesDDD.sln` |
| Build | `dotnet build MicroservicesDDD.sln -c Release --no-restore` |
| Test | `dotnet test tests/Tests/Tests.csproj -c Release --no-build` (31 tests) |
| Run all services | `docker compose up --build` |

Requires: .NET 10 SDK (CI pins `10.0.x`). Solution: `MicroservicesDDD.sln`.
Each service has a Dockerfile under its `WebApi` project — except Order, which
has no Dockerfile yet and is not in `docker-compose.yml`.

## Architecture

```
src/
  Common/
    Domain.Common/           Command/CommandHandler, AggregateRoot, Entity,
                             DomainEvent/Event/EventHandler, RejectedEvent
    Infrastructure.Common/   shared infra building blocks (mostly empty)
    Saga.Common/             saga orchestrator kernel: SagaStep(action+
                             compensation), SagaOrchestrator, SagaResult
                             (consumed by Order's CreateOrder use case)
  Services/
    Customer/
      Customer.Domain/       Customer, Address, PhoneNumber entities,
                             CustomerRepository (interface), domain events
      Customer.Application/  RegisterCustomer use case + command + handler,
                             InMemoryCustomerRepository, ServiceCollectionExt
      Customer.WebApi/       ASP.NET host + CustomerController
    Product/
      Product.Domain/        Product entity + domain events
      Product.Application/   CreateProduct use case (publishes ProductCreated),
                             InMemoryPublisherServices, ServiceCollectionExt
      Product.Infrastructure/ persistence/integration (mostly stub)
      Product.WebApi/        ASP.NET host + ProductsController
    Order/
      Order.Domain/          Order aggregate, OrderLine value object,
                             OrderCreated domain event
      Order.Application/     CreateOrder use case (a compensating saga over
                             Saga.Common) + ports CustomerDirectory/
                             ProductCatalog/OrderRepository with in-memory
                             adapters, InMemoryPublisherServices,
                             ServiceCollectionExt
      Order.WebApi/          ASP.NET host + OrdersController (POST /Orders)
    Identity/Identity.WebApi/  auth-ish host (WeatherForecast scaffold)
tests/Tests/                 Product.Domain, Product.App, Product.WebApi,
                             Customer.App, Order.App, Order.WebApi,
                             Saga tests
docker-compose.yml           Identity + Customer + Product APIs (no Order)
```

Layering rule per service: **Domain is framework-free** (entities,
events, repository interfaces); **Application** holds use cases/commands/
handlers; **Infrastructure** persistence; **WebApi** the host. Don't let
web concerns leak into Domain.

## Conventions

- DDD building blocks come from `Domain.Common` — reuse
  `Command`/`CommandHandler`/`AggregateRoot`/`DomainEvent` rather than
  inventing parallels (DRY across services).
- In-memory repositories are the default (`InMemoryCustomerRepository`) —
  persistence is deliberately minimal in this reference project.
- Each service has its own `Startup.cs`/`Program.cs`; composition root
  per service, no shared host.
- KISS/SOLID/DRY: the layered split IS the lesson — keep dependencies
  pointing inward (WebApi → Application → Domain).

## Landmines

- **Default branch is `master`, not `main`.**
- README's stack section previously said ".NET Core 3.0/3.1, install the 3.1
  SDK"; it was corrected to .NET 10 (2026-11-16). csproj targets are
  `net10.0`/`netstandard2.1` and CI uses `10.0.x` — trust csproj + CI.
- `Identity.WebApi` and `Product.WebApi` still contain `WeatherForecast`
  scaffold controllers — template leftovers, not real features.
  `Product.WebApi` also has the real `ProductsController` (`POST /Products`).
- `CreatePruductUseCase.cs` has a typo'd filename ("Pruduct") — it's the
  real file; don't "fix" the name without checking references.
- `tests/Tests` covers Product, Customer, Order and `Saga.Common` — Identity
  has no tests.
- Order has **no Dockerfile** and is **not in `docker-compose.yml`**; the
  Docker and compose-smoke CI jobs enumerate services explicitly, so they
  don't build it. Containerising Order is a separate change.
- Dockerfiles in the working tree may be locally modified (aspnet 3.1→10.0
  base images) — check `git status`/`git diff` before assuming clean.

## Agent rules

- Branch from `master`; name branches `feat/*`, `fix/*`, `chore/*`, `docs/*`.
- Docs-only changes still go through a PR; never commit to `master` directly.
- Verify before claiming: `dotnet build` + `dotnet test` and report the
  real result.
- Keep changes minimal and consistent with surrounding code. No drive-by
  refactors.
- Update AGENTS.md / docs/ARCHITECTURE.md when you change service layout,
  shared contracts in `Domain.Common`, or commands/routes.
