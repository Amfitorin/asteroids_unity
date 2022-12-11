using System;
using CoreMechanics.ObjectLinks.UnityObjectLink;
using CoreMechanics.Systems;
using Cysharp.Threading.Tasks;
using Gameplay.ViewApi.CameraView;
using Gameplay.ViewApi.Player;
using UnityEngine;

namespace Gameplay.Player
{
    public class PlayerView : IPlayerView
    {
        private readonly ICameraView _camera;
        private readonly IObjectSpawnSystem _spawnSystem;
        private readonly Transform _playerRoot;
        private PLayerComponent _player;

        public PlayerView(Transform playerRoot, ICameraView camera, IObjectSpawnSystem spawnSystem)
        {
            _playerRoot = playerRoot;
            _camera = camera;
            _spawnSystem = spawnSystem;
        }

        public event Action Died;

        public async UniTask<Transform> SpawnPLayer(GameObjectLink prefab, Vector3 spawnPosition)
        {
            _player = await _spawnSystem.SpawnObject<PLayerComponent>(prefab, _playerRoot, spawnPosition);
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

        public Bounds GetBounds()
        {
            return _player.GetBounds();
        }
    }
}