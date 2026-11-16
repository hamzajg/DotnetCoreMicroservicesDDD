# Backlog: product-creation-slice

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

## US1 — As a backend developer, I want the Product use case to publish its domain event through an injectable publisher, so that the Product service follows the same Application-layer contract as Customer
- Scenarios: @w1
- Size: M
- Notes: Product.Application.Services + CreateProductUseCase interface + AddUseCases DI extension; mirror Customer.

## US2 — As an API consumer, I want a POST /Products endpoint, so that I can create a product through the Product service end-to-end
- Scenarios: @w2
- Depends: US1
- Size: M
- Notes: ProductsController + Startup registration; update AGENTS.md/ARCHITECTURE.md/README.
