using Gameplay.ViewApi.CameraView;
using Model.CustomTypes;
using UnityEngine;

namespace Gameplay.CameraView
{
    public class CameraView : ICameraView
    {
        private readonly Camera _camera;

        public CameraView(Camera camera)
        {
            _camera = camera;
            var screenCenter = _camera.ScreenToWorldPoint(new Vector3(Screen.width / 2f, Screen.height / 2f,
                _camera.farClipPlane - 10f));
            screenCenter.z = ConstZ;
            ScreenCenter = screenCenter;
            CameraBounds = new Bounds(ScreenCenter,
                camera.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height, 0) * 2));
        }

        private float ConstZ => _camera.farClipPlane - 10f;
        public Vector3 ScreenCenter { get; }

        public Vector3 RandomScreenPoint
        {
            get
            {
                var viewport = new Vector3(RangeFloat.One.GetRandom(), RangeFloat.One.GetRandom(), ConstZ);
                return _camera.ViewportToScreenPoint(viewport);
            }
        }

        public Bounds CameraBounds { get; }
    }
}