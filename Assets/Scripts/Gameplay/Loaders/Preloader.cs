using System;
using Core.Utils.Attributes;
using Core.Utils.Extensions;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.SceneManagement;

namespace Gameplay.Loaders
{
    public class Preloader : MonoBehaviour
    {
        [SerializeField, Scene]
        private string _gameplayScene;

        [SerializeField, Scene]
        private string _uiScene;

        private async void Awake()
        {
            await AppManager.Instance.AppController.SceneController.LoadSceneAsync(_gameplayScene, LoadSceneMode.Additive);
            await AppManager.Instance.AppController.SceneController.LoadSceneAsync(_uiScene, LoadSceneMode.Additive);
        }

        private void OnValidate()
        {
            Assert.IsFalse(_gameplayScene.IsNullOrEmpty(), "gameplay scene is empty");
            Assert.IsFalse(_uiScene.IsNullOrEmpty(), "ui scene is empty");
        }
    }
}