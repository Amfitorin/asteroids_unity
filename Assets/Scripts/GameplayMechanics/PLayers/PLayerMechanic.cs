using System.Threading;
using Cysharp.Threading.Tasks;
using Gameplay.Gameplay;
using Gameplay.ViewApi.CameraView;
using Gameplay.ViewApi.Player;
using GameplayMechanics.Configs;
using Model.Configs.Player;

namespace GameplayMechanics.PLayers
{
    public class PLayerMechanic : IPlayerMechanic
    {
        private readonly GameplayController _controller;
        private CancellationTokenSource _cancellationTokenSource;
        private readonly PLayerConfig _playerConfig;
        private readonly IPlayerView _playerView;
        private readonly ICameraView _cameraView;

        public PLayerMechanic(GameplayController controller, CancellationTokenSource cancellationTokenSource, ConfigProvider provider)
        {
            _controller = controller;
            _cancellationTokenSource = cancellationTokenSource;
            _playerView = controller.PlayerView;
            _playerConfig = provider.PLayerConfig;
            _cameraView = controller.Camera;
        }

        public async UniTask StartGame()
        {
            await _playerView.SpawnPLayer(_cameraView.ScreenCenter);
        }

        public void Attack()
        {
        }

        public void UseExtraGun()
        {
        }
        
        public void SetupTokenSource(CancellationTokenSource tokenSource)
        {
            _cancellationTokenSource = tokenSource;
        }
    }
}