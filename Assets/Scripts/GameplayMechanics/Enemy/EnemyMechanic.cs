using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using Gameplay.ViewApi.Gameplay;
using MechanicsApi.Enemy;
using Model.Configs;

namespace GameplayMechanics.Enemy
{
    public class EnemyMechanic : IEnemyMechanic
    {
        private readonly IAsteroidMechanic _asteroidsMechanic;
        private readonly INloMechanic _nloMechanic;

        public EnemyMechanic(IGameplayController controller, CancellationTokenSource tokenSource,
            IConfigProvider provider)
        {
            _asteroidsMechanic = new AsteroidMechanic(controller, provider, controller.AsteroidsView);
            _nloMechanic = new NloMechanic(controller, tokenSource, provider, controller.NloView);
        }

        public async UniTask StartGame()
        {
            await _nloMechanic.StartGame();
            await _asteroidsMechanic.StartGame();

            WaitCancelLevel().Forget();
        }

        public void SetupLevel(int level)
        {
            _asteroidsMechanic.SetupLevel(level);
            _nloMechanic.SetupLevel(level);
        }

        public void SetupTokenSource(CancellationTokenSource tokenSource)
        {
            _asteroidsMechanic.SetupTokenSource(tokenSource);
            _nloMechanic.SetupTokenSource(tokenSource);
        }

        public IAsteroidMechanic AsteroidMechanic => _asteroidsMechanic;
        public INloMechanic Nlo => _nloMechanic;
        public event Action AllDied;

        private async UniTaskVoid WaitCancelLevel()
        {
            await _asteroidsMechanic.WaitDieAllElements();
            CheckLevelEnd();
        }

        private void CheckLevelEnd()
        {
            if (!_asteroidsMechanic.HasEnemies && !_nloMechanic.HasNlo)
            {
                AllDied?.Invoke();
            }
        }
    }
}