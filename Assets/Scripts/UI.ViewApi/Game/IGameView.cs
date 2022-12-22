using UI.ViewApi.View;
using UnityEngine;

namespace UI.ViewApi.Game
{
    public interface IGameView : IScreenView
    {
        void SetupLevel(int level);
        void SetupAsteroids(int big, int med, int small);
        void SetupNlo(int count);

        void SetupProgress(float time, float totalTime);
        void FullCharges();
        void SetCharges(int charges);
        void TimeUse(float time);

        void UpdatePosition(Transform player, float speed);

        void UpdateLifes(int lifes);
        void LaserCancelled();
    }
}