using UnityEngine;

namespace Gameplay.Gun
{
    public class GunComponent : MonoBehaviour
    {
        [SerializeField]
        private Transform _bulletRoot;

        public Transform BulletRoot => _bulletRoot;
    }
}