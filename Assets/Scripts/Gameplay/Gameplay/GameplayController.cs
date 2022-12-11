using CoreMechanics.Systems;
using Gameplay.Player;
using Gameplay.ViewApi.CameraView;
using Gameplay.ViewApi.Gameplay;
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

        public ICameraView Camera { get; private set; }

        public void SetupSpawnSystem(IObjectSpawnSystem objectSpawnSystem)
        {
            _spawnSystem = objectSpawnSystem;
        }

        public void StartGame(IConfigProvider configProvider)
        {
            Camera = new CameraView.CameraView(_camera);
            PlayerView = new PlayerView(_playerRoot, Camera, _spawnSystem);
        }
    }
}