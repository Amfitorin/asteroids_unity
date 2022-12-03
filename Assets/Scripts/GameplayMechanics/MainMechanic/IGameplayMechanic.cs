using Cysharp.Threading.Tasks;

namespace GameplayMechanics.MainMechanic
{
    public interface IGameplayMechanic
    {
        UniTaskVoid Update();
        UniTaskVoid LateUpdate();
        UniTaskVoid Pause(bool state);
        UniTaskVoid StartGame();
    }
}