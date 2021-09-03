using System.Threading;

namespace BlobStorage
{
    public class BlobProviderGetArgs : BlobProviderArgs
    {
        public BlobProviderGetArgs(
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
