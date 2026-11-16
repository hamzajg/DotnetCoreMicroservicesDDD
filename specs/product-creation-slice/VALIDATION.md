# Validation — product-creation-slice

## Traceability

| AC | Criterion | Scenarios | Wave(s) | Test evidence |
|----|-----------|-----------|---------|---------------|
| 1 | Creating a product from a valid command publishes exactly… | AddUseCases registers the product use case and its publisher, a null command is rejected, an empty title is rejected and nothing is published, creating a product publishes a ProductCreated event | 1 | CreatePeoductUseCaseTest.cs, ServiceCollectionExtTests.cs |
| 2 | A null command is rejected with `ArgumentNullException`. | AddUseCases registers the product use case and its publisher, a null command is rejected, an empty title is rejected and nothing is published, creating a product publishes a ProductCreated event | 1 | CreatePeoductUseCaseTest.cs, ServiceCollectionExtTests.cs |
| 3 | An empty title is rejected by the domain and nothing is… | AddUseCases registers the product use case and its publisher, a null command is rejected, an empty title is rejected and nothing is published, creating a product publishes a ProductCreated event | 1 | CreatePeoductUseCaseTest.cs, ServiceCollectionExtTests.cs |
| 4 | `AddUseCases` registers the Product use case and its… | AddUseCases registers the product use case and its publisher, a null command is rejected, an empty title is rejected and nothing is published, creating a product publishes a ProductCreated event | 1 | CreatePeoductUseCaseTest.cs, ServiceCollectionExtTests.cs |
| 5 | `POST /Products` returns `202 Accepted` echoing the… | POST /Products publishes a ProductCreated event, POST /Products returns 202 Accepted, an empty title over the endpoint is surfaced as an error | 2 | ProductsControllerTests.cs |
| 6 | `POST /Products` publishes a `ProductCreated` event through… | POST /Products publishes a ProductCreated event, POST /Products returns 202 Accepted, an empty title over the endpoint is surfaced as an error | 2 | ProductsControllerTests.cs |
| 7 | An empty title submitted over the endpoint is surfaced as… | POST /Products publishes a ProductCreated event, POST /Products returns 202 Accepted, an empty title over the endpoint is surfaced as an error | 2 | ProductsControllerTests.cs |

## Scenario coverage

- Scenarios: 7 total, 7 with detected test evidence, 0 without
- ✓ `creating a product publishes a ProductCreated event` (@w1)
- ✓ `a null command is rejected` (@w1)
- ✓ `an empty title is rejected and nothing is published` (@w1)
- ✓ `AddUseCases registers the product use case and its publisher` (@w1)
- ✓ `POST /Products returns 202 Accepted` (@w2)
- ✓ `POST /Products publishes a ProductCreated event` (@w2)
- ✓ `an empty title over the endpoint is surfaced as an error` (@w2)

## Demo script (manual walkthrough)

1. **creating a product publishes a ProductCreated event** — run the app, perform the scenario, observe the outcome.

## Gaps

- none — spec is fully traced

Note: 'no evidence' means neither the scenario name nor a
token match was found in test files — verify by hand before
treating it as a real gap. '~' marks weak token-match evidence.