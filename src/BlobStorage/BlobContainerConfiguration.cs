using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlobStorage
{
    public class BlobContainerConfiguration
    {
        public BlobContainerConfiguration()
        {
            Extensions = new List<IBlobStorageOptionsExtension>();
        }

        internal IList<IBlobStorageOptionsExtension> Extensions { get; }

        /// <summary>
        /// The provider to be used to store BLOBs of this container.
        /// </summary>
        public Type ProviderType { get; set; }

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
