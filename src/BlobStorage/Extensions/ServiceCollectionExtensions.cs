using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using System;

namespace BlobStorage
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddBlobStorage(this IServiceCollection services, Action<BlobStorageOptions> setupAction)
        {
            if (setupAction == null)
            {
                throw new ArgumentNullException(nameof(setupAction));
            }

            services.TryAddSingleton<IBlobNamingValidatorSelector, BlobNamingValidatorSelector>();
            services.TryAddSingleton<IBlobProviderSelector, DefaultBlobProviderSelector>();
            services.TryAddSingleton<IBlobContainerFactory, BlobContainerFactory>();
            services.TryAddSingleton<IBlobContainer, BlobContainer<DefaultContainer>>();           
            services.TryAddSingleton(typeof(IBlobContainer<>), typeof(BlobContainer<>));

            var options = new BlobStorageOptions();
            setupAction(options);
            foreach (var serviceExtension in options.Extensions)
            {
                serviceExtension.AddServices(services);
            }
            foreach (var serviceExtension in options.Containers.GetExtensions())
            {
                serviceExtension.AddServices(services);
            }

            services.Configure(setupAction);

            return services;
        }
    }
}
