using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Order.Application.Ports;

namespace Order.Application.Adapters
{
    /// <summary>
    /// In-memory stand-in for the Product service's read side: the set of
    /// products this process knows about. Replace with an HTTP adapter against
    /// Product.WebApi in a real deployment — the saga only knows the port.
    /// </summary>
    public class InMemoryProductCatalog : ProductCatalog
    {
        private readonly HashSet<Guid> _products = new HashSet<Guid>();

        public InMemoryProductCatalog()
        {
        }

        public InMemoryProductCatalog(IEnumerable<Guid> products)
        {
            foreach (var productId in products)
                _products.Add(productId);
        }

        public void Add(Guid productId)
        {
            _products.Add(productId);
        }

        public Task<bool> ExistsAsync(Guid productId)
        {
            return Task.FromResult(_products.Contains(productId));
        }
    }
}
