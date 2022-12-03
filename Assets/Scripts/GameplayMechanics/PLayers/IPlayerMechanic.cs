using GameplayMechanics.MainMechanic;

namespace GameplayMechanics.PLayers
{
    public interface IPlayerMechanic : IGameplayMechanic
    {
        void Attack();
        void UseExtraGun();
    }
}