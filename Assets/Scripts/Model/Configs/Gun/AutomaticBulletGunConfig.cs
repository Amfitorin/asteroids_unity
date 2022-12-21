using System;
using CoreMechanics.ObjectLinks.Configs;
using Model.CustomTypes;
using UnityEngine;

namespace Model.Configs.Gun
{
    [CreateAssetMenu(menuName = "Configs/AutomaticBulletGun")]
    public class AutomaticBulletGunConfig : BulletGunConfig
    {
        [SerializeField]
        private RangeFloat _fireDelay;

        public RangeFloat FireDelay => _fireDelay;
    }

    [Serializable]
    public class AutomaticBulletGunConfigLink : ConfigLink<AutomaticBulletGunConfig>
    {
    }
}