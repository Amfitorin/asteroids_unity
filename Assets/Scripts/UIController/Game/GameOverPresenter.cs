using Cysharp.Threading.Tasks;
using MechanicsApi.App;
using MechanicsApi.Gameplay;
using UI.ViewApi.Game;
using UI.ViewApi.View;

#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine;

namespace UIController.Game
{
    public class GameOverPresenter : IScreenPresenter<IGameOverView>
    {
        private readonly IMainMechanic _mainMechanic;
        private readonly IPointsController _points;
        private readonly IAppController _appController;
        private IGameOverView _view;

        public GameOverPresenter(IMainMechanic mainMechanic, IPointsController points, IAppController appController)
        {
            _mainMechanic = mainMechanic;
            _points = points;
            _appController = appController;
        }

        public void OnOpen(IGameOverView view)
        {
            _view = view;
            _view.SetupScore(_points.Score, _points.BestScore);
            _view.CloseClick += OnCloseClick;
            _view.NewGameClick += OnNewGameClick;
        }

        private void OnNewGameClick()
        {
            StartNewGame().Forget();
        }

        private void OnCloseClick()
        {
            if (Application.isEditor)
            {
#if UNITY_EDITOR
                EditorApplication.ExitPlaymode();
                return;
#endif
            }

            Application.Quit();
        }

        private async UniTaskVoid StartNewGame()
        {
            await _mainMechanic.Destroy();
            await _appController.NewGame();
        }

        public void OnClose(IGameOverView view)
        {
        }
    }
}