using System;
using CoreMechanics.Managers.Configs;
using CoreMechanics.ObjectLinks.Configs;
using CoreMechanics.ObjectLinks.UnityObjectLink;
using Model.Configs.Gun;
using Model.CustomTypes;
using UnityEngine;

namespace Model.Configs.Enemy
{
    [CreateAssetMenu(menuName = "Configs/Nlo")]
    public class NloConfig : ConfigBase
    {
        [SerializeField]
        private GameObjectLink _prefab;

        [SerializeField]
        private RangeFloat _duration;

        [SerializeField]
        private AutomaticBulletGunConfigLink _gun;

        [SerializeField]
        private RangeFloat _changeDirectionDuration;

        [SerializeField]
        private RangeFloat _speed;

        public RangeFloat ChangeDirectionDuration => _changeDirectionDuration;

        public GameObjectLink Prefab => _prefab;

        public RangeFloat Duration => _duration;

        public AutomaticBulletGunConfigLink Gun => _gun;
        public RangeFloat Speed => _speed;
    }

    [Serializable]
    public class NloConfigLink : ConfigLink<NloConfig>
    {
    }
}