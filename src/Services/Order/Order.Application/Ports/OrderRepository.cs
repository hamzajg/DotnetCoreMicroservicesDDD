using System;
using System.Threading.Tasks;

namespace Order.Application.Ports
{
    /// <summary>
    /// Write side of the Order service. The saga's persist step saves through
    /// this port and its compensation removes what the step saved.
    /// </summary>
    public interface OrderRepository
    {
        Task SaveAsync(Order.Domain.Entities.Order order);
        Task<Order.Domain.Entities.Order> GetAsync(Guid id);
        Task RemoveAsync(Guid id);
    }
}
