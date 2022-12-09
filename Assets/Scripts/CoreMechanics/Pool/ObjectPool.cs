using System.Collections.Generic;
using Object = UnityEngine.Object;

namespace CoreMechanics.Pool
{
    public class ObjectPool : IObjectPool
    {
        private readonly Dictionary<PoolType, PoolContainer> _pools = new();
        public void MoveToPool(string path, Object obj, PoolType type)
        {
            var pool = GetPool(type);
        }

        public bool TryGetFromPool<T>(string path, PoolType type, out T obj) where T : Object
        {
            var pool = GetPool(type);
            return pool.TryGetObject(path, out obj);
        }

        public void TryGetFromPool(Object obj, PoolType type)
        {
            throw new System.NotImplementedException();
        }

        public void ClearPool(PoolType type)
        {
            throw new System.NotImplementedException();
        }

        private PoolContainer GetPool(PoolType type)
        {
            if (_pools.TryGetValue(type, out var container))
            {
                return container;
            }

            container = new PoolContainer(type.ToString());
            _pools.Add(type, container);
            return container;
        }
    }
}