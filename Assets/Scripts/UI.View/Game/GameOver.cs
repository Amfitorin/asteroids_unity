using System;
using JetBrains.Annotations;
using TMPro;
using UI.View.Screen;
using UI.ViewApi.Game;
using UnityEngine;

namespace UI.View.Game
{
    public class GameOver : ScreenView<IGameOverView>, IGameOverView
    {
        [SerializeField]
        private TextMeshProUGUI _score;

        protected override IGameOverView View => this;
        public event Action CloseClick;
        public event Action NewGameClick;

        public void SetupScore(int score, int bestScore)
        {
            _score.text = $"Your score is {score}. Best score: {bestScore}";
        }

        [UsedImplicitly]
        public void Close()
        {
            CloseClick?.Invoke();
        }

        [UsedImplicitly]
        public void NewGame()
        {
            NewGameClick?.Invoke();
        }
    }
}