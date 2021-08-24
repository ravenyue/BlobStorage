using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlobStorage.AmazonS3
{
    public class AmazonS3Options
    {
        public string AccessKeyId { get; set; }
        public string SecretAccessKey { get; set; }
        public string Region { get; set; }
    }
}
