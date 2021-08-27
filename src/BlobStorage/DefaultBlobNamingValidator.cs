using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlobStorage
{
    public class DefaultBlobNamingValidator : IBlobNamingValidator
    {
        public static readonly DefaultBlobNamingValidator Instance = new();

        public bool ValidateBlobName(string blobName)
        {
            return true;
        }

        public bool ValidateBucketName(string bucketName)
        {
            return true;
        }
    }
}
