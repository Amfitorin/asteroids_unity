using System;

namespace MechanicsApi.Gun
{
    public interface IExtraGunMechanic : IGunMechanic
    {
        bool IsActive { get; }
        void Die();
        event Action<float, float> CooldownProgress;
        int ChargesCount { get; }
    }
}