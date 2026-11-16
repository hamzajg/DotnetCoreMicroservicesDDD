using System.Threading.Tasks;
using Domain.Common.Commands;
using Order.Application.Commands;

namespace Order.Application.UseCases
{
    /// <summary>
    /// Contract for placing an order. The WebApi layer depends on this
    /// abstraction, not the implementation — mirrors Customer's and Product's
    /// use-case interface + impl split.
    /// </summary>
    public interface CreateOrderUseCase : CommandHandler<CreateOrderCommand>
    {
    }
}
