# product-creation-slice — specifications by example
# Scenario tags @w<N> map items to delivery waves (see WAVES.md / BACKLOG.md).

Feature: product-creation-slice
  The Product service gains the same Domain → Application → WebApi slice the
  Customer service already has: a use case that publishes the ProductCreated
  domain event through an injectable publisher, a DI registration, and a
  POST /Products endpoint.

  # ── Wave 1 — Application slice ────────────────────────────────────────────

  @w1 @happy-path
  Scenario: creating a product publishes a ProductCreated event
    Given a CreatePruductUseCase with an in-memory publisher
    When a valid CreateProductCommand is handled
    Then exactly one ProductCreated domain event is published

  @w1
  Scenario: a null command is rejected
    Given a CreatePruductUseCase
    When a null command is handled
    Then an ArgumentNullException is thrown

  @w1
  Scenario: an empty title is rejected and nothing is published
    Given a CreatePruductUseCase with an in-memory publisher
    When a command with an empty title is handled
    Then an ArgumentNullException is thrown
    And no event is published

  @w1
  Scenario: AddUseCases registers the product use case and its publisher
    Given an empty service collection
    When the Product Application use cases are registered
    Then the product use case and its publisher resolve from the collection

  # ── Wave 2 — WebApi slice ─────────────────────────────────────────────────

  @w2
  Scenario: POST /Products returns 202 Accepted
    Given a ProductsController over the product use case
    When a valid CreateProductCommand is posted
    Then the response is 202 Accepted
    And its body echoes the accepted product

  @w2
  Scenario: POST /Products publishes a ProductCreated event
    Given a ProductsController over a use case with an in-memory publisher
    When a valid CreateProductCommand is posted
    Then the publisher records one ProductCreated domain event

  @w2
  Scenario: an empty title over the endpoint is surfaced as an error
    Given a ProductsController over the product use case
    When a command with an empty title is posted
    Then an error is surfaced rather than an Accepted response
