using Aliyun.OSS.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace BlobStorage.AliyunOss
{
    public static class ExceptionExtensions
    {
        public static bool IsAccessDeniedError(this OssException ex)
        {
            return ex.ErrorCode == "AccessDenied";
        }

        public static bool IsBucketNotFoundError(this OssException ex)
        {
            return ex.ErrorCode == "NoSuchBucket";
        }

        public static bool IsNotFoundError(this OssException ex)
        {
            return ex.ErrorCode == "NoSuchBucket" ||
                ex.ErrorCode == "NoSuchKey";
        }

        public static bool IsAccessDeniedError(this HttpRequestException ex)
        {
            return ex.StatusCode == HttpStatusCode.Forbidden;
        }
    }
}
