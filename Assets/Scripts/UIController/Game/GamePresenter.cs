using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using Cysharp.Threading.Tasks.Linq;
using Gameplay.ViewApi.Enemy;
using MechanicsApi.Enemy;
using MechanicsApi.Gameplay;
using MechanicsApi.Player;
using Model.EnvObject;
using UI.ViewApi.Game;
using UI.ViewApi.View;

namespace UIController.Game
{
    public class GamePresenter : IScreenPresenter<IGameView>
    {
        private readonly IAsteroidMechanic _asteroids;
        private readonly IMainMechanic _mainMechanic;
        private readonly INloMechanic _nlo;
        private readonly IPlayerMechanic _playerMechanic;
        private readonly CancellationTokenSource _screenToken;

        private int _bigAsteroids;
        private int _medAsteroids;
        private int _smallAsteroids;
        private IGameView _view;

        public GamePresenter(IPlayerMechanic playerMechanic, IAsteroidMechanic asteroids, INloMechanic nlo,
            IMainMechanic mainMechanic)
        {
            _playerMechanic = playerMechanic;
            _asteroids = asteroids;
            _nlo = nlo;
            _mainMechanic = mainMechanic;
            _screenToken = new CancellationTokenSource();
        }

        public void OnOpen(IGameView view)
        {
            _view = view;
            StartSeekPlayer().Forget();
            CheckAsteroids();
            _view.SetupLevel(_mainMechanic.CurrentLevel);
            _view.SetCharges(_playerMechanic.Laser.ChargesCount);
            _view.TimeUse(0f);

            _asteroids.AsteroidDie += OnAsteroidDie;
            _asteroids.AsteroidsSpawned += OnAsteroidsSpawn;
            _nlo.NloChanged += OnNloChanged;
            _mainMechanic.LevelChanged += OnLevelChanged;
            _playerMechanic.Laser.ChargesChanged += OnChargesChanged;
            _playerMechanic.Laser.UseProgress += OnUseProgress;
            _playerMechanic.Laser.CooldownProgress += OnCooldownProgress;
            _playerMechanic.Laser.LaserCancelled += OnLaserCancelled;
        }

        public void OnClose(IGameView view)
        {
            _screenToken.Cancel();
            _asteroids.AsteroidDie -= OnAsteroidDie;
            _asteroids.AsteroidsSpawned -= OnAsteroidsSpawn;
            _nlo.NloChanged -= OnNloChanged;
            _mainMechanic.LevelChanged -= OnLevelChanged;
            _playerMechanic.Laser.ChargesChanged -= OnChargesChanged;
            _playerMechanic.Laser.UseProgress -= OnUseProgress;
            _playerMechanic.Laser.CooldownProgress -= OnCooldownProgress;
            _playerMechanic.Laser.LaserCancelled -= OnLaserCancelled;
        }

        private void OnCooldownProgress(float current, float total)
        {
            _view.SetupProgress(current, total);
        }

        private void OnUseProgress(float time)
        {
            _view.TimeUse(time);
        }

        private void OnChargesChanged(int count, bool isFull)
        {
            _view.SetCharges(count);
            if (isFull)
            {
                _view.FullCharges();
            }
        }

        private void OnLevelChanged(int obj)
        {
            _view.SetupLevel(_mainMechanic.CurrentLevel);
        }

        private void OnNloChanged()
        {
            _view.SetupNlo(_nlo.HasNlo ? 1 : 0);
        }

        private void OnLaserCancelled()
        {
            _view.LaserCancelled();
        }

        private async UniTaskVoid StartSeekPlayer()
        {
            await UniTaskAsyncEnumerable.EveryUpdate(PlayerLoopTiming.PreLateUpdate)
                .ForEachAsync(
                    _ => { _view.UpdatePosition(_playerMechanic.PlayerTransform, _playerMechanic.CurrentSpeed); },
                    cancellationToken: _screenToken.Token);
        }

        private void OnAsteroidsSpawn(int count, AsteroidType type)
        {
            ChangeAsteroidsCount(type, count);
            UpdateAsteroidsCount();
        }

        private void OnAsteroidDie(IAsteroidComponent asteroid)
        {
            ChangeAsteroidsCount(asteroid.SpawnData.Config.Type, -1);

            UpdateAsteroidsCount();
        }

        private void ChangeAsteroidsCount(AsteroidType asteroid, int count)
        {
            switch (asteroid)
            {
                case AsteroidType.Big:
                    _bigAsteroids += count;
                    break;
                case AsteroidType.Medium:
                    _medAsteroids += count;
                    break;
                case AsteroidType.Small:
                    _smallAsteroids += count;
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        private void CheckAsteroids()
        {
            var big = 0;
            var medium = 0;
            var small = 0;
            foreach (var component in _asteroids.Asteroids)
            {
                ChangeAsteroidsCount(component.SpawnData.Config.Type, 1);
            }

            _bigAsteroids = big;
            _medAsteroids = medium;
            _smallAsteroids = small;
            UpdateAsteroidsCount();
        }

        private void UpdateAsteroidsCount()
        {
            _view.SetupAsteroids(_bigAsteroids, _medAsteroids, _smallAsteroids);
        }
    }
}