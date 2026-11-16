using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Order.Application.Ports;

namespace Order.Application.Adapters
{
    /// <summary>
    /// In-memory Order repository used by tests and the reference app —
    /// persistence is deliberately minimal here, mirroring
    /// InMemoryCustomerRepository. RemoveAsync is what the saga's persist step
    /// runs to compensate a failed order.
    /// </summary>
    public class InMemoryOrderRepository : OrderRepository
    {
        private readonly Dictionary<Guid, Order.Domain.Entities.Order> _orders =
            new Dictionary<Guid, Order.Domain.Entities.Order>();

        public IReadOnlyCollection<Order.Domain.Entities.Order> Orders => _orders.Values;

        public Task SaveAsync(Order.Domain.Entities.Order order)
        {
            _orders[order.Id] = order;
            return Task.CompletedTask;
        }

        public Task<Order.Domain.Entities.Order> GetAsync(Guid id)
        {
            _orders.TryGetValue(id, out var order);
            return Task.FromResult(order);
        }

        public Task RemoveAsync(Guid id)
        {
            _orders.Remove(id);
            return Task.CompletedTask;
        }
    }
}
