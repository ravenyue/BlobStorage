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

            if (configuration.ProviderType == null)
            {
                throw new Exception($"Container {containerName} has no provider set");
            }

            var provider = ServiceProvider.GetService(configuration.ProviderType);

            if (provider == null)
            {
                throw new Exception(
                    $"Could not find the BLOB Storage provider with the type ({configuration.ProviderType.AssemblyQualifiedName}) configured for the container {containerName} and no default provider was set."
                );
            }

            if (provider is IBlobProvider blobProvider)
            {
                return blobProvider;
            }

            throw new Exception(
                $"Type ({configuration.ProviderType.AssemblyQualifiedName}) does not implement the ({typeof(IBlobProvider).AssemblyQualifiedName}) interface"
            );
        }
    }
}
