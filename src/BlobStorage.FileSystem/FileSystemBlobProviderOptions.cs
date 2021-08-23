using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlobStorage.FileSystem
{
    public class FileSystemBlobProviderOptions
    {
        public FileSystemBlobProviderOptions()
        {
            AppendBucketNameToBasePath = true;
        }

        public string BasePath { get; set; }
        public bool AppendBucketNameToBasePath { get; set; }
    }
}
