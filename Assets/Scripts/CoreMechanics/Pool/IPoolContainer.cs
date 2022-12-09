using UnityEngine;

namespace CoreMechanics.Pool
{
    public interface IPoolContainer
    {
        void StoreObject(string path, GameObject obj);
        bool TryGetObject<T>(string path, out T obj) where T : Object;
    }
}