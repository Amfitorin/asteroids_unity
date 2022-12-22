using System.Threading.Tasks;
using CoreMechanics.Systems;
using Cysharp.Threading.Tasks;

namespace MechanicsApi.Gameplay
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

        UniTask Destroy();
    }
}