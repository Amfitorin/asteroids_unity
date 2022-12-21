using System.Collections.Generic;
using System.Threading;
using Cysharp.Threading.Tasks;
using Cysharp.Threading.Tasks.Linq;
using Gameplay.ViewApi.Gameplay;
using GameplayMechanics.App;
using GameplayMechanics.Enemy;
using GameplayMechanics.PLayers;
using Model.Configs;
using UnityEngine;

namespace GameplayMechanics.MainMechanic
{
    public class GameplayMechanic : IGameplayMechanic
    {
        private readonly IAppController _appController;
        private readonly IConfigProvider _configProvider;
        private readonly IGameplayController _controller;
        private int _currentLevel;
        private IEnemyMechanic _enemyMechanic;
        private List<IGameplayMechanic> _gameplayMechanics;
        private CancellationTokenSource _tokenSource;

        public GameplayMechanic(IAppController appController, IGameplayController controller,
            IConfigProvider configProvider)
        {
            _appController = appController;
            _controller = controller;
            _configProvider = configProvider;
            _tokenSource = new CancellationTokenSource();
            StartGame().Forget();
            appController.AppEventProvider.AppPaused += AppEventProviderOnAppPaused;
            appController.AppEventProvider.AppQuit += AppEventProviderOnAppQuit;
        }

        public void Release()
        {
            _gameplayMechanics.ForEach(mechanic => { mechanic.Release(); });
        }

        public async UniTask StartGame()
        {
            _controller.StartGame(_configProvider, _tokenSource);
            _enemyMechanic = new EnemyMechanic(_controller, _tokenSource, _configProvider);
            _enemyMechanic.AllDied += EnemyMechanicOnAllDied;
            var pLayerMechanic = new PLayerMechanic(_controller, _configProvider);
            pLayerMechanic.Died += PLayerMechanicOnDied;
            _gameplayMechanics = new List<IGameplayMechanic>
            {
                pLayerMechanic,
                _enemyMechanic
            };
            SetupLevel(++_currentLevel);
            await UniTask.WhenAll(_gameplayMechanics.Select(x => x.StartGame()));
            RunUpdateLoop();
        }

        public void SetupLevel(int level)
        {
            _gameplayMechanics.ForEach(mechanic => { mechanic.SetupLevel(level); });
        }

        public void SetupTokenSource(CancellationTokenSource tokenSource)
        {
            _tokenSource = tokenSource;
            _gameplayMechanics.ForEach(mechanic => { mechanic.SetupTokenSource(tokenSource); });
        }

        public async UniTaskVoid Update()
        {
            await foreach (var _ in UniTaskAsyncEnumerable.EveryUpdate()
                               .WithCancellation(_tokenSource.Token))
            {
                _gameplayMechanics.ForEach(mechanic => { mechanic.Update().Forget(); });
                await UniTask.Yield();
            }
        }

        public async UniTaskVoid LateUpdate()
        {
            await foreach (var _ in UniTaskAsyncEnumerable.EveryUpdate(PlayerLoopTiming.LastUpdate)
                               .WithCancellation(_tokenSource.Token))
            {
                _gameplayMechanics.ForEach(mechanic => { mechanic.LateUpdate().Forget(); });
                await UniTask.Yield();
            }
        }

        public async UniTaskVoid Pause(bool state)
        {
            if (state)
            {
                _tokenSource.Cancel();
            }
            else
            {
                SetupTokenSource(new CancellationTokenSource());
                RunUpdateLoop();
            }

            Time.timeScale = state ? 0f : 1f;

            await UniTask.Yield();
        }

        private void AppEventProviderOnAppQuit()
        {
            _appController.AppEventProvider.AppPaused -= AppEventProviderOnAppPaused;
            _appController.AppEventProvider.AppQuit -= AppEventProviderOnAppQuit;
            Pause(true).Forget();
        }

        private void AppEventProviderOnAppPaused(bool state)
        {
            Pause(state).Forget();
        }

        private void PLayerMechanicOnDied()
        {
            //GameOver
        }

        private void EnemyMechanicOnAllDied()
        {
            SetupLevel(++_currentLevel);
            StartNewEnemyLevel().Forget();
        }

        private async UniTaskVoid StartNewEnemyLevel()
        {
            await _enemyMechanic.StartGame();
        }


        private void RunUpdateLoop()
        {
            Update().Forget();
            LateUpdate().Forget();
        }
    }
}