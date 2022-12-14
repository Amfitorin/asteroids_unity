using System.Linq;
using CoreMechanics.ObjectLinks.UnityObjectLink;
using UnityEngine;
using Action = Unity.Plastic.Newtonsoft.Json.Serialization.Action;

namespace Gameplay.Player
{
    public class PLayerComponent : MonoBehaviour, IPlayerComponent
    {
        [SerializeField]
        private Transform _mainRoot;

        [SerializeField]
        private Transform _bulletRoot;

        [SerializeField]
        private SpriteRenderer[] _renderers;

        public Transform BulletRoot => _bulletRoot;

        private Bounds ZeroBounds => new(transform.position, Vector3.zero);

        private void OnValidate()
        {
            _renderers = GetComponentsInChildren<SpriteRenderer>();
        }

        public event Action OnDied;

        public Bounds GetBounds()
        {
            if (_renderers == null || _renderers.Length == 0)
            {
                return ZeroBounds;
            }

            var renderers = _renderers.Where(x => x.gameObject.activeInHierarchy).ToArray();
            if (renderers.Length == 0)
            {
                return ZeroBounds;
            }

            var bounds = renderers[0].bounds;

            for (var i = 1; i < renderers.Length; i++)
            {
                var bound = renderers[i].bounds;
                bounds.Encapsulate(bound);
            }

            return bounds;
        }
    }
}