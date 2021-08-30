using BlobStorage.Tests.Fakes;
using BlobStorage.Tests.TestObjects;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlobStorage.Tests
{
    public class BlobStorageTestBase
    {
        public IServiceProvider Provider { get; }
        public BlobStorageTestBase()
        {
            var services = new ServiceCollection();

            services.AddSingleton<FakeBlobProvider1>();
            services.AddSingleton<FakeBlobProvider2>();
            services.AddSingleton<FakeBlobProvider4>();

            services.AddSingleton<FakeBlobNamingValidator1>();
            services.AddSingleton<FakeBlobNamingValidator2>();
            services.AddSingleton<FakeBlobNamingValidator4>();

            services.AddBlobStorage(options =>
            {
                options
                    .ConfigureDefaultContainer(container =>
                    {
                        container.ProviderType = typeof(FakeBlobProvider1);
                        container.NamingValidatorType = typeof(FakeBlobNamingValidator1);
                    })
                    .ConfigureContainer<TestContainer1>(container =>
                    {
                        container.ProviderType = typeof(FakeBlobProvider1);
                        container.NamingValidatorType = typeof(FakeBlobNamingValidator1);
                    })
                    .ConfigureContainer<TestContainer2>(container =>
                    {
                        container.ProviderType = typeof(FakeBlobProvider2);
                        container.NamingValidatorType = typeof(FakeBlobNamingValidator2);
                    })
                    .ConfigureContainer<TestContainer3>(container =>
                    {
                        container.ProviderType = typeof(FakeBlobProvider3);
                        container.NamingValidatorType = typeof(FakeBlobNamingValidator3);
                    })
                    .ConfigureContainer<TestContainer4>(container =>
                    {
                        container.ProviderType = typeof(FakeBlobProvider4);
                        container.NamingValidatorType = typeof(FakeBlobNamingValidator4);
                    })
                    .ConfigureContainer("none", container => { });
            });

            Provider = services.BuildServiceProvider();
        }
    }
}
