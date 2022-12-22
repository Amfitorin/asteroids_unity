using CoreMechanics.Systems;
using Cysharp.Threading.Tasks;
using Gameplay.ViewApi.Gun;
using Gameplay.ViewApi.Player;
using Model.Configs.Player;
using UnityEngine;

namespace Gameplay.Player
{
    public class PlayerView : IPlayerView
    {
        private readonly Transform _playerRoot;
        private readonly IObjectSpawnSystem _spawnSystem;
        private PLayerComponent _player;

        public PlayerView(Transform playerRoot, IObjectSpawnSystem spawnSystem)
        {
            _playerRoot = playerRoot;
            _spawnSystem = spawnSystem;
        }

        public ILaserComponent Laser => _player.Laser;

        public async UniTask<IPlayerComponent> SpawnPLayer(PLayerConfig config, Vector3 spawnPosition)
        {
            _player = await _spawnSystem.SpawnObject<PLayerComponent>(config.Prefab, _playerRoot, spawnPosition);
            await _player.Init(config, _spawnSystem);
            return _player;
        }

        public async UniTask Despawn()
        {
            await _player.Destroy();
            await _spawnSystem.DestroyObject(_player);
        }

        public void AddRotation(float angle)
        {
            _player.transform.rotation *= Quaternion.Euler(0f, 0f, angle);
        }

        public void MoveTo(Vector3 position)
        {
            _player.transform.position = position;
        }

        public Transform ExtraGunTransform()
        {
            return _player.LaserRoot;
        }

        public void ApplySpeed(float percent)
        {
            _player.ApplySpeed(percent);
        }


        public Bounds GetBounds()
        {
            return _player.GetBounds();
        }

        public Transform BaseGunTransform()
        {
            return _player.BulletRoot;
        }
    }
}