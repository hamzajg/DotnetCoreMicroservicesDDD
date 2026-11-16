using Product.Application.Commands;
using Product.Application.Services;
using Product.Application.UseCases;
using System;
using System.Threading.Tasks;
using Xunit;

namespace Tests.Product.App.UseCases
{
    public class CreatePeoductUseCaseTest : IDisposable
    {
        private readonly InMemoryPublisherServices _publisher = new InMemoryPublisherServices();

        public void Dispose()
        {
        }

        private static CreateProductCommand ValidCommand() => new CreateProductCommand
        {
            Title = "Test Product",
            OldPrice = 100.0f,
            CurrentPrice = 80.0f,
        };

        [Fact]
        public async Task CreatingAProductPublishesAProductCreatedEvent()
        {
            // GIVEN a use case over an in-memory publisher
            var useCase = new CreatePruductUseCase(_publisher);

            // WHEN a valid command is handled
            await useCase.HandleAsync(ValidCommand());

            // THEN exactly one domain event was published
            Assert.Single(_publisher.Events);
        }

        [Fact]
        public async Task ANullCommandIsRejected()
        {
            var useCase = new CreatePruductUseCase(_publisher);

            await Assert.ThrowsAsync<ArgumentNullException>(() => useCase.HandleAsync(null));
        }

        [Fact]
        public async Task AnEmptyTitleIsRejectedAndNothingIsPublished()
        {
            var useCase = new CreatePruductUseCase(_publisher);

            await Assert.ThrowsAsync<ArgumentNullException>(() => useCase.HandleAsync(new CreateProductCommand
            {
                Title = "",
                OldPrice = 10.0f,
                CurrentPrice = 5.0f,
            }));

            Assert.Empty(_publisher.Events);
        }
    }
}
