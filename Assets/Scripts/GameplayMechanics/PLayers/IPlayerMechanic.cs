using System;
using GameplayMechanics.MainMechanic;

namespace GameplayMechanics.PLayers
{
    public interface IPlayerMechanic : IGameplayMechanic
    {
        event Action Died;
    }
}