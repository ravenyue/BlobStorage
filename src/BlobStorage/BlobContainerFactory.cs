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
        protected IBlobNamingValidatorSelector BlobNamingValidatorSelector { get; }

        public BlobContainerFactory(
            IOptions<BlobStorageOptions> options,
            IBlobProviderSelector providerSelector,
            IBlobNamingValidatorSelector blobNamingValidatorSelector)
        {
            Options = options.Value;
            ProviderSelector = providerSelector;
            BlobNamingValidatorSelector = blobNamingValidatorSelector;
        }

        public virtual IBlobContainer Create(string name)
        {
            var configuration = Options.GetConfiguration(name);
            var validator = BlobNamingValidatorSelector.Get(name);
            var provider = ProviderSelector.Get(name);

            return new BlobContainer(
                name,
                configuration,
                provider,
                validator);
        }
    }
}
