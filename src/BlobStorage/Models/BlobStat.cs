using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlobStorage
{
    public class BlobStat
    {
        public BlobStat(
            long size,
            string eTag,
            DateTimeOffset lastModified,
            IDictionary<string, string> metadata)
        {
            Size = size;
            ETag = eTag;
            LastModified = lastModified;
            Metadata = metadata;
        }

        public long Size { get; }
        public string ETag { get; }
        public DateTimeOffset LastModified { get; }
        public IDictionary<string, string> Metadata { get; }
    }
}
