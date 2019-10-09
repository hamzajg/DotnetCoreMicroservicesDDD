using Services.Common.Domain.Events;

namespace Customer.App.Events
{
    public class CustomerRegistered : DomainEvent
    {

        public Domain.Customer Customer;

        public CustomerRegistered(Domain.Customer customer) {
            this.Customer = customer;
        }
        
    }
}