using Microsoft.Extensions.Configuration;
using System;

namespace BlobStorage.AmazonS3
{
    public static class BlobStorageBuilderExtensions
    {
        public static BlobStorageBuilder AddAmazonS3<TContainer>(
            this BlobStorageBuilder builder,
            Action<AmazonS3Options> configureAction)
        {
            builder.ConfigureContainer<TContainer>(container =>
            {
                container.UseAmazonS3Config();
            }, new AmazonS3OptionsExtension(configureAction));

            return builder;
        }

        public static BlobStorageBuilder AddAmazonS3(
            this BlobStorageBuilder builder,
            string name,
            Action<AmazonS3Options> configureAction)
        {
            builder.ConfigureContainer(name, container =>
            {
                container.UseAmazonS3Config();
            }, new AmazonS3OptionsExtension(configureAction));

            return builder;
        }

        public static BlobStorageBuilder AddAmazonS3(
            this BlobStorageBuilder builder,
            Action<AmazonS3Options> configureAction)
        {
            builder.ConfigureDefaultContainer(container =>
            {
                container.UseAmazonS3Config();
            }, new AmazonS3OptionsExtension(configureAction));

            return builder;
        }

        public static BlobStorageBuilder AddAmazonS3<TContainer>(
            this BlobStorageBuilder builder,
            IConfiguration configuration)
        {
            builder.ConfigureContainer<TContainer>(container =>
            {
                container.UseAmazonS3Config();
            }, new AmazonS3OptionsExtension(configuration));

            return builder;
        }

        public static BlobStorageBuilder AddAmazonS3(
            this BlobStorageBuilder builder,
            string name,
            IConfiguration configuration)
        {
            builder.ConfigureContainer(name, container =>
            {
                container.UseAmazonS3Config();
            }, new AmazonS3OptionsExtension(configuration));

            return builder;
        }

        public static BlobStorageBuilder AddAmazonS3(
            this BlobStorageBuilder builder,
            IConfiguration configuration)
        {
            builder.ConfigureDefaultContainer(container =>
            {
                container.UseAmazonS3Config();
            }, new AmazonS3OptionsExtension(configuration));

            return builder;
        }
    }
}
