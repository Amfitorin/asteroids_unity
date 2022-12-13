using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using Gameplay.Gameplay;
using GameplayMechanics.Configs;

namespace GameplayMechanics.Enemy
{
    public class EnemyMechanic : IEnemyMechanic
    {
        private readonly ConfigProvider _provider;
        private readonly IAsteroidMechanic _asteroidsMechanic;
        private readonly INloMechanic _nloMechanic;

        public EnemyMechanic(GameplayController controller, CancellationTokenSource tokenSource,
            ConfigProvider provider)
        {
            _provider = provider;
            _asteroidsMechanic = new AsteroidMechanic(controller, _provider, controller.AsteroidsView);
            _nloMechanic = new NloMechanic(controller, tokenSource, _provider);
        }

        public async UniTask StartGame()
        {
            await _nloMechanic.StartGame();
            await _asteroidsMechanic.StartGame();

            WaitCancelLevel().Forget();
        }

        private async UniTaskVoid WaitCancelLevel()
        {
            await _asteroidsMechanic.WaitDieAllElements();
            AllDied?.Invoke();
        }

        public void SetupLevel(int level)
        {
            _asteroidsMechanic.SetupLevel(level);
            _nloMechanic.SetupLevel(level);
        }

        public void SetupTokenSource(CancellationTokenSource tokenSource)
        {
            _asteroidsMechanic.SetupTokenSource(tokenSource);
        }

        public event Action AllDied;
    }
}