namespace BlobStorage
{
    public interface IBlobContainerFactory
    {
        IBlobContainer Create(string name);
    }
}