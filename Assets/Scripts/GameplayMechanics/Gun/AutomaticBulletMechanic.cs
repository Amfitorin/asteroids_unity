using System;
using System.Threading;
using Core.Utils.Extensions;
using Cysharp.Threading.Tasks;
using Gameplay.ViewApi.Gun;
using Model.Configs.Gun;
using UnityEngine;
using Random = UnityEngine.Random;

namespace GameplayMechanics.Gun
{
    public sealed class AutomaticBulletMechanic : BulletGunMechanicAbstract
    {
        private readonly AutomaticBulletGunConfig _automaticConfig;
        private readonly CancellationTokenSource _parentLifeToken;

        public AutomaticBulletMechanic(AutomaticBulletGunConfig config, IBulletView view, Func<Vector3> getSpawnPoint,
            CancellationTokenSource parentLifeToken)
            : base(config, view, getSpawnPoint)
        {
            DirectionFunc = () => Random.insideUnitCircle;
            _automaticConfig = config;
            _parentLifeToken = parentLifeToken;
            StartBullet().Forget();
        }

        protected Func<Vector3> DirectionFunc { get; set; }

        private async UniTask StartBullet()
        {
            await UniTask.WhenAny(UniTask.Delay(_automaticConfig.FireDelay.GetRandom().AsMS()),
                UniTask.WaitUntilCanceled(_parentLifeToken.Token));
            if (_parentLifeToken.IsCancellationRequested)
            {
                return;
            }

            var bullet = await View.RunBullet(Config.Bullet, GetSpawnPoint(), DirectionFunc());
            StartBulletDie(bullet).Forget();
            StartBulletDelay().Forget();
            StartBullet().Forget();
        }

        public override async UniTaskVoid LateUpdate()
        {
            await UniTask.Yield();
        }
    }
}