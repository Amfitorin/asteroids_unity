using System.Threading;
using CoreMechanics.Systems;
using Gameplay.Enemy;
using Gameplay.Gun;
using Gameplay.Player;
using Gameplay.ViewApi.CameraView;
using Gameplay.ViewApi.Enemy;
using Gameplay.ViewApi.Gameplay;
using Gameplay.ViewApi.Gun;
using Gameplay.ViewApi.Player;
using Model.Configs;
using Model.Level;
using UnityEngine;

namespace Gameplay.Gameplay
{
    public class GameplayController : MonoBehaviour, IGameplayController
    {
        [SerializeField]
        private Transform _enemyRoot;

        [SerializeField]
        private Transform _playerRoot;

        [SerializeField]
        private Camera _camera;

        private LevelSettings _currentLevel;
        private IObjectSpawnSystem _spawnSystem;

        public IPlayerView PlayerView { get; private set; }

        public INloView NloView { get; private set; }

        public IBulletView BulletView { get; private set; }

        public IAsteroidsView AsteroidsView { get; private set; }

        public ICameraView Camera { get; private set; }
        public ILaserView LaserView { get; private set; }

        public void SetupSpawnSystem(IObjectSpawnSystem objectSpawnSystem)
        {
            _spawnSystem = objectSpawnSystem;
        }

        public void StartGame(IConfigProvider configProvider, CancellationTokenSource tokenSource)
        {
            Camera = new CameraView.CameraView(_camera);
            PlayerView = new PlayerView(_playerRoot, Camera, _spawnSystem);
            BulletView = new BulletView(_spawnSystem, Camera, tokenSource, _enemyRoot);
            AsteroidsView = new AsteroidsView(_enemyRoot, Camera, _spawnSystem, tokenSource);
            LaserView = new LaserView(Camera);
            NloView = new NloView(_spawnSystem, Camera, tokenSource, _enemyRoot);
        }
    }
}