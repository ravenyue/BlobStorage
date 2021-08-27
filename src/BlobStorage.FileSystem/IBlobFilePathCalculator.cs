using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlobStorage.FileSystem
{
    public interface IBlobFilePathCalculator
    {
        string Calculate(BlobProviderArgs args, FileSystemOptions options);
    }
}
