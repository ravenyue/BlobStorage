using Microsoft.Extensions.Configuration;
using System;

namespace BlobStorage.AzureBlob
{
    public static class BlobContainerConfigurationExtensions
    {
        public static BlobContainerConfiguration UseAzureBlob(
            this BlobContainerConfiguration container,
            Action<AzureBlobOptions> configureAction)
        {
            Check.NotNull(configureAction, nameof(configureAction));

            container.UseAzureBlobConfig();
            container.RegisterExtension(new AzureBlobOptionsExtension(configureAction));

            return container;
        }

        public static BlobContainerConfiguration UseAzureBlob(
            this BlobContainerConfiguration container,
            IConfiguration configuration)
        {
            Check.NotNull(configuration, nameof(configuration));

            container.UseAzureBlobConfig();
            container.RegisterExtension(new AzureBlobOptionsExtension(configuration));

            return container;
        }

        internal static BlobContainerConfiguration UseAzureBlobConfig(
            this BlobContainerConfiguration container)
        {
            container.ProviderType = typeof(AzureBlobProvider);
            container.NamingValidatorType = typeof(AzureBlobNamingValidator);

            return container;
        }
    }
}
