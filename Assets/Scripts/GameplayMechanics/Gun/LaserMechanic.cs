using System.Threading;
using Cysharp.Threading.Tasks;
using Gameplay.ViewApi.Gun;
using UnityEngine;

namespace GameplayMechanics.Gun
{
    public class LaserMechanic : ILaserMechanic
    {
        private ILaserView _view;
        private bool _isCoolDown;
        private bool _hasCharge;

        public async UniTaskVoid LateUpdate()
        {
            var fire = Input.GetButtonDown("Bullet");
            if (fire && !_isCoolDown && _hasCharge)
            {
                // var bullet = await _view.RunBullet(_config.Bullet, _getSpawnPoint(), _directionFunc());
                // StartBulletDie(bullet).Forget();
                StartBulletDelay().Forget();
            }

            await UniTask.Yield();
        }

        private async UniTaskVoid StartBulletDelay()
        {
            _isCoolDown = true;
            // await UniTask.Delay(_config.Cooldown.AsMS());
            _isCoolDown = false;
        }

        private async UniTaskVoid StartBulletDie(IBulletComponent bullet)
        {
            var awaited = await bullet.WaitDie();
            // await _view.DestroyBullet(bullet);
        }
        
        public void SetupTokenSource(CancellationTokenSource tokenSource)
        {
            
        }
    }
}