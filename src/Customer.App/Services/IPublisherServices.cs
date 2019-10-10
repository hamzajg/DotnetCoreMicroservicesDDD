using System.Collections.Generic;
using System.Threading.Tasks;
using Services.Common.Domain.Events;

namespace Customer.App.Services
{
    public interface IPublisherServices
    {
        //IReadOnlyCollection<IEvent> Events { get; }

        Task PublishAsync(DomainEvent @event);
    }
}