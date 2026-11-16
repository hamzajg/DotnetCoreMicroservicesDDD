using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Order.Application.Ports;

namespace Order.Application.Adapters
{
    /// <summary>
    /// In-memory stand-in for the Customer service's read side: the set of
    /// customers this process knows about. Replace with an HTTP adapter against
    /// Customer.WebApi in a real deployment — the saga only knows the port.
    /// </summary>
    public class InMemoryCustomerDirectory : CustomerDirectory
    {
        private readonly HashSet<Guid> _customers = new HashSet<Guid>();

        public InMemoryCustomerDirectory()
        {
        }

        public InMemoryCustomerDirectory(IEnumerable<Guid> customers)
        {
            foreach (var customerId in customers)
                _customers.Add(customerId);
        }

        public void Register(Guid customerId)
        {
            _customers.Add(customerId);
        }

        public Task<bool> ExistsAsync(Guid customerId)
        {
            return Task.FromResult(_customers.Contains(customerId));
        }
    }
}
