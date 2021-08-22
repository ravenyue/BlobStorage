using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlobStorage
{
    public class BlobStorageOptions
    {
        public BlobStorageOptions()
        {
            Containers = new BlobContainerConfigurations();
            Extensions = new List<IBlobStorageOptionsExtension>();
        }

        internal IList<IBlobStorageOptionsExtension> Extensions { get; }

        public BlobContainerConfigurations Containers { get; }

        public void RegisterExtension(IBlobStorageOptionsExtension extension)
        {
            if (extension == null)
            {
                throw new ArgumentNullException(nameof(extension));
            }

            Extensions.Add(extension);
        }
    }
}
