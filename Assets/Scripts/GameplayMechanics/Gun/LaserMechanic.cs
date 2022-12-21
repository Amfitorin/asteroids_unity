using System;
using System.Threading;
using Core.Utils.Extensions;
using Cysharp.Threading.Tasks;
using Gameplay.ViewApi.Gun;
using Model.Configs.Gun;
using UnityEngine;
using Timer = Core.Time.Timer;

namespace GameplayMechanics.Gun
{
    public class LaserMechanic : ILaserMechanic
    {
        private readonly LaserGunConfig _config;
        private readonly Progress<float> _progress;
        private readonly Timer _regenTimer;
        private readonly ILaserView _view;
        private int _charges;
        private bool _isCoolDown;
        private Transform _parent;

        public LaserMechanic(LaserGunConfig config, ILaserView view, CancellationTokenSource globalToken)
        {
            _config = config;
            _view = view;
            _regenTimer = new Timer(config.Settings.Cooldown, globalToken);
            _progress = new Progress<float>(progress => CooldownProgress?.Invoke(progress));
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
        }

        public bool IsActive { get; private set; }

        public void Die()
        {
            _view.DieLaser();
            IsActive = false;
        }

        public event Action<float> CooldownProgress;

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

            IsActive = true;
            await UniTask.Delay(_config.Settings.Time.AsMS());
            Die();
        }

        private async UniTaskVoid StartTimer()
        {
            while (_charges < _config.Settings.MaxCount)
            {
                await _regenTimer.ToUniTask(progress: _progress);
                _charges++;
                _regenTimer.Reset();
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