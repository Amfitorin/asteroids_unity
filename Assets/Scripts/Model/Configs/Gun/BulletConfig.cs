using System;
using Core.Utils.Extensions;
using CoreMechanics.Managers.Configs;
using CoreMechanics.ObjectLinks.Configs;
using CoreMechanics.ObjectLinks.UnityObjectLink;
using UnityEngine;
using UnityEngine.Serialization;

namespace Model.Configs.Gun
{
    [CreateAssetMenu(menuName = "Configs/Bullet")]
    public class BulletConfig : ConfigBase
    {
        [SerializeField]
        private GameObjectLink _prefab;

        [SerializeField]
        private float _speed;

        [FormerlySerializedAs("_lifeTimeMS")]
        [SerializeField]
        private float _lifeTime;

        public int LifeTimeMS => _lifeTime.AsMS();

        public GameObjectLink Prefab => _prefab;

        public float Speed => _speed;
    }

    [Serializable]
    public class BulletConfigLink : ConfigLink<BulletConfig>
    {
    }
}