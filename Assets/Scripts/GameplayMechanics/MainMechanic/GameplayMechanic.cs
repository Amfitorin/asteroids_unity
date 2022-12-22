using System;
using System.Collections.Generic;
using System.Threading;
using Cysharp.Threading.Tasks;
using Cysharp.Threading.Tasks.Linq;
using Gameplay.ViewApi.Gameplay;
using GameplayMechanics.App;
using GameplayMechanics.Enemy;
using GameplayMechanics.PLayers;
using MechanicsApi.App;
using MechanicsApi.Enemy;
using MechanicsApi.Gameplay;
using MechanicsApi.Player;
using Model.Configs;
using UIController.Game;
using UIController.Manager;
using UnityEngine;

namespace GameplayMechanics.MainMechanic
{
    public class GameplayMechanic : IMainMechanic, IPointsController
    {
        private const string BestScoreKey = "BestPlayerScore";
        private readonly IAppController _appController;
        private readonly IConfigProvider _configProvider;
        private readonly IGameplayController _controller;
        private int _bestScore = -1;
        private int _currentLevel;
        private IEnemyMechanic _enemyMechanic;
        private List<IGameplayMechanic> _gameplayMechanics = new();
        private IPlayerMechanic _pLayerMechanic;
        private CancellationTokenSource _tokenSource;

        public GameplayMechanic(IAppController appController, IGameplayController controller,
            IConfigProvider configProvider)
        {
            _appController = appController;
            _controller = controller;
            _configProvider = configProvider;
            _tokenSource = new CancellationTokenSource();
            appController.AppEventProvider.AppPaused += AppEventProviderOnAppPaused;
            appController.AppEventProvider.AppQuit += AppEventProviderOnAppQuit;
        }

        public void Release()
        {
            _gameplayMechanics.ForEach(mechanic => { mechanic.Release(); });
        }

        public async UniTask StartGame()
        {
            var best = PlayerPrefs.GetInt(BestScoreKey, 0);
            if (best > _bestScore)
            {
                _bestScore = best;
            }

            _controller.StartGame(_configProvider, _tokenSource);
            _enemyMechanic = new EnemyMechanic(_controller, _tokenSource, _configProvider, this);
            _enemyMechanic.AllDied += EnemyMechanicOnAllDied;
            _pLayerMechanic = new PLayerMechanic(_controller, _configProvider, _tokenSource);
            _pLayerMechanic.Died += PLayerMechanicOnDied;
            _gameplayMechanics = new List<IGameplayMechanic>
            {
                _pLayerMechanic,
                _enemyMechanic
            };
            Score = 0;
            SetupLevel(++_currentLevel);
            await UniTask.WhenAll(_gameplayMechanics.Select(x => x.StartGame()));
            WindowManager.Instance.OpenScreen(_configProvider.UIPrefabs.GameScreen,
                new GamePresenter(PlayerMechanic, AsteroidMechanic,
                    Nlo, this, PointsController));
            RunUpdateLoop();
        }

        public void SetupLevel(int level)
        {
            _gameplayMechanics.ForEach(mechanic => { mechanic.SetupLevel(level); });
        }

        public async UniTask Destroy()
        {
            await UniTask.WhenAll(_gameplayMechanics.Select(x => x.Destroy()));
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

        public event Action<int> LevelChanged;
        public IPlayerMechanic PlayerMechanic => _pLayerMechanic;
        public IEnemyMechanic EnemyMechanic => _enemyMechanic;
        public IAsteroidMechanic AsteroidMechanic => _enemyMechanic.AsteroidMechanic;
        public INloMechanic Nlo => _enemyMechanic.Nlo;
        public int CurrentLevel => _currentLevel;
        public IPointsController PointsController => this;

        public event Action ScoreChanged;
        public int Score { get; private set; }

        public void AddScore(int count)
        {
            Score += count;
            ScoreChanged?.Invoke();
        }

        public int BestScore
        {
            get
            {
                if (_bestScore == -1)
                {
                    _bestScore = PlayerPrefs.GetInt(BestScoreKey, 0);
                }

                return _bestScore;
            }
            private set
            {
                var best = PlayerPrefs.GetInt(BestScoreKey, 0);
                if (best == 0 || best < value)
                {
                    PlayerPrefs.SetInt(BestScoreKey, value);
                }
            }
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
            _tokenSource.Cancel();
            BestScore = Score;
            WindowManager.Instance.OpenScreen(_configProvider.UIPrefabs.GameOverWindow,
                new GameOverPresenter(this, this, _appController));
        }

        private void EnemyMechanicOnAllDied()
        {
            SetupLevel(++_currentLevel);
            LevelChanged?.Invoke(_currentLevel);
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