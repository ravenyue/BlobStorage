using Microsoft.Extensions.Configuration;
using System;

namespace BlobStorage.Minio
{
    public static class BlobContainerConfigurationExtensions
    {
        public static BlobContainerConfiguration UseMinio(
            this BlobContainerConfiguration container,
            Action<MinioOptions> configureAction)
        {
            Check.NotNull(configureAction, nameof(configureAction));

            container.UseMinioConfig();
            container.RegisterExtension(new MinioOptionsExtension(configureAction));

            return container;
        }

        public static BlobContainerConfiguration UseMinio(
            this BlobContainerConfiguration container,
            IConfiguration configuration)
        {
            Check.NotNull(configuration, nameof(configuration));

            container.UseMinioConfig();
            container.RegisterExtension(new MinioOptionsExtension(configuration));

            return container;
        }

        internal static BlobContainerConfiguration UseMinioConfig(
            this BlobContainerConfiguration container)
        {
            container.ProviderType = typeof(MinioBlobProvider);
            container.NamingValidatorType = typeof(MinioBlobNamingValidator);

            return container;
        }
    }
}
