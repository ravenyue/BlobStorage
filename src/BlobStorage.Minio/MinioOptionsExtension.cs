using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using System;

namespace BlobStorage.Minio
{
    public class MinioOptionsExtension : IBlobStorageOptionsExtension
    {
        private readonly Action<MinioOptions> _configureAction;
        private readonly IConfiguration _configuration;

        public MinioOptionsExtension(Action<MinioOptions> configureAction)
        {
            _configureAction = configureAction;
        }

        public MinioOptionsExtension(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public void AddServices(IServiceCollection services)
        {
            services.TryAddSingleton<MinioBlobProvider>();
            services.TryAddSingleton<MinioBlobNamingValidator>();

            if (_configureAction != null)
            {
                services.Configure(_configureAction);
            }
            else
            {
                services.Configure<MinioOptions>(_configuration);
            }
        }
    }
}