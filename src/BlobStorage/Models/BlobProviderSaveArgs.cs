using System.Collections.Generic;
using System.IO;
using System.Threading;

namespace BlobStorage
{
    public class BlobProviderSaveArgs : BlobProviderArgs
    {
        public Stream BlobStream { get; }

        public bool OverrideExisting { get; }

        public Dictionary<string, string> Metadata { get; }

        public BlobProviderSaveArgs(
            string bucketName,
            string blobName,
            Stream blobStream,
            bool overrideExisting = false,
            Dictionary<string, string> metadata = null,
            CancellationToken cancellationToken = default)
            : base(
                bucketName,
                blobName,
                cancellationToken)
        {
            BlobStream = Check.NotNull(blobStream, nameof(blobStream));
            OverrideExisting = overrideExisting;
            Metadata = metadata;
        }
    }
}
