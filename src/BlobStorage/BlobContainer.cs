using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace BlobStorage
{
    public class BlobContainer<TContainer> : IBlobContainer<TContainer>
        where TContainer : class
    {
        private readonly IBlobContainer _container;

        public BlobContainer(IBlobContainerFactory blobContainerFactory)
        {
            _container = blobContainerFactory.Create<TContainer>();
        }

        public string ContainerName => _container.ContainerName;

        public Task SaveAsync(
            string bucketName,
            string blobName,
            Stream stream,
            bool overrideExisting = true,
            CancellationToken cancellationToken = default)
        {
            return _container.SaveAsync(
                bucketName,
                blobName,
                stream,
                overrideExisting,
                cancellationToken
            );
        }

        public Task<bool> DeleteAsync(
            string bucketName,
            string blobName,
            CancellationToken cancellationToken = default)
        {
            return _container.DeleteAsync(
                bucketName,
                blobName,
                cancellationToken
            );
        }

        public Task<bool> ExistsAsync(
            string bucketName,
            string blobName,
            CancellationToken cancellationToken = default)
        {
            return _container.ExistsAsync(
                bucketName,
                blobName,
                cancellationToken
            );
        }

        public Task<Stream> GetAsync(
            string bucketName,
            string blobName,
            CancellationToken cancellationToken = default)
        {
            return _container.GetAsync(
                bucketName,
                blobName,
                cancellationToken
            );
        }

        public Task<Stream> GetOrNullAsync(
            string bucketName,
            string blobName,
            CancellationToken cancellationToken = default)
        {
            return _container.GetOrNullAsync(
                bucketName,
                blobName,
                cancellationToken
            );
        }
    }

    public class BlobContainer : IBlobContainer
    {
        public string ContainerName { get; }

        protected BlobContainerConfiguration Configuration { get; }

        protected IBlobProvider Provider { get; }

        protected IBlobNamingValidator BlobNamingValidator { get; }

        public BlobContainer(
            string containerName,
            BlobContainerConfiguration configuration,
            IBlobProvider provider,
            IBlobNamingValidator blobNamingValidator)
        {
            ContainerName = containerName;
            Configuration = configuration;
            Provider = provider;
            BlobNamingValidator = blobNamingValidator;
        }

        public virtual Task SaveAsync(
            string bucketName,
            string blobName,
            Stream stream,
            bool overrideExisting = true,
            CancellationToken cancellationToken = default)
        {
            Check.NotNullOrWhiteSpace(bucketName, nameof(bucketName));
            Check.NotNullOrWhiteSpace(blobName, nameof(blobName));
            Check.NotNull(stream, nameof(stream));

            BlobNamingValidator.EnsureValidNameing(
                bucketName, blobName,
                Configuration.ProviderType.FullName);

            return Provider.SaveAsync(
                new BlobProviderSaveArgs(
                    bucketName,
                    blobName,
                    stream,
                    overrideExisting,
                    cancellationToken
                )
            );
        }

        public virtual Task<bool> DeleteAsync(
            string bucketName,
            string blobName,
            CancellationToken cancellationToken = default)
        {
            Check.NotNullOrWhiteSpace(bucketName, nameof(bucketName));
            Check.NotNullOrWhiteSpace(blobName, nameof(blobName));

            BlobNamingValidator.EnsureValidNameing(
                bucketName, blobName,
                Configuration.ProviderType.FullName);

            return Provider.DeleteAsync(
                new BlobProviderDeleteArgs(
                    bucketName,
                    blobName,
                    cancellationToken
                    )
                );
        }

        public virtual Task<bool> ExistsAsync(
            string bucketName,
            string blobName,
            CancellationToken cancellationToken = default)
        {
            Check.NotNullOrWhiteSpace(bucketName, nameof(bucketName));
            Check.NotNullOrWhiteSpace(blobName, nameof(blobName));

            BlobNamingValidator.EnsureValidNameing(
                bucketName, blobName,
                Configuration.ProviderType.FullName);

            return Provider.ExistsAsync(
                new BlobProviderExistsArgs(
                    bucketName,
                    blobName,
                    cancellationToken
                )
            );
        }

        public virtual async Task<Stream> GetAsync(
            string bucketName,
            string blobName,
            CancellationToken cancellationToken = default)
        {
            Check.NotNullOrWhiteSpace(bucketName, nameof(bucketName));
            Check.NotNullOrWhiteSpace(blobName, nameof(blobName));

            BlobNamingValidator.EnsureValidNameing(
                bucketName, blobName,
                Configuration.ProviderType.FullName);

            var stream = await GetOrNullAsync(bucketName, blobName, cancellationToken);
            if (stream == null)
            {
                throw new BlobNotFoundException(bucketName, blobName);
            }

            return stream;
        }

        public virtual Task<Stream> GetOrNullAsync(
            string bucketName,
            string blobName,
            CancellationToken cancellationToken = default)
        {
            Check.NotNullOrWhiteSpace(bucketName, nameof(bucketName));
            Check.NotNullOrWhiteSpace(blobName, nameof(blobName));

            BlobNamingValidator.EnsureValidNameing(
                bucketName, blobName,
                Configuration.ProviderType.FullName);

            return Provider.GetOrNullAsync(
                new BlobProviderGetArgs(
                    bucketName,
                    blobName,
                    cancellationToken
                )
            );
        }
    }
}
