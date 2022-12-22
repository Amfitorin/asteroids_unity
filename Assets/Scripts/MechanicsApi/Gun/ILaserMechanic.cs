using System;
using Gameplay.ViewApi.Gun;
using UnityEngine;

namespace MechanicsApi.Gun
{
    public interface ILaserMechanic : IExtraGunMechanic
    {
        event Action<float> UseProgress;
        event Action LaserCancelled;
        event Action<int, bool> ChargesChanged;
        void Init(ILaserComponent laser, Transform parent);
    }
}