using System;
using System.Threading.Tasks;
using Customer.App.Domain;
using Customer.App.Events;

namespace Customer.App.UseCases
{
    public class RegisterCustomerUseCase : IRegisterCustomerUseCase
    {
        public RegisterCustomerUseCase()
        {
        }
        
        public Task HandleAsync(RegisterCustomerCommandMessage command) 
        {
            if(command == null)
                throw new Exception();
            var e = new CustomerRegistered(new Domain.Customer(command.FirstName, command.LasrName, command.Email,
             new PhoneNumber(command.PhoneNumber), new Address(command.Address)));
            return Task.CompletedTask;
        } 
    }
}