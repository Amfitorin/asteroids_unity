using Cysharp.Threading.Tasks;
using MechanicsApi.App;
using MechanicsApi.Gameplay;
using Model.Configs.UI;
using UI.ViewApi.MainMenu;
using UI.ViewApi.View;
using UIController.Game;
using UIController.Manager;

#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine;

namespace UIController.Menu
{
    public class MainMenuPresenter : IScreenPresenter<IMainMenuView>
    {
        private readonly IAppController _appController;
        private readonly UIPrefabsConfig _uiPrefabs;

        public MainMenuPresenter(IAppController appController, UIPrefabsConfig uiPrefabs)
        {
            _appController = appController;
            _uiPrefabs = uiPrefabs;
        }

        public void OnOpen(IMainMenuView view)
        {
            view.CloseClick += OnCloseClick;
            view.ResultsClick += OnResultsClick;
            view.NewGameClick += OnNewGameClick;
        }

        public void OnClose(IMainMenuView view)
        {
            view.CloseClick -= OnCloseClick;
            view.ResultsClick -= OnResultsClick;
            view.NewGameClick -= OnNewGameClick;
        }

        private void OnNewGameClick()
        {
            StartNewGame().Forget();
        }

        private async UniTaskVoid StartNewGame()
        {
            await _appController.NewGame();
        }

        private void OnResultsClick()
        {
            Debug.Log("Open results screen");
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
    }
}