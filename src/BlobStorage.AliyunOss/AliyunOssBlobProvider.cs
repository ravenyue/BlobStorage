using Aliyun.OSS;
using Microsoft.Extensions.Options;
using System;
using System.IO;
using System.Net;
using System.Net.Http;
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

        public virtual async Task SaveAsync(BlobProviderSaveArgs args)
        {
            if (!args.OverrideExisting &&
                BlobExists(OssClient, args.BucketName, args.BlobName))
            {
                throw new BlobAlreadyExistsException(
                    $"Saving BLOB '{args.BlobName}' does already exists in the container '{args.BucketName}'! Set {nameof(args.OverrideExisting)} if it should be overwritten.",
                    args.BucketName,
                    args.BlobName);
            }

            var result = await OssClient.PutObjectAsync(
                args.BucketName,
                args.BlobName,
                args.BlobStream);

            if (result.HttpStatusCode != HttpStatusCode.OK)
            {
                if (result.HttpStatusCode == HttpStatusCode.Forbidden)
                {
                    throw new BlobAccessDeniedException(args.BucketName, args.BlobName);
                }
                else
                {
                    throw new HttpRequestException(
                        $"Response status code does not indicate success: {(int)result.HttpStatusCode} ({result.HttpStatusCode}).",
                        null, result.HttpStatusCode);
                }
            }
        }

        public virtual Task<bool> DeleteAsync(BlobProviderDeleteArgs args)
        {
            if (!OssClient.DoesBucketExist(args.BucketName))
            {
                return Task.FromResult(false);
            }
            var result = OssClient.DeleteObject(args.BucketName, args.BlobName);
            return Task.FromResult(result.DeleteMarker);
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

            return HandleOssObjectError(ossObject);
        }

        protected virtual bool BlobExists(IOss ossClient, string bucketName, string blobName)
        {
            try
            {
                return OssClient.DoesObjectExist(bucketName, blobName);
            }
            catch (HttpRequestException ex)
            {
                if (ex.StatusCode == HttpStatusCode.Forbidden)
                {
                    throw new BlobAccessDeniedException(bucketName, blobName, ex);
                }
                throw;
            }
        }

        private Stream HandleOssObjectError(OssObject ossObject)
        {
            if (ossObject.HttpStatusCode != HttpStatusCode.OK)
            {
                if (ossObject.HttpStatusCode == HttpStatusCode.NotFound)
                {
                    return null;
                }
                else if (ossObject.HttpStatusCode == HttpStatusCode.Forbidden)
                {
                    throw new BlobAccessDeniedException(ossObject.BucketName, ossObject.Key);
                }
                else
                {
                    throw new HttpRequestException(
                        $"Response status code does not indicate success: {(int)ossObject.HttpStatusCode} ({ossObject.HttpStatusCode}).",
                        null, ossObject.HttpStatusCode);
                }
            }

            return ossObject.Content;
        }
    }
}
