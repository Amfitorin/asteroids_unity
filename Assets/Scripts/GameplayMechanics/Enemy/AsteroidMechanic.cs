using System.Threading;
using Cysharp.Threading.Tasks;
using Gameplay.Gameplay;
using GameplayMechanics.MainMechanic;

namespace GameplayMechanics.Enemy
{
    public class AsteroidMechanic : IGameplayMechanic
    {
        private readonly GameplayController _controller;
        private CancellationTokenSource _cancellationTokenSource;

        public AsteroidMechanic(GameplayController controller, CancellationTokenSource cancellationTokenSource)
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