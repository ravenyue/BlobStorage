using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlobStorage.FileSystem
{
    public class DefaultBlobFilePathCalculator : IBlobFilePathCalculator
    {
        public string Calculate(BlobProviderArgs args, FileSystemBlobProviderOptions options)
        {
            var blobPath = options.BasePath;

            if (options.AppendBucketNameToBasePath)
            {
                blobPath = Path.Combine(blobPath, args.BucketName);
            }

            blobPath = Path.Combine(blobPath, args.BlobName);

            return blobPath;
        }
    }
}
