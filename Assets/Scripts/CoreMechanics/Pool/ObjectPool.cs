using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UnityEngine;
using Object = UnityEngine.Object;

namespace CoreMechanics.Pool
{
    public class ObjectPool : IObjectPool
    {
        private readonly Dictionary<PoolType, PoolContainer> _pools = new();

        public async UniTask MoveToPool(string path, GameObject obj, PoolType type)
        {
            var pool = GetPool(type);
            await pool.StoreObject(path, obj);
        }

        public bool TryGetFromPool<T>(string path, PoolType type, out T obj) where T : Object
        {
            var pool = GetPool(type);
            return pool.TryGetObject(path, out obj);
        }

        public void ClearPool(PoolType type)
        {
            throw new NotImplementedException();
        }

        public void TryGetFromPool(Object obj, PoolType type)
        {
            throw new NotImplementedException();
        }

        private PoolContainer GetPool(PoolType type)
        {
            if (_pools.TryGetValue(type, out var container)) return container;

            container = new PoolContainer(type.ToString());
            _pools.Add(type, container);
            return container;
        }
    }
}