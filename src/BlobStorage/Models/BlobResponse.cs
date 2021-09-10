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
        public BlobResponse(Stream content, BlobStat stat)
        {
            Content = content;
            Stat = stat;
        }

        public Stream Content { get; set; }

        public BlobStat Stat { get; set; }
    }
}
