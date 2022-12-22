using System;
using JetBrains.Annotations;
using UI.View.Screen;
using UI.ViewApi.MainMenu;

namespace UI.View.MainMenu
{
    public class MainMenu : ScreenView<IMainMenuView>, IMainMenuView
    {
        protected override IMainMenuView View => this;
        public event Action NewGameClick;
        public event Action ResultsClick;
        public event Action CloseClick;


        [UsedImplicitly]
        public void NewGame()
        {
            NewGameClick?.Invoke();
        }

        [UsedImplicitly]
        public void Results()
        {
            ResultsClick?.Invoke();
        }

        [UsedImplicitly]
        public void Close()
        {
            CloseClick?.Invoke();
        }
    }
}