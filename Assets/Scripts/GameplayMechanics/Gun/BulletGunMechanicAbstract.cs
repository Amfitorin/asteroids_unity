using System;
using System.Threading;
using Core.Utils.Extensions;
using Cysharp.Threading.Tasks;
using Gameplay.ViewApi.Gun;
using MechanicsApi.Gun;
using Model.Configs.Gun;
using Model.Gun;
using UnityEngine;

namespace GameplayMechanics.Gun
{
    public abstract class BulletGunMechanicAbstract : IBulletMechanic
    {
        protected readonly BulletGunSettings Config;
        protected readonly Func<Vector3> GetSpawnPoint;
        protected readonly IBulletView View;
        protected bool IsCoolDown;

        protected BulletGunMechanicAbstract(BulletGunConfig config, IBulletView view, Func<Vector3> getSpawnPoint)
        {
            View = view;
            GetSpawnPoint = getSpawnPoint;
            Config = config.Settings;
        }

        public void SetupTokenSource(CancellationTokenSource tokenSource)
        {
            View.SetupTokenSource(tokenSource);
        }

        public abstract UniTaskVoid LateUpdate();

        public async UniTask Destroy()
        {
            await View.DestroyAllBullets();
        }

        protected async UniTaskVoid StartBulletDelay()
        {
            IsCoolDown = true;
            await UniTask.Delay(Config.Cooldown.AsMS());
            IsCoolDown = false;
        }

        protected async UniTaskVoid StartBulletDie(IBulletComponent bullet)
        {
            var awaited = await bullet.WaitDie();
            await View.DestroyBullet(bullet);
        }
    }
}