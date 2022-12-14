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

        public ObjectSpawnSystem(IEnumerable<PoolElement> elements, ObjectPool pool)
        {
            _pool = pool;
            _poolObjects = elements?.ToDictionary(x => x.Prefab) ?? new Dictionary<GameObjectLink, PoolElement>();
        }

        public async UniTask<T> SpawnObject<T>(GameObjectLink prefab, Transform root, Vector3 position) where T : Object
        {
            if (_poolObjects.TryGetValue(prefab, out var poolElement) &&
                _pool.TryGetFromPool<T>(prefab.Path, poolElement.PoolType, out var obj))
            {
                var gameObject = await GetGameObject(obj);
                gameObject.transform.SetParent(root);
                gameObject.transform.position = position;
                gameObject.transform.rotation = Quaternion.identity;
                return obj;
            }

            var spawned = Object.Instantiate<GameObject>(prefab, position, Quaternion.identity, root);
            if (poolElement != null)
            {
                _poolSpawned.Add(spawned, poolElement);
            }

            if (typeof(Component).IsAssignableFrom(typeof(T)))
            {
                return spawned.GetComponentInChildren<T>();
            }

            return spawned as T;
        }

        public async UniTask DestroyObject<T>(T obj) where T : Object
        {
            var gameObject = await GetGameObject(obj);
            if (_poolSpawned.TryGetValue(gameObject, out var pool))
            {
                _pool.MoveToPool(pool.Prefab.Path, gameObject, pool.PoolType);

                _poolSpawned.Remove(obj);
                return;
            }

            Object.Destroy(gameObject);
        }

        private async UniTask<GameObject> GetGameObject<T>(T obj) where T : Object
        {
            if (!typeof(Component).IsAssignableFrom(typeof(T))) return obj as GameObject;
            await UniTask.SwitchToMainThread();
            return (obj as Component)?.gameObject;
        }
    }
}