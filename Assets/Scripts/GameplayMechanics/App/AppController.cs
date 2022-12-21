using CoreMechanics.ObjectLinks.UnityObjectLink;
using CoreMechanics.Pool;
using CoreMechanics.Scene;
using CoreMechanics.Systems;
using Cysharp.Threading.Tasks;
using Gameplay.App;
using Gameplay.Gameplay;
using GameplayMechanics.MainMechanic;
using Model.Configs;
using UnityEngine;

namespace GameplayMechanics.App
{
    public class AppController : IAppController
    {
        private readonly IConfigProvider _configProvider;
        private readonly GameObjectLink _gameplayInitPrefab;
        private readonly GameObjectLink _screenInitPrefab;
        private readonly IObjectSpawnSystem _spawnSystem;

        private IGameplayMechanic _mainMechanic;

        public AppController(IAppEventProvider eventProvider, ISceneController sceneController,
            IConfigProvider configProvider)
        {
            _configProvider = configProvider;
            SceneController = sceneController;
            AppEventProvider = eventProvider;
            var pool = new ObjectPool();
            if (Application.isEditor)
            {
                _spawnSystem = new ObjectSpawnSystem(configProvider.PoolSettings != null ? configProvider.PoolSettings.Elements : null, pool);
            }
        }

        public ISceneController SceneController { get; }
        public IAppEventProvider AppEventProvider { get; }

        public async UniTask StartGame()
        {
            await SceneController.LoadMainSceneAsync();
            var controller = Object.FindObjectOfType<GameplayController>();
            controller.SetupSpawnSystem(_spawnSystem);
            _mainMechanic = new GameplayMechanic(this, controller, _configProvider);
            // await SceneController.LoadScreenSceneAsync();
        }
    }
}