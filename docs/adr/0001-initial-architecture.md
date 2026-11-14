# ADR 0001 — Initial architecture

Date: 2026-11-13 (baseline recorded retroactively)
Status: accepted (describes what is built)

## Context

A DDD microservices reference project: three services demonstrating
layered architecture, domain events, and command handling — optimised for
learning the structure, not production traffic.

## Decisions

1. **Strict per-service layering.** Domain (framework-free) → Application
   (use cases/commands) → Infrastructure → WebApi. Rationale: the layering
   is the teaching content; violating it defeats the repo's purpose.
2. **Shared `Domain.Common` kernel.** Command/handler, aggregate root,
   entity, domain-event primitives live once and are reused by all
   services. Rationale: DRY — each service demonstrates the same patterns
   without copy-paste.
3. **In-memory repositories.** `InMemoryCustomerRepository` etc. — no DB.
   Rationale: keeps the focus on domain/application structure; persistence
   is Infrastructure's job when needed.
4. **One host per service, own Startup/Program.** No shared web host or
   API gateway. Rationale: microservice independence is the point;
   composition happens in docker-compose.
5. **Domain events + handlers.** `CustomerCreated`/`CustomerRegistered`
   events with handlers in Application. Rationale: demonstrates event-driven
   domain reactions without a message broker.
6. **Modernised to net10.0.** Originally .NET Core 3.x (EOL); retargeted to
   `net10.0`/`netstandard2.1` with CI on `10.0.x`. Rationale: run on a
   supported SDK. NOTE: README still says 3.x — stale doc, csproj is truth.

## Consequences

- No real persistence, auth, or inter-service messaging — accepted for a
  reference project.
- `WeatherForecast` scaffolds remain in Identity/Product WebApi —
  template leftovers, harmless but misleading; cleanup candidate.
- Services don't call each other — composition is external.
