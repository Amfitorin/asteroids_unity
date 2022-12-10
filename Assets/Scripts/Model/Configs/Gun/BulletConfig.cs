using System;
using CoreMechanics.Managers.Configs;
using CoreMechanics.ObjectLinks.Configs;
using CoreMechanics.ObjectLinks.UnityObjectLink;
using UnityEngine;

namespace Model.Configs.Gun
{
    [CreateAssetMenu(menuName = "Configs/Bullet")]
    public class BulletConfig : ConfigBase
    {
        [SerializeField]
        private GameObjectLink _prefab;

        [SerializeField]
        private float _speed;

        public GameObjectLink Prefab => _prefab;

        public float Speed => _speed;
    }

    [Serializable]
    public class BulletConfigLink : ConfigLink<BulletConfig>
    {
    }
}