using System.Diagnostics;
using System.Linq;
using System.Collections.Generic;
using System;
using System.Threading.Tasks;
using Services.Common.Domain.Events;

namespace Customer.App.Services
{
    public class InMemoryPublisherServices : IPublisherServices
    {
        private readonly Dictionary<Guid, IEvent> _events = new Dictionary<Guid, IEvent>();
        public IReadOnlyCollection<IEvent> Events => _events.Values.ToList();
        public Task PublishAsync(DomainEvent @event)
        {
            _events.Add(@event.Id, @event);
            return Task.CompletedTask;
        }
    }
}