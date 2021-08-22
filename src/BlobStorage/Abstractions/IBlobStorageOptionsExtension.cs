using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlobStorage
{
    public interface IBlobStorageOptionsExtension
    {
        /// <summary>
        /// Registered child service.
        /// </summary>
        /// <param name="services">add service to the <see cref="IServiceCollection" /></param>
        void AddServices(IServiceCollection services);
    }
}
