using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlobStorage
{
    public class InvalidBlobNameException : BlobStorageException
    {
        public InvalidBlobNameException(string message, string blobName)
            : base(message, "", blobName)
        {

        }

        public InvalidBlobNameException(string message, string blobName, Exception innerException)
            : base(message, "", blobName, innerException)
        {

        }
    }
}
