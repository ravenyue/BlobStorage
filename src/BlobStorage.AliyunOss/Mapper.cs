using Aliyun.OSS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlobStorage.AliyunOss
{
    internal class Mapper
    {
        public static BlobResponse MapBlobResponse(OssObject ossObject)
        {
            return new BlobResponse(
                ossObject.Content,
                MapBlobMetadata(ossObject.Metadata));
        }

        public static BlobStat MapBlobMetadata(ObjectMetadata objectMetadata)
        {
            return new BlobStat(
                objectMetadata.ContentLength,
                objectMetadata.ETag,
                objectMetadata.LastModified,
                objectMetadata.UserMetadata);
        }
        
        public static ObjectMetadata MapObjectMetadata(IDictionary<string,string> metadata)
        {
            var objectMetadata = new ObjectMetadata();
            foreach (var data in metadata)
            {
                objectMetadata.UserMetadata.Add(data.Key, data.Value);
            }
            return objectMetadata;
        }
    }
}
