using System;
using Customer.Domain.Events;
using Services.Common.Domain;

namespace Customer.Domain.Entites
{
    public class Customer : AggregateRoot<Guid>
    {
        private string _firstName;
        private string _lastName;

        private string _email;
        private PhoneNumber _phoneNumber;
        private Address _address;

        public string FirstName { get => _firstName; private set => _firstName = value; }
        public string LastName { get => _lastName; private set => _lastName = value; }
        public PhoneNumber PhoneNumber { get => _phoneNumber; private set => _phoneNumber = value; }
        public Address Address { get => _address; private set => _address = value; }
        public string Email { get => _email; private set => _email = value; }

        public Customer(string firstName, string lastName, string email, PhoneNumber phoneNumber, Address address)
        {
            FirstName = firstName ?? throw new ArgumentNullAggregateException(nameof(firstName));
            LastName = lastName ?? throw new ArgumentNullAggregateException(nameof(lastName));
            Email = email ?? throw new ArgumentNullAggregateException(nameof(email));
            PhoneNumber = phoneNumber ?? throw new ArgumentNullAggregateException(nameof(phoneNumber));
            Address = address ?? throw new ArgumentNullAggregateException(nameof(address));

            AddDomainEvent(new CustomerCreated(this));
        }

    }
}