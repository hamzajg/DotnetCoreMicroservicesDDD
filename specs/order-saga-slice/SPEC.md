# Spec: order-saga-slice

> Spec-driven development artifact. This file is the contract: implementation
> is done when every acceptance criterion maps to a scenario in
> `EXAMPLES.feature` and every scenario has a passing check.

## Problem

The repo ships a saga kernel (`src/Common/Saga.Common`: `SagaStep`,
`SagaOrchestrator`, `SagaResult`) with its own tests, but **nothing uses it** —
it is dead code. There is also no Order service: `product-creation-slice`'s own
SPEC says so explicitly ("No Order service, no cross-service saga — that is a
later, separate slice"). As a result the repo demonstrates the layered slice and
the saga kernel *separately*, and an agent (or human) cannot see the two
composed: a business write that spans Customer, Product and Order and rolls
back through compensation when a step fails.

## Users

- Backend/reference developer — wants to see the saga kernel applied to a real
  use case, not just unit-tested in isolation.
- API consumer / integration test — wants a `POST /Orders` endpoint that
  accepts an order through the Order Application layer.
- Agent working in the repo — wants the saga path navigable and verifiable by a
  command, mirroring the Customer and Product slices.

## Outcome hypothesis

- Expected user outcome: a developer reading (or asking an agent about) the
  Order service sees how partial failures are undone by compensation, so the
  saga pattern is usable rather than theoretical.
- Opportunity evidence: `specs/product-creation-slice/SPEC.md` names this slice
  as the deliberate next step; `Saga.Common` is implemented and tested but
  unreferenced by any service. (Assumption: this is intended as the reference
  slice.)
- Signal: `POST /Orders` is served by a use case that runs `SagaOrchestrator`
  steps with compensations; `dotnet test tests/Tests/Tests.csproj` covers the
  success and compensation paths.
- Follow-up owner/date: repo maintainer — reviewed when the slice is validated.

## Scope (in)

- A new **Order** service mirroring the existing layering:
  - `Order.Domain`: an `Order` aggregate (`AggregateRoot<Guid>`) + `OrderLine`
    value object + an `OrderCreated` domain event.
  - `Order.Application`: a `CreateOrder` command, a `CreateOrderUseCase` whose
    handler runs the order flow as a compensating saga on `Saga.Common`, the
    `CustomerDirectory` / `ProductCatalog` / `OrderRepository` ports with
    in-memory adapters, `PublisherServices` for the `OrderCreated` event, and a
    `ServiceCollectionExt.AddUseCases()` DI extension.
  - `Order.WebApi`: an `OrdersController` exposing `POST /Orders` and a
    `Startup` registering the Order use cases.
- Solution wiring: `Order.Domain`, `Order.Application`, `Order.WebApi` added to
  `MicroservicesDDD.sln`; the three referenced from `tests/Tests/Tests.csproj`.
- xUnit tests for the saga use case, the DI registration and the controller
  (no new packages beyond the existing xunit/moq + ASP.NET Core reference).
- Docs: `README.md`, `AGENTS.md` and `docs/ARCHITECTURE.md` updated to describe
  the Order slice and the saga it runs.

## Scope (out)

- No real persistence or message broker — in-memory adapters and the in-memory
  publisher only, matching the existing reference convention.
- No change to Customer, Product or Identity behaviour — the Order service adds
  new code, it does not modify the other services.
- No `Order.Infrastructure` project: persistence is deliberately minimal in
  this reference project, so the ports are satisfied in-memory from
  Application (mirroring `InMemoryCustomerRepository`).
- No containerisation for Order in this slice (no Dockerfile, no
  `docker-compose.yml` entry, no `docker.yml` matrix entry). Container builds
  are verified only in CI, which is out of reach here; the slice is verified by
  `dotnet build` + `dotnet test`.

## Acceptance criteria

<!-- Scenario(s): use scenario names verbatim from EXAMPLES.feature and/or
     @w<N> tags — ship_validate resolves both for traceability. -->

| # | Criterion | Scenario(s) |
|---|-----------|-------------|
| 1 | A valid order runs the reserve-customer, reserve-product and persist-order steps in order and saves the order. | @w1 |
| 2 | A null command is rejected with `ArgumentNullException`. | @w1 |
| 3 | When the customer does not exist the saga fails on its first step and nothing is saved. | @w1 |
| 4 | When a product does not exist the saga fails and compensates the already-completed steps in reverse order. | @w1 |
| 5 | A successful saga publishes exactly one `OrderCreated` domain event. | @w1 |
| 6 | `AddUseCases` registers the order use case, its saga orchestrator and its collaborators. | @w1 |
| 7 | `POST /Orders` returns `202 Accepted` echoing the accepted order. | @w2 |
| 8 | `POST /Orders` publishes an `OrderCreated` event through the publisher. | @w2 |
| 9 | A command whose saga fails over the endpoint is surfaced as an error, not a success. | @w2 |

## Decisions

- ADR-1: Add a third layered service (Order) mirroring Customer and Product
  exactly, rather than bolting the saga onto an existing service — the layering
  IS the lesson and the two existing slices read the same.
- ADR-2: Order creation is a **compensating saga** built on the existing
  `Saga.Common` kernel instead of a plain use case, because the repo already
  ships that kernel and the slice's value is composing it. A failed step
  compensates completed steps in reverse.
- ADR-3: The collaborators the saga touches are **ports**
  (`CustomerDirectory`, `ProductCatalog`, `OrderRepository`) with in-memory
  adapters. `SagaStep` is transport-agnostic, so a real HTTP/adapter
  implementation can replace the in-memory one without touching the saga.
- ADR-4: On failure the use case throws `OrderSagaFailedException` carrying the
  `SagaResult` (error + compensated steps), so the controller surfaces the
  failure the same way the Product slice surfaces an invalid title.
- ADR-5: The controller echoes the accepted command in the `202` body (like
  `CustomerController`/`ProductsController`); the endpoint proves the slice, it
  does not invent an id scheme.
