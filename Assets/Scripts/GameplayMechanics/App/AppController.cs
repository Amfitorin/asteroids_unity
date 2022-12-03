using CoreMechanics.ObjectLinks.UnityObjectLink;
using CoreMechanics.Scene;
using Cysharp.Threading.Tasks;
using Gameplay.App;
using Gameplay.Gameplay;
using GameplayMechanics.MainMechanic;
using UnityEngine;

namespace GameplayMechanics.App
{
    public class AppController : IAppController
    {
        private readonly GameObjectLink _gameplayInitPrefab;
        private readonly GameObjectLink _screenInitPrefab;

        private GameplayMechanic _mainMechanic;

        public AppController(IAppEventProvider eventProvider, ISceneController sceneController)
        {
            SceneController = sceneController;
           AppEventProvider = eventProvider;
        }

        public ISceneController SceneController { get; }
        public IAppEventProvider AppEventProvider { get; }

        public async UniTask StartGame()
        {
            await SceneController.LoadMainSceneAsync();
            var controller = Object.FindObjectOfType<GameplayController>();
            _mainMechanic = new GameplayMechanic(controller);
            await SceneController.LoadScreenSceneAsync();
        }
    }
}