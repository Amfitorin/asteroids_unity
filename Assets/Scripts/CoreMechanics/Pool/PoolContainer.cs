using System.Collections.Generic;
using UnityEngine;

namespace CoreMechanics.Pool
{
    public class PoolContainer : IPoolContainer
    {
        private readonly Transform _poolRoot;

        public PoolContainer(string poolName)
        {
            var pool = new GameObject($"{poolName}Pool");
            var instance = Object.Instantiate(pool, Vector3.one * 20000, Quaternion.identity);
            _poolRoot = instance.transform;
        }

        private Dictionary<string, Stack<GameObject>> PoolObjects { get; } = new();

        public void StoreObject(string path, GameObject obj)
        {
            if (!PoolObjects.TryGetValue(path, out var elements))
            {
                elements = new Stack<GameObject>();
                PoolObjects.Add(path, elements);
            }

            elements.Push(obj);
            obj.transform.SetParent(_poolRoot);
        }

        public bool TryGetObject<T>(string path, out T obj) where T : Object
        {
            obj = null;
            if (!PoolObjects.TryGetValue(path, out var elements)) return false;

            if (!elements.TryPop(out var element)) return false;

            if (typeof(Component).IsAssignableFrom(typeof(T)))
                obj = element.GetComponentInChildren<T>();
            else
                obj = element as T;
            return true;
        }
    }
}