using Minio.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace BlobStorage.Minio
{
    public static class ExceptionExtensions
    {
        public static bool IsAccessDeniedError(this MinioException ex)
        {
            return (ex is AccessDeniedException) ||
                (ex.Response != null && ex.Response.Code == "AccessDenied") ||
                (ex.ServerResponse != null && ex.ServerResponse.StatusCode == HttpStatusCode.Forbidden);
        }

        public static bool IsNotFoundError(this MinioException ex)
        {
            return (ex is ObjectNotFoundException) ||
                (ex is BucketNotFoundException);
        }

        //public static void WrapAccessDeniedException(MinioException ex, string bucketName, string blobName, string message = null)
        //{
        //    if ((ex is AccessDeniedException) ||
        //        (ex.Response != null && ex.Response.Code == "AccessDenied") ||
        //        (ex.ServerResponse != null && ex.ServerResponse.StatusCode == HttpStatusCode.Forbidden))
        //    {
        //        if (message == null)
        //            throw new BlobAccessDeniedException(bucketName, blobName, ex);
        //        else
        //            throw new BlobAccessDeniedException(message, bucketName, blobName, ex);
        //    }
        //}
    }
}
