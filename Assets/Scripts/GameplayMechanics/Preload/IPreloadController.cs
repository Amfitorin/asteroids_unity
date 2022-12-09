using Cysharp.Threading.Tasks;
using Gameplay.App;
using GameplayMechanics.Configs;

namespace GameplayMechanics.Preload
{
    public interface IPreloadController
    {
        UniTask InitGame(IAppEventProvider eventProvider, string mainScene, string screenScene,
            ConfigProvider configProvider);
    }
}