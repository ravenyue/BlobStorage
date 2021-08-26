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

            containerConfiguration.ProviderType = typeof(AliyunOssBlobProvider);
            containerConfiguration.RegisterExtension(new AliyunOssOptionsExtension(aliyunConfigureAction));

            return containerConfiguration;
        }

        public static BlobContainerConfiguration UseAliyunOss(
            this BlobContainerConfiguration containerConfiguration,
            IConfiguration configuration)
        {
            Check.NotNull(configuration, nameof(configuration));

            containerConfiguration.ProviderType = typeof(AliyunOssBlobProvider);
            containerConfiguration.RegisterExtension(new AliyunOssOptionsExtension(configuration));

            return containerConfiguration;
        }
    }
}
