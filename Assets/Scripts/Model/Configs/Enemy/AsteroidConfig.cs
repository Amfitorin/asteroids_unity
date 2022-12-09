using System;
using CoreMechanics.Managers.Configs;
using CoreMechanics.ObjectLinks.Configs;
using CoreMechanics.ObjectLinks.UnityObjectLink;
using Model.EnvObject;
using UnityEngine;

namespace Model.Configs.Enemy
{
    [CreateAssetMenu(menuName = "Configs/Asteroid")]
    public class AsteroidConfig : ConfigBase
    {
        [SerializeField]
        private GameObjectLink _prefab;

        [SerializeField]
        private float _speed;

        [SerializeField]
        private int _shardCount;

        [SerializeField]
        private AsteroidConfigLink _shard;

        public AsteroidConfigLink Shard => _shard;

        public GameObjectLink Prefab => _prefab;

        public float Speed => _speed;

        public int ShardCount => _shardCount;
    }

    [Serializable]
    public class AsteroidConfigLink : ConfigLink<AsteroidConfig>
    {}
}