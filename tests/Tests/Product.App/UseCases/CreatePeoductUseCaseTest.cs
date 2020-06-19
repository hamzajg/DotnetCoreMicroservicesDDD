using Product.Application.Commands;
using Product.Application.UseCases;
using System;
using Xunit;

namespace Tests.Product.App.UseCases
{
    public class CreatePeoductUseCaseTest : IDisposable
    {
        public void Dispose()
        {
        }

        [Fact]
        public void ShouldCreateProductFromCommand()
        {
            // GIVEN
            var _createProductUseCase = new CreatePruductUseCase();
            _createProductUseCase.HandleAsync(new CreateProductCommand() { Title = "Test" });

        }
    }
}
