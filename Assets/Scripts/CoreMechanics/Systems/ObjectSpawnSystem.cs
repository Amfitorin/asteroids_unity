using System;
using CoreMechanics.ObjectLinks.UnityObjectLink;
using CoreMechanics.Pool;
using Cysharp.Threading.Tasks;
using UnityEngine;
using Object = UnityEngine.Object;

namespace CoreMechanics.Systems
{
    public class ObjectSpawnSystem : IObjectSpawnSystem
    {
        public ObjectSpawnSystem(PoolElement[] elements)
        {
        }

        public async UniTask SpawnObject<T>(GameObjectLink prefab, Transform root, Vector3 position)
        {
        }

        public UniTask DestroyObject(Object obj)
        {
            throw new NotImplementedException();
        }
    }
}