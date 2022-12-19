using System.Linq;
using CoreMechanics.Systems;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using DoTween.Modules;
using Gameplay.Gun;
using Gameplay.ViewApi.Gun;
using Model.Configs.Player;
using UnityEngine;
using Action = Unity.Plastic.Newtonsoft.Json.Serialization.Action;

namespace Gameplay.Player
{
    public class PLayerComponent : MonoBehaviour, IPlayerComponent
    {
        [SerializeField]
        private SpriteRenderer[] _renderers;

        [SerializeField]
        private Transform _baseGunRoot;

        [SerializeField]
        private Transform _extraGunRoot;

        private IObjectSpawnSystem _spawnSystem;

        [SerializeField]
        private SpriteRenderer _fire;

        private ILaserComponent _laser;

        public Transform BulletRoot { get; private set; }
        public Transform LaserRoot { get; private set; }

        private Bounds ZeroBounds => new(transform.position, Vector3.zero);
        public ILaserComponent Laser => _laser;

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

        public void ApplySpeed(float percent)
        {
            _fire.DOFade(percent, 0.1f);
            _fire.transform.DOScaleY(percent, 0.1f);
        }


        public async UniTask Init(PLayerConfig config, IObjectSpawnSystem spawnSystem)
        {
            _spawnSystem = spawnSystem;
            var baseGunPrefab = config.BulletGun.Config.Settings.Prefab;
            var extraGunPrefab = config.LaserGun.Config.Settings.Prefab;
            var guns = await UniTask.WhenAll(
                _spawnSystem.SpawnObject<GunComponent>(
                    baseGunPrefab, _baseGunRoot, Vector3.zero),
                _spawnSystem.SpawnObject<GunComponent>(
                    extraGunPrefab, _extraGunRoot, Vector3.zero));
            var baseGun = guns.Item1;
            baseGun.transform.localPosition = baseGunPrefab.Resource.transform.localPosition;
            BulletRoot = baseGun.BulletRoot;
            var extraGun = guns.Item2;
            extraGun.transform.localPosition = extraGunPrefab.Resource.transform.localPosition;
            LaserRoot = extraGun.BulletRoot;
            _laser = extraGun.GetComponentInChildren<ILaserComponent>(true);
        }
    }
}