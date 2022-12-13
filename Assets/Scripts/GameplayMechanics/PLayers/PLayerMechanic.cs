using System;
using System.Threading;
using Core.Utils.Extensions;
using Cysharp.Threading.Tasks;
using Gameplay.Gameplay;
using Gameplay.ViewApi.CameraView;
using Gameplay.ViewApi.Player;
using GameplayMechanics.Configs;
using Model.Configs.Player;
using UnityEngine;

namespace GameplayMechanics.PLayers
{
    public class PLayerMechanic : IPlayerMechanic
    {
        private readonly ICameraView _cameraView;
        private readonly PLayerConfig _playerConfig;
        private readonly IPlayerView _playerView;
        private Transform _playerTransform;

        private float _speed;
        private Vector3 _direction = Vector3.zero;
        private float _currentMaxSpeed;
        private float _slowTime;
        private Vector3 _slowDirection;

        public PLayerMechanic(GameplayController controller, ConfigProvider provider)
        {
            _playerView = controller.PlayerView;
            _playerConfig = provider.PLayerConfig;
            _cameraView = controller.Camera;
        }

        public async UniTask StartGame()
        {
            _playerTransform = await _playerView.SpawnPLayer(_playerConfig.Prefab, _cameraView.ScreenCenter);
        }

        public event Action Died;

        public void Attack()
        {
        }

        public void UseExtraGun()
        {
        }

        public void SetupTokenSource(CancellationTokenSource tokenSource)
        {
        }

        public async UniTaskVoid LateUpdate()
        {
            var rotation = Input.GetAxis("Rotate");
            if (!Mathf.Approximately(rotation, 0))
            {
                var angle = rotation * _playerConfig.RotationSpeed;
                _playerView.AddRotation(angle);
            }

            Vector3 position = _playerTransform.position;
            CornerRect cornerRect = _playerView.GetBounds();
            var posChanged = false;
            if (!_cameraView.IsObjectVisible(cornerRect))
            {
                position = _cameraView.InversePosition(position, cornerRect);
                posChanged = true;
            }

            var drive = Input.GetAxis("Drive");
            if (drive > 0.1f)
            {
                _slowTime = 0f;
                var forward = _playerTransform.up;
                _speed = Mathf.Clamp(_speed + _playerConfig.Acceleration * Time.deltaTime, 0f,
                    _playerConfig.MaxSpeed);
                if (_direction == Vector3.zero)
                {
                    _direction = forward;
                }
                else
                {
                    _direction = Vector3.Lerp(_direction, forward, Time.deltaTime);
                }

                _currentMaxSpeed = _speed;
                _slowDirection = _direction;

                position += _direction * _speed * Time.deltaTime;
                posChanged = true;
            }
            else if (_speed > 0)
            {
                _direction = Vector3.zero;
                _slowTime += Time.deltaTime;
                _speed = Mathf.Lerp(_currentMaxSpeed, 0f, _slowTime / _playerConfig.SlowTime);
                position += _slowDirection * _speed * Time.deltaTime;
                posChanged = true;
            }

            if (posChanged)
            {
                _playerView.MoveTo(position);
            }

            await UniTask.Yield();
        }
    }
}