using System;
using GameplayMechanics.MainMechanic;

namespace GameplayMechanics.Enemy
{
    public interface IEnemyMechanic : IGameplayMechanic
    {
        event Action AllDied;
    }
}