using CoreMechanics.ObjectLinks.UnityObjectLink;
using CoreMechanics.Pool;
using CoreMechanics.Scene;
using CoreMechanics.Systems;
using Cysharp.Threading.Tasks;
using Gameplay.App;
using Gameplay.Gameplay;
using Gameplay.ViewApi.Gameplay;
using GameplayMechanics.MainMechanic;
using MechanicsApi.App;
using MechanicsApi.Gameplay;
using Model.Configs;
using UIController.Manager;
using UIController.Menu;
using UnityEngine;

namespace GameplayMechanics.App
{
    public class AppController : IAppController
    {
        private readonly IConfigProvider _configProvider;
        private readonly GameObjectLink _gameplayInitPrefab;
        private readonly GameObjectLink _screenInitPrefab;
        private readonly IObjectSpawnSystem _spawnSystem;

        private IMainMechanic _mainMechanic;
        private IGameplayController _controller;

        public AppController(IAppEventProvider eventProvider, ISceneController sceneController,
            IConfigProvider configProvider)
        {
            _configProvider = configProvider;
            SceneController = sceneController;
            AppEventProvider = eventProvider;
            var pool = new ObjectPool();

            _spawnSystem =
                new ObjectSpawnSystem(
                    configProvider.PoolSettings != null ? configProvider.PoolSettings.Elements : null, pool);
        }

        public ISceneController SceneController { get; }
        public IAppEventProvider AppEventProvider { get; }

        public async UniTask StartGame()
        {
            await SceneController.LoadMainSceneAsync();
            _controller = Object.FindObjectOfType<GameplayController>();
            _controller.SetupSpawnSystem(_spawnSystem);
            await SceneController.LoadScreenSceneAsync();
            WindowManager.Instance.OpenScreen(_configProvider.UIPrefabs.MainMenuScreen,
                new MainMenuPresenter(this, _configProvider.UIPrefabs));
        }

        public async UniTask NewGame()
        {
            _mainMechanic = new GameplayMechanic(this, _controller, _configProvider);
            await _mainMechanic.StartGame();
        }
    }
}