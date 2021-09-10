using Amazon.S3.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlobStorage.AmazonS3
{
    internal class Mapper
    {
        public static BlobResponse MapBlobResponse(GetObjectResponse response)
        {
            return new BlobResponse(
                response.ResponseStream,
                MapBlobMetadata(response));
        }

        public static BlobStat MapBlobMetadata(GetObjectResponse response)
        {
            return new BlobStat(
                response.ContentLength,
                response.ETag,
                response.LastModified,
                MapDictionary(response.Metadata));
        }

        public static BlobStat MapBlobMetadata(GetObjectMetadataResponse response)
        {
            return new BlobStat(
                response.ContentLength,
                response.ETag,
                response.LastModified,
                MapDictionary(response.Metadata));
        }

        public static IDictionary<string, string> MapDictionary(MetadataCollection collection)
        {
            var dict = new Dictionary<string, string>();

            foreach (var key in collection.Keys)
            {
                dict.Add(key, collection[key]);
            }
            return dict;
        }
    }
}
