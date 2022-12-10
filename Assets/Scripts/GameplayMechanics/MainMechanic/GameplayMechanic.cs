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

namespace GameplayMechanics.MainMechanic
{
    public class GameplayMechanic : IGameplayMechanic
    {
        private readonly IAppController _appController;
        private readonly ConfigProvider _configProvider;
        private readonly IObjectSpawnSystem _objectSpawnSystem;
        private readonly GameplayController _controller;
        private List<IGameplayMechanic> _gameplayMechanics;
        private CancellationTokenSource _tokenSource;

        public GameplayMechanic(IAppController appController, GameplayController controller,
            ConfigProvider configProvider, IObjectSpawnSystem objectSpawnSystem)
        {
            _appController = appController;
            _controller = controller;
            _configProvider = configProvider;
            _objectSpawnSystem = objectSpawnSystem;
            _tokenSource = new CancellationTokenSource();
            StartGame().Forget();
        }

        public async UniTask StartGame()
        {
            _controller.StartGame(_configProvider);
            _gameplayMechanics = new List<IGameplayMechanic>
            {
                new PLayerMechanic(_controller, _tokenSource, _configProvider),
                new AsteroidMechanic(_controller, _tokenSource),
                new NloMechanic(_controller, _tokenSource)
            };
            await UniTask.WhenAll(_gameplayMechanics.Select(x => x.StartGame()));
        }

        public async UniTaskVoid SetupLevel(int level)
        {
            _gameplayMechanics.ForEach(mechanic => { mechanic.SetupLevel(level).Forget(); });
            await UniTask.Yield();
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
                _tokenSource.Cancel();
            else
                SetupTokenSource(new CancellationTokenSource());

            await UniTask.Yield();
        }
    }
}