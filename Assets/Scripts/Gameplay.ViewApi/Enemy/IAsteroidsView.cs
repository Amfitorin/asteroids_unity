using System;
using System.Collections.Generic;
using CoreMechanics.Systems;
using Cysharp.Threading.Tasks;
using Model.Configs.Enemy;
using UnityEngine;

namespace Gameplay.ViewApi.Enemy
{
    public interface IAsteroidsView : ITokenCancelSource
    {
        UniTask DestroyAsteroids();
        UniTask<List<IAsteroidComponent>> SpawnAsteroids(int count, AsteroidConfig asteroid, Func<Vector3> getPosition);
        UniTask DestroyAsteroid(AsteroidSpawnData spawnData);
    }
}