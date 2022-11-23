using CoreMechanics.Scene;

namespace CoreMechanics.App
{
    public class AppController : IAppController
    {
        public ISceneController SceneController { get; }
        public IAppEventProvider AppEventProvider { get; }

        public AppController(IAppEventProvider eventProvider)
        {
           SceneController = new SceneController();
           AppEventProvider = eventProvider;
        }
    }
}