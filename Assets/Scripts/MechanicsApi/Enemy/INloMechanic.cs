using System;
using MechanicsApi.Gameplay;

namespace MechanicsApi.Enemy
{
    public interface INloMechanic : IGameplayMechanic
    {
        event Action NloChanged;
        bool HasNlo { get; }
    }
}