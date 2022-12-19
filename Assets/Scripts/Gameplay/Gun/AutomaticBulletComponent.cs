using Cysharp.Threading.Tasks;
using Gameplay.ViewApi.Gun;
using UnityEngine;

namespace Gameplay.Gun
{
    public class AutomaticBulletComponent : MonoBehaviour, IAutomaticBulletComponent
    {
        public async UniTask<bool> WaitDie()
        {
            await UniTask.Yield();
            return false;
        }
    }
}