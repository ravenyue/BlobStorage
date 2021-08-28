using Microsoft.Extensions.DependencyInjection;
using System;

namespace BlobStorage
{
    public class BlobStorageBuilder
    {
        public BlobStorageBuilder(IServiceCollection services)
            => Services = services;

        public virtual IServiceCollection Services { get; }


        public BlobStorageBuilder ConfigureContainer<TContainer>(
            Action<BlobContainerConfiguration> configureAction,
            IBlobStorageOptionsExtension extension = null)
        {
            Services.Configure<BlobStorageOptions>(o =>
            {
                o.ConfigureContainer<TContainer>(configureAction);
            });

            if (extension != null)
            {
                extension.AddServices(Services);
            }
            return this;
        }

        public BlobStorageBuilder ConfigureContainer(
            string name,
            Action<BlobContainerConfiguration> configureAction,
            IBlobStorageOptionsExtension extension = null)
        {
            Services.Configure<BlobStorageOptions>(o =>
            {
                o.ConfigureContainer(name, configureAction);
            });

            if (extension != null)
            {
                extension.AddServices(Services);
            }
            return this;
        }

        public BlobStorageBuilder ConfigureDefaultContainer(
            Action<BlobContainerConfiguration> configureAction,
            IBlobStorageOptionsExtension extension = null)
        {
            Services.Configure<BlobStorageOptions>(o =>
            {
                o.ConfigureDefaultContainer(configureAction);
            });

            if (extension != null)
            {
                extension.AddServices(Services);
            }
            return this;
        }
    }
}
