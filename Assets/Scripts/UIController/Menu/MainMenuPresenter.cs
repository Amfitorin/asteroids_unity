using Cysharp.Threading.Tasks;
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
        private readonly IMainMechanic _gameplayMechanic;
        private readonly UIPrefabsConfig _uiPrefabs;

        public MainMenuPresenter(IMainMechanic gameplayMechanic, UIPrefabsConfig uiPrefabs)
        {
            _gameplayMechanic = gameplayMechanic;
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
            await _gameplayMechanic.StartGame();
            WindowManager.Instance.OpenScreen(_uiPrefabs.GameScreen,
                new GamePresenter(_gameplayMechanic.PlayerMechanic, _gameplayMechanic.AsteroidMechanic,
                    _gameplayMechanic.Nlo, _gameplayMechanic));
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