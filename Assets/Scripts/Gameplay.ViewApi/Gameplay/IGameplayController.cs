using Model.EnvObject;

namespace Gameplay.ViewApi.Gameplay
{
    public interface IGameplayController
    {
        void LoadPlayer();
        void LoadNlo();
        void LoadAsteroid(AsteroidType type);
    }
}