using System;
using System.Linq;
using System.Threading;
using CoreMechanics.Systems;
using Cysharp.Threading.Tasks;
using Cysharp.Threading.Tasks.Triggers;
using DG.Tweening;
using DoTween.Modules;
using Gameplay.Gun;
using Gameplay.ViewApi.Gun;
using Gameplay.ViewApi.Player;
using Model.Configs.Player;
using UnityEngine;

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

        [SerializeField]
        private GameObject _bodyDetector;

        [SerializeField]
        private SpriteRenderer _fire;

        private GunComponent _baseGun;
        private GunComponent _extraGun;

        private ILaserComponent _laser;
        private CancellationTokenSource _lifeToken;

        private IObjectSpawnSystem _spawnSystem;

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

        public Transform Transform => transform;

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
            _baseGun = guns.Item1;
            _baseGun.transform.localPosition = baseGunPrefab.Resource.transform.localPosition;
            BulletRoot = _baseGun.BulletRoot;
            _extraGun = guns.Item2;
            _extraGun.transform.localPosition = extraGunPrefab.Resource.transform.localPosition;
            LaserRoot = _extraGun.BulletRoot;
            _laser = _extraGun.GetComponentInChildren<ILaserComponent>(true);
            _lifeToken = new CancellationTokenSource();

            WaitDie().Forget();
        }

        public async UniTask Destroy()
        {
            await UniTask.WhenAll(_spawnSystem.DestroyObject(_baseGun), _spawnSystem.DestroyObject(_extraGun));
        }

        private async UniTaskVoid WaitDie()
        {
            var asyncTriggerEnter = _bodyDetector.GetAsyncTriggerEnter2DTrigger();
            await asyncTriggerEnter.OnTriggerEnter2DAsync(_lifeToken.Token);
            OnDied?.Invoke();
        }
    }
}