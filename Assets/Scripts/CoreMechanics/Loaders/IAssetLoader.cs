using System;
using System.Collections;
using CoreMechanics.Managers.Configs;
using JetBrains.Annotations;
using Object = UnityEngine.Object;

namespace CoreMechanics.Loaders
{
    public interface IAssetLoader
    {
        [CanBeNull]
        T Load<T>(string assetPath)
            where T : Object;

        bool TryLoadResource<T>(string path, out T res) where T : Object;
        bool TryLoadConfig<T>(string path, out T res) where T : ConfigBase;


        IEnumerator Load<T>(string assetPath, Action<T> callback)
            where T : Object;
    }
}