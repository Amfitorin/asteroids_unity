using CoreMechanics.Scene;
using Cysharp.Threading.Tasks;
using Gameplay.App;

namespace MechanicsApi.App
{
    public interface IAppController
    {
        ISceneController SceneController { get; }
        IAppEventProvider AppEventProvider { get; }

        UniTask StartGame();
        UniTask NewGame();
    }
}