using Customer.App.Domain;
using Services.Common.Domain.Events;
namespace Customer.App.Events
{
    public class CustomerCreated : DomainEvent
    {
        public CustomerCreated(string firstName, string lastName, string email, PhoneNumber phoneNumber, Address address)
        {
        }
    }
}