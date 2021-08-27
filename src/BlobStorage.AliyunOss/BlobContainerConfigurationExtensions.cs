using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlobStorage.AliyunOss
{
    public static class BlobContainerConfigurationExtensions
    {
        public static BlobContainerConfiguration UseAliyunOss(
            this BlobContainerConfiguration containerConfiguration,
            Action<AliyunOssOptions> aliyunConfigureAction)
        {
            Check.NotNull(aliyunConfigureAction, nameof(aliyunConfigureAction));

            ConfigBlobContainerConfiguration(
                containerConfiguration,
                new AliyunOssOptionsExtension(aliyunConfigureAction));
            return containerConfiguration;
        }

        public static BlobContainerConfiguration UseAliyunOss(
            this BlobContainerConfiguration containerConfiguration,
            IConfiguration configuration)
        {
            Check.NotNull(configuration, nameof(configuration));

            ConfigBlobContainerConfiguration(
                containerConfiguration,
                new AliyunOssOptionsExtension(configuration));

            return containerConfiguration;
        }

        private static void ConfigBlobContainerConfiguration(
            BlobContainerConfiguration containerConfiguration,
            IBlobStorageOptionsExtension extension)
        {
            containerConfiguration.ProviderType = typeof(AliyunOssBlobProvider);
            containerConfiguration.NamingValidatorType = typeof(AliyunOssBlobNamingValidator);
            containerConfiguration.RegisterExtension(extension);
        }
    }
}
