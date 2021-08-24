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
            Action<MinioOptions> aliyunConfigureAction)
        {
            Check.NotNull(aliyunConfigureAction, nameof(aliyunConfigureAction));

            containerConfiguration.ProviderType = typeof(MinioBlobProvider);
            containerConfiguration.RegisterExtension(new MinioOptionsExtension(aliyunConfigureAction));

            return containerConfiguration;
        }

        public static BlobContainerConfiguration UseMinio(
            this BlobContainerConfiguration containerConfiguration,
            IConfiguration configuration)
        {
            Check.NotNull(configuration, nameof(configuration));

            containerConfiguration.ProviderType = typeof(MinioBlobProvider);
            containerConfiguration.RegisterExtension(new MinioOptionsExtension(configuration));

            return containerConfiguration;
        }
    }
}
