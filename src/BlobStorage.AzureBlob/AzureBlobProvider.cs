using Azure;
using Azure.Storage.Blobs;
using Microsoft.Extensions.Options;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace BlobStorage.AzureBlob
{
    public class AzureBlobProvider : IBlobProvider
    {
        protected AzureBlobOptions Options { get; }

        public AzureBlobProvider(IOptions<AzureBlobOptions> options)
        {
            Options = options.Value;
        }

        public virtual async Task SaveAsync(BlobProviderSaveArgs args)
        {
            var (containerClient, blobClient) = GetBlobClient(args.BucketName, args.BlobName);

            if (!args.OverrideExisting &&
                await BlobExistsAsync(blobClient, args.CancellationToken))
            {
                throw new BlobAlreadyExistsException(
                    $"Saving BLOB '{args.BlobName}' does already exists in the container '{args.BucketName}'! Set {nameof(args.OverrideExisting)} if it should be overwritten.",
                    args.BucketName,
                    args.BlobName);
            }

            try
            {
                if (Options.CreateBucketIfNotExists)
                {
                    await containerClient.CreateIfNotExistsAsync(
                        cancellationToken: args.CancellationToken);
                }
                await blobClient.UploadAsync(args.BlobStream, true, args.CancellationToken);
            }
            catch (RequestFailedException ex)
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
            var (_, blobClient) = GetBlobClient(args.BucketName, args.BlobName);
            try
            {
                return await blobClient.DeleteIfExistsAsync(cancellationToken: args.CancellationToken);
            }
            catch (RequestFailedException ex)
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
        }

        public Task<bool> ExistsAsync(BlobProviderExistsArgs args)
        {
            var (_, blobClient) = GetBlobClient(args.BucketName, args.BlobName);

            return BlobExistsAsync(blobClient, args.CancellationToken);
        }

        public async Task<Stream> GetOrNullAsync(BlobProviderGetArgs args)
        {
            var (_, blobClient) = GetBlobClient(args.BucketName, args.BlobName);

            try
            {
                var response = await blobClient.DownloadAsync(args.CancellationToken);
                return response.Value.Content;
            }
            catch (RequestFailedException ex)
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

        protected virtual async Task<bool> BlobExistsAsync(
            BlobClient blobClient,
            CancellationToken cancellationToken = default)
        {
            try
            {
                return (await blobClient.ExistsAsync(cancellationToken)).Value;
            }
            catch (RequestFailedException ex)
            {
                if (ex.IsNotFoundError())
                {
                    return false;
                }
                if (ex.IsAccessDeniedError())
                {
                    throw new BlobAccessDeniedException(blobClient.BlobContainerName, blobClient.Name, ex);
                }
                throw;
            }
        }

        protected virtual (BlobContainerClient, BlobClient) GetBlobClient(string bucketName, string blobName)
        {
            var containerClient = GetBlobContainerClient(bucketName);
            var blobClient = containerClient.GetBlobClient(blobName);

            return (containerClient, blobClient);
        }

        protected virtual BlobContainerClient GetBlobContainerClient(string bucketName)
        {
            var blobServiceClient = new BlobServiceClient(Options.ConnectionString);
            return blobServiceClient.GetBlobContainerClient(bucketName);
        }
    }
}
