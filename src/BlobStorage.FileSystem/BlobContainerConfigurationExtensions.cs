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
            Action<FileSystemOptions> fileSystemConfigureAction)
        {
            Check.NotNull(fileSystemConfigureAction, nameof(fileSystemConfigureAction));

            ConfigBlobContainerConfiguration(
                containerConfiguration,
                new FileSystemOptionsExtension(fileSystemConfigureAction));

            return containerConfiguration;
        }

        public static BlobContainerConfiguration UseFileSystem(
            this BlobContainerConfiguration containerConfiguration,
            IConfiguration configuration)
        {
            Check.NotNull(configuration, nameof(configuration));

            ConfigBlobContainerConfiguration(
                containerConfiguration,
                new FileSystemOptionsExtension(configuration));

            return containerConfiguration;
        }

        private static void ConfigBlobContainerConfiguration(
            BlobContainerConfiguration containerConfiguration,
            IBlobStorageOptionsExtension extension)
        {
            containerConfiguration.ProviderType = typeof(FileSystemBlobProvider);
            containerConfiguration.NamingValidatorType = typeof(FileSystemBlobNamingValidator);
            containerConfiguration.RegisterExtension(extension);
        }
    }
}
