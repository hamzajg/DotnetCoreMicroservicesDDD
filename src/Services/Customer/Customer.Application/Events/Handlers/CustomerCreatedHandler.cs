using Customer.Domain.Events;
using Domain.Common.Events;
using System.Threading.Tasks;

namespace Customer.App.Handlers
{
    public class CustomerCreatedHandler : EventHandler<CustomerCreated>
    {
        public Task HandleAsync(Event @event) {
            return Task.CompletedTask;
        }
    }
}