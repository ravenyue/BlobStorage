using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlobStorage.Tests.Fakes
{
    public class FakeBlobNamingValidator1 : IBlobNamingValidator
    {
        public bool ValidateBlobName(string blobName)
        {
            throw new NotImplementedException();
        }

        public bool ValidateBucketName(string bucketName)
        {
            throw new NotImplementedException();
        }
    }

    public class FakeBlobNamingValidator2 : IBlobNamingValidator
    {
        public bool ValidateBlobName(string blobName)
        {
            throw new NotImplementedException();
        }

        public bool ValidateBucketName(string bucketName)
        {
            throw new NotImplementedException();
        }
    }

    public class FakeBlobNamingValidator3 : IBlobNamingValidator
    {
        public bool ValidateBlobName(string blobName)
        {
            throw new NotImplementedException();
        }

        public bool ValidateBucketName(string bucketName)
        {
            throw new NotImplementedException();
        }
    }

    public class FakeBlobNamingValidator4
    {
    }
}
