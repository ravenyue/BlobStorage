using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlobStorage
{
    public class BlobResponse
    {
        public BlobResponse(Stream content, BlobMetadata metadata)
        {
            Content = content;
            Metadata = metadata;
        }

        public Stream Content { get; set; }

        public BlobMetadata Metadata { get; set; }
    }
}
