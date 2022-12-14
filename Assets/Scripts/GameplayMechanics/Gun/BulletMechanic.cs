using System.Threading;
using Core.Utils.Extensions;
using Cysharp.Threading.Tasks;
using Gameplay.ViewApi.Gun;
using Model.Configs.Gun;
using Model.Gun;
using Unity.Plastic.Newtonsoft.Json.Serialization;
using UnityEngine;

namespace GameplayMechanics.Gun
{
    public class BulletMechanic : IBulletMechanic
    {
        private readonly BulletGunSettings _config;
        private readonly Func<Vector3> _getSpawnPoint;
        private readonly IBulletView _view;
        private readonly Func<Vector3> _directionFunc;
        private CancellationTokenSource _tokenSource;
        private bool _isCoolDown;

        public BulletMechanic(BulletGunConfig config, IBulletView view, Func<Vector3> getSpawnPoint,
            Func<Vector3> getDirection)
        {
            _view = view;
            _getSpawnPoint = getSpawnPoint;
            _config = config.Settings;
            _directionFunc = getDirection;
        }

        public void SetupTokenSource(CancellationTokenSource tokenSource)
        {
        }

        public async UniTaskVoid LateUpdate()
        {
            var fire = Input.GetAxis("Bullet");
            if (fire > 0 && !_isCoolDown)
            {
                var bullet = await _view.RunBullet(_config.Bullet, _getSpawnPoint(), _directionFunc());
                StartBulletDie(bullet).Forget();
                StartBulletDelay().Forget();
            }

            await UniTask.Yield();
        }

        private async UniTaskVoid StartBulletDelay()
        {
            _isCoolDown = true;
            await UniTask.Delay(_config.Cooldown.AsMS());
            _isCoolDown = false;
        }

        private async UniTaskVoid StartBulletDie(IBulletComponent bullet)
        {
            var awaited = await bullet.WaitDie();
            await _view.DestroyBullet(bullet);
        }
    }
}