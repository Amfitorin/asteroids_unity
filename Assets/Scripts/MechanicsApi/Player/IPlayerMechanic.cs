using System;
using MechanicsApi.Gameplay;
using MechanicsApi.Gun;
using UnityEngine;

namespace MechanicsApi.Player
{
    public interface IPlayerMechanic : IGameplayMechanic
    {
        ILaserMechanic Laser { get; }
        event Action Died;
        Transform PlayerTransform { get; }
        float CurrentSpeed { get; }
    }
}