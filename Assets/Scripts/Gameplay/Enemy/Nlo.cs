using System.Threading;
using Core.Utils.Extensions;
using CoreMechanics.Systems;
using Cysharp.Threading.Tasks;
using Cysharp.Threading.Tasks.Linq;
using Cysharp.Threading.Tasks.Triggers;
using Gameplay.Gun;
using Gameplay.ViewApi.CameraView;
using Gameplay.ViewApi.Enemy;
using Model.Configs.Enemy;
using Model.Configs.Gun;
using UnityEngine;

namespace Gameplay.Enemy
{
    public class Nlo : MonoBehaviour, INloComponent, ITokenCancelSource
    {
        [SerializeField]
        private SpriteRenderer _renderer;

        [SerializeField]
        private GameObject _triggerDetector;

        [SerializeField]
        private Transform _gunRoot;

        private ICameraView _cameraView;
        private NloConfig _config;

        private Vector3 _direction;
        private CancellationTokenSource _globalToken;
        private CancellationTokenSource _lifeToken;
        private float _speed;

        private void OnDisable()
        {
            ClearToken();
        }

        private void OnDestroy()
        {
            ClearToken();
        }

        public Transform BulletRoot { get; private set; }
        public CancellationTokenSource LifeToken => _lifeToken;

        public Transform Root => transform;

        public void ChangeDirection(Vector3 direction)
        {
            _direction += direction;
        }

        public async UniTask<Vector3> WaitDie()
        {
            var asyncTriggerEnter = _triggerDetector.GetAsyncTriggerEnter2DTrigger();
            await asyncTriggerEnter.OnTriggerEnter2DAsync(_lifeToken.Token);
            return transform.position;
        }

        public async UniTask WaitInvisible()
        {
            await UniTask.WaitWhile(() =>
            {
                CornerRect rendererBounds = _renderer.bounds;
                return _cameraView.IsObjectVisible(rendererBounds) ||
                       _cameraView.Camera.WorldToViewportPoint(transform.position).x is > 0f and < 1f;
            }, cancellationToken: _lifeToken.Token);
        }

        public void SetupTokenSource(CancellationTokenSource tokenSource)
        {
            _globalToken = tokenSource;
            StartMove().Forget();
        }

        private void ClearToken()
        {
            _lifeToken?.Cancel();
            _lifeToken = null;
        }

        public async UniTask Init(NloConfig config, Vector3 direction, ICameraView cameraView,
            CancellationTokenSource globalToken, IObjectSpawnSystem spawnSystem)
        {
            _globalToken = globalToken;
            _direction = direction;
            _config = config;
            _cameraView = cameraView;
            ClearToken();
            _lifeToken = new CancellationTokenSource();
            _speed = _config.Speed.GetRandom();

            await SpawnGun(config.Gun, spawnSystem);

            StartMove().Forget();
        }

        private async UniTask SpawnGun(BulletGunConfig config, IObjectSpawnSystem spawnSystem)
        {
            var gunPrefab = config.Settings.Prefab;
            var gun = await spawnSystem.SpawnObject<GunComponent>(gunPrefab, _gunRoot, Vector3.zero);
            gun.transform.localPosition = gunPrefab.Resource.transform.localPosition;
            BulletRoot = gun.BulletRoot;
        }

        private async UniTaskVoid StartMove()
        {
            var linkedSource =
                CancellationTokenSource.CreateLinkedTokenSource(_lifeToken.Token, _globalToken.Token);
            await UniTaskAsyncEnumerable.EveryUpdate(PlayerLoopTiming.PreLateUpdate)
                .ForEachAsync(_ => { Move(); }, cancellationToken: linkedSource.Token);
        }

        private void Move()
        {
            var position = transform.position;
            CornerRect rendererBounds = _renderer.bounds;
            if (!_cameraView.IsObjectVisible(rendererBounds) &&
                _cameraView.Camera.WorldToViewportPoint(position).x is > 0f and < 1f)
            {
                position = _cameraView.InversePosition(position, rendererBounds);
            }

            position += _direction * _speed * Time.deltaTime;

            transform.position = position;
        }
    }
}