using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Core.Utils.Extensions;
using CoreMechanics.Systems;
using Cysharp.Threading.Tasks;
using Gameplay.ViewApi.CameraView;
using Gameplay.ViewApi.Enemy;
using Model.Configs.Enemy;
using UnityEngine;
using UnityEngine.Assertions;
using Random = UnityEngine.Random;

namespace Gameplay.Enemy
{
    public class AsteroidsView : IAsteroidsView
    {
        private readonly List<Asteroid> _asteroids = new();
        private readonly ICameraView _cameraView;
        private readonly Transform _root;
        private readonly IObjectSpawnSystem _spawnSystem;
        private readonly CancellationTokenSource _tokenSource;
        private int _lastAsteroidId;

        public AsteroidsView(Transform root, ICameraView cameraView, IObjectSpawnSystem spawnSystem,
            CancellationTokenSource tokenSource)
        {
            _root = root;
            _cameraView = cameraView;
            _spawnSystem = spawnSystem;
            _tokenSource = tokenSource;
        }

        public async UniTask DestroyAsteroids()
        {
            await UniTask.WhenAll(_asteroids.Select(x => _spawnSystem.DestroyObject(x)));
            _asteroids.Clear();
        }

        public async UniTask<List<IAsteroidComponent>> SpawnAsteroids(int count, AsteroidConfig config,
            Func<Vector3> getPosition)
        {
            Assert.IsNotNull(config);
            var result = new List<IAsteroidComponent>();
            for (var i = 0; i < count; i++)
            {
                var spawned = await _spawnSystem.SpawnObject<Asteroid>(config.Prefab, _root,
                    getPosition());
                spawned.Init(new AsteroidSpawnData(++_lastAsteroidId, config, Random.insideUnitCircle),
                    _cameraView, _tokenSource);
                result.Add(spawned);
                _asteroids.Add(spawned);
            }

            return result;
        }

        public async UniTask DestroyAsteroid(AsteroidSpawnData spawnData)
        {
            var asteroid = _asteroids.First(x => x.SpawnData.ID == spawnData.ID);
            await _spawnSystem.DestroyObject(asteroid);
            _asteroids.RemoveWithReplaceLast(asteroid);
        }

        public void SetupTokenSource(CancellationTokenSource tokenSource)
        {
            foreach (var asteroid in _asteroids)
            {
                asteroid.SetupTokenSource(tokenSource);
            }
        }
    }
}