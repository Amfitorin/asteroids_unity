using System.Threading;
using CoreMechanics.Systems;
using Gameplay.ViewApi.CameraView;
using Gameplay.ViewApi.Enemy;
using Gameplay.ViewApi.Gun;
using Gameplay.ViewApi.Player;
using Model.Configs;

namespace Gameplay.ViewApi.Gameplay
{
    public interface IGameplayController
    {
        IPlayerView PlayerView { get; }
        ICameraView Camera { get; }
        IBulletView BulletView { get; }
        ILaserView LaserView { get; }
        IAsteroidsView AsteroidsView { get; }
        INloView NloView { get; }
        void StartGame(IConfigProvider configProvider, CancellationTokenSource tokenSource);
        void SetupSpawnSystem(IObjectSpawnSystem spawnSystem);
    }
}