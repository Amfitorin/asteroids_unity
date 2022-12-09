using UnityEngine;

namespace CoreMechanics.Pool
{
    public interface IObjectPool
    {
        void MoveToPool(string path, Object obj, PoolType type);
        bool TryGetFromPool<T>(string path, PoolType type, out T obj) where T : Object;
        void ClearPool(PoolType type);
    }
}