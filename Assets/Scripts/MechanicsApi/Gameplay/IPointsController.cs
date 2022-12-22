using System;

namespace MechanicsApi.Gameplay
{
    public interface IPointsController
    {
        event Action ScoreChanged;
        int Score { get; }
        void AddScore(int count);
        int BestScore { get; }
    }
}