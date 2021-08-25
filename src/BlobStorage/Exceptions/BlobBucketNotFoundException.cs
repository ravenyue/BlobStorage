using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace BlobStorage
{
    public class BlobBucketNotFoundException : BlobStorageException
    {
        public BlobBucketNotFoundException() { }

        public BlobBucketNotFoundException(string bucketName)
            : base($"Could not found bucket '{bucketName}'!", bucketName, "") { }

        public BlobBucketNotFoundException(string bucketName, Exception innerException)
            : base($"Could not found bucket '{bucketName}'!", bucketName, "", innerException) { }

        public BlobBucketNotFoundException(string message, string bucketName)
            : base(message, bucketName, "") { }

        public BlobBucketNotFoundException(string message, string bucketName, Exception innerException)
            : base(message, bucketName, "", innerException) { }

        protected BlobBucketNotFoundException(SerializationInfo serializationInfo, StreamingContext context)
            : base(serializationInfo, context) { }
    }
}
