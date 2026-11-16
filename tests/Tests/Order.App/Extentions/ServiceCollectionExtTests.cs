using Microsoft.Extensions.DependencyInjection;
using Order.Application.Extentions;
using Order.Application.Ports;
using Order.Application.Services;
using Order.Application.UseCases;
using Saga.Common;
using Xunit;

namespace Tests.Order.App.Extentions
{
    public class ServiceCollectionExtTests
    {
        // Scenario: AddUseCases registers the order saga and its collaborators
        [Fact]
        public void AddUseCasesRegistersTheOrderSagaAndItsCollaborators()
        {
            // GIVEN an empty service collection
            var services = new ServiceCollection();

            // WHEN the Order Application use cases are registered
            services.AddUseCases();
            var provider = services.BuildServiceProvider();

            // THEN the order use case, its saga orchestrator and its
            // collaborators resolve from the collection
            Assert.NotNull(provider.GetService<CreateOrderUseCase>());
            Assert.NotNull(provider.GetService<SagaOrchestrator>());
            Assert.NotNull(provider.GetService<CustomerDirectory>());
            Assert.NotNull(provider.GetService<ProductCatalog>());
            Assert.NotNull(provider.GetService<OrderRepository>());
            Assert.NotNull(provider.GetService<PublisherServices>());
        }
    }
}
