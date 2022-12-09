using System.Threading;
using Cysharp.Threading.Tasks;

namespace GameplayMechanics.MainMechanic
{
    public interface IGameplayMechanic
    {
        async UniTaskVoid Update()
        {
            await UniTask.Yield();
        }

        async UniTaskVoid LateUpdate()
        {
            await UniTask.Yield();
        }

        async UniTaskVoid Pause(bool state)
        {
            await UniTask.Yield();
        }

        async UniTask StartGame()
        {
            await UniTask.Yield();
        }

        async UniTaskVoid SetupLevel(int level)
        {
            await UniTask.Yield();
        }

        void SetupTokenSource(CancellationTokenSource tokenSource);
    }
}