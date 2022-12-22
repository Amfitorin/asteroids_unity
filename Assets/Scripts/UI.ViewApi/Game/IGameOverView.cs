using System;
using UI.ViewApi.View;

namespace UI.ViewApi.Game
{
    public interface IGameOverView : IScreenView
    {
        event Action CloseClick;
        event Action NewGameClick;
        void SetupScore(int score, int bestScore);
    }
}