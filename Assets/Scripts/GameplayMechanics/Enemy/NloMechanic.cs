using System.Threading;
using Gameplay.Gameplay;
using GameplayMechanics.MainMechanic;

namespace GameplayMechanics.Enemy
{
    public class NloMechanic : IGameplayMechanic
    {
        private readonly GameplayController _controller;
        private CancellationTokenSource _cancellationTokenSource;

        public NloMechanic(GameplayController controller, CancellationTokenSource cancellationTokenSource)
        {
            _controller = controller;
            _cancellationTokenSource = cancellationTokenSource;
        }

        public void SetupTokenSource(CancellationTokenSource tokenSource)
        {
            _cancellationTokenSource = tokenSource;
        }
    }
}