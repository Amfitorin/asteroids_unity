using System;
using CoreMechanics.Managers.Configs;
using CoreMechanics.ObjectLinks.Configs;
using Model.Gun;
using UnityEngine;

namespace Model.Configs.Gun
{
    [CreateAssetMenu(menuName = "Configs/BulletGun")]
    public class BulletGunConfig : ConfigBase
    {
        [SerializeField]
        private BulletGunSettings _settings;

        public BulletGunSettings Settings => _settings;
    }

    [Serializable]
    public class BulletGunConfigLink : ConfigLink<BulletGunConfig>
    {
    }
}