using Microsoft.Extensions.Configuration;
using System;

namespace BlobStorage.AmazonS3
{
    public static class BlobContainerConfigurationExtensions
    {
        public static BlobContainerConfiguration UseAmazonS3(
            this BlobContainerConfiguration container,
            Action<AmazonS3Options> configureAction)
        {
            Check.NotNull(configureAction, nameof(configureAction));

            container.UseAmazonS3Config();
            container.RegisterExtension(new AmazonS3OptionsExtension(configureAction));

            return container;
        }

        public static BlobContainerConfiguration UseAmazonS3(
            this BlobContainerConfiguration container,
            IConfiguration configuration)
        {
            Check.NotNull(configuration, nameof(configuration));

            container.UseAmazonS3Config();
            container.RegisterExtension(new AmazonS3OptionsExtension(configuration));

            return container;
        }

        internal static BlobContainerConfiguration UseAmazonS3Config(
            this BlobContainerConfiguration container)
        {
            container.ProviderType = typeof(AmazonS3BlobProvider);
            container.NamingValidatorType = typeof(AmazonS3BlobNamingValidator);
            
            return container;
        }
    }
}
