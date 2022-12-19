using System.Threading;
using Gameplay.ViewApi.CameraView;
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
        void StartGame(IConfigProvider configProvider, CancellationTokenSource tokenSource);
    }
}