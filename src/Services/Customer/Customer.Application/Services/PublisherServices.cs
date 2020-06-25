using Domain.Common.Events;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Customer.App.Services
{
    public interface PublisherServices
    {
        //IReadOnlyCollection<Event> Events { get; }

        Task PublishAsync(DomainEvent @event);
    }
}