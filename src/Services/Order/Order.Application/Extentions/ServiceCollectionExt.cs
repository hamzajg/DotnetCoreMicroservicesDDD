using Microsoft.Extensions.DependencyInjection;
using Order.Application.Adapters;
using Order.Application.Ports;
using Order.Application.Services;
using Order.Application.UseCases;
using Saga.Common;

namespace Order.Application.Extentions
{
    /// <summary>
    /// Composition root for the Order Application layer — registers the use
    /// case, the saga orchestrator and the ports it depends on, so the WebApi
    /// host only calls AddUseCases(). Mirrors Customer's and Product's
    /// ServiceCollectionExt.
    /// </summary>
    public static class ServiceCollectionExt
    {
        public static void AddUseCases(this IServiceCollection services)
        {
            services.AddSingleton<SagaOrchestrator>();
            services.AddSingleton<PublisherServices, InMemoryPublisherServices>();
            services.AddSingleton<CustomerDirectory, InMemoryCustomerDirectory>();
            services.AddSingleton<ProductCatalog, InMemoryProductCatalog>();
            services.AddSingleton<OrderRepository, InMemoryOrderRepository>();
            services.AddSingleton<CreateOrderUseCase, CreateOrderUseCaseImpl>();
        }
    }
}
