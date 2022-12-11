using System.Linq;
using Core.Utils.Extensions;
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
                ConstZ));
            ScreenCenter = screenCenter;
            var bottomLeft = camera.ViewportToWorldPoint(Vector3.zero);
            bottomLeft.z = _camera.nearClipPlane;
            var topRight = camera.ViewportToWorldPoint(new Vector3(1f, 1f, 0f));
            topRight.z = _camera.farClipPlane;
            var size = topRight - bottomLeft;
            ViewRect = new Bounds(ScreenCenter, size).AsRect();
        }

        public float ConstZ => _camera.farClipPlane - 10f;
        public Vector3 ScreenCenter { get; }

        public Vector3 RandomScreenPoint
        {
            get
            {
                var viewport = new Vector3(RangeFloat.One.GetRandom(), RangeFloat.One.GetRandom(), ConstZ);
                return _camera.ViewportToScreenPoint(viewport);
            }
        }

        public Rect ViewRect { get; }

        public bool IsObjectVisible(CornerRect rect)
        {
            return rect.All(x => ViewRect.Contains(x));
        }

        public Vector3 InversePosition(Vector3 position, CornerRect rect)
        {
            if (IsObjectVisible(rect))
            {
                return position;
            }

            if (rect.Rect.yMin > ViewRect.yMax)
            {
                position.y -= ViewRect.size.y + rect.Rect.size.y;
            }
            else if (rect.Rect.yMax < ViewRect.yMin)
            {
                position.y += ViewRect.size.y + rect.Rect.size.y;
            }

            if (rect.Rect.xMin > ViewRect.xMax)
            {
                position.x -= ViewRect.size.x + rect.Rect.size.x;
            }
            else if (rect.Rect.xMax < ViewRect.xMin)
            {
                position.x += ViewRect.size.x + rect.Rect.size.x;
            }

            return position;
        }
    }
}