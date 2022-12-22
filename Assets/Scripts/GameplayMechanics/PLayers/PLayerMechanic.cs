using System;
using System.Threading;
using Core.Utils.Extensions;
using Cysharp.Threading.Tasks;
using Gameplay.ViewApi.CameraView;
using Gameplay.ViewApi.Gameplay;
using Gameplay.ViewApi.Player;
using GameplayMechanics.Gun;
using MechanicsApi.Gun;
using MechanicsApi.Player;
using Model.Configs;
using Model.Configs.Player;
using UnityEngine;

namespace GameplayMechanics.PLayers
{
    public class PLayerMechanic : IPlayerMechanic
    {
        private readonly IGunMechanic _baseGunMechanic;
        private readonly ICameraView _cameraView;
        private readonly PLayerConfig _playerConfig;
        private readonly IPlayerView _playerView;
        private float _currentMaxSpeed;
        private Vector3 _direction = Vector3.zero;
        private ILaserMechanic _extraGunMechanic;
        private IPlayerComponent _player;
        private Vector3 _slowDirection;
        private float _slowTime;

        private float _speed;

        public PLayerMechanic(IGameplayController controller, IConfigProvider provider,
            CancellationTokenSource tokenSource)
        {
            _playerView = controller.PlayerView;
            _playerConfig = provider.PLayerConfig;
            _cameraView = controller.Camera;
            _baseGunMechanic = new BulletMechanic(provider.PLayerConfig.BulletGun, controller.BulletView, BaseGunPoint,
                () => _player.Transform.up.normalized);
            _extraGunMechanic = new LaserMechanic(provider.PLayerConfig.LaserGun, controller.LaserView, tokenSource);
        }

        public async UniTask StartGame()
        {
            _player = await _playerView.SpawnPLayer(_playerConfig, _cameraView.ScreenCenter);
            _player.OnDied += OnOnDied;

            _extraGunMechanic.Init(_playerView.Laser, _playerView.ExtraGunTransform());
            _playerView.ApplySpeed(0f);
        }

        public async UniTask Destroy()
        {
            await UniTask.Yield();
        }

        public ILaserMechanic Laser => _extraGunMechanic;
        public event Action Died;
        public Transform PlayerTransform => _player.Transform;
        public float CurrentSpeed => _speed;

        public void SetupTokenSource(CancellationTokenSource tokenSource)
        {
            _baseGunMechanic.SetupTokenSource(tokenSource);
            _extraGunMechanic.SetupTokenSource(tokenSource);
        }

        public async UniTaskVoid LateUpdate()
        {
            var rotation = Input.GetAxis("Rotate");
            if (!Mathf.Approximately(rotation, 0))
            {
                var angle = rotation * _playerConfig.RotationSpeed;
                _playerView.AddRotation(angle);
            }

            Vector3 position = _player.Transform.position;
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
                var forward = _player.Transform.up.normalized;
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
                _slowTime += Time.deltaTime;
                var slowProgress = _slowTime / _playerConfig.SlowTime;
                _speed = Mathf.Lerp(_currentMaxSpeed, 0f, slowProgress);
                _direction = Vector3.Lerp(_direction, Vector3.zero, slowProgress);
                position += _slowDirection * _speed * Time.deltaTime;
                posChanged = true;
            }

            if (posChanged)
            {
                _playerView.ApplySpeed(_speed / _playerConfig.MaxSpeed);
                _playerView.MoveTo(position);
            }

            _baseGunMechanic.LateUpdate().Forget();
            _extraGunMechanic.LateUpdate().Forget();

            await UniTask.Yield();
        }

        private void OnOnDied()
        {
            StartPlayerDestroy().Forget();
        }

        private async UniTaskVoid StartPlayerDestroy()
        {
            await _playerView.Despawn();
            Died?.Invoke();
        }

        private Vector3 BaseGunPoint()
        {
            return _playerView.BaseGunTransform().position;
        }
    }
}