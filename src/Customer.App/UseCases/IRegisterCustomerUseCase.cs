using System.Threading.Tasks;
using Customer.App.Commands;
using Services.Common.Commands;
namespace Customer.App.UseCases
{
    public interface IRegisterCustomerUseCase : ICommandHandler<RegisterCustomer>
    {
    }
}