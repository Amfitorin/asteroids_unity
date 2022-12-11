using System;
using CoreMechanics.Managers.Configs;
using CoreMechanics.ObjectLinks.Configs;
using CoreMechanics.ObjectLinks.UnityObjectLink;
using Model.Configs.Gun;
using UnityEngine;

namespace Model.Configs.Player
{
    [CreateAssetMenu(menuName = "Configs/Player")]
    public class PLayerConfig : ConfigBase
    {
        [SerializeField]
        private GameObjectLink _prefab;

        [SerializeField]
        private float _maxSpeed;

        [SerializeField]
        private float _acceleration;

        [SerializeField]
        private float _slowTime;

        [SerializeField]
        private BulletGunConfigLink _bulletGun;

        [SerializeField]
        private LaserGunConfigLink _laserGun;

        [SerializeField]
        private float _rotationSpeed;

        public float RotationSpeed => _rotationSpeed;

        public GameObjectLink Prefab => _prefab;

        public float MaxSpeed => _maxSpeed;

        public float Acceleration => _acceleration;

        public float SlowTime => _slowTime;

        public BulletGunConfigLink BulletGun => _bulletGun;

        public LaserGunConfigLink LaserGun => _laserGun;
    }

    [Serializable]
    public class PlayerConfigLink : ConfigLink<PLayerConfig>
    {}
}