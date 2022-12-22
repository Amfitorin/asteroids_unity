using JetBrains.Annotations;

namespace UI.View.Screen
{
    public class ScreenBehaviour : WindowBase
    {
        [UsedImplicitly]
        public void CloseScreen()
        {
            WindowCanvas.Current.Close();
        }

        [UsedImplicitly]
        public void CloseAllScreens()
        {
            WindowCanvas.Current.CloseAll();
        }
    }
}