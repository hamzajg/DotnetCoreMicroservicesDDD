using Microsoft.Extensions.DependencyInjection;
using Product.Application.Services;
using Product.Application.UseCases;

namespace Product.Application.Extentions
{
    /// <summary>
    /// Composition root for the Product Application layer — registers the use
    /// cases and their collaborators so each WebApi host only calls
    /// AddUseCases(). Mirrors Customer's ServiceCollectionExt.
    /// </summary>
    public static class ServiceCollectionExt
    {
        public static void AddUseCases(this IServiceCollection services)
        {
            services.AddSingleton<PublisherServices, InMemoryPublisherServices>();
            services.AddSingleton<CreateProductUseCase, CreatePruductUseCase>();
        }
    }
}
