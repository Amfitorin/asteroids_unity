using System;
using CoreMechanics.Managers.Configs;
using CoreMechanics.ObjectLinks.Configs;
using CoreMechanics.Pool;
using UnityEngine;

namespace Model.Configs.Pool
{
    [CreateAssetMenu(menuName = "Configs/PoolConfig")]
    public class PoolSettingsConfig : ConfigBase
    {
        [SerializeField]
        private PoolElement[] _elements;

        public PoolElement[] Elements => _elements;
    }

    [Serializable]
    public class PoolSettingsConfigLink : ConfigLink<PoolSettingsConfig>
    {
    }
}