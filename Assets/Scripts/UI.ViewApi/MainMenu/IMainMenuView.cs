using System;
using UI.ViewApi.View;

namespace UI.ViewApi.MainMenu
{
    public interface IMainMenuView : IScreenView
    {
        event Action NewGameClick;
        event Action ResultsClick;
        event Action CloseClick;
        
    }
}