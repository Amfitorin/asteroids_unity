using System;
using System.Collections;
using System.Collections.Generic;
using Core.Utils.Extensions;
using UnityEngine;

namespace CoreMechanics.Loaders
{
    public sealed class AssetLoader : AssetLoaderBase
    {
        private readonly IAssetLoaderCache _loaderCache;
        private PackInfo[] _externalPackInfos;

        private IDisposable _metadataLoader;

        public AssetLoader(IAssetLoaderCache loaderCache)
        {
            _loaderCache = loaderCache;
        }

        public override void Release()
        {
            if (_metadataLoader != null)
            {
                _metadataLoader.Dispose();
                _metadataLoader = null;
            }
        }

        public override T Load<T>(string path)
        {
            if (path.IsNullOrEmpty())
                return null;

            path = GetAssetPath(path);
            var obj = _loaderCache.GetFromCache(path) as T;
            if (obj != null)
            {
                return obj;
            }


            if (_loaderCache.PutToCache(path, obj))
            {
                return obj;
            }

            Debug.LogErrorFormat("Can't load asset '{0}' with type {1}", path, typeof(T));
            return null;
        }

        public override IEnumerator Load<T>(string path, Action<T> callback)
        {
            T obj = null;
            if (!path.IsNullOrEmpty())
                if (!TryLoadResource(path, out obj))
                {
                }

            try
            {
                callback(obj);
            }
            catch (Exception e)
            {
                Debug.LogException(e);
            }

            yield return null;
        }

        protected override bool IsAssetExists(string fullAssetName)
        {
            if (string.IsNullOrEmpty(fullAssetName))
                return false;

            return true;
        }

        private class PackInfo
        {
            public string FileName;
            public Dictionary<string, ulong> Offsets;
        }
    }
}