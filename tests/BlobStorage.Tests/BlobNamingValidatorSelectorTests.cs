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
    public class BlobNamingValidatorSelectorTests : BlobStorageTestBase
    {
        private readonly IBlobNamingValidatorSelector _selector;

        public BlobNamingValidatorSelectorTests()
        {
            _selector = Provider.GetRequiredService<IBlobNamingValidatorSelector>();
        }

        [Fact]
        public void Should_Select_Configured_Validator()
        {
            var name1 = BlobContainerNameAttribute
                .GetContainerName<TestContainer1>();

            var name2 = BlobContainerNameAttribute
                .GetContainerName<TestContainer2>();

            _selector.Get(name1)
                .Should()
                .BeAssignableTo<FakeBlobNamingValidator1>();

            _selector.Get(name2)
                .Should()
                .BeAssignableTo<FakeBlobNamingValidator2>();
        }

        [Fact]
        public void Should_Select_Default_Validator_If_Not_Set()
        {
            _selector.Get("none")
                .Should()
                .BeAssignableTo<DefaultBlobNamingValidator>();
        }

        [Fact]
        public void Should_Throw_Exception_If_Provider_Not_Register()
        {
            Action act = () => _selector.Get(TestContainer3.Name);

            act.Should()
                .Throw<InvalidOperationException>()
                .WithMessage($"Type {typeof(FakeBlobNamingValidator3).AssemblyQualifiedName} is not registered");
        }

        [Fact]
        public void Should_Throw_Exception_If_Provider_Unimplemented_IBlobProvider()
        {
            Action act = () => _selector.Get(TestContainer4.Name);

            act.Should()
                .Throw<InvalidOperationException>()
                .WithMessage($"Type ({typeof(FakeBlobNamingValidator4).AssemblyQualifiedName}) does not implement the ({typeof(IBlobNamingValidator).AssemblyQualifiedName}) interface");
        }
    }
}
