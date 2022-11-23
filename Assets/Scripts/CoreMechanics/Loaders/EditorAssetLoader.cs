#if UNITY_EDITOR
using System;
using System.Collections;
using System.IO;
using Core.Utils.Extensions;
using UnityEngine;
using UnityEditor;


namespace CoreMechanics.Loaders
{
    public class EditorAssetLoader : AssetLoaderBase
    {
        private readonly IAssetLoaderCache _loaderCache;

        public EditorAssetLoader(IAssetLoaderCache loaderCache)
        {
            _loaderCache = loaderCache;
        }

        public override void Release()
        {
        }

        protected override bool IsAssetExists(string fullAssetName)
        {
            if (string.IsNullOrEmpty(fullAssetName))
                return false;

            return File.Exists(fullAssetName) && !AssetDatabase.AssetPathToGUID(fullAssetName).IsNullOrEmpty();
        }

        public override T Load<T>(string path)
        {
            if (path.IsNullOrEmpty())
            {
                return null;
            }

            path = GetAssetPath(path);
            var obj = _loaderCache.GetFromCache(path) as T;
            if (obj != null)
                return obj;

            obj = AssetDatabase.LoadAssetAtPath<T>(path);
            if (_loaderCache.PutToCache(path, obj)) return obj;
            Debug.LogErrorFormat("Can't load asset '{0}' with type {1}", path, typeof(T));
            return null;
        }

        public override IEnumerator Load<T>(string path, Action<T> callback)
        {
            var obj = Load<T>(path);
            try
            {
                callback(obj);
            }
            catch (Exception e)
            {
                Debug.LogException(e);
            }

            yield break;
        }
    }
}
#endif