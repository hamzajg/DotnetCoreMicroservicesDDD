# ADR 0002 — Saga orchestration kernel (`Common/Saga.Common`)

## Status

Accepted — implemented on `feat/saga-orchestrator`.

## Context

The queued candidate was "a saga orchestrator agent driving the
customer/order/product services via REST". On inspection the premise
only half-holds: `Customer.WebApi` exposes exactly one endpoint
(`POST /Customer`), `Product.WebApi` still serves only the
`WeatherForecast` scaffold (its `CreateProduct` use case is not exposed),
and there is no Order service anywhere — `develop` has nothing either.
A REST-driving orchestrator today would be a client for endpoints that
don't exist.

## Decision

Ship the **kernel**, not a fake driver: `src/Common/Saga.Common`
(netstandard2.1, same target as the other Common libs):

- `SagaStep` — name + `Func<Task>` action + optional compensation.
  Transport-agnostic: HTTP calls, in-memory writes, or domain commands
  all fit; no orchestration framework, no vendor dependency.
- `SagaOrchestrator.RunAsync` — sequential execution; on first failure,
  compensates completed steps in reverse. A failing compensation is
  recorded in `SagaResult.CompensationErrors` and never masks the
  original error.
- `SagaResult` — `{Succeeded, CompletedSteps, CompensatedSteps,
  CompensationErrors, Error}`.

Six xUnit tests cover ordering, reverse compensation, the failed-step
exclusion, compensation-failure semantics, missing compensations, and the
empty saga.

## Consequences

- The pattern is real and tested; it is **not yet wired to services** —
  that awaits real endpoints (a Product controller, an Order bounded
  context). When they land, steps are thin `HttpClient` calls or direct
  use-case invocations.
- Choreography (each service owns its saga logic) vs orchestration
  (central coordinator) — this kernel is orchestration-shaped but
  transport-free, so either style can adopt it.
- Deliberately no persistence/retry/event-bus — KISS; a saga that must
  survive restarts needs a saga store, which is a separate ADR when a
  real consumer appears.
