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

        public GameObjectLink MainMenuScreen => _mainMenuScreen;

        public GameObjectLink GameScreen => _gameScreen;
    }
    
    [Serializable]
    public class UIPrefabsConfigLink : ConfigLink<UIPrefabsConfig>
    {}
}