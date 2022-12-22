using System;
using UnityEngine;

namespace Gameplay.ViewApi.Player
{
    public interface IPlayerComponent
    {
        event Action OnDied;
        Bounds GetBounds();
        void ApplySpeed(float percent);

        Transform Transform { get; }
    }
}