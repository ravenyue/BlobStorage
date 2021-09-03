using Amazon;
using Amazon.S3;
using Amazon.S3.Model;
using Amazon.S3.Util;
using Microsoft.Extensions.Options;
using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace BlobStorage.AmazonS3
{
    public class AmazonS3BlobProvider : IBlobProvider, IDisposable
    {
        protected AmazonS3Options Options { get; set; }
        protected IAmazonS3 AmazonS3Client { get; set; }
        public AmazonS3BlobProvider(IOptions<AmazonS3Options> options)
        {
            Options = options.Value;
            AmazonS3Client = GetAmazonS3Client(Options);
        }

        protected virtual AmazonS3Client GetAmazonS3Client(AmazonS3Options options)
        {
            Check.NotNullOrWhiteSpace(options.AccessKeyId, nameof(options.AccessKeyId));
            Check.NotNullOrWhiteSpace(options.SecretAccessKey, nameof(options.SecretAccessKey));

            var region = RegionEndpoint.GetBySystemName(options.Region);
            return new AmazonS3Client(options.AccessKeyId, options.SecretAccessKey, region);
        }

        public async Task SaveAsync(BlobProviderSaveArgs args)
        {
            var exists = await BlobExistsAsync(
                AmazonS3Client,
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
                await CreateBucketIfNotExists(AmazonS3Client, args.BucketName);
            }

            try
            {
                var request = new PutObjectRequest
                {
                    BucketName = args.BucketName,
                    Key = args.BlobName,
                    InputStream = args.BlobStream,
                };

                if (args.Metadata != null)
                {
                    foreach (var data in args.Metadata)
                    {
                        request.Metadata.Add(data.Key, data.Value);
                    }
                }
                await AmazonS3Client.PutObjectAsync(request, args.CancellationToken);
            }
            catch (AmazonS3Exception ex)
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
        }

        public async Task<bool> DeleteAsync(BlobProviderDeleteArgs args)
        {
            if (!await BlobExistsAsync(
                AmazonS3Client,
                args.BucketName,
                args.BlobName,
                args.CancellationToken))
            {
                return false;
            }

            try
            {
                await AmazonS3Client.DeleteObjectAsync(new DeleteObjectRequest
                {
                    BucketName = args.BucketName,
                    Key = args.BlobName,
                }, args.CancellationToken);
            }
            catch (AmazonS3Exception ex)
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

        public Task<bool> ExistsAsync(BlobProviderExistsArgs args)
        {
            return BlobExistsAsync(AmazonS3Client, args.BucketName, args.BlobName, args.CancellationToken);
        }

        public async Task<Stream> GetOrNullAsync(BlobProviderGetArgs args)
        {
            try
            {
                var response = await AmazonS3Client.GetObjectAsync(new GetObjectRequest
                {
                    BucketName = args.BucketName,
                    Key = args.BlobName,
                }, args.CancellationToken);

                return response.ResponseStream;
            }
            catch (AmazonS3Exception ex)
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

        public async Task<BlobMetadata> GetOrNullMetadataAsync(BlobProviderGetArgs args)
        {
            try
            {
                var response = await AmazonS3Client
                    .GetObjectMetadataAsync(
                        args.BucketName,
                        args.BlobName,
                        args.CancellationToken);

                return Mapper.MapBlobMetadata(response);
            }
            catch (AmazonS3Exception ex)
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

        public async Task<BlobResponse> GetOrNullWithMetadataAsync(BlobProviderGetArgs args)
        {
            try
            {
                var response = await AmazonS3Client.GetObjectAsync(new GetObjectRequest
                {
                    BucketName = args.BucketName,
                    Key = args.BlobName,
                }, args.CancellationToken);

                return Mapper.MapBlobResponse(response);
            }
            catch (AmazonS3Exception ex)
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

        public void Dispose()
        {
            AmazonS3Client?.Dispose();
        }

        protected virtual async Task<bool> BlobExistsAsync(
            IAmazonS3 amazonS3Client,
            string bucketName,
            string blobName,
            CancellationToken cancellationToken = default)
        {
            try
            {
                await amazonS3Client.GetObjectMetadataAsync(bucketName, blobName, cancellationToken);
            }
            catch (AmazonS3Exception ex)
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

        protected virtual async Task CreateBucketIfNotExists(IAmazonS3 amazonS3Client, string bucketName)
        {
            try
            {
                if (!await AmazonS3Util.DoesS3BucketExistV2Async(amazonS3Client, bucketName))
                {
                    await amazonS3Client.PutBucketAsync(new PutBucketRequest
                    {
                        BucketName = bucketName
                    });
                }
            }
            catch (AmazonS3Exception ex)
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
