using Azure.Storage.Blobs.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlobStorage.AzureBlob
{
    internal class Mapper
    {
        public static BlobResponse MapBlobResponse(BlobDownloadInfo info)
        {
            return new BlobResponse(
                info.Content,
                MapBlobMetadata(info.Details));
        }

        public static BlobStat MapBlobMetadata(BlobProperties properties)
        {
            return new BlobStat(
                properties.ContentLength,
                properties.ETag.ToString(),
                properties.LastModified,
                properties.Metadata);
        }
        
        public static BlobStat MapBlobMetadata(BlobDownloadDetails details)
        {
            return new BlobStat(
                details.ContentLength,
                details.ETag.ToString(),
                details.LastModified,
                details.Metadata);
        }
    }
}
