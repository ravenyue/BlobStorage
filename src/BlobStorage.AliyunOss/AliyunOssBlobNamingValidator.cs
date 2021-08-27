using Aliyun.OSS.Util;

namespace BlobStorage.AliyunOss
{
    public class AliyunOssBlobNamingValidator : IBlobNamingValidator
    {
        public virtual bool ValidateBucketName(string bucketName)
        {
            return OssUtils.IsBucketNameValid(bucketName);
        }

        public virtual bool ValidateBlobName(string blobName)
        {
            return OssUtils.IsObjectKeyValid(blobName);
        }
    }
}
