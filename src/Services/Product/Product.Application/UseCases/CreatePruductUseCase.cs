using Domain.Common.Commands;
using Product.Application.Commands;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Product.Application.UseCases
{
    public class CreatePruductUseCase : CommandHandler<CreateProductCommand>
    {
        public Task HandleAsync(CreateProductCommand command)
        {
            throw new NotImplementedException();
        }
    }
}
