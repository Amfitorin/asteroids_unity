using System;
using Cysharp.Threading.Tasks;
using Gameplay.ViewApi.CameraView;
using Gameplay.ViewApi.Player;
using UnityEngine;

namespace Gameplay.Player
{
    public class PlayerView : IPlayerView
    {
        private readonly ICameraView _camera;
        private readonly Transform _playerRoot;

        public PlayerView(Transform playerRoot, ICameraView camera)
        {
            _playerRoot = playerRoot;
            _camera = camera;
        }

        public event Action Died;

        public async UniTask SpawnPLayer(Vector2 spawnPosition)
        {
        }
    }
}