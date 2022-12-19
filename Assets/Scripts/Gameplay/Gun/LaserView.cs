using Gameplay.ViewApi.CameraView;
using Gameplay.ViewApi.Gun;
using UnityEngine;

namespace Gameplay.Gun
{
    public class LaserView : ILaserView
    {
        private readonly ICameraView _cameraView;
        private ILaserComponent _laser;

        public LaserView(ICameraView cameraView)
        {
            _cameraView = cameraView;
        }


        public void Init(ILaserComponent component)
        {
            _laser = component;
        }

        public void SetPoints(Vector3 first, Vector3 second)
        {
            _laser.UpdatePositions(first, second);
        }

        public void SetDirection(Vector3 first, Vector3 direction)
        {
            var viewport = _cameraView.GetBorderPointByDirection(first, direction);
            _laser.UpdatePositions(first, viewport);
        }

        public void RunLaser()
        {
            _laser.Show();
        }

        public void DieLaser()
        {
            _laser.Hide();
        }
    }
}