using System.Threading.Tasks;
using Services.Common.Domain.Events;
using Customer.App.Events;

namespace Customer.App.Handlers
{
    public class CustomerCreatedHandler : IEventHandler<CustomerCreated>
    {
        public Task HandleAsync(IEvent @event) {
            return Task.CompletedTask;
        }
    }
}