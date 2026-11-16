using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Order.Application.Commands;
using Order.Application.Ports;
using Order.Application.Services;
using Saga.Common;
using OrderAggregate = Order.Domain.Entities.Order;
using OrderLine = Order.Domain.Entities.OrderLine;

namespace Order.Application.UseCases
{
    /// <summary>
    /// Places an order as a compensating saga on the shared Saga.Common kernel:
    /// reserve the customer, reserve each product, then persist the order. The
    /// first failing step rolls the completed steps back in reverse order, and a
    /// failure surfaces as <see cref="OrderSagaFailedException"/> carrying the
    /// saga result. Mirrors the Customer/Product use cases in shape.
    /// </summary>
    public class CreateOrderUseCaseImpl : CreateOrderUseCase
    {
        private readonly SagaOrchestrator _saga;
        private readonly CustomerDirectory _customers;
        private readonly ProductCatalog _products;
        private readonly OrderRepository _orders;
        private readonly PublisherServices _publisher;

        public CreateOrderUseCaseImpl(
            SagaOrchestrator saga,
            CustomerDirectory customers,
            ProductCatalog products,
            OrderRepository orders,
            PublisherServices publisher)
        {
            _saga = saga;
            _customers = customers;
            _products = products;
            _orders = orders;
            _publisher = publisher;
        }

        public async Task HandleAsync(CreateOrderCommand command)
        {
            if (command == null)
                throw new ArgumentNullException(nameof(command));

            var order = OrderAggregate.Of(
                command.CustomerId,
                command.Items.Select(item => OrderLine.Of(item.ProductId, item.Quantity)));

            var steps = new List<SagaStep>
            {
                // A read-only check has nothing to undo, but it is still a step:
                // registering a compensation keeps it in the rollback record.
                new SagaStep(
                    "reserve-customer",
                    async () =>
                    {
                        if (!await _customers.ExistsAsync(command.CustomerId))
                            throw new InvalidOperationException(
                                $"Customer {command.CustomerId} does not exist");
                    },
                    () => Task.CompletedTask),
            };

            foreach (var item in command.Items)
            {
                var productId = item.ProductId;
                steps.Add(new SagaStep(
                    $"reserve-product-{productId}",
                    async () =>
                    {
                        if (!await _products.ExistsAsync(productId))
                            throw new InvalidOperationException(
                                $"Product {productId} does not exist");
                    },
                    () => Task.CompletedTask));
            }

            steps.Add(new SagaStep(
                "persist-order",
                () => _orders.SaveAsync(order),
                () => _orders.RemoveAsync(order.Id)));

            var result = await _saga.RunAsync(steps);
            if (!result.Succeeded)
                throw new OrderSagaFailedException(result);

            await _publisher.PublishAsync(order.DomainEvents.LastOrDefault());
        }
    }
}
