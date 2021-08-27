using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlobStorage.AmazonS3
{
    public static class BlobContainerConfigurationExtensions
    {
        public static BlobContainerConfiguration UseAmazonS3(
            this BlobContainerConfiguration containerConfiguration,
            Action<AmazonS3Options> s3ConfigureAction)
        {
            Check.NotNull(s3ConfigureAction, nameof(s3ConfigureAction));

            ConfigBlobContainerConfiguration(
                containerConfiguration,
                new AmazonS3OptionsExtension(s3ConfigureAction));

            return containerConfiguration;
        }

        public static BlobContainerConfiguration UseAmazonS3(
            this BlobContainerConfiguration containerConfiguration,
            IConfiguration configuration)
        {
            Check.NotNull(configuration, nameof(configuration));

            ConfigBlobContainerConfiguration(
                containerConfiguration,
                new AmazonS3OptionsExtension(configuration));

            return containerConfiguration;
        }

        private static void ConfigBlobContainerConfiguration(
            BlobContainerConfiguration containerConfiguration,
            IBlobStorageOptionsExtension extension)
        {
            containerConfiguration.ProviderType = typeof(AmazonS3BlobProvider);
            containerConfiguration.NamingValidatorType = typeof(AmazonS3BlobNamingValidator);
            containerConfiguration.RegisterExtension(extension);
        }
    }
}
