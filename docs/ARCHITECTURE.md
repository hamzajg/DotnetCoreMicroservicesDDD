# Architecture — DotnetCoreMicroservicesDDD

Current-state map. Descriptive, not aspirational; see `docs/adr/` for why.

## System shape

```
                    docker-compose.yml
   ┌──────────┐   ┌──────────┐   ┌──────────┐   ┌──────────┐
   │ Identity │   │ Customer │   │ Product  │   │  Order   │  each = own WebApi host
   │ .WebApi  │   │ .WebApi  │   │ .WebApi  │   │ .WebApi  │  (Order not in compose yet)
   └──────────┘   └──────────┘   └──────────┘   └──────────┘
        │              │              │              │
        ▼              ▼              ▼              ▼
   Application    Application    Application    Application   use cases, commands, handlers
        │              │              │              │
        ▼              ▼              ▼              ▼
   Domain         Domain         Domain         Domain        entities, events, repo interfaces
        ▲              ▲              ▲              ▲
        └──── Domain.Common / Infrastructure.Common / Saga.Common (shared building blocks)
```

## Per-service layering

| Layer | Contains | Rule |
|---|---|---|
| `*.Domain` | Entities, value objects, domain events, repository **interfaces** | No framework deps |
| `*.Application` | Commands, use cases, handlers, in-memory repos | Orchestrates domain |
| `*.Infrastructure` | Persistence/integrations (mostly stub here) | Implements domain interfaces |
| `*.WebApi` | Controllers, Startup/Program, Dockerfile | Host only |

## Shared kernel

`src/Common/Domain.Common` provides the DDD vocabulary every service
reuses: `Command`/`CommandHandler`, `AggregateRoot`, `Entity`,
`DomainEvent`/`Event`/`EventHandler`, `RejectedEvent`. Customer's
`CustomerCreatedHandler` shows the event-handler pattern in use.

`src/Common/Saga.Common` provides the saga kernel — `SagaStep` (an action plus
its compensation), `SagaOrchestrator` (runs steps in order, rolls the completed
ones back in reverse on the first failure) and `SagaResult`. Order is its
consumer: `CreateOrderUseCaseImpl` builds a `reserve-customer` step, one
`reserve-product-*` step per line, and a `persist-order` step, and surfaces a
failure as `OrderSagaFailedException` carrying the `SagaResult`.

## State

Repositories are **in-memory** (`InMemoryCustomerRepository`, and Order's
`InMemoryOrderRepository`) — this is a structural reference, not a persistence
demo. Product publishes domain events through an in-memory `PublisherServices`
instead of a broker; Order does the same via `InMemoryPublisherServices`.
Customer/product lookups in the Order saga go through the `CustomerDirectory`
and `ProductCatalog` **ports**, served today by in-memory adapters. Docker
Compose runs the three APIs; there is no DB container.

## Frontend ↔ backend

No frontend in this repo — APIs only. Consumers are the WebApi controllers
(`CustomerController` with `POST /Customer`, `ProductsController` with
`POST /Products`, `OrdersController` with `POST /Orders`); contracts are the
command/event shapes in Application/Domain. The slices are wired the same way:
controller → use case interface → impl → `PublisherServices`, registered
through `AddUseCases`.
