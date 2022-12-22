using System;
using CoreMechanics.Managers.Configs;
using CoreMechanics.ObjectLinks.Configs;
using CoreMechanics.ObjectLinks.UnityObjectLink;
using UnityEngine;

namespace Model.Configs.UI
{
    [CreateAssetMenu(menuName = "Configs/UI Prefabs")]
    public class UIPrefabsConfig : ConfigBase
    {
        [SerializeField]
        private GameObjectLink _mainMenuScreen;
        
        [SerializeField]
        private GameObjectLink _gameScreen;
        
        [SerializeField]
        private GameObjectLink _gameOverWindow;

        public GameObjectLink MainMenuScreen => _mainMenuScreen;

        public GameObjectLink GameScreen => _gameScreen;

        public GameObjectLink GameOverWindow => _gameOverWindow;
    }
    
    [Serializable]
    public class UIPrefabsConfigLink : ConfigLink<UIPrefabsConfig>
    {}
}