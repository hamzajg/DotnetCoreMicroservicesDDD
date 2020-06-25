using System;
using System.Linq;
using System.Threading.Tasks;
using Customer.App.Services;
using Customer.App.Commands;
using Entities = Customer.Domain.Entites;

namespace Customer.App.UseCases
{
    public class RegisterCustomerUseCaseImpl : RegisterCustomerUseCase
    {

        private readonly PublisherServices _pubServices;
        public RegisterCustomerUseCaseImpl(PublisherServices pubServices)
        {
            this._pubServices = pubServices;
        }
        
        public async Task HandleAsync(RegisterCustomer command) 
        {
            if(command == null)
                throw new Exception();
            await  _pubServices.PublishAsync(new Entities.Customer(command.FirstName, command.LasrName, command.Email,
             new Entities.PhoneNumber(command.PhoneNumber), new Entities.Address(command.Address)).DomainEvents.LastOrDefault());
        } 
    }
}