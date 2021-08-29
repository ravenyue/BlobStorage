using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlobStorage.Tests.Fakes
{
    public class FakeBlobProvider3 : IBlobProvider
    {
        public virtual Task SaveAsync(BlobProviderSaveArgs args)
        {
            throw new NotImplementedException();
        }

        public virtual Task<bool> DeleteAsync(BlobProviderDeleteArgs args)
        {
            throw new NotImplementedException();
        }

        public virtual Task<bool> ExistsAsync(BlobProviderExistsArgs args)
        {
            throw new NotImplementedException();
        }

        public virtual Task<Stream> GetAsync(BlobProviderGetArgs args)
        {
            throw new NotImplementedException();
        }

        public virtual Task<Stream> GetOrNullAsync(BlobProviderGetArgs args)
        {
            throw new NotImplementedException();
        }

        Task<Stream> IBlobProvider.GetOrNullAsync(BlobProviderGetArgs args)
        {
            throw new NotImplementedException();
        }
    }
}
