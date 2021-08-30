using Amazon.S3;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace BlobStorage.AmazonS3
{
    public static class ExceptionExtensions
    {
        public static bool IsAccessDeniedError(this AmazonS3Exception ex)
        {
            return ex.StatusCode == HttpStatusCode.Forbidden;
        }

        public static bool IsNotFoundError(this AmazonS3Exception ex)
        {
            return ex.StatusCode == HttpStatusCode.NotFound;
        }

        public static bool IsBucketNotFoundError(this AmazonS3Exception ex)
        {
            return ex.StatusCode == HttpStatusCode.NotFound &&
                ex.ErrorCode == "NoSuchBucket";
        }
    }
}
