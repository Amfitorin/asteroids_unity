using Cysharp.Threading.Tasks;
using GameplayMechanics.MainMechanic;

namespace GameplayMechanics.Enemy
{
    public interface IAsteroidMechanic : IGameplayMechanic
    {
        bool HasEnemies { get; }
        UniTask WaitDieAllElements();
    }
}