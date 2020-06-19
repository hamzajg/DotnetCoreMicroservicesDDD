using System.Diagnostics;
using System.Linq;
using System.Collections.Generic;
using System;
using System.Threading.Tasks;
using Domain.Common.Events;

namespace Customer.App.Services
{
    public class InMemoryPublisherServices : PublisherServices
    {
        private readonly Dictionary<Guid, Event> _events = new Dictionary<Guid, Event>();
        public IReadOnlyCollection<Event> Events => _events.Values.ToList();
        public Task PublishAsync(DomainEvent @event)
        {
            _events.Add(@event.Id, @event);
            return Task.CompletedTask;
        }
    }
}