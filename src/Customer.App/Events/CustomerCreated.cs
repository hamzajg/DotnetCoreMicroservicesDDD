using Services.Common.Domain.Events;

namespace Customer.App.Events
{
    public class CustomerCreated : DomainEvent
    {
        private readonly Domain.Customer _custmer;
        public CustomerCreated(Domain.Customer customer)
        {
            this._custmer = customer;
        }
    }
}