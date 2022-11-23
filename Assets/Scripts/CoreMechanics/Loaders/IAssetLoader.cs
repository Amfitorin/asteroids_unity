using System;
using System.Collections;
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

        IEnumerator Load<T>(string assetPath, Action<T> callback)
            where T : Object;
    }
}