using Aliyun.OSS;
using Aliyun.OSS.Common;
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
        protected AliyunOssOptions Options { get; }
        protected IOss OssClient { get; }

        public AliyunOssBlobProvider(
            IOptions<AliyunOssOptions> options)
        {
            Options = options.Value;
            OssClient = GetOssClient(Options);
        }

        protected virtual IOss GetOssClient(AliyunOssOptions options)
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
                throw new BlobAlreadyExistsException(
                    $"Saving BLOB '{args.BlobName}' does already exists in the container '{args.BucketName}'! Set {nameof(args.OverrideExisting)} if it should be overwritten.",
                    args.BucketName,
                    args.BlobName);
            }

            if (Options.CreateBucketIfNotExists)
            {
                CreateBucketIfNotExists(OssClient, args.BucketName);
            }

            try
            {
                ObjectMetadata metadata = null;
                if (args.Metadata != null)
                {
                    metadata = Mapper.MapObjectMetadata(args.Metadata);
                }
                var result = OssClient.PutObject(
                    args.BucketName,
                    args.BlobName,
                    args.BlobStream,
                    metadata);
            }
            catch (OssException ex)
            {
                if (ex.IsBucketNotFoundError())
                {
                    throw new BlobBucketNotFoundException(args.BucketName, ex);
                }
                if (ex.IsAccessDeniedError())
                {
                    throw new BlobAccessDeniedException(args.BucketName, args.BlobName, ex);
                }
                throw;
            }
            return Task.CompletedTask;
        }

        public virtual Task<bool> DeleteAsync(BlobProviderDeleteArgs args)
        {
            try
            {
                var result = OssClient.DeleteObject(args.BucketName, args.BlobName);
                return Task.FromResult(result.DeleteMarker);
            }
            catch (OssException ex)
            {
                if (ex.IsBucketNotFoundError())
                {
                    return Task.FromResult(false);
                }
                if (ex.IsAccessDeniedError())
                {
                    throw new BlobAccessDeniedException(args.BucketName, args.BlobName, ex);
                }
                throw;
            }
        }

        public virtual Task<bool> ExistsAsync(BlobProviderExistsArgs args)
        {
            var result = BlobExists(OssClient, args.BucketName, args.BlobName);
            return Task.FromResult(result);
        }

        public virtual Task<Stream> GetOrNullAsync(BlobProviderGetArgs args)
        {
            try
            {
                var ossObject = OssClient.GetObject(
                    args.BucketName,
                    args.BlobName);

                return Task.FromResult(ossObject.Content);
            }
            catch (OssException ex)
            {
                if (ex.IsNotFoundError())
                {
                    return Task.FromResult<Stream>(null);
                }
                if (ex.IsAccessDeniedError())
                {
                    throw new BlobAccessDeniedException(args.BucketName, args.BlobName, ex);
                }
                throw;
            }
        }

        public Task<BlobMetadata> GetOrNullMetadataAsync(BlobProviderGetArgs args)
        {
            try
            {
                var objectMetadata = OssClient.GetObjectMetadata(
                    args.BucketName,
                    args.BlobName);

                return Task.FromResult(Mapper.MapBlobMetadata(objectMetadata));
            }
            catch (OssException ex)
            {
                if (ex.IsNotFoundError())
                {
                    return Task.FromResult<BlobMetadata>(null);
                }
                if (ex.IsAccessDeniedError())
                {
                    throw new BlobAccessDeniedException(args.BucketName, args.BlobName, ex);
                }
                throw;
            }
        }

        public Task<BlobResponse> GetOrNullWithMetadataAsync(BlobProviderGetArgs args)
        {
            try
            {
                var ossObject = OssClient.GetObject(
                    args.BucketName,
                    args.BlobName);

                return Task.FromResult(Mapper.MapBlobResponse(ossObject));
            }
            catch (OssException ex)
            {
                if (ex.IsNotFoundError())
                {
                    return Task.FromResult<BlobResponse>(null);
                }
                if (ex.IsAccessDeniedError())
                {
                    throw new BlobAccessDeniedException(args.BucketName, args.BlobName, ex);
                }
                throw;
            }
        }

        protected virtual bool BlobExists(IOss ossClient, string bucketName, string blobName)
        {
            try
            {
                return OssClient.DoesObjectExist(bucketName, blobName);
            }
            catch (HttpRequestException ex)
            {
                if (ex.IsAccessDeniedError())
                {
                    throw new BlobAccessDeniedException(bucketName, blobName, ex);
                }
                throw;
            }
        }

        protected virtual void CreateBucketIfNotExists(IOss client, string bucketName)
        {
            try
            {
                if (!client.DoesBucketExist(bucketName))
                {
                    client.CreateBucket(bucketName);
                }
            }
            catch (OssException ex)
            {
                if (ex.IsAccessDeniedError())
                {
                    throw new BlobAccessDeniedException($"Access denied to bucket '{bucketName}'!", bucketName, "", ex);
                }
                throw;
            }
        }
    }
}
