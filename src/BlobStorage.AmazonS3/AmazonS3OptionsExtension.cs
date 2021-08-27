using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using System;

namespace BlobStorage.AmazonS3
{
    public class AmazonS3OptionsExtension : IBlobStorageOptionsExtension
    {
        private readonly Action<AmazonS3Options> _configureAction;
        private readonly IConfiguration _configuration;

        public AmazonS3OptionsExtension(Action<AmazonS3Options> configureAction)
        {
            _configureAction = configureAction;
        }

        public AmazonS3OptionsExtension(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public void AddServices(IServiceCollection services)
        {
            services.TryAddSingleton<AmazonS3BlobProvider>();
            services.TryAddSingleton<AmazonS3BlobNamingValidator>();

            if (_configureAction != null)
            {
                services.Configure(_configureAction);
            }
            else
            {
                services.Configure<AmazonS3Options>(_configuration);
            }
        }
    }
}