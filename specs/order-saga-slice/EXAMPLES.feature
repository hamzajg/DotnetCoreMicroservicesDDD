# order-saga-slice — specifications by example
# Scenario tags @w<N> map items to delivery waves (see WAVES.md / BACKLOG.md).

Feature: order-saga-slice
  The Order service creates an order as a compensating saga built on the shared
  Saga.Common kernel: it reserves the customer, reserves each product, then
  persists the order — and when a step fails, the completed steps are rolled
  back in reverse order.

  # ── Wave 1 — Application: the saga ────────────────────────────────────────

  @w1 @happy-path
  Scenario: a valid order runs every saga step and is saved
    Given a CreateOrderUseCase with a known customer and known products
    When a valid CreateOrder command is handled
    Then every saga step completes in order
    And the order is saved in the order repository

  @w1
  Scenario: a null command is rejected
    Given a CreateOrderUseCase
    When a null command is handled
    Then an ArgumentNullException is thrown

  @w1
  Scenario: a missing customer fails the saga before anything is saved
    Given a CreateOrderUseCase with an unknown customer
    When a CreateOrder command is handled
    Then the saga fails on the reserve-customer step
    And no order is saved

  @w1
  Scenario: a missing product compensates the completed reservation steps
    Given a CreateOrderUseCase with a known customer and a missing product
    When a CreateOrder command is handled
    Then the saga fails on the product step
    And the completed steps are compensated in reverse order
    And no order is saved

  @w1
  Scenario: a successful saga publishes an OrderCreated event
    Given a CreateOrderUseCase with an in-memory publisher
    When a valid CreateOrder command is handled
    Then exactly one OrderCreated domain event is published

  @w1
  Scenario: AddUseCases registers the order saga and its collaborators
    Given an empty service collection
    When the Order Application use cases are registered
    Then the order use case, its saga orchestrator and its collaborators resolve from the collection

  # ── Wave 2 — WebApi: the endpoint ─────────────────────────────────────────

  @w2
  Scenario: POST /Orders returns 202 Accepted
    Given an OrdersController over the order use case
    When a valid CreateOrder command is posted
    Then the response is 202 Accepted
    And its body echoes the accepted order

  @w2
  Scenario: POST /Orders publishes an OrderCreated event
    Given an OrdersController over a use case with an in-memory publisher
    When a valid CreateOrder command is posted
    Then the publisher records one OrderCreated domain event

  @w2
  Scenario: a failing saga over the endpoint is surfaced as an error
    Given an OrdersController over a use case whose saga fails
    When a command for an unknown customer is posted
    Then an error is surfaced rather than an Accepted response
