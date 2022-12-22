using System;
using MechanicsApi.Gameplay;

namespace MechanicsApi.Enemy
{
    public interface IEnemyMechanic : IGameplayMechanic
    {
        IAsteroidMechanic AsteroidMechanic { get; }
        INloMechanic Nlo { get; }
        event Action AllDied;
    }
}