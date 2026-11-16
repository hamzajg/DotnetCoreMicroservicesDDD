using Domain.Common.Commands;
using Product.Application.Commands;

namespace Product.Application.UseCases
{
    /// <summary>
    /// Contract for creating a product. The WebApi layer depends on this
    /// abstraction, not the implementation — mirrors Customer's use-case
    /// interface + impl split.
    /// </summary>
    public interface CreateProductUseCase : CommandHandler<CreateProductCommand>
    {
    }
}
