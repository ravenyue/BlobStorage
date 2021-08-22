using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace BlobStorage
{
    public interface IBlobContainer<TContainer> : IBlobContainer
        where TContainer : class
    {

    }

    public interface IBlobContainer
    {
        string ContainerName { get; }
        Task SaveAsync(string bucketName, string blobName, Stream stream, bool overrideExisting = true, CancellationToken cancellationToken = default);
        Task<bool> DeleteAsync(string bucketName, string blobName, CancellationToken cancellationToken = default);
        Task<bool> ExistsAsync(string bucketName, string blobName, CancellationToken cancellationToken = default);
        Task<Stream> GetAsync(string bucketName, string blobName, CancellationToken cancellationToken = default);
        Task<Stream> GetOrNullAsync(string bucketName, string blobName, CancellationToken cancellationToken = default);
    }
}
