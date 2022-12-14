using Cysharp.Threading.Tasks;

namespace Gameplay.ViewApi.Gun
{
    public interface IBulletComponent
    {
        UniTask<bool> WaitDie();
    }
}