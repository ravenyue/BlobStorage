using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlobStorage
{
    public class BlobContainerConfigurations
    {
        private BlobContainerConfiguration Default => GetConfiguration<DefaultContainer>();
        private readonly Dictionary<string, BlobContainerConfiguration> _containers;

        public BlobContainerConfigurations()
        {
            _containers = new Dictionary<string, BlobContainerConfiguration>
            {
                //Add default container
                [BlobContainerNameAttribute.GetContainerName<DefaultContainer>()] = new BlobContainerConfiguration()
            };
        }

        public BlobContainerConfigurations Configure<TContainer>(
            Action<BlobContainerConfiguration> configureAction)
        {
            return Configure(
                BlobContainerNameAttribute.GetContainerName<TContainer>(),
                configureAction
            );
        }

        public BlobContainerConfigurations Configure(
            string name,
            Action<BlobContainerConfiguration> configureAction)
        {
            Check.NotNullOrWhiteSpace(name, nameof(name));
            Check.NotNull(configureAction, nameof(configureAction));

            configureAction(
                _containers.GetOrAdd(
                    name,
                    () => new BlobContainerConfiguration()
                )
            );

            return this;
        }

        public BlobContainerConfigurations ConfigureDefault(Action<BlobContainerConfiguration> configureAction)
        {
            configureAction(Default);
            return this;
        }

        public BlobContainerConfigurations ConfigureAll(Action<string, BlobContainerConfiguration> configureAction)
        {
            foreach (var container in _containers)
            {
                configureAction(container.Key, container.Value);
            }

            return this;
        }

        public BlobContainerConfiguration GetConfiguration<TContainer>()
        {
            return GetConfiguration(BlobContainerNameAttribute.GetContainerName<TContainer>());
        }

        public BlobContainerConfiguration GetConfiguration(string name)
        {
            Check.NotNullOrWhiteSpace(name, nameof(name));

            return _containers.GetOrDefault(name) ?? Default;
        }

        internal IEnumerable<IBlobStorageOptionsExtension> GetExtensions()
        {
            foreach (var container in _containers)
            {
                foreach (var extension in container.Value.Extensions)
                {
                    yield return extension;
                }
            }
        }
    }
}
