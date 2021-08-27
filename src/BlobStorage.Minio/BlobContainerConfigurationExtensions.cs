using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlobStorage.Minio
{
    public static class BlobContainerConfigurationExtensions
    {
        public static BlobContainerConfiguration UseMinio(
            this BlobContainerConfiguration containerConfiguration,
            Action<MinioOptions> minioConfigureAction)
        {
            Check.NotNull(minioConfigureAction, nameof(minioConfigureAction));

            ConfigBlobContainerConfiguration(
                containerConfiguration,
                new MinioOptionsExtension(minioConfigureAction));

            return containerConfiguration;
        }

        public static BlobContainerConfiguration UseMinio(
            this BlobContainerConfiguration containerConfiguration,
            IConfiguration configuration)
        {
            Check.NotNull(configuration, nameof(configuration));

            ConfigBlobContainerConfiguration(
                containerConfiguration,
                new MinioOptionsExtension(configuration));

            return containerConfiguration;
        }

        private static void ConfigBlobContainerConfiguration(
            BlobContainerConfiguration containerConfiguration,
            IBlobStorageOptionsExtension extension)
        {
            containerConfiguration.ProviderType = typeof(MinioBlobProvider);
            containerConfiguration.NamingValidatorType = typeof(MinioBlobNamingValidator);
            containerConfiguration.RegisterExtension(extension);
        }
    }
}
