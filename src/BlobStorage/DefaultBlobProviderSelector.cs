using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlobStorage
{
    public class DefaultBlobProviderSelector : IBlobProviderSelector
    {
        protected BlobStorageOptions Options { get; }
        protected IServiceProvider ServiceProvider;

        public DefaultBlobProviderSelector(
            IOptions<BlobStorageOptions> options,
            IServiceProvider serviceProvider)
        {
            Options = options.Value;
            ServiceProvider = serviceProvider;
        }

        public virtual IBlobProvider Get(string containerName)
        {
            Check.NotNull(containerName, nameof(containerName));

            var configuration = Options.GetConfiguration(containerName);
            var providerType = configuration.ProviderType;

            if (providerType == null)
            {
                throw new InvalidOperationException($"Container {containerName} has no provider set");
            }

            var provider = ServiceProvider.GetService(configuration.ProviderType);

            if (provider == null)
            {
                throw new InvalidOperationException($"Type {providerType.AssemblyQualifiedName} is not registered");
            }

            if (provider is IBlobProvider blobProvider)
            {
                return blobProvider;
            }

            throw new InvalidOperationException(
                $"Type ({providerType.AssemblyQualifiedName}) does not implement the ({typeof(IBlobProvider).AssemblyQualifiedName}) interface"
            );
        }
    }
}
