using Microsoft.Extensions.Configuration;
using System;

namespace BlobStorage.Minio
{
    public static class BlobStorageBuilderExtensions
    {
        public static BlobStorageBuilder AddMinio<TContainer>(
            this BlobStorageBuilder builder,
            Action<MinioOptions> configureAction)
        {
            builder.ConfigureContainer<TContainer>(container =>
            {
                container.UseMinioConfig();
            }, new MinioOptionsExtension(configureAction));

            return builder;
        }

        public static BlobStorageBuilder AddMinio(
            this BlobStorageBuilder builder,
            string name,
            Action<MinioOptions> configureAction)
        {
            builder.ConfigureContainer(name, container =>
            {
                container.UseMinioConfig();
            }, new MinioOptionsExtension(configureAction));

            return builder;
        }

        public static BlobStorageBuilder AddMinio(
            this BlobStorageBuilder builder,
            Action<MinioOptions> configureAction)
        {
            builder.ConfigureDefaultContainer(container =>
            {
                container.UseMinioConfig();
            }, new MinioOptionsExtension(configureAction));

            return builder;
        }

        public static BlobStorageBuilder AddMinio<TContainer>(
            this BlobStorageBuilder builder,
            IConfiguration configuration)
        {
            builder.ConfigureContainer<TContainer>(container =>
            {
                container.UseMinioConfig();
            }, new MinioOptionsExtension(configuration));

            return builder;
        }

        public static BlobStorageBuilder AddMinio(
            this BlobStorageBuilder builder,
            string name,
            IConfiguration configuration)
        {
            builder.ConfigureContainer(name, container =>
            {
                container.UseMinioConfig();
            }, new MinioOptionsExtension(configuration));

            return builder;
        }

        public static BlobStorageBuilder AddMinio(
            this BlobStorageBuilder builder,
            IConfiguration configuration)
        {
            builder.ConfigureDefaultContainer(container =>
            {
                container.UseMinioConfig();
            }, new MinioOptionsExtension(configuration));

            return builder;
        }
    }
}
