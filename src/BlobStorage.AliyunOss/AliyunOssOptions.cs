using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlobStorage.AliyunOss
{
    public class AliyunOssOptions
    {
        public string AccessKeyId { get; set; }
        public string AccessKeySecret { get; set; }
        public string Endpoint { get; set; }
        public bool CreateBucketIfNotExists { get; set; }
    }
}
