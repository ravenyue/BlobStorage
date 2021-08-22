using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace BlobStorage
{
    public class BlobAlreadyExistsException : BlobStorageException
    {
        public BlobAlreadyExistsException() { }

        public BlobAlreadyExistsException(string message)
            : base(message) { }

        public BlobAlreadyExistsException(string message, Exception innerException)
            : base(message, innerException) { }

        public BlobAlreadyExistsException(string message, string bucketName, string blobName)
            : base(message, bucketName, blobName) { }

        public BlobAlreadyExistsException(string message, string bucketName, string blobName, Exception innerException)
            : base(message, bucketName, blobName, innerException) { }

        protected BlobAlreadyExistsException(SerializationInfo serializationInfo, StreamingContext context)
            : base(serializationInfo, context) { }
    }
}
