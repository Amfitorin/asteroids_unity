namespace CoreMechanics.Loaders
{
    public interface IAssetLoaderCache
    {
        bool PutToCache(string path, object value);
        object GetFromCache(string path);
        void ClearCache();
    }
}