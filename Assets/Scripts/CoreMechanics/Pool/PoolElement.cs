using System;
using CoreMechanics.ObjectLinks.UnityObjectLink;
using UnityEngine;

namespace CoreMechanics.Pool
{
    [Serializable]
    public class PoolElement
    {
        [SerializeField]
        private PoolType _poolType;

        [SerializeField]
        private GameObjectLink _prefab;

        public PoolType PoolType => _poolType;

        public GameObjectLink Prefab => _prefab;
    }
}