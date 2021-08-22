using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlobStorage
{
    public class BlobContainerFactory : IBlobContainerFactory
    {
        protected BlobStorageOptions Options { get; }
        protected IBlobProviderSelector ProviderSelector { get; }

        public BlobContainerFactory(
            IOptions<BlobStorageOptions> options,
            IBlobProviderSelector providerSelector)
        {
            Options = options.Value;
            ProviderSelector = providerSelector;
        }

        public virtual IBlobContainer Create(string name)
        {
            var configuration = Options.Containers.GetConfiguration(name);

            return new BlobContainer(
                name,
                configuration,
                ProviderSelector.Get(name));
        }
    }
}
