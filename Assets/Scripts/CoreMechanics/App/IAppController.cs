using CoreMechanics.Scene;

namespace CoreMechanics.App
{
    public interface IAppController
    {
        ISceneController SceneController { get; }
        IAppEventProvider AppEventProvider { get; }
    }
}