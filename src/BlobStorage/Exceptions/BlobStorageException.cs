using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace BlobStorage
{
    public class BlobStorageException : Exception
    {
        public BlobStorageException() { }

        public BlobStorageException(string message)
            : base(message) { }

        public BlobStorageException(string message, Exception innerException)
            : base(message, innerException) { }

        public BlobStorageException(string message, string bucketName, string blobName)
            : base(message)
        {
            BucketName = bucketName;
            BlobName = blobName;
        }

        public BlobStorageException(string message, string bucketName, string blobName, Exception innerException)
            : base(message, innerException)
        {
            BucketName = bucketName;
            BlobName = blobName;
        }

        protected BlobStorageException(SerializationInfo serializationInfo, StreamingContext context)
            : base(serializationInfo, context) { }

        public string BucketName { get; }

        public string BlobName { get; }
    }
}
