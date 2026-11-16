using System.Threading.Tasks;
using Domain.Common.Events;

namespace Product.Application.Services
{
    /// <summary>
    /// Publishes domain events out of the Application layer. An abstraction so
    /// the use cases don't depend on a concrete bus/broker — mirrors the
    /// Customer service's publisher contract.
    /// </summary>
    public interface PublisherServices
    {
        Task PublishAsync(DomainEvent @event);
    }
}
