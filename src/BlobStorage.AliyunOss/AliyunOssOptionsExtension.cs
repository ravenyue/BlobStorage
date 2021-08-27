using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using System;

namespace BlobStorage.AliyunOss
{
    public class AliyunOssOptionsExtension : IBlobStorageOptionsExtension
    {
        private readonly Action<AliyunOssOptions> _configureAction;
        private readonly IConfiguration _configuration;

        public AliyunOssOptionsExtension(Action<AliyunOssOptions> configureAction)
        {
            _configureAction = configureAction;
        }

        public AliyunOssOptionsExtension(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public void AddServices(IServiceCollection services)
        {
            services.TryAddSingleton<AliyunOssBlobProvider>();
            services.TryAddSingleton<AliyunOssBlobNamingValidator>();

            if (_configureAction != null)
            {
                services.Configure(_configureAction);
            }
            else
            {
                services.Configure<AliyunOssOptions>(_configuration);
            }
        }
    }
}
