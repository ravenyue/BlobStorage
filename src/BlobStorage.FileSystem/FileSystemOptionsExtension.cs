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
    public class FileSystemOptionsExtension : IBlobStorageOptionsExtension
    {
        private readonly Action<FileSystemOptions> _configureAction;
        private readonly IConfiguration _configuration;

        public FileSystemOptionsExtension(Action<FileSystemOptions> configureAction)
        {
            _configureAction = configureAction;
        }

        public FileSystemOptionsExtension(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public void AddServices(IServiceCollection services)
        {
            services.TryAddSingleton<IBlobFilePathCalculator, DefaultBlobFilePathCalculator>();
            services.TryAddSingleton<FileSystemBlobProvider>();
            services.TryAddSingleton<FileSystemBlobNamingValidator>();

            if (_configureAction != null)
            {
                services.Configure(_configureAction);
            }
            else
            {
                services.Configure<FileSystemOptions>(_configuration);
            }
        }
    }
}
