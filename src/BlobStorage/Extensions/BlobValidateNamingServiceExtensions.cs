using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlobStorage
{
    public static class BlobValidateNamingServiceExtensions
    {
        public static void EnsureValidNameing(
            this IBlobNamingValidator validator,
            string bucketName, string blobName, string blobProviderName = "")
        {
            if (!validator.ValidateBucketName(bucketName))
            {
                throw new InvalidBucketNameException($"The bucket name '{bucketName}' is invalid in the '{blobProviderName}'", bucketName);
            }
            if (!validator.ValidateBlobName(blobName))
            {
                throw new InvalidBlobNameException($"The blob name '{blobName}' is invalid in the '{blobProviderName}'", blobName);
            }
        }
    }
}
