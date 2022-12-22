using Cysharp.Threading.Tasks;
using Gameplay.App;
using Model.Configs;

namespace MechanicsApi.Preload
{
    public interface IPreloadController
    {
        UniTask InitGame(IAppEventProvider eventProvider, string mainScene, string screenScene,
            IConfigProvider configProvider);
    }
}