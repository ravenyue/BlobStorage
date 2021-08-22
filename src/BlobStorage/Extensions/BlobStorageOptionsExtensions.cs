using System;

namespace BlobStorage
{
    public static class BlobStorageOptionsExtensions
    {
        public static BlobStorageOptions ConfigureContainer<TContainer>(
            this BlobStorageOptions options,
            Action<BlobContainerConfiguration> configureAction)
        {
            options.Containers.Configure<TContainer>(configureAction);
            return options;
        }

        public static BlobStorageOptions ConfigureContainer(
            this BlobStorageOptions options,
            string name,
            Action<BlobContainerConfiguration> configureAction)
        {
            options.Containers.Configure(name, configureAction);
            return options;
        }

        public static BlobStorageOptions ConfigureDefaultContainer(
            this BlobStorageOptions options,
            Action<BlobContainerConfiguration> configureAction)
        {
            options.Containers.ConfigureDefault(configureAction);
            return options;
        }

        public static BlobStorageOptions ConfigureAllContainer(
            this BlobStorageOptions options,
            Action<string, BlobContainerConfiguration> configureAction)
        {
            options.Containers.ConfigureAll(configureAction);
            return options;
        }
    }
}
