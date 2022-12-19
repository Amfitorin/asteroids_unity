using System;

namespace GameplayMechanics.Gun
{
    public interface IExtraGunMechanic : IGunMechanic
    {
        bool IsActive { get; }
        void Die();
        event Action<float> CooldownProgress;
    }
}