using System.Threading;
using Core.Utils.Extensions;
using CoreMechanics.Systems;
using Cysharp.Threading.Tasks;
using Cysharp.Threading.Tasks.Linq;
using Cysharp.Threading.Tasks.Triggers;
using Gameplay.ViewApi.CameraView;
using Gameplay.ViewApi.Gun;
using Model.Configs.Gun;
using UnityEngine;

namespace Gameplay.Gun
{
    public class BulletGun : MonoBehaviour, IBulletComponent, ITokenCancelSource
    {
        [SerializeField]
        private SpriteRenderer _renderer;

        [SerializeField]
        private GameObject _detector;

        private ICameraView _cameraView;
        private BulletConfig _config;
        private Vector3 _direction;
        private CancellationTokenSource _globalToken;
        private CancellationTokenSource _lifeToken;
        private CancellationTokenSource _timerToken;

        private void OnDisable()
        {
            ClearTokens();
        }

        private void OnValidate()
        {
            _renderer = GetComponentInChildren<SpriteRenderer>();
        }

        public async UniTask<bool> WaitDie()
        {
            var asyncTriggerEnter = _detector.GetAsyncTriggerEnter2DTrigger();
            var result = await asyncTriggerEnter.OnTriggerEnter2DAsync(_timerToken.Token).SuppressCancellationThrow();
            return result.IsCanceled;
        }

        public void SetupTokenSource(CancellationTokenSource tokenSource)
        {
            _globalToken = tokenSource;
            RunBullet().Forget();
        }

        public void Init(BulletConfig config, Vector3 direction, ICameraView cameraView,
            CancellationTokenSource globalToken)
        {
            _cameraView = cameraView;
            _config = config;
            _direction = direction;
            _globalToken = globalToken;
            ReInitTokens();
            RunBullet().Forget();
        }

        private void ReInitTokens()
        {
            ClearTokens();
            _timerToken = new CancellationTokenSource(_config.LifeTimeMS);
            _lifeToken = new CancellationTokenSource();
        }

        private void ClearTokens()
        {
            _timerToken?.Cancel();
            _timerToken = null;
            _lifeToken?.Cancel();
            _lifeToken = null;
        }

        private async UniTaskVoid RunBullet()
        {
            var linkedSource =
                CancellationTokenSource.CreateLinkedTokenSource(_globalToken.Token, _timerToken.Token,
                    _lifeToken.Token);
            await UniTaskAsyncEnumerable.EveryUpdate(PlayerLoopTiming.LastUpdate)
                .ForEachAsync(_ => { Move(); }, cancellationToken: linkedSource.Token);
        }

        private void Move()
        {
            var position = transform.position;
            CornerRect rendererBounds = _renderer.bounds;
            if (!_cameraView.IsObjectVisible(rendererBounds))
            {
                position = _cameraView.InversePosition(position, rendererBounds);
            }

            position += _direction * _config.Speed * Time.deltaTime;
            transform.position = position;
        }
    }
}