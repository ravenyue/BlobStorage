using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlobStorage.Tests.TestObjects
{
    public class TestContainer1
    {

    }

    [BlobContainerName(Name)]
    public class TestContainer2
    {
        public const string Name = "Test2";
    }

    [BlobContainerName(Name)]
    public class TestContainer3
    {
        public const string Name = "Test3";
    }

    [BlobContainerName(Name)]
    public class TestContainer4
    {
        public const string Name = "Test4";
    }
}
