# Waves — product-creation-slice

Each wave IS an iteration: a time-boxed, independently shippable
increment — one branch, one PR, green tests, demoable outcome.
Do not start wave N+1 until wave N merges.

## Wave 1 — iteration 1

Scenarios proven by this wave:
- creating a product publishes a ProductCreated event
- a null command is rejected
- an empty title is rejected and nothing is published
- AddUseCases registers the product use case and its publisher

| Item | Title | Size | Deps | Notes |
|------|-------|------|------|-------|
| US1 | As a backend developer, I want the Product use case to publish its domain event through an injectable publisher, so that the Product service follows the same Application-layer contract as Customer | M | — | Product.Application.Services + CreateProductUseCase interface + AddUseCases DI extension; mirror Customer. |

## Wave 2 — iteration 2

Scenarios proven by this wave:
- POST /Products returns 202 Accepted
- POST /Products publishes a ProductCreated event
- an empty title over the endpoint is surfaced as an error

| Item | Title | Size | Deps | Notes |
|------|-------|------|------|-------|
| US2 | As an API consumer, I want a POST /Products endpoint, so that I can create a product through the Product service end-to-end | M | US1 | ProductsController + Startup registration; update AGENTS.md/ARCHITECTURE.md/README. |
