using Cysharp.Threading.Tasks;
using GameplayMechanics.MainMechanic;

namespace GameplayMechanics.Enemy
{
    public interface IAsteroidMechanic : IGameplayMechanic
    {
        UniTask WaitDieAllElements();
    }
}