using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlobStorage.FileSystem
{
    public class FileSystemBlobProviderOptionsExtension : IBlobStorageOptionsExtension
    {
        private readonly Action<FileSystemBlobProviderOptions> _configureAction;
        private readonly IConfiguration _configuration;

        public FileSystemBlobProviderOptionsExtension(Action<FileSystemBlobProviderOptions> configureAction)
        {
            _configureAction = configureAction;
        }

        public FileSystemBlobProviderOptionsExtension(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public void AddServices(IServiceCollection services)
        {
            services.TryAddSingleton<IBlobFilePathCalculator, DefaultBlobFilePathCalculator>();
            services.TryAddSingleton<FileSystemBlobProvider>();

            if (_configureAction != null)
            {
                services.Configure(_configureAction);
            }
            else
            {
                services.Configure<FileSystemBlobProviderOptions>(_configuration);
            }
        }
    }
}
