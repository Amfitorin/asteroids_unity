using System;
using Cysharp.Threading.Tasks;
using Gameplay.ViewApi.Gun;
using Model.Configs.Gun;
using UnityEngine;

namespace GameplayMechanics.Gun
{
    public sealed class BulletMechanic : BulletGunMechanicAbstract
    {
        public BulletMechanic(BulletGunConfig config, IBulletView view, Func<Vector3> getSpawnPoint,
            Func<Vector3> getDirection) : base(config, view, getSpawnPoint)
        {
            DirectionFunc = getDirection;
        }

        protected override Func<Vector3> DirectionFunc { get; set; }

        public override async UniTaskVoid LateUpdate()
        {
            var fire = Input.GetButtonDown("Bullet");
            if (fire && !_isCoolDown)
            {
                var bullet = await _view.RunBullet(_config.Bullet, _getSpawnPoint(), DirectionFunc());
                StartBulletDie(bullet).Forget();
                StartBulletDelay().Forget();
            }

            await UniTask.Yield();
        }
    }
}