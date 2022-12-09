using Core.Utils.Attributes;
using Core.Utils.Extensions;
using Gameplay.App;
using GameplayMechanics.Configs;
using UnityEngine;
using UnityEngine.Assertions;

namespace GameplayMechanics.Preload
{
    public class Preloader : MonoBehaviour
    {
        [SerializeField, Scene]
        private string _gameplayScene;

        [SerializeField, Scene]
        private string _uiScene;

        [SerializeField]
        private ConfigProvider _configs;

        [SerializeField]
        private AppEventProvider _eventProvider;

        private async void Awake()
        {
            await AppManager.Instance.PreloadController.InitGame(_eventProvider, _gameplayScene, _uiScene, _configs);
        }

        private void OnValidate()
        {
            Assert.IsFalse(_gameplayScene.IsNullOrEmpty(), "gameplay scene is empty");
            Assert.IsFalse(_uiScene.IsNullOrEmpty(), "ui scene is empty");
            Assert.IsFalse(_eventProvider == null, "_eventProvider is empty");
        }
    }
}