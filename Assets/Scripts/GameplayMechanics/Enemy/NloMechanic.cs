using System.Threading;
using Cysharp.Threading.Tasks;
using Gameplay.Gameplay;
using GameplayMechanics.MainMechanic;

namespace GameplayMechanics.Enemy
{
    public class NloMechanic : IGameplayMechanic
    {
        private readonly GameplayController _controller;
        private readonly CancellationTokenSource _cancellationTokenSource;

        public NloMechanic(GameplayController controller, CancellationTokenSource cancellationTokenSource)
        {
            _controller = controller;
            _cancellationTokenSource = cancellationTokenSource;
        }

        public async UniTaskVoid Update()
        {
            
        }

        public async UniTaskVoid LateUpdate()
        {
            
        }

        public async UniTaskVoid Pause(bool state)
        {
            
        }

        public async UniTaskVoid StartGame()
        {
            
        }
    }
}