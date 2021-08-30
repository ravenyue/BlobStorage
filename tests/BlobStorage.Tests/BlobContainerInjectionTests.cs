using BlobStorage.Tests.TestObjects;
using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace BlobStorage.Tests
{
    public class BlobContainerInjectionTests : BlobStorageTestBase
    {
        [Fact]
        public void Should_Inject_DefaultContainer_For_Non_Generic_Interface()
        {
            Provider.GetRequiredService<IBlobContainer>()
                .Should()
                .BeOfType<BlobContainer<DefaultContainer>>();
        }

        [Fact]
        public void Should_Inject_Specified_Container_For_Generic_Interface()
        {
            Provider.GetRequiredService<IBlobContainer<DefaultContainer>>()
                .Should()
                .BeOfType<BlobContainer<DefaultContainer>>();

            Provider.GetRequiredService<IBlobContainer<TestContainer1>>()
                .Should()
                .BeOfType<BlobContainer<TestContainer1>>();

            Provider.GetRequiredService<IBlobContainer<TestContainer2>>()
                .Should()
                .BeOfType<BlobContainer<TestContainer2>>();
        }
    }
}
