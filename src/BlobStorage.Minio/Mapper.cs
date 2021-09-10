using Minio.DataModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlobStorage.Minio
{
    public class Mapper
    {
        public static BlobStat MapBlobMetadata(ObjectStat stat)
        {
            return new BlobStat(
                stat.Size,
                stat.ETag,
                stat.LastModified,
                stat.MetaData);
        }
    }
}
