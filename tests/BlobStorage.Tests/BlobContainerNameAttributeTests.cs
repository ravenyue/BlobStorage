using BlobStorage.Tests.TestObjects;
using FluentAssertions;
using System;
using Xunit;

namespace BlobStorage.Tests
{
    public class BlobContainerNameAttributeTests
    {
        [Fact]
        public void Should_Get_Specified_Name()
        {
            BlobContainerNameAttribute
                .GetContainerName<TestContainer2>()
                .Should()
                .Be("Test2");
        }

        [Fact]
        public void Should_Get_Full_Class_Name_If_Not_Specified()
        {
            BlobContainerNameAttribute
                .GetContainerName<TestContainer1>()
                .Should()
                .Be(typeof(TestContainer1).FullName);
        }
    }
}
