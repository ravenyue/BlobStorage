using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlobStorage.Tests.Fakes
{
    public class FakeBlobProvider1 : IBlobProvider
    {
        public Task<bool> DeleteAsync(BlobProviderDeleteArgs args)
        {
            throw new NotImplementedException();
        }

        public Task<bool> ExistsAsync(BlobProviderExistsArgs args)
        {
            throw new NotImplementedException();
        }

        public Task<BlobResponse> GetOrNullAsync(BlobProviderGetArgs args)
        {
            throw new NotImplementedException();
        }

        public Task SaveAsync(BlobProviderSaveArgs args)
        {
            throw new NotImplementedException();
        }

        public Task<BlobStat> StatOrNullAsync(BlobProviderGetArgs args)
        {
            throw new NotImplementedException();
        }
    }
}
