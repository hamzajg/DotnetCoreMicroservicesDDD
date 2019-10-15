using System;
using System.Linq;
using System.Threading.Tasks;
using Customer.App.Domain;
using Customer.App.Events;
using Customer.App.Services;
using Customer.App.Commands;

namespace Customer.App.UseCases
{
    public class RegisterCustomerUseCase : IRegisterCustomerUseCase
    {

        private readonly IPublisherServices _pubServices;
        public RegisterCustomerUseCase(IPublisherServices pubServices)
        {
            this._pubServices = pubServices;
        }
        
        public async Task HandleAsync(RegisterCustomer command) 
        {
            if(command == null)
                throw new Exception();
            await  _pubServices.PublishAsync(new Domain.Customer(command.FirstName, command.LasrName, command.Email,
             new PhoneNumber(command.PhoneNumber), new Address(command.Address)).DomainEvents.LastOrDefault());
        } 
    }
}