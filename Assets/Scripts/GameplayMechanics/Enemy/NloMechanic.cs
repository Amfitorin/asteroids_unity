using System;
using System.Threading;
using Core.Utils.Extensions;
using Cysharp.Threading.Tasks;
using Gameplay.ViewApi.Enemy;
using Gameplay.ViewApi.Gameplay;
using GameplayMechanics.Gun;
using MechanicsApi.Enemy;
using MechanicsApi.Gameplay;
using Model.Configs;
using Model.Configs.Enemy;
using Model.Configs.Level;
using UnityEngine;
using Random = UnityEngine.Random;

namespace GameplayMechanics.Enemy
{
    public class NloMechanic : INloMechanic
    {
        private readonly IGameplayController _controller;
        private readonly LevelsConfig _levelConfig;
        private readonly INloView _view;
        private readonly IPointsController _pointsController;
        private AutomaticBulletMechanic _bulletMechanic;
        private DirectionType _currentDirection;
        private NloConfig _currentLevelSettings;
        private CancellationTokenSource _directionToken;
        private CancellationTokenSource _levelToken;
        private INloComponent _nlo;


        public NloMechanic(IGameplayController controller, IConfigProvider configProvider, INloView view,
            IPointsController pointsController)
        {
            _controller = controller;

            _levelConfig = configProvider.LevelsConfig;
            _view = view;
            _pointsController = pointsController;
        }

        public void SetupTokenSource(CancellationTokenSource tokenSource)
        {
            _view.SetupTokenSource(tokenSource);
        }

        public async UniTask StartGame()
        {
            StartSpawnTimer().Forget();
            await UniTask.Yield();
        }

        public void SetupLevel(int level)
        {
            _currentLevelSettings = _levelConfig.GetLevelSettings(level).Enemies.Nlo.Config;
            _view.Destroy().Forget();
            ResetLevelToken();
        }

        public async UniTask Destroy()
        {
            _levelToken.Cancel();
            await _view.Destroy();
        }

        public event Action NloChanged;
        public bool HasNlo => _nlo != null;

        private async UniTaskVoid StartSpawnTimer()
        {
            await UniTask.WhenAny(UniTask.Delay(_currentLevelSettings.Duration.GetRandom().AsMS()),
                UniTask.WaitUntilCanceled(_levelToken.Token));
            if (_levelToken.IsCancellationRequested)
            {
                return;
            }

            await SpawnNlo();
            _bulletMechanic = new AutomaticBulletMechanic(_currentLevelSettings.Gun,
                _controller.BulletView, () => _nlo.BulletRoot.position, _nlo.LifeToken);
            _currentDirection = DirectionType.Horizontal;
            _directionToken = new CancellationTokenSource();
            StartDirectionChange().Forget();
            WaitNloDie().Forget();
        }

        private async UniTaskVoid WaitNloDie()
        {
            var index = await UniTask.WhenAny(_nlo.WaitDie(), _nlo.WaitInvisible());
            if (index.hasResultLeft)
            {
                _pointsController.AddScore(_currentLevelSettings.Score);
            }

            await _view.Destroy();
            _directionToken.Cancel();
            _nlo = null;
            _bulletMechanic = null;
            NloChanged?.Invoke();
            StartSpawnTimer().Forget();
        }

        private async UniTaskVoid StartDirectionChange()
        {
            await UniTask.WhenAny(UniTask.Delay(_currentLevelSettings.ChangeDirectionDuration.GetRandom().AsMS()),
                UniTask.WaitUntilCanceled(_directionToken.Token));
            if (_directionToken.IsCancellationRequested)
            {
                return;
            }

            switch (_currentDirection)
            {
                case DirectionType.Up:
                    _currentDirection = DirectionType.Horizontal;
                    _nlo.ChangeDirection(Vector3.down);
                    break;
                case DirectionType.Down:
                    _currentDirection = DirectionType.Horizontal;
                    _nlo.ChangeDirection(Vector3.up);
                    break;
                case DirectionType.Horizontal:
                    var up = Random.Range(0, 2);
                    if (up > 0)
                    {
                        _currentDirection = DirectionType.Up;
                        _nlo.ChangeDirection(Vector3.up);
                        break;
                    }

                    _currentDirection = DirectionType.Down;
                    _nlo.ChangeDirection(Vector3.down);
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            StartDirectionChange().Forget();
        }

        private async UniTask SpawnNlo()
        {
            var (position, direction) = _controller.Camera.RandomPointOnSideBorder;
            _nlo = await _view.Spawn(_currentLevelSettings, position, direction);
            NloChanged?.Invoke();
        }

        private void ResetLevelToken()
        {
            _levelToken?.Cancel();
            _levelToken = new CancellationTokenSource();
        }

        private enum DirectionType
        {
            Up,
            Down,
            Horizontal
        }
    }
}