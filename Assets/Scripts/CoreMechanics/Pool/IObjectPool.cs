using Cysharp.Threading.Tasks;
using UnityEngine;

namespace CoreMechanics.Pool
{
    public interface IObjectPool
    {
        UniTask MoveToPool(string path, GameObject obj, PoolType type);
        bool TryGetFromPool<T>(string path, PoolType type, out T obj) where T : Object;
        void ClearPool(PoolType type);
    }
}