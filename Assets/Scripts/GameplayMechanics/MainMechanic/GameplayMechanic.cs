using System.Collections.Generic;
using System.Threading;
using CoreMechanics.Systems;
using Cysharp.Threading.Tasks;
using Cysharp.Threading.Tasks.Linq;
using Gameplay.Gameplay;
using GameplayMechanics.App;
using GameplayMechanics.Configs;
using GameplayMechanics.Enemy;
using GameplayMechanics.PLayers;
using UnityEngine;

namespace GameplayMechanics.MainMechanic
{
    public class GameplayMechanic : IGameplayMechanic
    {
        private readonly IAppController _appController;
        private readonly ConfigProvider _configProvider;
        private readonly GameplayController _controller;
        private List<IGameplayMechanic> _gameplayMechanics;
        private CancellationTokenSource _tokenSource;
        private int _currentLevel;
        private EnemyMechanic _enemyMechanic;

        public GameplayMechanic(IAppController appController, GameplayController controller,
            ConfigProvider configProvider, IObjectSpawnSystem objectSpawnSystem)
        {
            _appController = appController;
            _controller = controller;
            _configProvider = configProvider;
            _tokenSource = new CancellationTokenSource();
            StartGame().Forget();
            appController.AppEventProvider.AppPaused += AppEventProviderOnAppPaused;
            appController.AppEventProvider.AppQuit += AppEventProviderOnAppQuit;
        }

        private void AppEventProviderOnAppQuit()
        {
            _appController.AppEventProvider.AppPaused -= AppEventProviderOnAppPaused;
            _appController.AppEventProvider.AppQuit -= AppEventProviderOnAppQuit;
            Pause(true).Forget();
        }

        public void Release()
        {
            _gameplayMechanics.ForEach(mechanic => { mechanic.Release(); });
        }

        private void AppEventProviderOnAppPaused(bool state)
        {
            Pause(state).Forget();
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
    }
}