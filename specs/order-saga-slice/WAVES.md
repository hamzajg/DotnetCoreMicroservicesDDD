# Waves — order-saga-slice

Each wave IS an iteration: a time-boxed, independently shippable
increment — one branch, one PR, green tests, demoable outcome.
Do not start wave N+1 until wave N merges.

## Wave 1 — iteration 1

Scenarios proven by this wave:
- a valid order runs every saga step and is saved
- a null command is rejected
- a missing customer fails the saga before anything is saved
- a missing product compensates the completed reservation steps
- a successful saga publishes an OrderCreated event
- AddUseCases registers the order saga and its collaborators

| Item | Title | Size | Deps | Notes |
|------|-------|------|------|-------|
| US1 | As a backend developer, I want order creation to run as a compensating saga over the customer, product and order steps, so that a partial failure rolls the completed steps back instead of leaving a half-written order | L | — | Order.Domain (Order/OrderLine/OrderCreated) + Order.Application (command, ports + in-memory adapters, CreateOrderUseCase on Saga.Common, publisher, AddUseCases); add the three projects to the solution and Tests.csproj. |

## Wave 2 — iteration 2

Scenarios proven by this wave:
- POST /Orders returns 202 Accepted
- POST /Orders publishes an OrderCreated event
- a failing saga over the endpoint is surfaced as an error

| Item | Title | Size | Deps | Notes |
|------|-------|------|------|-------|
| US2 | As an API consumer, I want a POST /Orders endpoint, so that I can place an order through the Order service end-to-end | M | US1 | OrdersController + Order.WebApi Startup; update README/AGENTS/ARCHITECTURE. |
