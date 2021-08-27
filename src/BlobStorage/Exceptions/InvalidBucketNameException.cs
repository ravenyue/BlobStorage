using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlobStorage
{
    public class InvalidBucketNameException : BlobStorageException
    {
        public InvalidBucketNameException(string message, string bucketName)
            : base(message, bucketName, "")
        {

        }

        public InvalidBucketNameException(string message, string bucketName, Exception innerException)
            : base(message, bucketName, "", innerException)
        {

        }
    }
}
