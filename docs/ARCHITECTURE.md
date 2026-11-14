# Architecture — DotnetCoreMicroservicesDDD

Current-state map. Descriptive, not aspirational; see `docs/adr/` for why.

## System shape

```
                    docker-compose.yml
   ┌──────────┐   ┌──────────┐   ┌──────────┐
   │ Identity │   │ Customer │   │ Product  │   each = own WebApi host
   │ .WebApi  │   │ .WebApi  │   │ .WebApi  │
   └──────────┘   └──────────┘   └──────────┘
        │              │              │
        ▼              ▼              ▼
   Application    Application    Application   use cases, commands, handlers
        │              │              │
        ▼              ▼              ▼
   Domain         Domain         Domain        entities, events, repo interfaces
        ▲              ▲              ▲
        └──── Domain.Common / Infrastructure.Common (shared building blocks)
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

## State

Repositories are **in-memory** (`InMemoryCustomerRepository`) — this is a
structural reference, not a persistence demo. Docker Compose runs the three
APIs; there is no DB container.

## Frontend ↔ backend

No frontend in this repo — APIs only. Consumers are the WebApi controllers
(`CustomerController` etc.); contracts are the command/event shapes in
Application/Domain.
