using System.Text.RegularExpressions;

namespace BlobStorage.AzureBlob
{
    public class AzureBlobNamingValidator : IBlobNamingValidator
    {
        public virtual bool ValidateBucketName(string bucketName)
        {
            if (string.IsNullOrWhiteSpace(bucketName))
                return false;

            const string pattern = "^[a-z0-9][a-z0-9\\-]{1,61}[a-z0-9]$";
            var regex = new Regex(pattern);
            var m = regex.Match(bucketName);

            return m.Success;
        }

        public virtual bool ValidateBlobName(string blobName)
        {
            return !string.IsNullOrWhiteSpace(blobName) &&
                blobName.Length < 512;
        }
    }
}