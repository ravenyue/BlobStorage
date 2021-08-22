using System.Threading;

namespace BlobStorage
{
    public abstract class BlobProviderArgs
    {
        public string BucketName { get; }

        public string BlobName { get; }

        public CancellationToken CancellationToken { get; }

        protected BlobProviderArgs(
            string bucketName,
            string blobName,
            CancellationToken cancellationToken = default)
        {
            BucketName = Check.NotNullOrWhiteSpace(bucketName, nameof(bucketName));
            BlobName = Check.NotNullOrWhiteSpace(blobName, nameof(blobName));
            CancellationToken = cancellationToken;
        }
    }
}
