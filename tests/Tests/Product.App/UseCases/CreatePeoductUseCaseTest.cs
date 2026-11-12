using Product.Application.Commands;
using Product.Application.UseCases;
using System;
using System.Threading.Tasks;
using Xunit;

namespace Tests.Product.App.UseCases
{
    public class CreatePeoductUseCaseTest : IDisposable
    {
        public void Dispose()
        {
        }

        [Fact]
        public async Task ShouldCreateProductFromCommand()
        {
            // GIVEN
            var createProductUseCase = new CreatePruductUseCase();
            var command = new CreateProductCommand()
            {
                Title = "Test Product",
                OldPrice = 100.0f,
                CurrentPrice = 80.0f
            };

            // WHEN
            await createProductUseCase.HandleAsync(command);

            // THEN completes without exception
        }

        [Fact]
        public async Task ShouldThrowWhenCommandIsNull()
        {
            var createProductUseCase = new CreatePruductUseCase();
            await Assert.ThrowsAsync<ArgumentNullException>(() => createProductUseCase.HandleAsync(null));
        }

        [Fact]
        public async Task ShouldThrowWhenTitleIsNullOrEmpty()
        {
            var createProductUseCase = new CreatePruductUseCase();
            await Assert.ThrowsAsync<ArgumentNullException>(() => createProductUseCase.HandleAsync(new CreateProductCommand()
            {
                Title = "",
                OldPrice = 10.0f,
                CurrentPrice = 5.0f
            }));
        }
    }
}
