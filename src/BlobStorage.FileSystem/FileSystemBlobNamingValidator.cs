using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlobStorage.FileSystem
{
    public class FileSystemBlobNamingValidator : IBlobNamingValidator
    {
        public virtual bool ValidateBucketName(string bucketName)
        {
            return (!string.IsNullOrWhiteSpace(bucketName) &&
                bucketName.IndexOfAny(Path.GetInvalidPathChars()) < 0);
        }

        public virtual bool ValidateBlobName(string blobName)
        {
            return (!string.IsNullOrWhiteSpace(blobName) &&
                blobName.IndexOfAny(Path.GetInvalidFileNameChars()) < 0);
        }
    }
}
