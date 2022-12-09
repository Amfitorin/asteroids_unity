using System;
using Model.Configs;
using Model.Configs.Level;
using Model.Configs.Player;
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
        
        public PLayerConfig PLayerConfig  => _player;
        public LevelsConfig LevelsConfig => _levels;
    }
}