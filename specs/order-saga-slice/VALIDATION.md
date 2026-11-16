# Validation — order-saga-slice

## Traceability

| AC | Criterion | Scenarios | Wave(s) | Test evidence |
|----|-----------|-----------|---------|---------------|
| 1 | A valid order runs the reserve-customer, reserve-product… | AddUseCases registers the order saga and its collaborators, a missing customer fails the saga before anything is saved, a missing product compensates the completed reservation steps, a null command is rejected, a successful saga publishes an OrderCreated event, a valid order runs every saga step and is saved | 1 | CreateOrderUseCaseTests.cs, CreatePeoductUseCaseTest.cs, ServiceCollectionExtTests.cs |
| 2 | A null command is rejected with `ArgumentNullException`. | AddUseCases registers the order saga and its collaborators, a missing customer fails the saga before anything is saved, a missing product compensates the completed reservation steps, a null command is rejected, a successful saga publishes an OrderCreated event, a valid order runs every saga step and is saved | 1 | CreateOrderUseCaseTests.cs, CreatePeoductUseCaseTest.cs, ServiceCollectionExtTests.cs |
| 3 | When the customer does not exist the saga fails on its… | AddUseCases registers the order saga and its collaborators, a missing customer fails the saga before anything is saved, a missing product compensates the completed reservation steps, a null command is rejected, a successful saga publishes an OrderCreated event, a valid order runs every saga step and is saved | 1 | CreateOrderUseCaseTests.cs, CreatePeoductUseCaseTest.cs, ServiceCollectionExtTests.cs |
| 4 | When a product does not exist the saga fails and… | AddUseCases registers the order saga and its collaborators, a missing customer fails the saga before anything is saved, a missing product compensates the completed reservation steps, a null command is rejected, a successful saga publishes an OrderCreated event, a valid order runs every saga step and is saved | 1 | CreateOrderUseCaseTests.cs, CreatePeoductUseCaseTest.cs, ServiceCollectionExtTests.cs |
| 5 | A successful saga publishes exactly one `OrderCreated`… | AddUseCases registers the order saga and its collaborators, a missing customer fails the saga before anything is saved, a missing product compensates the completed reservation steps, a null command is rejected, a successful saga publishes an OrderCreated event, a valid order runs every saga step and is saved | 1 | CreateOrderUseCaseTests.cs, CreatePeoductUseCaseTest.cs, ServiceCollectionExtTests.cs |
| 6 | `AddUseCases` registers the order use case, its saga… | AddUseCases registers the order saga and its collaborators, a missing customer fails the saga before anything is saved, a missing product compensates the completed reservation steps, a null command is rejected, a successful saga publishes an OrderCreated event, a valid order runs every saga step and is saved | 1 | CreateOrderUseCaseTests.cs, CreatePeoductUseCaseTest.cs, ServiceCollectionExtTests.cs |
| 7 | `POST /Orders` returns `202 Accepted` echoing the accepted… | POST /Orders publishes an OrderCreated event, POST /Orders returns 202 Accepted, a failing saga over the endpoint is surfaced as an error | 2 | OrdersControllerTests.cs |
| 8 | `POST /Orders` publishes an `OrderCreated` event through… | POST /Orders publishes an OrderCreated event, POST /Orders returns 202 Accepted, a failing saga over the endpoint is surfaced as an error | 2 | OrdersControllerTests.cs |
| 9 | A command whose saga fails over the endpoint is surfaced as… | POST /Orders publishes an OrderCreated event, POST /Orders returns 202 Accepted, a failing saga over the endpoint is surfaced as an error | 2 | OrdersControllerTests.cs |

## Scenario coverage

- Scenarios: 9 total, 9 with detected test evidence, 0 without
- ✓ `a valid order runs every saga step and is saved` (@w1)
- ✓ `a null command is rejected` (@w1)
- ✓ `a missing customer fails the saga before anything is saved` (@w1)
- ✓ `a missing product compensates the completed reservation steps` (@w1)
- ✓ `a successful saga publishes an OrderCreated event` (@w1)
- ✓ `AddUseCases registers the order saga and its collaborators` (@w1)
- ✓ `POST /Orders returns 202 Accepted` (@w2)
- ✓ `POST /Orders publishes an OrderCreated event` (@w2)
- ✓ `a failing saga over the endpoint is surfaced as an error` (@w2)

## Demo script (manual walkthrough)

1. **a valid order runs every saga step and is saved** — run the app, perform the scenario, observe the outcome.

## Gaps

- none — spec is fully traced

Note: 'no evidence' means neither the scenario name nor a
token match was found in test files — verify by hand before
treating it as a real gap. '~' marks weak token-match evidence.

## Outcome follow-up

Optional; this is a post-release learning record, not a finish gate.
- Status: not yet measured
- Owner and review date: (record when known)
- Signal and observed result: (record after release; do not invent)
- Learning or next action: (record after review)