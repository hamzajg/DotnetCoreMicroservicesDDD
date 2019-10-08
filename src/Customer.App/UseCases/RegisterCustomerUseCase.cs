using System.Threading.Tasks;

namespace Customer.App.UseCases
{
    public class RegisterCustomerUseCase : IRegisterCustomerUseCase
    {
        public RegisterCustomerUseCase()
        {
        }
        
        public Task HandleAsync(RegisterCustomerCommandMessage command) 
        {
            var event = new CustomerRegistered();
            return Task.CompletedTask;
        } 
    }
}