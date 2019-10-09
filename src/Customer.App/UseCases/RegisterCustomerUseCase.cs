using System;
using System.Threading.Tasks;
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
            var e = new CustomerRegistered();
            return Task.CompletedTask;
        } 
    }
}