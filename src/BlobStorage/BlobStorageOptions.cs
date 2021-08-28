using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlobStorage
{
    public class BlobStorageOptions
    {
        private BlobContainerConfiguration DefaultContainer => GetConfiguration<DefaultContainer>();
        private readonly Dictionary<string, BlobContainerConfiguration> _containers;

        public BlobStorageOptions()
        {
            _containers = new Dictionary<string, BlobContainerConfiguration>
            {
                //Add default container
                [BlobContainerNameAttribute.GetContainerName<DefaultContainer>()] = new BlobContainerConfiguration()
            };
            Extensions = new List<IBlobStorageOptionsExtension>();
        }

        internal List<IBlobStorageOptionsExtension> Extensions { get; }

        public void RegisterExtension(IBlobStorageOptionsExtension extension)
        {
            if (extension == null)
            {
                throw new ArgumentNullException(nameof(extension));
            }

            Extensions.Add(extension);
        }

        public BlobStorageOptions ConfigureContainer<TContainer>(
             Action<BlobContainerConfiguration> configureAction)
        {
            return ConfigureContainer(
                BlobContainerNameAttribute.GetContainerName<TContainer>(),
                configureAction);
        }

        public BlobStorageOptions ConfigureContainer(
            string name,
            Action<BlobContainerConfiguration> configureAction)
        {
            Check.NotNullOrWhiteSpace(name, nameof(name));
            Check.NotNull(configureAction, nameof(configureAction));

            var container = _containers.GetOrAdd(name, () => new BlobContainerConfiguration());
            configureAction(container);

            Extensions.AddRange(container.Extensions);
            container.Extensions.Clear();

            return this;
        }

        public BlobStorageOptions ConfigureDefaultContainer(Action<BlobContainerConfiguration> configureAction)
        {
            configureAction(DefaultContainer);

            Extensions.AddRange(DefaultContainer.Extensions);
            DefaultContainer.Extensions.Clear();

            return this;
        }

        public BlobContainerConfiguration GetConfiguration<TContainer>()
        {
            return GetConfiguration(BlobContainerNameAttribute.GetContainerName<TContainer>());
        }

        public BlobContainerConfiguration GetConfiguration(string name)
        {
            Check.NotNullOrWhiteSpace(name, nameof(name));

            return _containers.GetOrDefault(name) ?? DefaultContainer;
        }
    }
}
