using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging.Abstractions;
using Order.Application.Adapters;
using Order.Application.Commands;
using Order.Application.Services;
using Order.Application.UseCases;
using Order.WebApi.Controllers;
using Saga.Common;
using Xunit;
using OrderCreatedEvent = Order.Domain.Events.OrderCreated;

namespace Tests.Order.WebApi.Controllers
{
    public class OrdersControllerTests
    {
        private static readonly Guid KnownCustomer = Guid.Parse("11111111-1111-1111-1111-111111111111");
        private static readonly Guid KnownProduct = Guid.Parse("33333333-3333-3333-3333-333333333333");
        private static readonly Guid UnknownCustomer = Guid.Parse("99999999-9999-9999-9999-999999999999");

        private readonly InMemoryPublisherServices _publisher = new InMemoryPublisherServices();
        private readonly InMemoryCustomerDirectory _customers = new InMemoryCustomerDirectory();
        private readonly InMemoryProductCatalog _products = new InMemoryProductCatalog();
        private readonly InMemoryOrderRepository _orders = new InMemoryOrderRepository();

        public OrdersControllerTests()
        {
            _customers.Register(KnownCustomer);
            _products.Add(KnownProduct);
        }

        private OrdersController Controller() =>
            new OrdersController(
                NullLogger<OrdersController>.Instance,
                new CreateOrderUseCaseImpl(
                    new SagaOrchestrator(), _customers, _products, _orders, _publisher));

        private static CreateOrderCommand ValidCommand() => new CreateOrderCommand
        {
            CustomerId = KnownCustomer,
            Items = new List<CreateOrderItem>
            {
                new CreateOrderItem { ProductId = KnownProduct, Quantity = 1 },
            },
        };

        // Scenario: POST /Orders returns 202 Accepted
        [Fact]
        public async Task PostOrdersReturns202Accepted()
        {
            // GIVEN a controller over the order use case
            var command = ValidCommand();

            // WHEN a valid command is posted
            var result = await Controller().Post(command);

            // THEN the response is 202 Accepted echoing the accepted order
            var accepted = Assert.IsType<AcceptedResult>(result);
            Assert.Equal(202, accepted.StatusCode);
            Assert.Same(command, accepted.Value);
        }

        // Scenario: POST /Orders publishes an OrderCreated event
        [Fact]
        public async Task PostOrdersPublishesAnOrderCreatedEvent()
        {
            // GIVEN a controller over a use case with an in-memory publisher
            // WHEN a valid command is posted
            await Controller().Post(ValidCommand());

            // THEN the publisher recorded exactly one OrderCreated domain event
            Assert.Single(_publisher.Events);
            Assert.IsType<OrderCreatedEvent>(_publisher.Events.Single());
        }

        // Scenario: a failing saga over the endpoint is surfaced as an error
        [Fact]
        public async Task AFailingSagaOverTheEndpointIsSurfacedAsAnError()
        {
            // GIVEN a controller over a use case whose saga fails
            var command = ValidCommand();
            command.CustomerId = UnknownCustomer;

            // WHEN a command for an unknown customer is posted
            // THEN an error surfaces rather than an Accepted response
            await Assert.ThrowsAsync<OrderSagaFailedException>(() => Controller().Post(command));
            Assert.Empty(_publisher.Events);
        }
    }
}
