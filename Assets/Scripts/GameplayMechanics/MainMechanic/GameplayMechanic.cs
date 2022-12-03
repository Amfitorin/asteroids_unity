using System.Collections.Generic;
using System.Threading;
using Cysharp.Threading.Tasks;
using Cysharp.Threading.Tasks.Linq;
using Gameplay.Gameplay;
using GameplayMechanics.Enemy;
using GameplayMechanics.PLayers;

namespace GameplayMechanics.MainMechanic
{
    public class GameplayMechanic : IGameplayMechanic
    {
        private readonly GameplayController _controller;
        private readonly List<IGameplayMechanic> _gameplayMechanics;
        private CancellationTokenSource _tokenSource;

        public GameplayMechanic(GameplayController controller)
        {
            _controller = controller;
            _tokenSource = new CancellationTokenSource();
            _gameplayMechanics = new List<IGameplayMechanic>
            {
                new PLayerMechanic(_controller, _tokenSource),
                new AsteroidMechanic(_controller, _tokenSource),
                new NloMechanic(_controller, _tokenSource)
            };
            StartGame().Forget();
        }

        public async UniTaskVoid StartGame()
        {
            _gameplayMechanics.ForEach(mechanic =>
            {
                mechanic.StartGame().Forget();
            });
            await UniTask.Yield();
        }

        public async UniTaskVoid Update()
        {
            await foreach (var _ in UniTaskAsyncEnumerable.EveryUpdate()
                               .WithCancellation(_tokenSource.Token))
            {
                _gameplayMechanics.ForEach(mechanic =>
                {
                    mechanic.Update().Forget();
                });
                await UniTask.Yield();
            }
        }

        public async UniTaskVoid LateUpdate()
        {
            await foreach (var _ in UniTaskAsyncEnumerable.EveryUpdate(PlayerLoopTiming.LastUpdate)
                               .WithCancellation(_tokenSource.Token))
            {
                _gameplayMechanics.ForEach(mechanic =>
                {
                    mechanic.LateUpdate().Forget();
                });
                await UniTask.Yield();
            }
        }

        public async UniTaskVoid Pause(bool state)
        {
            _tokenSource.Cancel();
        }
    }
}