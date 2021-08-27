using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using System;

namespace BlobStorage.AzureBlob
{
    public class AzureBlobOptionsExtension : IBlobStorageOptionsExtension
    {
        private readonly Action<AzureBlobOptions> _configureAction;
        private readonly IConfiguration _configuration;

        public AzureBlobOptionsExtension(Action<AzureBlobOptions> configureAction)
        {
            _configureAction = configureAction;
        }

        public AzureBlobOptionsExtension(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public void AddServices(IServiceCollection services)
        {
            services.TryAddSingleton<AzureBlobProvider>();
            services.TryAddSingleton<AzureBlobNamingValidator>();

            if (_configureAction != null)
            {
                services.Configure(_configureAction);
            }
            else
            {
                services.Configure<AzureBlobOptions>(_configuration);
            }
        }
    }
}