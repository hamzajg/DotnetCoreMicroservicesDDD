using System;
using Services.Common.Commands;

namespace Customer.App.Commands
{
    public class RegisterCustomer : ICommand
    {

        public Guid Id { get; set; }
        public string FirstName { get; set; }
        public string LasrName { get; set; }
        public string Address { get; set; }
        public string PhoneNumber { get; set; }
        public string Email { get; set; }


        protected RegisterCustomer()
        {

        }
        public RegisterCustomer(string firstName, string lastName, string email, 
                                                    string phoneNumber, string address)
        {
            FirstName = firstName;
            LasrName = lastName;
            Email = email;
            PhoneNumber = phoneNumber;
            Address = address;
        }
    }
}