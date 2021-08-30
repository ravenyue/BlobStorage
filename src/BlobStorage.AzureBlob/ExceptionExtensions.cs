using Azure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace BlobStorage.AzureBlob
{
    public static class ExceptionExtensions
    {
        public static bool IsAccessDeniedError(this RequestFailedException ex)
        {
            return ex.Status == (int)HttpStatusCode.Forbidden ||
                ex.Status == (int)HttpStatusCode.Unauthorized;
        }

        public static bool IsNotFoundError(this RequestFailedException ex)
        {
            return ex.Status == (int)HttpStatusCode.NotFound;
        }

        public static bool IsBucketNotFoundError(this RequestFailedException ex)
        {
            return ex.Status == (int)HttpStatusCode.NotFound &&
                ex.ErrorCode == "ContainerNotFound";
        }
    }
}
