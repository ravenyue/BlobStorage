using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlobStorage.FileSystem
{
    public static class BlobContainerConfigurationExtensions
    {
        public static BlobContainerConfiguration UseFileSystem(
            this BlobContainerConfiguration containerConfiguration,
            Action<FileSystemBlobProviderOptions> fileSystemConfigureAction)
        {
            Check.NotNull(fileSystemConfigureAction, nameof(fileSystemConfigureAction));

            containerConfiguration.ProviderType = typeof(FileSystemBlobProvider);
            containerConfiguration.RegisterExtension(new FileSystemBlobProviderOptionsExtension(fileSystemConfigureAction));

            return containerConfiguration;
        }

        public static BlobContainerConfiguration UseFileSystem(
            this BlobContainerConfiguration containerConfiguration,
            IConfiguration configuration)
        {
            Check.NotNull(configuration, nameof(configuration));

            containerConfiguration.ProviderType = typeof(FileSystemBlobProvider);
            containerConfiguration.RegisterExtension(new FileSystemBlobProviderOptionsExtension(configuration));

            return containerConfiguration;
        }
    }
}
