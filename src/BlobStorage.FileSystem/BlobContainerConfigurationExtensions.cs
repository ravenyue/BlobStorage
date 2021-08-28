using Microsoft.Extensions.Configuration;
using System;

namespace BlobStorage.FileSystem
{
    public static class BlobContainerConfigurationExtensions
    {
        public static BlobContainerConfiguration UseFileSystem(
            this BlobContainerConfiguration container,
            Action<FileSystemOptions> configureAction)
        {
            Check.NotNull(configureAction, nameof(configureAction));

            container.UseFileSystemConfig();
            container.RegisterExtension(new FileSystemOptionsExtension(configureAction));

            return container;
        }

        public static BlobContainerConfiguration UseFileSystem(
            this BlobContainerConfiguration container,
            IConfiguration configuration)
        {
            Check.NotNull(configuration, nameof(configuration));

            container.UseFileSystemConfig();
            container.RegisterExtension(new FileSystemOptionsExtension(configuration));

            return container;
        }

        internal static BlobContainerConfiguration UseFileSystemConfig(
            this BlobContainerConfiguration container)
        {
            container.ProviderType = typeof(FileSystemBlobProvider);
            container.NamingValidatorType = typeof(FileSystemBlobNamingValidator);

            return container;
        }
    }
}
