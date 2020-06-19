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
            services.AddSingleton<RegisterCustomerUseCase, RegisterCustomerUseCase>();
        }
    }
}
