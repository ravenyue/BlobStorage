using Aliyun.OSS;
using Microsoft.Extensions.Options;
using System;
using System.IO;
using System.Net;
using System.Threading.Tasks;

namespace BlobStorage.AliyunOss
{
    public class AliyunOssBlobProvider : IBlobProvider
    {
        protected AliyunOssBlobProviderOptions Options { get; }
        protected IOss OssClient { get; }

        public AliyunOssBlobProvider(
            IOptions<AliyunOssBlobProviderOptions> options)
        {
            Options = options.Value;
            OssClient = GetOssClient(Options);
        }

        protected virtual IOss GetOssClient(AliyunOssBlobProviderOptions options)
        {
            Check.NotNullOrWhiteSpace(options.AccessKeyId, nameof(options.AccessKeyId));
            Check.NotNullOrWhiteSpace(options.AccessKeySecret, nameof(options.AccessKeySecret));
            Check.NotNullOrWhiteSpace(options.Endpoint, nameof(options.Endpoint));

            return new OssClient(options.Endpoint, options.AccessKeyId, options.AccessKeySecret);
        }

        public virtual Task SaveAsync(BlobProviderSaveArgs args)
        {
            if (!args.OverrideExisting &&
                BlobExists(OssClient, args.BucketName, args.BlobName))
            {
                throw new BlobAlreadyExistsException($"Saving BLOB '{args.BlobName}' does already exists in the container '{args.BucketName}'! Set {nameof(args.OverrideExisting)} if it should be overwritten.");
            }

            return OssClient.PutObjectAsync(
                args.BucketName,
                args.BlobName,
                args.BlobStream);
        }

        public virtual Task<bool> DeleteAsync(BlobProviderDeleteArgs args)
        {
            OssClient.DeleteObject(args.BucketName, args.BlobName);
            return Task.FromResult(true);
        }

        public virtual Task<bool> ExistsAsync(BlobProviderExistsArgs args)
        {
            var result = BlobExists(OssClient, args.BucketName, args.BlobName);
            return Task.FromResult(result);
        }

        public virtual async Task<Stream> GetOrNullAsync(BlobProviderGetArgs args)
        {
            var ossObject = await OssClient.GetObjectAsync(
                args.BucketName,
                args.BlobName);

            if (ossObject.HttpStatusCode != HttpStatusCode.OK)
            {
                if (ossObject.HttpStatusCode == HttpStatusCode.NotFound)
                {
                    return null;
                }
                else
                {
                    throw new ApplicationException("请求出错");
                }
            }

            return ossObject.Content;
        }

        protected virtual bool BlobExists(IOss ossClient, string bucketName, string blobName)
        {
            return OssClient.DoesObjectExist(bucketName, blobName);
        }
    }
}
