using CoreMechanics.Scene;
using Cysharp.Threading.Tasks;
using Gameplay.App;

namespace GameplayMechanics.App
{
    public interface IAppController
    {
        ISceneController SceneController { get; }
        IAppEventProvider AppEventProvider { get; }

        UniTask StartGame();
    }
}