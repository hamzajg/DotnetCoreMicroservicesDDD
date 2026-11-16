# Backlog: order-saga-slice

Items are USER STORIES in the parseable format below — keep the heading
shape `## US<n> — <story>` and the field lines intact so `wave_plan` can
parse them. Write the story as: "As a <role>, I want <goal>, so that
<value>" — the title after the dash IS the story.

```
## US<n> — As a <role>, I want <goal>, so that <value>
- Scenarios: @w1, @w2
- Depends: US1, US3        (optional; omit for wave-1 items)
- Size: S|M|L
- Notes: <one line>
```

Order stories by user value; dependencies express technical order only.

## US1 — As a backend developer, I want order creation to run as a compensating saga over the customer, product and order steps, so that a partial failure rolls the completed steps back instead of leaving a half-written order
- Scenarios: @w1
- Size: L
- Notes: Order.Domain (Order/OrderLine/OrderCreated) + Order.Application (command, ports + in-memory adapters, CreateOrderUseCase on Saga.Common, publisher, AddUseCases); add the three projects to the solution and Tests.csproj.

## US2 — As an API consumer, I want a POST /Orders endpoint, so that I can place an order through the Order service end-to-end
- Scenarios: @w2
- Depends: US1
- Size: M
- Notes: OrdersController + Order.WebApi Startup; update README/AGENTS/ARCHITECTURE.
