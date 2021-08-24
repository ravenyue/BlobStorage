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
            Action<AmazonS3Options> aliyunConfigureAction)
        {
            Check.NotNull(aliyunConfigureAction, nameof(aliyunConfigureAction));

            containerConfiguration.ProviderType = typeof(AmazonS3BlobProvider);
            containerConfiguration.RegisterExtension(new AmazonS3OptionsExtension(aliyunConfigureAction));

            return containerConfiguration;
        }

        public static BlobContainerConfiguration UseAmazonS3(
            this BlobContainerConfiguration containerConfiguration,
            IConfiguration configuration)
        {
            Check.NotNull(configuration, nameof(configuration));

            containerConfiguration.ProviderType = typeof(AmazonS3BlobProvider);
            containerConfiguration.RegisterExtension(new AmazonS3OptionsExtension(configuration));

            return containerConfiguration;
        }
    }
}
