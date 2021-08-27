using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlobStorage
{
    public class BlobNamingValidatorSelector : IBlobNamingValidatorSelector
    {
        protected IServiceProvider ServiceProvider { get; }

        public BlobNamingValidatorSelector(IServiceProvider serviceProvider)
        {
            ServiceProvider = serviceProvider;
        }

        public IBlobNamingValidator GetNamingValidator(BlobContainerConfiguration configuration)
        {
            var validatorType = configuration.NamingValidatorType;

            if (configuration.NamingValidatorType == null)
            {
                return DefaultBlobNamingValidator.Instance;
            }

            if (!validatorType.IsAssignableTo(typeof(IBlobNamingValidator)))
            {
                throw new Exception(
                    $"Type ({configuration.NamingValidatorType.AssemblyQualifiedName}) does not implement the ({typeof(IBlobNamingValidator).AssemblyQualifiedName}) interface"
                );
            }

            var validator = ServiceProvider.GetRequiredService(validatorType);

            return (IBlobNamingValidator)validator;
        }
    }
}
