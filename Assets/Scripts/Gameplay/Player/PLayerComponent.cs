using System.Linq;
using CoreMechanics.ObjectLinks.UnityObjectLink;
using Unity.Plastic.Newtonsoft.Json.Serialization;
using UnityEngine;

namespace Gameplay.Player
{
    public class PLayerComponent : MonoBehaviour, IPlayerComponent
    {
        [SerializeField]
        private Transform _mainRoot;

        [SerializeField]
        private Transform _bulletRoot;

        [SerializeField]
        private GameObjectLink _bullet;

        [SerializeField]
        private SpriteRenderer[] _renderers;

        private Bounds _zeroBounds => new Bounds(transform.position, Vector3.zero);

        private void OnValidate()
        {
            _renderers = GetComponentsInChildren<SpriteRenderer>();
        }

        public event Action OnDied;

        public Bounds GetBounds()
        {
            if (_renderers == null || _renderers.Length == 0)
            {
                return _zeroBounds;
            }

            var renderers = _renderers.Where(x => x.gameObject.activeInHierarchy).ToArray();
            if (renderers.Length == 0)
            {
                return _zeroBounds;
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