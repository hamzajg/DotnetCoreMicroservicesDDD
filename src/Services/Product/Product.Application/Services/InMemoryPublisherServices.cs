using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Domain.Common.Events;

namespace Product.Application.Services
{
    /// <summary>
    /// In-memory publisher used by tests and the reference app — records the
    /// events it receives instead of sending them anywhere. Mirrors
    /// Customer's implementation so the two services read the same.
    /// </summary>
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
