using CoreMechanics.Scene;
using Cysharp.Threading.Tasks;
using Gameplay.App;
using GameplayMechanics.App;

namespace GameplayMechanics.Preload
{
    public class PreloadController : IPreloadController
    {
        private IAppController _appController;

        public async UniTask InitGame(IAppEventProvider eventProvider, string mainScene, string screenScene)
        {
            var sceneController = new SceneController(mainScene, screenScene);
            _appController = new AppController(eventProvider, sceneController);
            await _appController.StartGame();
        }
    }
}