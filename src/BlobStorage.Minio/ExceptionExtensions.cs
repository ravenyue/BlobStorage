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
    }
}
