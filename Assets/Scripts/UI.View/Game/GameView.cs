using TMPro;
using UI.View.Screen;
using UI.ViewApi.Game;
using UIModel.Window;
using UnityEngine;

namespace UI.View.Game
{
    public class GameView : ScreenView<IGameView>, IGameView
    {
        [SerializeField]
        private PlayerInfo _player;

        [SerializeField]
        private LevelInfo _level;

        [SerializeField]
        private TextMeshProUGUI _score;

        public override void BeforeOpen(WindowData windowData)
        {
            base.BeforeOpen(windowData);
            _player.Init();
        }

        [SerializeField]
        private LaserChargeView _laser;


        public void SetupLevel(int level)
        {
            _level.SetupLevel(level);
        }

        public void SetupAsteroids(int big, int med, int small)
        {
            _level.SetupAsteroids(big, med, small);
        }

        public void SetupNlo(int count)
        {
            _level.SetupNlo(count);
        }

        public void SetupProgress(float time, float totalTime)
        {
            _laser.SetupProgress(time, totalTime);
        }

        public void FullCharges()
        {
            _laser.FullCharges();
        }

        public void SetCharges(int charges)
        {
            _laser.SetCharges(charges);
        }

        public void TimeUse(float time)
        {
            _laser.TimeUse(time);
        }

        public void UpdatePosition(Transform player, float speed)
        {
            _player.UpdatePosition(player, speed);
        }

        public void UpdateLifes(int lifes)
        {
            _player.UpdateLifes(lifes);
        }

        public void LaserCancelled()
        {
            _laser.CancelUse();
        }

        public void SetScore(int score)
        {
            _score.text = $"Current score: {score}";
        }


        protected override IGameView View => this;
    }
}