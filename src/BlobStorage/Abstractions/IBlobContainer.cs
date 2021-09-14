using System;
using System.Collections.Generic;
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

        /// <summary>
        /// Saves a blob <see cref="Stream"/> to the bucket.
        /// </summary>
        /// <param name="bucketName">The name of the bucket</param>
        /// <param name="blobName">The name of the blob</param>
        /// <param name="stream">A stream for the blob</param>
        /// <param name="overrideExisting">
        /// Set true(default) to override if there is already a blob in the bucket with the given name.
        /// If set to false throws exception if there is already a blob in the bucket with the given name.
        /// </param>
        /// <param name="metadata">BLOB metadata to be stored. Defaults to null.</param>
        /// <param name="cancellationToken">Optional cancellation token to cancel the operation</param>
        /// <returns></returns>
        /// <exception cref="BlobAlreadyExistsException">Blob already exists and <paramref name="overrideExisting"/> is set to false</exception>
        /// <exception cref="BlobBucketNotFoundException">Bucket does not exist and CreateBucketIfNotExists is set to false</exception>
        /// <exception cref="BlobAccessDeniedException">No permission to access buckets or blob</exception>
        Task SaveAsync(
            string bucketName,
            string blobName,
            Stream stream,
            bool overrideExisting = true,
            Dictionary<string, string> metadata = null,
            CancellationToken cancellationToken = default);

        /// <summary>
        /// Deletes a blob from the bucket.
        /// </summary>
        /// <param name="bucketName">The name of the bucket</param>
        /// <param name="blobName">The name of the blob</param>
        /// <param name="cancellationToken">Optional cancellation token to cancel the operation</param>
        /// <returns>
        /// Returns true if actually deleted the blob.
        /// Returns false if the blob with the given <paramref name="blobName"/> was not exists.  
        /// </returns>
        /// <exception cref="BlobAccessDeniedException">No permission to access buckets or blob</exception>
        Task<bool> DeleteAsync(
            string bucketName,
            string blobName,
            CancellationToken cancellationToken = default);

        /// <summary>
        /// Checks if a blob does exists in the bucket.
        /// </summary>
        /// <param name="bucketName">The name of the bucket</param>
        /// <param name="blobName">The name of the blob</param>
        /// <param name="cancellationToken">Optional cancellation token to cancel the operation</param>
        /// <returns>Returns true if the blob exists.</returns>
        /// <exception cref="BlobAccessDeniedException">No permission to access buckets or blob</exception>
        Task<bool> ExistsAsync(
            string bucketName,
            string blobName,
            CancellationToken cancellationToken = default);

        /// <summary>
        /// Gets a blob from the bucket, or returns null if there is no blob with the given <paramref name="blobName"/>.
        /// </summary>
        /// <param name="bucketName">The name of the bucket</param>
        /// <param name="blobName">The name of the blob</param>
        /// <param name="cancellationToken">Optional cancellation token to cancel the operation</param>
        /// <returns><see cref="BlobResponse"/> with content stream and metadata</returns>
        /// <exception cref="BlobAccessDeniedException">No permission to access buckets or blob</exception>
        Task<BlobResponse> GetOrNullAsync(
            string bucketName,
            string blobName,
            CancellationToken cancellationToken = default);

        /// <summary>
        /// Gets a blob metadata, or returns null if there is no blob with the given <paramref name="blobName"/>.
        /// </summary>
        /// <param name="bucketName">The name of the bucket</param>
        /// <param name="blobName">The name of the blob</param>
        /// <param name="cancellationToken">Optional cancellation token to cancel the operation</param>
        /// <returns>A <see cref="BlobStat"/> describing the blob's metadata.</returns>
        /// <exception cref="BlobAccessDeniedException">No permission to access buckets or blob</exception>
        Task<BlobStat> StatOrNullAsync(
            string bucketName,
            string blobName,
            CancellationToken cancellationToken = default);
    }
}
