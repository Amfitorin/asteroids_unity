using System;
using System.Threading;
using Core.Utils.Extensions;
using Cysharp.Threading.Tasks;
using Gameplay.ViewApi.Gun;
using MechanicsApi.Gun;
using Model.Configs.Gun;
using UnityEngine;
using Timer = Core.Time.Timer;

namespace GameplayMechanics.Gun
{
    public class LaserMechanic : ILaserMechanic
    {
        private readonly LaserGunConfig _config;
        private readonly Progress<float> _cooldownProgress;
        private readonly Timer _regenTimer;
        private readonly Progress<float> _useProgress;
        private readonly Timer _useTimer;
        private readonly ILaserView _view;
        private int _charges;
        private CancellationTokenSource _globalToken;
        private bool _isCoolDown;
        private Transform _parent;

        public LaserMechanic(LaserGunConfig config, ILaserView view, CancellationTokenSource globalToken)
        {
            _config = config;
            _view = view;
            _globalToken = globalToken;
            _regenTimer = new Timer(config.Settings.Cooldown, globalToken);
            _useTimer = new Timer(config.Settings.Time, globalToken);
            _cooldownProgress = new Progress<float>(progress =>
                CooldownProgress?.Invoke(_regenTimer.CurrentTime, _regenTimer.TotalSeconds));
            _useProgress = new Progress<float>(progress =>
                UseProgress?.Invoke(_useTimer.TotalSeconds - _useTimer.CurrentTime));
            StartTimer().Forget();
        }


        private bool HasCharge => _charges > 0;

        public async UniTaskVoid LateUpdate()
        {
            var fire = Input.GetButtonDown("Laser");
            if (fire && !_isCoolDown && HasCharge)
            {
                RunLaser().Forget();
                StartDelay().Forget();
            }

            if (IsActive)
            {
                _view.SetDirection(_parent.position, _parent.up.normalized);
            }

            await UniTask.Yield();
        }

        public void SetupTokenSource(CancellationTokenSource tokenSource)
        {
            _globalToken = tokenSource;
        }

        public bool IsActive { get; private set; }

        public void Die()
        {
            _view.DieLaser();
            IsActive = false;
        }

        public event Action<float> UseProgress;
        public event Action LaserCancelled;
        public event Action<int, bool> ChargesChanged;
        public event Action<float, float> CooldownProgress;
        public int ChargesCount => _charges;

        public void Init(ILaserComponent laser, Transform parent)
        {
            _view.Init(laser);
            _parent = parent;
        }

        private async UniTaskVoid RunLaser()
        {
            _view.RunLaser();
            _charges--;
            if (!_regenTimer.IsRunned)
            {
                StartTimer().Forget();
            }

            ChargesChanged?.Invoke(_charges, false);

            IsActive = true;
            await _useTimer.ToUniTask(progress: _useProgress, cancellationToken: _globalToken.Token);
            LaserCancelled?.Invoke();
            _useTimer.Reset();
            Die();
        }

        private async UniTaskVoid StartTimer()
        {
            while (_charges < _config.Settings.MaxCount)
            {
                await _regenTimer.ToUniTask(progress: _cooldownProgress, cancellationToken: _globalToken.Token);
                _charges++;
                _regenTimer.Reset();
                ChargesChanged?.Invoke(_charges, _charges == _config.Settings.MaxCount);
            }
        }

        private async UniTaskVoid StartDelay()
        {
            _isCoolDown = true;
            await UniTask.Delay(_config.Settings.UseDelay.AsMS());
            _isCoolDown = false;
        }
    }
}