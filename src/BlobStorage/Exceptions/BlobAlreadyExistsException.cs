using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace BlobStorage
{
    public class BlobAlreadyExistsException : Exception
    {
        public BlobAlreadyExistsException()
        {

        }

        public BlobAlreadyExistsException(string message)
            : base(message)
        {

        }

        public BlobAlreadyExistsException(string message, Exception innerException)
            : base(message, innerException)
        {

        }

        public BlobAlreadyExistsException(SerializationInfo serializationInfo, StreamingContext context)
            : base(serializationInfo, context)
        {

        }
    }
}
