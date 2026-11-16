using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging.Abstractions;
using Product.Application.Commands;
using Product.Application.Services;
using Product.Application.UseCases;
using Product.WebApi.Controllers;
using Xunit;

namespace Tests.Product.WebApi.Controllers
{
    public class ProductsControllerTests
    {
        private readonly InMemoryPublisherServices _publisher = new InMemoryPublisherServices();

        private ProductsController Controller() =>
            new ProductsController(
                NullLogger<ProductsController>.Instance,
                new CreatePruductUseCase(_publisher));

        private static CreateProductCommand ValidCommand() => new CreateProductCommand
        {
            Title = "Test Product",
            OldPrice = 100.0f,
            CurrentPrice = 80.0f,
        };

        [Fact]
        public async Task PostProductsReturns202Accepted()
        {
            // GIVEN a controller over the product use case
            var command = ValidCommand();

            // WHEN a valid command is posted
            var result = await Controller().Post(command);

            // THEN the response is 202 Accepted echoing the accepted product
            var accepted = Assert.IsType<AcceptedResult>(result);
            Assert.Equal(202, accepted.StatusCode);
            Assert.Same(command, accepted.Value);
        }

        [Fact]
        public async Task PostProductsPublishesAProductCreatedEvent()
        {
            // GIVEN a controller over a use case with an in-memory publisher
            // WHEN a valid command is posted
            await Controller().Post(ValidCommand());

            // THEN the publisher recorded exactly one domain event
            Assert.Single(_publisher.Events);
        }

        [Fact]
        public async Task AnEmptyTitleOverTheEndpointIsSurfacedAsAnError()
        {
            // GIVEN a controller over the product use case
            var command = new CreateProductCommand { Title = "", OldPrice = 10.0f, CurrentPrice = 5.0f };

            // WHEN a command with an empty title is posted
            // THEN an error surfaces rather than an Accepted response
            await Assert.ThrowsAsync<ArgumentNullException>(() => Controller().Post(command));
            Assert.Empty(_publisher.Events);
        }
    }
}
