using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlobStorage.AzureBlob
{
    public class AzureBlobOptions
    {
        public string ConnectionString { get; set; }
        public bool CreateBucketIfNotExists { get; set; }
    }
}
