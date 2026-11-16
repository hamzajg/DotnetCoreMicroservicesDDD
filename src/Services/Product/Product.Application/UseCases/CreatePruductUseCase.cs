using Domain.Common.Commands;
using Product.Application.Commands;
using Product.Application.Services;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Product.Application.UseCases
{
    /// <summary>
    /// Creates a Product aggregate and publishes the domain event it raises
    /// (ProductCreated) through the injected publisher. Mirrors
    /// Customer's RegisterCustomerUseCaseImpl so the Application layer of both
    /// services behaves identically.
    /// </summary>
    public class CreatePruductUseCase : CreateProductUseCase
    {
        private readonly PublisherServices _pubServices;

        public CreatePruductUseCase(PublisherServices pubServices)
        {
            _pubServices = pubServices;
        }

        public async Task HandleAsync(CreateProductCommand command)
        {
            if (command == null)
                throw new ArgumentNullException(nameof(command));

            var product = Product.Domain.Entities.Product.Of(command.Title, command.OldPrice, command.CurrentPrice);
            await _pubServices.PublishAsync(product.DomainEvents.LastOrDefault());
        }
    }
}
