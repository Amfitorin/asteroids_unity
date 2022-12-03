using System.Threading;
using Cysharp.Threading.Tasks;
using Gameplay.Gameplay;

namespace GameplayMechanics.PLayers
{
    public class PLayerMechanic : IPlayerMechanic
    {
        private readonly GameplayController _controller;
        private readonly CancellationTokenSource _cancellationTokenSource;

        public PLayerMechanic(GameplayController controller, CancellationTokenSource cancellationTokenSource)
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

        public void Attack()
        {
        }

        public void UseExtraGun()
        {
        }
    }
}