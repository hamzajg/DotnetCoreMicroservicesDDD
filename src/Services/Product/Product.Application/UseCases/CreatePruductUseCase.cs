using Domain.Common.Commands;
using Product.Application.Commands;
using System;
using System.Threading.Tasks;

namespace Product.Application.UseCases
{
    public class CreatePruductUseCase : CommandHandler<CreateProductCommand>
    {
        public Task HandleAsync(CreateProductCommand command)
        {
            if (command == null)
                throw new ArgumentNullException(nameof(command));

            var product = Product.Domain.Entities.Product.Of(command.Title, command.OldPrice, command.CurrentPrice);
            return Task.CompletedTask;
        }
    }
}
