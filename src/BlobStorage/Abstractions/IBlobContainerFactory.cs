namespace BlobStorage
{
    public interface IBlobContainerFactory
    {
        /// <summary>
        /// Gets a named container.
        /// </summary>
        /// <param name="name">The name of the container</param>
        /// <returns>The container object.</returns>
        IBlobContainer Create(string name);
    }
}