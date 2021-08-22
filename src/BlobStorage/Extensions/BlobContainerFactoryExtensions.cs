namespace BlobStorage
{
    public static class BlobContainerFactoryExtensions
    {
        public static IBlobContainer Create<TContainer>(
            this IBlobContainerFactory blobContainerFactory
        )
        {
            return blobContainerFactory
                .Create(BlobContainerNameAttribute.GetContainerName<TContainer>());
        }
    }
}
