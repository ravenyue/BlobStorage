using BlobStorage.Tests.Fakes;
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
    public class BlobProviderSelectorTests : BlobStorageTestBase
    {
        private readonly IBlobProviderSelector _selector;

        public BlobProviderSelectorTests()
        {
            _selector = Provider.GetRequiredService<IBlobProviderSelector>();
        }

        [Fact]
        public void Should_Select_Configured_Provider()
        {
            _selector.Get<TestContainer1>()
                .Should()
                .BeAssignableTo<FakeBlobProvider1>();

            _selector.Get<TestContainer2>()
                .Should()
                .BeAssignableTo<FakeBlobProvider2>();
        }

        [Fact]
        public void Should_Select_Default_Provider_If_Not_Configured()
        {
            _selector.Get("abc")
                .Should()
                .BeAssignableTo<FakeBlobProvider1>();
        }

        [Fact]
        public void Should_Throw_Exception_If_Provider_Not_Set()
        {
            var containerName = "none";
            Action act = () => _selector.Get(containerName);

            act.Should()
                .Throw<InvalidOperationException>()
                .WithMessage($"Container {containerName} has no provider set");
        }

        [Fact]
        public void Should_Throw_Exception_If_Provider_Not_Register()
        {
            Action act = () => _selector.Get<TestContainer3>();

            act.Should()
                .Throw<InvalidOperationException>()
                .WithMessage($"Type {typeof(FakeBlobProvider3).AssemblyQualifiedName} is not registered");
        }

        [Fact]
        public void Should_Throw_Exception_If_Provider_Unimplemented_IBlobProvider()
        {
            Action act = () => _selector.Get<TestContainer4>();

            act.Should()
                .Throw<InvalidOperationException>()
                .WithMessage($"Type ({typeof(FakeBlobProvider4).AssemblyQualifiedName}) does not implement the ({typeof(IBlobProvider).AssemblyQualifiedName}) interface");
        }
    }
}
