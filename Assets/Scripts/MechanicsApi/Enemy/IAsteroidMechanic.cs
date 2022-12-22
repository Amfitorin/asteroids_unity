using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using Gameplay.ViewApi.Enemy;
using MechanicsApi.Gameplay;
using Model.EnvObject;

namespace MechanicsApi.Enemy
{
    public interface IAsteroidMechanic : IGameplayMechanic
    {
        event Action<IAsteroidComponent> AsteroidDie;
        event Action<int, AsteroidType> AsteroidsSpawned;
        bool HasEnemies { get; }
        UniTask WaitDieAllElements();
        IEnumerable<IAsteroidComponent> Asteroids { get; }
    }
}