using Services.Common.Domain.Events;

namespace Customer.App.Events
{
    public class CustomerRegistered : DomainEvent
    {

        private readonly Domain.Customer _customer;

        public CustomerRegistered(Domain.Customer customer) {
            this._customer = customer;
        }
        
    }
}