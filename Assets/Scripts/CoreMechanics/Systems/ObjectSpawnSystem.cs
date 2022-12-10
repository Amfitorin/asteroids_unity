using System.Collections.Generic;
using System.Linq;
using CoreMechanics.ObjectLinks.UnityObjectLink;
using CoreMechanics.Pool;
using Cysharp.Threading.Tasks;
using UnityEngine;
using Object = UnityEngine.Object;

namespace CoreMechanics.Systems
{
    public class ObjectSpawnSystem : IObjectSpawnSystem
    {
        private readonly ObjectPool _pool;
        private readonly Dictionary<GameObjectLink, PoolElement> _poolObjects;
        private readonly Dictionary<Object, PoolElement> _poolSpawned = new();

        public ObjectSpawnSystem(PoolElement[] elements, ObjectPool pool)
        {
            _pool = pool;
            _poolObjects = elements.ToDictionary(x => x.Prefab);
        }

        public async UniTask<T> SpawnObject<T>(GameObjectLink prefab, Transform root, Vector3 position) where T : Object
        {
            if (_poolObjects.TryGetValue(prefab, out var poolElement) &&
                _pool.TryGetFromPool<T>(prefab.Path, poolElement.PoolType, out var obj))
            {
                return obj;
            }

            var spawned = Object.Instantiate<GameObject>(prefab, position, Quaternion.identity, root);
            if (poolElement != null)
            {
                _poolSpawned.Add(spawned, poolElement);
            }
            if (typeof(T).IsAssignableFrom(typeof(Component)))
            {
                return spawned.GetComponentInChildren<T>();
            }

            return spawned as T;
        }

        public async UniTask DestroyObject(GameObject obj)
        {
            if (_poolSpawned.TryGetValue(obj, out var pool))
            {
                _pool.MoveToPool(pool.Prefab.Path, obj, pool.PoolType);
                obj.SetActive(false);
                _poolSpawned.Remove(obj);
                return;
            }
            
            Object.Destroy(obj);
        }
    }
}