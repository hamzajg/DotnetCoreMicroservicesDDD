using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Order.Application.Commands;
using Order.Application.Ports;
using Order.Application.Services;
using Order.Application.UseCases;
using Saga.Common;
using Xunit;
using OrderAggregate = Order.Domain.Entities.Order;
using OrderCreatedEvent = Order.Domain.Events.OrderCreated;

namespace Tests.Order.App.UseCases
{
    public class CreateOrderUseCaseTests
    {
        private static readonly Guid KnownCustomer = Guid.Parse("11111111-1111-1111-1111-111111111111");
        private static readonly Guid KnownProduct = Guid.Parse("33333333-3333-3333-3333-333333333333");
        private static readonly Guid SecondProduct = Guid.Parse("44444444-4444-4444-4444-444444444444");
        private static readonly Guid MissingProduct = Guid.Parse("55555555-5555-5555-5555-555555555555");

        private readonly List<string> _log = new List<string>();
        private readonly InMemoryPublisherServices _publisher = new InMemoryPublisherServices();
        private RecordingOrderRepository _orders;

        private CreateOrderUseCase BuildUseCase(IEnumerable<Guid> knownCustomers, IEnumerable<Guid> knownProducts)
        {
            _orders = new RecordingOrderRepository(_log);
            return new CreateOrderUseCaseImpl(
                new SagaOrchestrator(),
                new RecordingCustomerDirectory(knownCustomers, _log),
                new RecordingProductCatalog(knownProducts, _log),
                _orders,
                _publisher);
        }

        private static CreateOrderCommand ValidCommand() => new CreateOrderCommand
        {
            CustomerId = KnownCustomer,
            Items = new List<CreateOrderItem>
            {
                new CreateOrderItem { ProductId = KnownProduct, Quantity = 2 },
                new CreateOrderItem { ProductId = SecondProduct, Quantity = 1 },
            },
        };

        // Scenario: a valid order runs every saga step and is saved
        [Fact]
        public async Task AValidOrderRunsEverySagaStepAndIsSaved()
        {
            // GIVEN a use case with a known customer and known products
            var useCase = BuildUseCase(new[] { KnownCustomer }, new[] { KnownProduct, SecondProduct });

            // WHEN a valid command is handled
            await useCase.HandleAsync(ValidCommand());

            // THEN every saga step ran in order
            Assert.Equal(
                new[]
                {
                    "reserve-customer",
                    $"reserve-product-{KnownProduct}",
                    $"reserve-product-{SecondProduct}",
                    "persist-order",
                },
                _log);

            // AND the order is saved in the order repository
            Assert.Single(_orders.Orders);
        }

        // Scenario: a null command is rejected
        [Fact]
        public async Task ANullCommandIsRejected()
        {
            var useCase = BuildUseCase(new[] { KnownCustomer }, new[] { KnownProduct });

            await Assert.ThrowsAsync<ArgumentNullException>(() => useCase.HandleAsync(null));
        }

        // Scenario: a missing customer fails the saga before anything is saved
        [Fact]
        public async Task AMissingCustomerFailsTheSagaBeforeAnythingIsSaved()
        {
            // GIVEN a use case with an unknown customer
            var useCase = BuildUseCase(new Guid[0], new[] { KnownProduct, SecondProduct });

            // WHEN a command is handled
            var failure = await Assert.ThrowsAsync<OrderSagaFailedException>(
                () => useCase.HandleAsync(ValidCommand()));

            // THEN the saga fails on the reserve-customer step
            Assert.False(failure.Result.Succeeded);
            Assert.Contains("reserve-customer", failure.Result.Error);
            Assert.Empty(failure.Result.CompletedSteps);
            Assert.Empty(failure.Result.CompensatedSteps);

            // AND nothing is saved
            Assert.DoesNotContain("persist-order", _log);
            Assert.Empty(_orders.Orders);
        }

        // Scenario: a missing product compensates the completed reservation steps
        [Fact]
        public async Task AMissingProductCompensatesTheCompletedReservationSteps()
        {
            // GIVEN a use case with a known customer and a missing product
            var useCase = BuildUseCase(new[] { KnownCustomer }, new[] { KnownProduct });
            var command = new CreateOrderCommand
            {
                CustomerId = KnownCustomer,
                Items = new List<CreateOrderItem>
                {
                    new CreateOrderItem { ProductId = KnownProduct, Quantity = 1 },
                    new CreateOrderItem { ProductId = MissingProduct, Quantity = 1 },
                },
            };

            // WHEN a command is handled
            var failure = await Assert.ThrowsAsync<OrderSagaFailedException>(
                () => useCase.HandleAsync(command));

            // THEN the saga fails on the missing product's step
            Assert.False(failure.Result.Succeeded);
            Assert.Contains($"reserve-product-{MissingProduct}", failure.Result.Error);
            Assert.Equal(
                new[] { "reserve-customer", $"reserve-product-{KnownProduct}" },
                failure.Result.CompletedSteps);

            // AND the completed steps are compensated in reverse order
            Assert.Equal(
                new[] { $"reserve-product-{KnownProduct}", "reserve-customer" },
                failure.Result.CompensatedSteps);

            // AND no order is saved
            Assert.Empty(_orders.Orders);
        }

        // Scenario: a successful saga publishes an OrderCreated event
        [Fact]
        public async Task ASuccessfulSagaPublishesAnOrderCreatedEvent()
        {
            // GIVEN a use case with an in-memory publisher
            var useCase = BuildUseCase(new[] { KnownCustomer }, new[] { KnownProduct, SecondProduct });

            // WHEN a valid command is handled
            await useCase.HandleAsync(ValidCommand());

            // THEN exactly one OrderCreated domain event is published
            Assert.Single(_publisher.Events);
            Assert.IsType<OrderCreatedEvent>(_publisher.Events.Single());
        }

        private class RecordingCustomerDirectory : CustomerDirectory
        {
            private readonly HashSet<Guid> _known;
            private readonly List<string> _log;

            public RecordingCustomerDirectory(IEnumerable<Guid> known, List<string> log)
            {
                _known = new HashSet<Guid>(known);
                _log = log;
            }

            public Task<bool> ExistsAsync(Guid customerId)
            {
                _log.Add("reserve-customer");
                return Task.FromResult(_known.Contains(customerId));
            }
        }

        private class RecordingProductCatalog : ProductCatalog
        {
            private readonly HashSet<Guid> _known;
            private readonly List<string> _log;

            public RecordingProductCatalog(IEnumerable<Guid> known, List<string> log)
            {
                _known = new HashSet<Guid>(known);
                _log = log;
            }

            public Task<bool> ExistsAsync(Guid productId)
            {
                _log.Add($"reserve-product-{productId}");
                return Task.FromResult(_known.Contains(productId));
            }
        }

        private class RecordingOrderRepository : OrderRepository
        {
            private readonly Dictionary<Guid, OrderAggregate> _orders = new Dictionary<Guid, OrderAggregate>();
            private readonly List<string> _log;

            public RecordingOrderRepository(List<string> log)
            {
                _log = log;
            }

            public IReadOnlyCollection<OrderAggregate> Orders => _orders.Values;

            public Task SaveAsync(OrderAggregate order)
            {
                _log.Add("persist-order");
                _orders[order.Id] = order;
                return Task.CompletedTask;
            }

            public Task<OrderAggregate> GetAsync(Guid id)
            {
                _orders.TryGetValue(id, out var order);
                return Task.FromResult(order);
            }

            public Task RemoveAsync(Guid id)
            {
                _log.Add("remove-order");
                _orders.Remove(id);
                return Task.CompletedTask;
            }
        }
    }
}
