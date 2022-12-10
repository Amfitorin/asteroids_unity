using System.Collections.Generic;
using Core.Utils.Extensions;
using UnityEditor;

namespace CoreMechanics.Managers.Configs
{
    public class EditorConfigManager : IConfigManager
    {
        private readonly Dictionary<string, ConfigBase> _cache = new();

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
            return AssetDatabase.LoadAssetAtPath<T>(path);
        }
    }
}