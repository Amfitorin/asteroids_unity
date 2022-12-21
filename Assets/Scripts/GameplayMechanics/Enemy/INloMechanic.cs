using GameplayMechanics.MainMechanic;

namespace GameplayMechanics.Enemy
{
    public interface INloMechanic : IGameplayMechanic
    {
        bool HasNlo { get; }
    }
}