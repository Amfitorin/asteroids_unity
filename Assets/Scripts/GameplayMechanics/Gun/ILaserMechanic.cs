using Gameplay.ViewApi.Gun;
using UnityEngine;

namespace GameplayMechanics.Gun
{
    public interface ILaserMechanic : IExtraGunMechanic
    {
        void Init(ILaserComponent laser, Transform parent);
    }
}