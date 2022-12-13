using System.Threading;
using Cysharp.Threading.Tasks;
using Gameplay.Gameplay;
using GameplayMechanics.Configs;
using GameplayMechanics.MainMechanic;

namespace GameplayMechanics.Enemy
{
    public class NloMechanic : INloMechanic
    {
        private readonly GameplayController _controller;
        private CancellationTokenSource _cancellationTokenSource;

        public NloMechanic(GameplayController controller, CancellationTokenSource cancellationTokenSource,
            ConfigProvider configProvider)
        {
            _controller = controller;
            _cancellationTokenSource = cancellationTokenSource;
        }

        public void SetupTokenSource(CancellationTokenSource tokenSource)
        {
            _cancellationTokenSource = tokenSource;
        }

        public async UniTask StartGame()
        {
            await UniTask.Yield();
        }

        public void SetupLevel(int level)
        {
            
        }
    }
}