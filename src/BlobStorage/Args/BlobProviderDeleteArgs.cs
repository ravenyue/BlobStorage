using System.Threading;

namespace BlobStorage
{
    public class BlobProviderDeleteArgs : BlobProviderArgs
    {
        public BlobProviderDeleteArgs(
            string bucketName,
            string blobName,
            CancellationToken cancellationToken = default)
            : base(
                bucketName,
                blobName,
                cancellationToken)
        {
        }
    }
}
