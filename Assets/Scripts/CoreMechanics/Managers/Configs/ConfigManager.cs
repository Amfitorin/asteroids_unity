using System.Collections.Generic;
using Core.Utils.Extensions;
using UnityEngine;

namespace CoreMechanics.Managers.Configs
{
    public class ConfigManager : IConfigManager
    {
        private readonly Dictionary<string, ConfigBase> _cache = new();
        private const string ResourcesPath = "Assets/Resources/";

        public T Load<T>(string path) where T : ConfigBase
        {
            if (path.IsNullOrEmpty())
                return null;

            if (_cache.TryGetValue(path, out var cached) && cached != null)
                return (T)cached;

            return LoadInternal<T>(path);
        }

        private T LoadInternal<T>(string path) where T : ConfigBase
        {
            if (path.IsNullOrEmpty())
                return null;

            if (_cache.TryGetValue(path, out var obj))
            {
                return obj as T;
            }

            if (!path.IsNullOrEmpty())
                if (!LoadFromResources(path, out obj))
                {
                }


            if (_cache.TryAdd(path, obj))
            {
                return obj as T;
            }
            Debug.LogErrorFormat("Can't put config '{0}' with type {1}", path, typeof(T));
            return null;
        }

        private bool LoadFromResources<T>(string path, out T obj) where T : ConfigBase
        {
            obj = null;
            if (!path.StartsWith(ResourcesPath))
                return false;

            path = path[ResourcesPath.Length..];
            var dot = path.LastIndexOf('.');
            var slash = path.LastIndexOf('/');
            path = dot > slash ? path[..dot] : path;
            obj = Resources.Load<T>(path);
            if (obj == null)
                Debug.LogErrorFormat("Can't find Congig '{0}' with type {1}", path, typeof(T));
            return true;
        }
    }
}