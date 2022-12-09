using System;
using CoreMechanics.Managers.Configs;
using CoreMechanics.ObjectLinks.Configs;
using Model.Gun;
using UnityEngine;

namespace Model.Configs.Gun
{
    [CreateAssetMenu(menuName = "Configs/LaserGun")]
    public class LaserGunConfig : ConfigBase
    {
        [SerializeField]
        private LaserGunSettings _settings;

        public LaserGunSettings Settings => _settings;
    }

    [Serializable]
    public class LaserGunConfigLink : ConfigLink<LaserGunConfig>
    {
    }
}