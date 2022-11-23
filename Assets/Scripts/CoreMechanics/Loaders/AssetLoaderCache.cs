using System;
using System.Collections.Generic;
using JetBrains.Annotations;

namespace CoreMechanics.Loaders
{
    public class AssetLoaderCache : IAssetLoaderCache
    {
        private readonly Dictionary<string, WeakReference> _cache = new();

        public bool PutToCache(string path, object value)
        {
            if (value == null)
                return false;

            _cache[path] = new WeakReference(value);
            return true;
        }

        [CanBeNull]
        public object GetFromCache(string path)
        {
            if (!_cache.TryGetValue(path, out var reference))
                return null;

            var storedTarget = reference.Target;
            if (storedTarget == null)
                _cache.Remove(path);

            return storedTarget;
        }

        public void ClearCache()
        {
            _cache.Clear();
        }
    }
}