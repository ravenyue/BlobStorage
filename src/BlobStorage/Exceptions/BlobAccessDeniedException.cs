using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace BlobStorage
{
    public class BlobAccessDeniedException : BlobStorageException
    {
        public BlobAccessDeniedException() { }

        public BlobAccessDeniedException(string message)
            : base(message) { }

        public BlobAccessDeniedException(string message, Exception innerException)
            : base(message, innerException) { }

        public BlobAccessDeniedException(string bucketName, string blobName)
            : base($"Access denied to BLOB '{blobName}' in the bucket '{bucketName}'!", bucketName, blobName) { }

        public BlobAccessDeniedException(string bucketName, string blobName, Exception innerException)
            : base($"Access denied to BLOB '{blobName}' in the bucket '{bucketName}'!", bucketName, blobName, innerException) { }

        public BlobAccessDeniedException(string message, string bucketName, string blobName)
            : base(message, bucketName, blobName) { }

        public BlobAccessDeniedException(string message, string bucketName, string blobName, Exception innerException)
            : base(message, bucketName, blobName, innerException) { }

        protected BlobAccessDeniedException(SerializationInfo serializationInfo, StreamingContext context)
            : base(serializationInfo, context) { }
    }
}
