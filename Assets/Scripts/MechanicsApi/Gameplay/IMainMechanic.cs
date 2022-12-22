using System;
using Cysharp.Threading.Tasks;
using MechanicsApi.Enemy;
using MechanicsApi.Player;

namespace MechanicsApi.Gameplay
{
    public interface IMainMechanic : IGameplayMechanic
    {
        event Action<int> LevelChanged;

        IPlayerMechanic PlayerMechanic { get; }
        IEnemyMechanic EnemyMechanic { get; }
        IAsteroidMechanic AsteroidMechanic { get; }
        INloMechanic Nlo { get; }

        int CurrentLevel { get; }
        IPointsController PointsController { get; }
    }
}