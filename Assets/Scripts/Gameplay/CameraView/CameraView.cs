using System.Linq;
using Core.Utils.Extensions;
using Gameplay.ViewApi.CameraView;
using Model.CustomTypes;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Gameplay.CameraView
{
    public class CameraView : ICameraView
    {
        private readonly Camera _camera;
        private readonly Plane[] _planes = new Plane[6];

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
            GeometryUtility.CalculateFrustumPlanes(_camera, _planes);
        }

        public Camera Camera => _camera;
        public float ConstZ => _camera.farClipPlane - 10f;
        public Vector3 ScreenCenter { get; }

        public Vector3 RandomScreenPoint
        {
            get
            {
                var viewport = new Vector3(RangeFloat.One.GetRandom(), RangeFloat.One.GetRandom(), ConstZ);
                return _camera.ViewportToWorldPoint(viewport);
            }
        }

        public Vector3 RandomPointOnScreenBorder
        {
            get
            {
                const float borderSize = 0.1f;
                const float doubleBorder = borderSize * 2;
                var randomX = Random.Range(0f, doubleBorder);
                var randomY = Random.Range(0f, doubleBorder);

                var viewport = new Vector3(randomX > borderSize ? 1f - doubleBorder + randomX : randomX,
                    randomY > borderSize ? 1f - doubleBorder + randomY : randomY, ConstZ);
                return _camera.ViewportToWorldPoint(viewport);
            }
        }

        public (Vector3, Vector3) RandomPointOnSideBorder
        {
            get
            {
                const float borderSize = 0.1f;
                const float doubleBorder = borderSize * 2;
                var randomX = Random.Range(0f, doubleBorder);
                var randomY = Random.Range(0f, 1f);

                var direction = randomX > borderSize ? Vector3.left : Vector3.right;

                var viewport = new Vector3(randomX > borderSize ? 1f - doubleBorder + randomX : randomX, randomY,
                    ConstZ);
                return (_camera.ViewportToWorldPoint(viewport), direction);
            }
        }


        public Vector3 GetBorderPointByDirection(Vector3 point, Vector3 direction)
        {
            var ray = new Ray(point, direction);

            var minDistance = float.MaxValue;
            var result = point;

            for (var i = 0; i < 4; i++)
            {
                if (!_planes[i].Raycast(ray, out var distance)) continue;
                if (!(distance < minDistance)) continue;
                result = ray.GetPoint(distance);
                minDistance = distance;
            }

            return result;
        }

        public Vector3 RandomPointOutsideScreenBorder
        {
            get
            {
                const float borderSize = 0.1f;
                const float doubleBorder = borderSize * 2;
                var randomX = Random.Range(0f, doubleBorder);
                var randomY = Random.Range(0f, doubleBorder);

                var viewport = new Vector3(randomX > borderSize ? 1f + doubleBorder - randomX : -randomX,
                    randomY > borderSize ? 1f + doubleBorder - randomY : -randomY, ConstZ);
                return _camera.ViewportToWorldPoint(viewport);
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