using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlobStorage
{
    public class BlobMetadata
    {
        public BlobMetadata(
            long size,
            string eTag,
            DateTimeOffset lastModified,
            IDictionary<string, string> properties)
        {
            Size = size;
            ETag = eTag;
            LastModified = lastModified;
            Properties = properties;
        }

        public long Size { get; }
        public string ETag { get; }
        public DateTimeOffset LastModified { get; }
        public IDictionary<string, string> Properties { get; }
    }
}
