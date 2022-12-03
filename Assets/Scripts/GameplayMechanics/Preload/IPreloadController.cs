using Cysharp.Threading.Tasks;
using Gameplay.App;

namespace GameplayMechanics.Preload
{
    public interface IPreloadController
    {
        UniTask InitGame(IAppEventProvider eventProvider, string mainScene, string screenScene);
    }
}