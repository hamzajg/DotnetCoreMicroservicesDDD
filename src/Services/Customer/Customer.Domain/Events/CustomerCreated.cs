using Services.Common.Domain.Events;

namespace Customer.Domain.Events
{
    public class CustomerCreated : DomainEvent
    {
        private readonly Entites.Customer _custmer;
        public Entites.Customer Customer => _custmer;

        public CustomerCreated(Entites.Customer customer)
        {
            this._custmer = customer;
        }
    }
}