using Domain.Common.Events;

namespace Customer.Domain.Events
{
    public class CustomerRegistered : DomainEvent
    {

        public Entites.Customer Customer;

        public CustomerRegistered(Entites.Customer customer) {
            this.Customer = customer;
        }
        
    }
}