using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace BlobStorage
{
    public static class BlobContainerExtensions
    {
        public static async Task SaveAsync(
            this IBlobContainer container,
            string bucketName,
            string blobName,
            byte[] bytes,
            bool overrideExisting = false,
            CancellationToken cancellationToken = default
        )
        {
            using var memoryStream = new MemoryStream(bytes);
            await container.SaveAsync(
                bucketName,
                blobName,
                memoryStream,
                overrideExisting,
                cancellationToken
            );
        }

        public static async Task<byte[]> GetAllBytesAsync(
            this IBlobContainer container,
            string bucketName,
            string blobName,
            CancellationToken cancellationToken = default)
        {
            using var stream = await container.GetAsync(bucketName, blobName, cancellationToken);
            return await stream.GetAllBytesAsync(cancellationToken);
        }

        public static async Task<byte[]> GetAllBytesOrNullAsync(
            this IBlobContainer container,
            string bucketName,
            string blobName,
            CancellationToken cancellationToken = default)
        {
            var stream = await container.GetOrNullAsync(bucketName, blobName, cancellationToken);
            if (stream == null)
            {
                return null;
            }

            using (stream)
            {
                return await stream.GetAllBytesAsync(cancellationToken);
            }
        }

        public static Task SaveAsync(
            this IBlobContainer container,
            string blobName,
            byte[] bytes,
            bool overrideExisting = false,
            CancellationToken cancellationToken = default
        )
        {
            return SaveAsync(container, container.ContainerName, blobName, bytes, overrideExisting, cancellationToken);
        }

        public static Task<byte[]> GetAllBytesAsync(
            this IBlobContainer container,
            string blobName,
            CancellationToken cancellationToken = default)
        {
            return GetAllBytesAsync(container, container.ContainerName, blobName, cancellationToken);
        }

        public static Task<byte[]> GetAllBytesOrNullAsync(
            this IBlobContainer container,
            string blobName,
            CancellationToken cancellationToken = default)
        {
            return GetAllBytesOrNullAsync(container, container.ContainerName, blobName, cancellationToken);
        }

        public static Task SaveAsync(
            this IBlobContainer container,
            string blobName,
            Stream stream,
            bool overrideExisting = false,
            CancellationToken cancellationToken = default)
        {
            return container.SaveAsync(container.ContainerName, blobName, stream, overrideExisting, cancellationToken);
        }

        public static Task<bool> DeleteAsync(
            this IBlobContainer container,
            string blobName,
            CancellationToken cancellationToken = default)
        {
            return container.DeleteAsync(container.ContainerName, blobName, cancellationToken);
        }

        public static Task<bool> ExistsAsync(
            this IBlobContainer container,
            string blobName,
            CancellationToken cancellationToken = default)
        {
            return container.ExistsAsync(container.ContainerName, blobName, cancellationToken);
        }

        public static Task<Stream> GetAsync(
            this IBlobContainer container,
            string blobName,
            CancellationToken cancellationToken = default)
        {
            return container.GetAsync(container.ContainerName, blobName, cancellationToken);
        }

        public static Task<Stream> GetOrNullAsync(
            this IBlobContainer container,
            string blobName,
            CancellationToken cancellationToken = default)
        {
            return container.GetOrNullAsync(container.ContainerName, blobName, cancellationToken);
        }

        private static async Task<byte[]> GetAllBytesAsync(this Stream stream, CancellationToken cancellationToken = default)
        {
            using var memoryStream = new MemoryStream();
            if (stream.CanSeek)
            {
                stream.Position = 0;
            }
            await stream.CopyToAsync(memoryStream, cancellationToken);
            return memoryStream.ToArray();
        }
    }
}