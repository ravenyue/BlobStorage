using Amazon;
using Amazon.S3;
using Amazon.S3.Model;
using Amazon.S3.Util;
using Microsoft.Extensions.Options;
using System;
using System.IO;
using System.Net;
using System.Net.Http;
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
            if (!args.OverrideExisting &&
                await BlobExistsAsync(AmazonS3Client, args.BucketName, args.BlobName))
            {
                throw new BlobAlreadyExistsException(
                    $"Saving BLOB '{args.BlobName}' does already exists in the container '{args.BucketName}'! Set {nameof(args.OverrideExisting)} if it should be overwritten.",
                    args.BucketName,
                    args.BlobName);
            }

            await AmazonS3Client.PutObjectAsync(new PutObjectRequest
            {
                BucketName = args.BucketName,
                Key = args.BlobName,
                InputStream = args.BlobStream
            }, args.CancellationToken);
        }

        public async Task<bool> DeleteAsync(BlobProviderDeleteArgs args)
        {
            if (!await BlobExistsAsync(AmazonS3Client, args.BucketName, args.BlobName))
            {
                return false;
            }
            await AmazonS3Client.DeleteObjectAsync(new DeleteObjectRequest
            {
                BucketName = args.BucketName,
                Key = args.BlobName,
            });
            return true;
        }

        public Task<bool> ExistsAsync(BlobProviderExistsArgs args)
        {
            return BlobExistsAsync(AmazonS3Client, args.BucketName, args.BlobName);
        }

        public async Task<Stream> GetOrNullAsync(BlobProviderGetArgs args)
        {
            var response = await AmazonS3Client.GetObjectAsync(new GetObjectRequest
            {
                BucketName = args.BucketName,
                Key = args.BlobName,
            });

            return HandleObjectResponseError(response);
        }

        public void Dispose()
        {
            AmazonS3Client?.Dispose();
        }

        protected virtual async Task<bool> BlobExistsAsync(IAmazonS3 amazonS3Client, string bucketName, string blobName)
        {
            if (!await AmazonS3Util.DoesS3BucketExistV2Async(amazonS3Client, bucketName))
            {
                return false;
            }

            try
            {
                await amazonS3Client.GetObjectMetadataAsync(bucketName, blobName);
            }
            catch (AmazonS3Exception ex)
            {
                return false;
            }

            return true;
        }

        private Stream HandleObjectResponseError(GetObjectResponse response)
        {
            if (response.HttpStatusCode != HttpStatusCode.OK)
            {
                if (response.HttpStatusCode == HttpStatusCode.NotFound)
                {
                    return null;
                }
                else if (response.HttpStatusCode == HttpStatusCode.Forbidden)
                {
                    throw new BlobAccessDeniedException(response.BucketName, response.Key);
                }
                else
                {
                    throw new HttpRequestException(
                        $"Response status code does not indicate success: {(int)response.HttpStatusCode} ({response.HttpStatusCode}).",
                        null, response.HttpStatusCode);
                }
            }

            return response.ResponseStream;
        }
    }
}
