using Azure.Storage.Blobs;
using Microsoft.Extensions.Options;
using System;
using System.IO;
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
                await BlobExistsAsync(containerClient, blobClient))
            {
                throw new BlobAlreadyExistsException(
                    $"Saving BLOB '{args.BlobName}' does already exists in the container '{args.BucketName}'! Set {nameof(args.OverrideExisting)} if it should be overwritten.",
                    args.BucketName,
                    args.BlobName);
            }

            if (Options.CreateBucketIfNotExists)
            {
                await containerClient.CreateIfNotExistsAsync();
            }

            await blobClient.UploadAsync(args.BlobStream, true, args.CancellationToken);
        }

        public async Task<bool> DeleteAsync(BlobProviderDeleteArgs args)
        {
            var (_, blobClient) = GetBlobClient(args.BucketName, args.BlobName);
            return await blobClient.DeleteIfExistsAsync();
        }

        public Task<bool> ExistsAsync(BlobProviderExistsArgs args)
        {
            var (containerClient, blobClient) = GetBlobClient(args.BucketName, args.BlobName);

            return BlobExistsAsync(containerClient, blobClient);
        }

        public async Task<Stream> GetOrNullAsync(BlobProviderGetArgs args)
        {
            var (containerClient, blobClient) = GetBlobClient(args.BucketName, args.BlobName);

            if (!await BlobExistsAsync(containerClient, blobClient))
            {
                return null;
            }

            var response = await blobClient.DownloadAsync(args.CancellationToken);
            return response.Value.Content;
        }

        protected virtual async Task<bool> BlobExistsAsync(
            BlobContainerClient containerClient,
            BlobClient blobClient)
        {
            return (await containerClient.ExistsAsync()).Value &&
                   (await blobClient.ExistsAsync()).Value;
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
