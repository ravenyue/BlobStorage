using Aliyun.OSS;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace BlobStorage.AliyunOss
{
    public static class OssClientExtensions
    {
        public static Task<OssObject> GetObjectAsync(
            this IOss oss,
            GetObjectRequest getObjectRequest)
        {
            return Task<OssObject>.Factory.FromAsync(
                oss.BeginGetObject,
                oss.EndGetObject,
                getObjectRequest,
                null);
        }

        public static Task<OssObject> GetObjectAsync(
            this IOss oss,
            string bucketName,
            string key)
        {
            return GetObjectAsync(oss, new GetObjectRequest(bucketName, key));
        }

        public static Task<PutObjectResult> PutObjectAsync(
            this IOss oss,
            PutObjectRequest putObjectRequest)
        {
            return Task<PutObjectResult>.Factory.FromAsync(
                oss.BeginPutObject,
                oss.EndPutObject,
                putObjectRequest,
                null);
        }

        public static Task<PutObjectResult> PutObjectAsync(
            this IOss oss,
            string bucketName,
            string key,
            Stream content)
        {
            return PutObjectAsync(oss, new PutObjectRequest(bucketName, key, content));
        }

        public static Task<PutObjectResult> PutObjectAsync(
            this IOss oss,
            string bucketName,
            string key,
            Stream content,
            ObjectMetadata metadata)
        {
            return PutObjectAsync(oss, new PutObjectRequest(bucketName, key, content, metadata));
        }

        public static Task<PutObjectResult> PutObjectAsync(
           this IOss oss,
           string bucketName,
           string key,
           string fileToUpload)
        {
            using var content = File.OpenRead(fileToUpload);
            return PutObjectAsync(oss, new PutObjectRequest(bucketName, key, content));
        }

        public static Task<PutObjectResult> PutObjectAsync(
            this IOss oss,
            string bucketName,
            string key,
            string fileToUpload,
            ObjectMetadata metadata)
        {
            using var content = File.OpenRead(fileToUpload);
            return PutObjectAsync(oss, new PutObjectRequest(bucketName, key, content, metadata));
        }
    }
}
