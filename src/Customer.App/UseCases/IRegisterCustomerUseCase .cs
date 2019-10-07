using System.Threading.Tasks;
using Services.Common.Commands;
namespace Customer.App.UseCases
{
    public interface IRegisterCustomerUseCase : ICommandHandler<RegisterCustomerCommandMessage>
    {
    }
}