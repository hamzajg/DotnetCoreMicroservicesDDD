using Customer.App.Services;
using Customer.App.UseCases;
using Microsoft.Extensions.DependencyInjection;

namespace Customer.App.Extentions
{
    public static class ServiceCollectionExt
    {

        public static void AddUseCases(this IServiceCollection services)
        {
            services.AddSingleton<PublisherServices, InMemoryPublisherServices>();
            // The interface was registered as its own implementation, so the
            // container threw "Cannot instantiate implementation type" while
            // building the provider and the service never started.
            services.AddSingleton<RegisterCustomerUseCase, RegisterCustomerUseCaseImpl>();
        }
    }
}
