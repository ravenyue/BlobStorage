using Microsoft.Extensions.Options;
using System;
using System.IO;
using System.Threading.Tasks;

namespace BlobStorage.FileSystem
{
    public class FileSystemBlobProvider : IBlobProvider
    {
        protected IBlobFilePathCalculator FilePathCalculator { get; }
        protected FileSystemBlobProviderOptions Options { get; }

        public FileSystemBlobProvider(
            IBlobFilePathCalculator filePathCalculator,
            IOptions<FileSystemBlobProviderOptions> options)
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
    }
}
