using Microsoft.Extensions.Configuration;
using System;

namespace BlobStorage.AzureBlob
{
    public static class BlobStorageBuilderExtensions
    {
        public static BlobStorageBuilder AddAzureBlob<TContainer>(
            this BlobStorageBuilder builder,
            Action<AzureBlobOptions> configureAction)
        {
            builder.ConfigureContainer<TContainer>(container =>
            {
                container.UseAzureBlobConfig();
            }, new AzureBlobOptionsExtension(configureAction));

            return builder;
        }

        public static BlobStorageBuilder AddAzureBlob(
            this BlobStorageBuilder builder,
            string name,
            Action<AzureBlobOptions> configureAction)
        {
            builder.ConfigureContainer(name, container =>
            {
                container.UseAzureBlobConfig();
            }, new AzureBlobOptionsExtension(configureAction));

            return builder;
        }

        public static BlobStorageBuilder AddAzureBlob(
            this BlobStorageBuilder builder,
            Action<AzureBlobOptions> configureAction)
        {
            builder.ConfigureDefaultContainer(container =>
            {
                container.UseAzureBlobConfig();
            }, new AzureBlobOptionsExtension(configureAction));

            return builder;
        }

        public static BlobStorageBuilder AddAzureBlob<TContainer>(
            this BlobStorageBuilder builder,
            IConfiguration configuration)
        {
            builder.ConfigureContainer<TContainer>(container =>
            {
                container.UseAzureBlobConfig();
            }, new AzureBlobOptionsExtension(configuration));

            return builder;
        }

        public static BlobStorageBuilder AddAzureBlob(
            this BlobStorageBuilder builder,
            string name,
            IConfiguration configuration)
        {
            builder.ConfigureContainer(name, container =>
            {
                container.UseAzureBlobConfig();
            }, new AzureBlobOptionsExtension(configuration));

            return builder;
        }

        public static BlobStorageBuilder AddAzureBlob(
            this BlobStorageBuilder builder,
            IConfiguration configuration)
        {
            builder.ConfigureDefaultContainer(container =>
            {
                container.UseAzureBlobConfig();
            }, new AzureBlobOptionsExtension(configuration));

            return builder;
        }
    }
}
