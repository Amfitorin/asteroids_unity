using JetBrains.Annotations;
using UIController.Manager;

namespace UIController.Screen
{
    public class ScreenBehaviour : WindowBase
    {
        [UsedImplicitly]
        public void CloseScreen()
        {
            WindowManager.Instance.CloseScreen();
        }

        [UsedImplicitly]
        public void CloseAllScreens()
        {
            WindowManager.Instance.CloseNotAllScreens();
        }
    }
}