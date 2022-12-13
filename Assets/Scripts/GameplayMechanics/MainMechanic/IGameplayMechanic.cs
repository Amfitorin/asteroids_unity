using CoreMechanics.Systems;
using Cysharp.Threading.Tasks;

namespace GameplayMechanics.MainMechanic
{
    public interface IGameplayMechanic : ITokenCancelSource
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

        void Release()
        {
        }

        void SetupLevel(int level)
        {
        }
    }
}