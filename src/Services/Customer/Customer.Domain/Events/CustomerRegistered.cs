using Services.Common.Domain.Events;

namespace Customer.Domain.Events
{
    public class CustomerRegistered : DomainEvent
    {

        public Customer.Domain.Entites.Customer Customer;

        public CustomerRegistered(Customer.Domain.Entites.Customer customer) {
            this.Customer = customer;
        }
        
    }
}