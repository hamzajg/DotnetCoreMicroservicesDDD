using System.Threading.Tasks;
using Customer.App.Commands;
using Domain.Common.Commands;

namespace Customer.App.UseCases
{
    public interface RegisterCustomerUseCase : CommandHandler<RegisterCustomer>
    {
    }
}