using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace BlobStorage
{
    public class BlobNotFoundException : BlobStorageException
    {
        public BlobNotFoundException() { }

        public BlobNotFoundException(string message)
            : base(message) { }

        public BlobNotFoundException(string message, Exception innerException)
            : base(message, innerException) { }

        public BlobNotFoundException(string bucketName, string blobName)
            : base($"Could not found the requested BLOB '{blobName}' in the bucket '{bucketName}'!", bucketName, blobName) { }

        public BlobNotFoundException(string bucketName, string blobName, Exception innerException)
            : base($"Could not found the requested BLOB '{blobName}' in the bucket '{bucketName}'!", bucketName, blobName, innerException) { }

        public BlobNotFoundException(string message, string bucketName, string blobName)
            : base(message, bucketName, blobName) { }

        public BlobNotFoundException(string message, string bucketName, string blobName, Exception innerException)
            : base(message, bucketName, blobName, innerException) { }

        protected BlobNotFoundException(SerializationInfo serializationInfo, StreamingContext context)
            : base(serializationInfo, context) { }
    }
}
