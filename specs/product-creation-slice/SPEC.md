# Spec: product-creation-slice

> Spec-driven development artifact. This file is the contract: implementation
> is done when every acceptance criterion maps to a scenario in
> `EXAMPLES.feature` and every scenario has a passing check.

## Problem

The Customer service is the only fully-wired service in the repo: it has a
Domain entity, an Application use case that publishes the domain event through
an injectable `PublisherServices`, a DI extension (`AddUseCases`) and a
`CustomerController` exposing `POST /Customer`. The Product service stops at
the Domain: `Product.WebApi` still serves the `WeatherForecast` template
scaffold, `CreatePruductUseCase` builds a `Product` but discards it (its
`ProductCreated` domain event is never published), and there is no DI
registration or controller. As a result the repo's own lesson — the
Domain → Application → WebApi layering — is only demonstrated for one of the
three services, and an agent (or human) cannot exercise a Product write path
end-to-end.

## Users

- Backend/reference developer — wants a second service that shows the full
  layered slice, not a scaffold.
- API consumer / integration test — wants a `POST /Products` endpoint that
  accepts a product through the Application layer.
- Agent working in the repo — wants the Product path navigable and verifiable
  by a command, mirroring the Customer path exactly.

## Scope (in)

- `Product.Application`: a `PublisherServices` abstraction +
  `InMemoryPublisherServices`, a `CreateProductUseCase` interface, and
  `CreatePruductUseCase` wired to publish the created `ProductCreated` event;
  a `ServiceCollectionExt.AddUseCases()` DI extension.
- `Product.WebApi`: a `ProductsController` exposing `POST /Products`, and
  `Startup` registering the Product use cases.
- xUnit tests for the use case and the controller (no new packages beyond the
  existing xunit/moq + the ASP.NET Core framework reference).
- Docs: `AGENTS.md`, `docs/ARCHITECTURE.md`, `README.md` updated to describe
  the Product creation slice.

## Scope (out)

- No real persistence or message broker — in-memory publisher only, matching
  the Customer reference (persistence is deliberately minimal here).
- No change to `Product.Domain` behaviour (the entity, its events and its
  validation stay as they are).
- No removal/rewrite of the unrelated `WeatherForecast` scaffolds beyond the
  Product route the slice adds.
- No Order service, no cross-service saga — that is a later, separate slice.

## Acceptance criteria

<!-- Scenario(s): use scenario names verbatim from EXAMPLES.feature and/or
     @w<N> tags — ship_validate resolves both for traceability. -->

| # | Criterion | Scenario(s) |
|---|-----------|-------------|
| 1 | Creating a product from a valid command publishes exactly one `ProductCreated` domain event. | @w1 |
| 2 | A null command is rejected with `ArgumentNullException`. | @w1 |
| 3 | An empty title is rejected by the domain and nothing is published. | @w1 |
| 4 | `AddUseCases` registers the Product use case and its publisher, both resolvable from the service collection. | @w1 |
| 5 | `POST /Products` returns `202 Accepted` echoing the accepted product. | @w2 |
| 6 | `POST /Products` publishes a `ProductCreated` event through the publisher. | @w2 |
| 7 | An empty title submitted over the endpoint is surfaced as an error, not a success. | @w2 |

## Decisions

- ADR-1: Mirror the Customer slice exactly (interface + impl use case,
  `PublisherServices`/`InMemoryPublisherServices`, `AddUseCases` extension) so
  the two services read the same — the layering IS the lesson (DRY across
  services).
- ADR-2: Keep the `ProductCreated` event `internal`; the use case publishes it
  through the `DomainEvent` base type, so no new public surface is added to
  `Product.Domain`.
- ADR-3: The controller echoes the accepted command in the `202` body (like
  `CustomerController`), because `Product` has no generated identity yet — the
  endpoint proves the slice, it does not invent an id scheme.
