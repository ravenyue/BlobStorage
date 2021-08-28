using Microsoft.Extensions.Configuration;
using System;

namespace BlobStorage.FileSystem
{
    public static class BlobStorageBuilderExtensions
    {
        public static BlobStorageBuilder AddFileSystem<TContainer>(
            this BlobStorageBuilder builder,
            Action<FileSystemOptions> configureAction)
        {
            builder.ConfigureContainer<TContainer>(container =>
            {
                container.UseFileSystemConfig();
            }, new FileSystemOptionsExtension(configureAction));

            return builder;
        }

        public static BlobStorageBuilder AddFileSystem(
            this BlobStorageBuilder builder,
            string name,
            Action<FileSystemOptions> configureAction)
        {
            builder.ConfigureContainer(name, container =>
            {
                container.UseFileSystemConfig();
            }, new FileSystemOptionsExtension(configureAction));

            return builder;
        }

        public static BlobStorageBuilder AddFileSystem(
            this BlobStorageBuilder builder,
            Action<FileSystemOptions> configureAction)
        {
            builder.ConfigureDefaultContainer(container =>
            {
                container.UseFileSystemConfig();
            }, new FileSystemOptionsExtension(configureAction));

            return builder;
        }

        public static BlobStorageBuilder AddFileSystem<TContainer>(
            this BlobStorageBuilder builder,
            IConfiguration configuration)
        {
            builder.ConfigureContainer<TContainer>(container =>
            {
                container.UseFileSystemConfig();
            }, new FileSystemOptionsExtension(configuration));

            return builder;
        }

        public static BlobStorageBuilder AddFileSystem(
            this BlobStorageBuilder builder,
            string name,
            IConfiguration configuration)
        {
            builder.ConfigureContainer(name, container =>
            {
                container.UseFileSystemConfig();
            }, new FileSystemOptionsExtension(configuration));

            return builder;
        }

        public static BlobStorageBuilder AddFileSystem(
            this BlobStorageBuilder builder,
            IConfiguration configuration)
        {
            builder.ConfigureDefaultContainer(container =>
            {
                container.UseFileSystemConfig();
            }, new FileSystemOptionsExtension(configuration));

            return builder;
        }
    }
}
