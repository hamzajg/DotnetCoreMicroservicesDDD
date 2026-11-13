# AGENTS.md — DotnetCoreMicroservicesDDD

Guidance for AI agents (and humans) working in this repository.

## Project

A .NET microservices reference project built around Domain-Driven Design:
three services (Customer, Product, Identity) plus shared common libraries,
each service layered as Domain → Application → Infrastructure → WebApi.
Learning/reference project — structure is the lesson.
Stack: C#, `net10.0` (+ `netstandard2.1` for shared libs), ASP.NET Core,
Docker Compose, xUnit tests.

## Commands

Verified 2026-11-13 against CI (`.github/workflows/ci.yml`).

| Task | Command |
|---|---|
| Restore | `dotnet restore MicroservicesDDD.sln` |
| Build | `dotnet build MicroservicesDDD.sln -c Release --no-restore` |
| Test | `dotnet test tests/Tests/Tests.csproj -c Release --no-build` (12 tests) |
| Run all services | `docker compose up --build` |

Requires: .NET 10 SDK (CI pins `10.0.x`). Solution: `MicroservicesDDD.sln`.
Each service has a Dockerfile under its `WebApi` project.

## Architecture

```
src/
  Common/
    Domain.Common/           Command/CommandHandler, AggregateRoot, Entity,
                             DomainEvent/Event/EventHandler, RejectedEvent
    Infrastructure.Common/   shared infra building blocks (mostly empty)
  Services/
    Customer/
      Customer.Domain/       Customer, Address, PhoneNumber entities,
                             CustomerRepository (interface), domain events
      Customer.Application/  RegisterCustomer use case + command + handler,
                             InMemoryCustomerRepository, ServiceCollectionExt
      Customer.WebApi/       ASP.NET host + CustomerController
    Product/
      Product.Domain/        Product entity + domain events
      Product.Application/   CreateProduct use case/command
      Product.Infrastructure/ persistence/integration (mostly stub)
      Product.WebApi/        ASP.NET host
    Identity/Identity.WebApi/  auth-ish host (WeatherForecast scaffold)
tests/Tests/                 Product.Domain, Product.App, Customer.App tests
docker-compose.yml           Identity + Customer + Product APIs
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
- README is **stale**: it says ".NET Core 3.0/3.1, install the 3.1 SDK" —
  csproj targets are `net10.0`/`netstandard2.1` and CI uses `10.0.x`.
  Trust csproj + CI, not the README's stack section.
- `Identity.WebApi` and `Product.WebApi` still contain `WeatherForecast`
  scaffold controllers — template leftovers, not real features.
- `CreatePruductUseCase.cs` has a typo'd filename ("Pruduct") — it's the
  real file; don't "fix" the name without checking references.
- `tests/Tests` covers only Product + Customer — Identity has no tests.
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
