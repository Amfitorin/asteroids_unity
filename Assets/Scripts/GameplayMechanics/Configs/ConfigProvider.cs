using System;
using Model.Configs;
using Model.Configs.Level;
using Model.Configs.Player;
using Model.Configs.Pool;
using Model.Configs.UI;
using UnityEngine;

namespace GameplayMechanics.Configs
{
    [Serializable]
    public class ConfigProvider : IConfigProvider
    {
        [SerializeField]
        private LevelsConfigLink _levels;

        [SerializeField]
        private PlayerConfigLink _player;

        [SerializeField]
        private PoolSettingsConfigLink _poolSettings;
        
        [SerializeField]
        private UIPrefabsConfigLink _uiPrefabs;

        public UIPrefabsConfig UIPrefabs => _uiPrefabs;

        public PLayerConfig PLayerConfig => _player;
        public LevelsConfig LevelsConfig => _levels;
        public PoolSettingsConfig PoolSettings => _poolSettings;
    }
}