using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlobStorage
{
    public interface IBlobNamingValidator
    {
        bool ValidateBucketName(string bucketName);
        bool ValidateBlobName(string blobName);
    }
}
