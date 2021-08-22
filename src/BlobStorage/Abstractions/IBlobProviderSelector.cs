namespace BlobStorage
{
    public interface IBlobProviderSelector
    {
        IBlobProvider Get(string containerName);
    }
}
