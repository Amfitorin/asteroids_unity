using CoreMechanics.Systems;
using Cysharp.Threading.Tasks;
using Model.Configs.Gun;
using UnityEngine;

namespace Gameplay.ViewApi.Gun
{
    public interface IBulletView : ITokenCancelSource
    {
        UniTask<IBulletComponent> RunBullet(BulletConfig config, Vector3 position,
            Vector3 direction);

        UniTask DestroyBullet(IBulletComponent bullet);
    }
}