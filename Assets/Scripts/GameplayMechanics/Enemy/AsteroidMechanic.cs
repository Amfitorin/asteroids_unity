using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Core.Utils.Extensions;
using Cysharp.Threading.Tasks;
using Gameplay.Gameplay;
using Gameplay.ViewApi.Enemy;
using GameplayMechanics.Configs;
using Model.Configs.Level;
using Model.EnvObject;
using Model.Level;
using UnityEngine;

namespace GameplayMechanics.Enemy
{
    public class AsteroidMechanic : IAsteroidMechanic
    {
        private readonly GameplayController _controller;
        private readonly LevelsConfig _levelConfig;
        private readonly IAsteroidsView _view;
        private List<AsyncLazy<Vector3>> _asteroidAwaiters = new();
        private List<IAsteroidComponent> _asteroids = new();
        private LevelSettings _currentLevelSettings;

        public AsteroidMechanic(GameplayController controller, ConfigProvider configProvider, IAsteroidsView view)
        {
            _controller = controller;
            _view = view;
            _levelConfig = configProvider.LevelsConfig;
        }

        public void SetupTokenSource(CancellationTokenSource tokenSource)
        {
            _view.SetupTokenSource(tokenSource);
        }

        public async UniTask StartGame()
        {
            var results = await SpawnLevelAsteroids();
            _asteroids = results.ToList();
        }

        public void SetupLevel(int level)
        {
            _currentLevelSettings = _levelConfig.GetLevelSettings(level);
            _view.DestroyAsteroids();
        }

        private async UniTask<IEnumerable<IAsteroidComponent>> SpawnLevelAsteroids()
        {
            var tasks = new List<UniTask<List<IAsteroidComponent>>>();
            if (_currentLevelSettings.BigEnemyCount > 0)
            {
                var asteroid = _currentLevelSettings.Enemies.Asteroids.First(x => x.Type == AsteroidType.Big);
                tasks.Add(_view.SpawnAsteroids(_currentLevelSettings.BigEnemyCount, asteroid.Config,
                    () => _controller.Camera.RandomPointOnScreenBorder));
            }

            if (_currentLevelSettings.MediumEnemyCount > 0)
            {
                var asteroid = _currentLevelSettings.Enemies.Asteroids.First(x => x.Type == AsteroidType.Medium);
                tasks.Add(_view.SpawnAsteroids(_currentLevelSettings.MediumEnemyCount, asteroid.Config,
                    () => _controller.Camera.RandomPointOnScreenBorder));
            }

            if (_currentLevelSettings.SmallEnemyCount > 0)
            {
                var asteroid = _currentLevelSettings.Enemies.Asteroids.First(x => x.Type == AsteroidType.Small);
                tasks.Add(_view.SpawnAsteroids(_currentLevelSettings.SmallEnemyCount, asteroid.Config,
                    () => _controller.Camera.RandomPointOnScreenBorder));
            }

            var results = await UniTask.WhenAll(tasks);
            return results.SelectMany(x => x);
        }

        public async UniTask WaitDieAllElements()
        {
            _asteroidAwaiters.AddRange(_asteroids.Select(x => UniTask.Lazy(x.WaitDie)));
            while (_asteroidAwaiters.Count > 0)
            {
                var awaited = await UniTask.WhenAny(_asteroidAwaiters.Select(x => x.Task));
                var index = awaited.winArgumentIndex;
                var component = _asteroids[index];


                _asteroidAwaiters.RemoveAt(index);
                _asteroids.RemoveAt(index);
                await DieObject(awaited.result, component);
            }
        }

        private async UniTask DieObject(Vector3 awaitedResult, IAsteroidComponent asteroidComponent)
        {
            var spawnData = asteroidComponent.SpawnData;
            await _view.DestroyAsteroid(spawnData);
            if (spawnData.Config.ShardCount > 0 && spawnData.Config.Shard != null)
            {
                await SpawnShards(awaitedResult, spawnData);
            }
        }

        private async UniTask SpawnShards(Vector3 awaitedResult, AsteroidSpawnData spawnData)
        {
            var shards = await _view.SpawnAsteroids(spawnData.Config.ShardCount, spawnData.Config.Shard,
                () => awaitedResult);
            _asteroids.AddRange(shards);
            _asteroidAwaiters.AddRange(shards.Select(x => UniTask.Lazy(x.WaitDie)));
        }
    }
}