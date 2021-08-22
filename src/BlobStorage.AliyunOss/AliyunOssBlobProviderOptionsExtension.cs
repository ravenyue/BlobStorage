using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using System;

namespace BlobStorage.AliyunOss
{
    public class AliyunOssBlobProviderOptionsExtension : IBlobStorageOptionsExtension
    {
        private readonly Action<AliyunOssBlobProviderOptions> _configureAction;
        private readonly IConfiguration _configuration;

        public AliyunOssBlobProviderOptionsExtension(Action<AliyunOssBlobProviderOptions> configureAction)
        {
            _configureAction = configureAction;
        }

        public AliyunOssBlobProviderOptionsExtension(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public void AddServices(IServiceCollection services)
        {
            services.TryAddSingleton<AliyunOssBlobProvider>();

            if (_configureAction != null)
            {
                services.Configure(_configureAction);
            }
            else
            {
                services.Configure<AliyunOssBlobProviderOptions>(_configuration);
            }
        }
    }
}
