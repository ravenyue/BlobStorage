using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace BlobStorage.FileSystem
{
    public class FileSystemBlobProvider : IBlobProvider
    {
        protected IBlobFilePathCalculator FilePathCalculator { get; }
        protected FileSystemOptions Options { get; }

        public FileSystemBlobProvider(
            IBlobFilePathCalculator filePathCalculator,
            IOptions<FileSystemOptions> options)
        {
            FilePathCalculator = filePathCalculator;
            Options = options.Value;
        }

        public async Task SaveAsync(BlobProviderSaveArgs args)
        {
            var filePath = FilePathCalculator.Calculate(args, Options);

            if (!args.OverrideExisting && File.Exists(filePath))
            {
                throw new BlobAlreadyExistsException(
                    $"Saving BLOB '{args.BlobName}' does already exists in the container '{args.BucketName}'! Set {nameof(args.OverrideExisting)} if it should be overwritten.",
                    args.BucketName,
                    args.BlobName);
            }

            FileHelper.CreateDirIfNotExists(Path.GetDirectoryName(filePath));

            var fileMode = args.OverrideExisting
                ? FileMode.Create
                : FileMode.CreateNew;

            using var fileStream = File.Open(filePath, fileMode, FileAccess.Write);
            await args.BlobStream.CopyToAsync(fileStream, args.CancellationToken);
            await fileStream.FlushAsync();
        }

        public Task<bool> DeleteAsync(BlobProviderDeleteArgs args)
        {
            var filePath = FilePathCalculator.Calculate(args, Options);
            return Task.FromResult(FileHelper.DeleteIfExists(filePath));
        }

        public Task<bool> ExistsAsync(BlobProviderExistsArgs args)
        {
            var filePath = FilePathCalculator.Calculate(args, Options);
            return Task.FromResult(File.Exists(filePath));
        }

        public Task<Stream> GetOrNullAsync(BlobProviderGetArgs args)
        {
            var filePath = FilePathCalculator.Calculate(args, Options);

            if (!File.Exists(filePath))
            {
                return Task.FromResult<Stream>(null);
            }

            return Task.FromResult((Stream)File.OpenRead(filePath));
        }

        public Task<BlobMetadata> GetOrNullMetadataAsync(BlobProviderGetArgs args)
        {
            var filePath = FilePathCalculator.Calculate(args, Options);
            var fileInfo = new FileInfo(filePath);

            if (!fileInfo.Exists)
            {
                return Task.FromResult<BlobMetadata>(null);
            }

            var metadata = new BlobMetadata(
                fileInfo.Length,
                string.Empty,
                fileInfo.LastWriteTimeUtc,
                null);

            return Task.FromResult(metadata);
        }

        public async Task<BlobResponse> GetOrNullWithMetadataAsync(BlobProviderGetArgs args)
        {
            var metadata = await GetOrNullMetadataAsync(args);
            if (metadata == null)
                return null;

            var stream = await GetOrNullAsync(args);
            if (stream == null)
                return null;

            return new BlobResponse(stream, metadata);
        }
    }
}
