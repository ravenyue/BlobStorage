using Microsoft.Extensions.Options;
using System;

namespace BlobStorage
{
    public class BlobNamingValidatorSelector : IBlobNamingValidatorSelector
    {
        public BlobStorageOptions Options { get; }
        protected IServiceProvider ServiceProvider { get; }

        public BlobNamingValidatorSelector(
            IOptions<BlobStorageOptions> options,
            IServiceProvider serviceProvider)
        {
            Options = options.Value;
            ServiceProvider = serviceProvider;
        }

        public IBlobNamingValidator Get(string containerName)
        {
            var configuration = Options.GetConfiguration(containerName);
            var validatorType = configuration.NamingValidatorType;

            if (configuration.NamingValidatorType == null)
            {
                return DefaultBlobNamingValidator.Instance;
            }

            var validator = ServiceProvider.GetService(validatorType);

            if (validator == null)
            {
                throw new InvalidOperationException($"Type {validatorType.AssemblyQualifiedName} is not registered");
            }

            if (validator is IBlobNamingValidator blobNamingValidator)
            {
                return blobNamingValidator;
            }

            throw new InvalidOperationException(
                $"Type ({validatorType.AssemblyQualifiedName}) does not implement the ({typeof(IBlobNamingValidator).AssemblyQualifiedName}) interface"
            );
        }
    }
}
