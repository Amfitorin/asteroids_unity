using System;
using System.Threading;
using Core.Utils.Extensions;
using Cysharp.Threading.Tasks;
using Gameplay.ViewApi.Gun;
using Model.Configs.Gun;
using Model.Gun;
using UnityEngine;

namespace GameplayMechanics.Gun
{
    public abstract class BulletGunMechanicAbstract : IBulletMechanic
    {
        protected readonly BulletGunSettings _config;
        protected abstract Func<Vector3> DirectionFunc { get; set; }
        protected readonly Func<Vector3> _getSpawnPoint;
        protected readonly IBulletView _view;
        protected bool _isCoolDown;

        protected BulletGunMechanicAbstract(BulletGunConfig config, IBulletView view, Func<Vector3> getSpawnPoint)
        {
            _view = view;
            _getSpawnPoint = getSpawnPoint;
            _config = config.Settings;
        }

        public void SetupTokenSource(CancellationTokenSource tokenSource)
        {
            _view.SetupTokenSource(tokenSource);
        }

        public abstract UniTaskVoid LateUpdate();

        protected async UniTaskVoid StartBulletDelay()
        {
            _isCoolDown = true;
            await UniTask.Delay(_config.Cooldown.AsMS());
            _isCoolDown = false;
        }

        protected async UniTaskVoid StartBulletDie(IBulletComponent bullet)
        {
            var awaited = await bullet.WaitDie();
            await _view.DestroyBullet(bullet);
        }
    }
}