using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace BlobStorage.Minio
{
    public class MinioBlobNamingValidator : IBlobNamingValidator
    {
        public virtual bool ValidateBucketName(string bucketName)
        {
            if (string.IsNullOrWhiteSpace(bucketName))
                return false;

            const string pattern = "^[a-z0-9][a-z0-9\\-\\.]{1,61}[a-z0-9]$";
            var regex = new Regex(pattern);
            var m = regex.Match(bucketName);

            return m.Success &&
                !bucketName.StartsWith("xn--") &&
                !bucketName.EndsWith("-s3alias") &&
                !IPAddress.TryParse(bucketName, out var _);
        }

        public virtual bool ValidateBlobName(string blobName)
        {
            return !string.IsNullOrWhiteSpace(blobName) &&
                blobName.Length < 512;
        }
    }
}
