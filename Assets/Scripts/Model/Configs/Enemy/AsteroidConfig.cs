using System;
using CoreMechanics.Managers.Configs;
using CoreMechanics.ObjectLinks.Configs;
using CoreMechanics.ObjectLinks.UnityObjectLink;
using Model.CustomTypes;
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

        [SerializeField]
        private SpriteLink[] _sprites;

        [SerializeField]
        private RangeFloat _angleSpeed;

        [SerializeField]
        private AsteroidType _type;

        [SerializeField]
        private int _score;

        public RangeFloat AngleSpeed => _angleSpeed;

        public AsteroidConfigLink Shard => _shard;

        public GameObjectLink Prefab => _prefab;

        public float Speed => _speed;

        public int ShardCount => _shardCount;

        public SpriteLink[] Sprites => _sprites;

        public AsteroidType Type => _type;
        public int Score => _score;
    }

    [Serializable]
    public class AsteroidConfigLink : ConfigLink<AsteroidConfig>
    {
    }
}