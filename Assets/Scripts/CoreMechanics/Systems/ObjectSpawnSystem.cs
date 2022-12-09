using CoreMechanics.Pool;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace CoreMechanics.Systems
{
    public class ObjectSpawnSystem : IObjectSpawnSystem
    {
        public ObjectSpawnSystem(Poolset)
        
        public UniTask SpawnObject(Transform root, Vector3 position, PoolType pool)
        {
            
        }

        public UniTask DestroyObject(Object obj)
        {
            throw new System.NotImplementedException();
        }
    }
}