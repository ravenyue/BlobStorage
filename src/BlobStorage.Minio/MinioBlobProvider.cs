using Microsoft.Extensions.Options;
using Minio;
using Minio.Exceptions;
using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace BlobStorage.Minio
{
    public class MinioBlobProvider : IBlobProvider
    {
        protected MinioOptions Options { get; }
        protected MinioClient MinioClient { get; }

        public MinioBlobProvider(IOptions<MinioOptions> options)
        {
            Options = options.Value;
            MinioClient = GetMinioClient(Options);
        }

        protected virtual MinioClient GetMinioClient(MinioOptions options)
        {
            Check.NotNullOrWhiteSpace(options.Endpoint, nameof(options.Endpoint));
            Check.NotNullOrWhiteSpace(options.AccessKey, nameof(options.AccessKey));
            Check.NotNullOrWhiteSpace(options.SecretKey, nameof(options.SecretKey));

            var client = new MinioClient(options.Endpoint, options.AccessKey, options.SecretKey);
            if (options.WithSSL)
            {
                client.WithSSL();
            }

            return client;
        }

        public virtual async Task SaveAsync(BlobProviderSaveArgs args)
        {
            var exists = await BlobExistsAsync(
                MinioClient,
                args.BucketName,
                args.BlobName,
                args.CancellationToken);

            if (!args.OverrideExisting && exists)
            {
                throw new BlobAlreadyExistsException(
                    $"Saving BLOB '{args.BlobName}' does already exists in the container '{args.BucketName}'! Set {nameof(args.OverrideExisting)} if it should be overwritten.",
                    args.BucketName,
                    args.BlobName);
            }

            if (Options.CreateBucketIfNotExists)
            {
                await CreateBucketIfNotExists(MinioClient, args.BucketName);
            }

            try
            {
                await MinioClient.PutObjectAsync(
                    args.BucketName,
                    args.BlobName,
                    args.BlobStream,
                    args.BlobStream.Length,
                    cancellationToken: args.CancellationToken);
            }
            catch (MinioException ex)
            {
                if (ex.IsAccessDeniedError())
                {
                    throw new BlobAccessDeniedException(args.BucketName, args.BlobName, ex);
                }
                throw;
            }
        }

        public virtual async Task<bool> DeleteAsync(BlobProviderDeleteArgs args)
        {
            var exists = await BlobExistsAsync(
                MinioClient,
                args.BucketName,
                args.BlobName,
                args.CancellationToken);

            if (!exists) return false;

            try
            {
                await MinioClient.RemoveObjectAsync(
                    args.BucketName,
                    args.BlobName,
                    args.CancellationToken);
            }
            catch (MinioException ex)
            {
                if (ex.IsNotFoundError())
                {
                    return false;
                }
                if (ex.IsAccessDeniedError())
                {
                    throw new BlobAccessDeniedException(args.BucketName, args.BlobName, ex);
                }
                throw;
            }
            return true;
        }

        public virtual Task<bool> ExistsAsync(BlobProviderExistsArgs args)
        {
            return BlobExistsAsync(
                MinioClient,
                args.BucketName,
                args.BlobName,
                args.CancellationToken);
        }

        public virtual async Task<Stream> GetOrNullAsync(BlobProviderGetArgs args)
        {
            try
            {
                var memoryStream = new MemoryStream();
                await MinioClient.GetObjectAsync(args.BucketName, args.BlobName, stream =>
                {
                    if (stream != null)
                    {
                        stream.CopyTo(memoryStream);
                        memoryStream.Seek(0, SeekOrigin.Begin);
                    }
                    else
                    {
                        memoryStream = null;
                    }
                });
                return memoryStream;
            }
            catch (MinioException ex)
            {
                if (ex.IsNotFoundError())
                {
                    return null;
                }
                if (ex.IsAccessDeniedError())
                {
                    throw new BlobAccessDeniedException(args.BucketName, args.BlobName, ex);
                }
                throw;
            }
        }

        protected virtual async Task<bool> BlobExistsAsync(MinioClient client, string bucketName, string blobName, CancellationToken cancellationToken = default)
        {
            try
            {
                await client.StatObjectAsync(bucketName, blobName, cancellationToken: cancellationToken);
            }
            catch (MinioException ex)
            {
                if (ex.IsNotFoundError())
                {
                    return false;
                }
                if (ex.IsAccessDeniedError())
                {
                    throw new BlobAccessDeniedException(bucketName, blobName, ex);
                }
                throw;
            }
            return true;
        }

        protected virtual async Task CreateBucketIfNotExists(MinioClient client, string bucketName)
        {
            try
            {
                if (!await client.BucketExistsAsync(bucketName))
                {
                    await client.MakeBucketAsync(bucketName);
                }
            }
            catch (MinioException ex)
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
