using System.Threading;

namespace BlobStorage
{
    public class BlobProviderExistsArgs : BlobProviderArgs
    {
        public BlobProviderExistsArgs(
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
