using System;
using Model.Configs.Gun;
using UnityEngine;

namespace Model.Gun
{
    [Serializable]
    public class BulletGunSettings : GunSettings
    {
        [SerializeField]
        private BulletConfigLink _bullet;

        public BulletConfigLink Bullet => _bullet;
    }
}