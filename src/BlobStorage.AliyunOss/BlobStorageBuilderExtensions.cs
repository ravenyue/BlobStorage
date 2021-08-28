using Microsoft.Extensions.Configuration;
using System;

namespace BlobStorage.AliyunOss
{
    public static class BlobStorageBuilderExtensions
    {
        public static BlobStorageBuilder AddAliyunOss<TContainer>(
            this BlobStorageBuilder builder,
            Action<AliyunOssOptions> configureAction)
        {
            builder.ConfigureContainer<TContainer>(container =>
            {
                container.UseAliyunOssConfig();
            }, new AliyunOssOptionsExtension(configureAction));

            return builder;
        }

        public static BlobStorageBuilder AddAliyunOss(
            this BlobStorageBuilder builder,
            string name,
            Action<AliyunOssOptions> configureAction)
        {
            builder.ConfigureContainer(name, container =>
            {
                container.UseAliyunOssConfig();
            }, new AliyunOssOptionsExtension(configureAction));

            return builder;
        }

        public static BlobStorageBuilder AddAliyunOss(
            this BlobStorageBuilder builder,
            Action<AliyunOssOptions> configureAction)
        {
            builder.ConfigureDefaultContainer(container =>
            {
                container.UseAliyunOssConfig();
            }, new AliyunOssOptionsExtension(configureAction));

            return builder;
        }

        public static BlobStorageBuilder AddAliyunOss<TContainer>(
            this BlobStorageBuilder builder,
            IConfiguration configuration)
        {
            builder.ConfigureContainer<TContainer>(container =>
            {
                container.UseAliyunOssConfig();
            }, new AliyunOssOptionsExtension(configuration));

            return builder;
        }

        public static BlobStorageBuilder AddAliyunOss(
            this BlobStorageBuilder builder,
            string name,
            IConfiguration configuration)
        {
            builder.ConfigureContainer(name, container =>
            {
                container.UseAliyunOssConfig();
            }, new AliyunOssOptionsExtension(configuration));

            return builder;
        }

        public static BlobStorageBuilder AddAliyunOss(
            this BlobStorageBuilder builder,
            IConfiguration configuration)
        {
            builder.ConfigureDefaultContainer(container =>
            {
                container.UseAliyunOssConfig();
            }, new AliyunOssOptionsExtension(configuration));

            return builder;
        }
    }
}
