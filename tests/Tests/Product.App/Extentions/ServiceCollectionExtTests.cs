using Microsoft.Extensions.DependencyInjection;
using Product.Application.Extentions;
using Product.Application.Services;
using Product.Application.UseCases;
using Xunit;

namespace Tests.Product.App.Extentions
{
    public class ServiceCollectionExtTests
    {
        [Fact]
        public void AddUseCasesRegistersTheProductUseCaseAndItsPublisher()
        {
            // GIVEN an empty service collection
            var services = new ServiceCollection();

            // WHEN the Product Application use cases are registered
            services.AddUseCases();
            var provider = services.BuildServiceProvider();

            // THEN both the use case and its publisher resolve
            Assert.NotNull(provider.GetService<CreateProductUseCase>());
            Assert.NotNull(provider.GetService<PublisherServices>());
        }
    }
}
