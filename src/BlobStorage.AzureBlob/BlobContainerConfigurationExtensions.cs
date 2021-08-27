using Microsoft.Extensions.Configuration;
using System;

namespace BlobStorage.AzureBlob
{
    public static class BlobContainerConfigurationExtensions
    {
        public static BlobContainerConfiguration UseAzureBlob(
            this BlobContainerConfiguration containerConfiguration,
            Action<AzureBlobOptions> azureConfigureAction)
        {
            Check.NotNull(azureConfigureAction, nameof(azureConfigureAction));

            ConfigBlobContainerConfiguration(
                containerConfiguration,
                new AzureBlobOptionsExtension(azureConfigureAction));

            return containerConfiguration;
        }

        public static BlobContainerConfiguration UseAzureBlob(
            this BlobContainerConfiguration containerConfiguration,
            IConfiguration configuration)
        {
            Check.NotNull(configuration, nameof(configuration));

            ConfigBlobContainerConfiguration(
                containerConfiguration,
                new AzureBlobOptionsExtension(configuration));

            return containerConfiguration;
        }

        private static void ConfigBlobContainerConfiguration(
            BlobContainerConfiguration containerConfiguration,
            IBlobStorageOptionsExtension extension)
        {
            containerConfiguration.ProviderType = typeof(AzureBlobProvider);
            containerConfiguration.NamingValidatorType = typeof(AzureBlobNamingValidator);
            containerConfiguration.RegisterExtension(extension);
        }
    }
}
