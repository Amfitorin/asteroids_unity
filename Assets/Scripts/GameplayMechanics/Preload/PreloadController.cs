using CoreMechanics.Scene;
using Cysharp.Threading.Tasks;
using Gameplay.App;
using GameplayMechanics.App;
using GameplayMechanics.Configs;

namespace GameplayMechanics.Preload
{
    public class PreloadController : IPreloadController
    {
        private IAppController _appController;

        public async UniTask InitGame(IAppEventProvider eventProvider, string mainScene, string screenScene,
            ConfigProvider configProvider)
        {
            var sceneController = new SceneController(mainScene, screenScene);
            _appController = new AppController(eventProvider, sceneController, configProvider);
            await _appController.StartGame();
        }
    }
}