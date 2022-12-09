using CoreMechanics.Pool;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace CoreMechanics.Systems
{
    public interface IObjectSpawnSystem
    {
        UniTask SpawnObject(Transform root, Vector3 position, PoolType pool);
        UniTask DestroyObject(Object obj)
    }
}