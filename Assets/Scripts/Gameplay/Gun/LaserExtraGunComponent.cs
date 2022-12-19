using Gameplay.ViewApi.Gun;
using UnityEngine;

namespace Gameplay.Gun
{
    public class LaserExtraGunComponent : MonoBehaviour, ILaserComponent
    {
        [SerializeField]
        private LineRenderer _laser;

        [SerializeField]
        private CapsuleCollider2D _collider;


        public void Show()
        {
            gameObject.SetActive(true);
        }

        public void Hide()
        {
            gameObject.SetActive(false);
        }

        public void UpdatePositions(Vector3 first, Vector3 second)
        {
            _laser.SetPosition(0, first);
            _laser.SetPosition(1, second);
            var size = (second - first).magnitude;
            _collider.offset = new Vector2(0f, size / 2f);
            _collider.size = new Vector2(_collider.size.x, size);
        }
    }
}