using Core.Singleton;

namespace GameplayMechanics.Preload
{
    public class AppManager : Singleton<AppManager>
    {
        public IPreloadController PreloadController { get; private set; }

        protected override void Initialize()
        {
            PreloadController = new PreloadController();
        }
    }
}