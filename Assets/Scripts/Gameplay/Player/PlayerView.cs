using System;
using CoreMechanics.Systems;
using Cysharp.Threading.Tasks;
using Gameplay.ViewApi.CameraView;
using Gameplay.ViewApi.Player;
using Model.Configs.Player;
using UnityEngine;

namespace Gameplay.Player
{
    public class PlayerView : IPlayerView
    {
        private readonly ICameraView _camera;
        private readonly Transform _playerRoot;
        private readonly IObjectSpawnSystem _spawnSystem;
        private PLayerComponent _player;

        public PlayerView(Transform playerRoot, ICameraView camera, IObjectSpawnSystem spawnSystem)
        {
            _playerRoot = playerRoot;
            _camera = camera;
            _spawnSystem = spawnSystem;
        }

        public event Action Died;

        public async UniTask<Transform> SpawnPLayer(PLayerConfig config, Vector3 spawnPosition)
        {
            _player = await _spawnSystem.SpawnObject<PLayerComponent>(config.Prefab, _playerRoot, spawnPosition);
            await _player.Init(config, _spawnSystem);
            return _player.transform;
        }

        public async UniTask Despawn()
        {
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

        public void ApplySpeed(float percent)
        {
            _player.ApplySpeed(percent);
        }


        public Bounds GetBounds()
        {
            return _player.GetBounds();
        }

        public Vector3 GetBaseGunPoint()
        {
            return _player.BulletRoot.position;
        }

        public Vector3 GetExtraGunPoint()
        {
            return _player.LaserRoot.position;
        }
    }
}