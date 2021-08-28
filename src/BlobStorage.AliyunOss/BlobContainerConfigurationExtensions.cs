using Microsoft.Extensions.Configuration;
using System;

namespace BlobStorage.AliyunOss
{
    public static class BlobContainerConfigurationExtensions
    {
        public static BlobContainerConfiguration UseAliyunOss(
            this BlobContainerConfiguration container,
            Action<AliyunOssOptions> configureAction)
        {
            Check.NotNull(configureAction, nameof(configureAction));

            container.UseAliyunOssConfig();
            container.RegisterExtension(new AliyunOssOptionsExtension(configureAction));

            return container;
        }

        public static BlobContainerConfiguration UseAliyunOss(
            this BlobContainerConfiguration container,
            IConfiguration configuration)
        {
            Check.NotNull(configuration, nameof(configuration));

            container.UseAliyunOssConfig();
            container.RegisterExtension(new AliyunOssOptionsExtension(configuration));

            return container;
        }

        internal static BlobContainerConfiguration UseAliyunOssConfig(
            this BlobContainerConfiguration container)
        {
            container.ProviderType = typeof(AliyunOssBlobProvider);
            container.NamingValidatorType = typeof(AliyunOssBlobNamingValidator);

            return container;
        }
    }
}
