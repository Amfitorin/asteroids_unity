using System.Threading;
using Core.Utils.Extensions;
using CoreMechanics.Systems;
using Cysharp.Threading.Tasks;
using Cysharp.Threading.Tasks.Linq;
using Cysharp.Threading.Tasks.Triggers;
using Gameplay.ViewApi.CameraView;
using Gameplay.ViewApi.Enemy;
using UnityEngine;

namespace Gameplay.Enemy
{
    public class Asteroid : MonoBehaviour, ITokenCancelSource, IAsteroidComponent
    {
        [SerializeField]
        private SpriteRenderer _renderer;

        [SerializeField]
        private GameObject _detector;

        private ICameraView _cameraView;
        private CancellationToken _globalToken;

        private CancellationTokenSource _lifeToken;
        private float _rotationSpeed;

        private void OnDisable()
        {
            ClearToken();
        }

        private void OnDestroy()
        {
            _lifeToken?.Cancel();
            _lifeToken = null;
        }

        private void OnValidate()
        {
            _renderer = GetComponentInChildren<SpriteRenderer>();
        }

        public AsteroidSpawnData SpawnData { get; private set; }

        public async UniTask<Vector3> WaitDie()
        {
            var asyncTriggerEnter = _detector.GetAsyncTriggerEnter2DTrigger();
            await asyncTriggerEnter.OnTriggerEnter2DAsync(_lifeToken.Token);
            return transform.position;
        }

        public void SetupTokenSource(CancellationTokenSource tokenSource)
        {
            _globalToken = tokenSource.Token;
            StartMove().Forget();
        }

        private void ClearToken()
        {
            _lifeToken?.Cancel();
            _lifeToken = null;
        }

        private void ReInitToken()
        {
            ClearToken();
            _lifeToken = new CancellationTokenSource();
        }

        public void Init(AsteroidSpawnData spawnData, ICameraView cameraView, CancellationTokenSource globalToken)
        {
            SpawnData = spawnData;
            _cameraView = cameraView;
            ReInitToken();
            _globalToken = globalToken.Token;
            _rotationSpeed = spawnData.Config.AngleSpeed.GetRandom();

            if (_renderer != null && spawnData.Config.Sprites.Length > 0)
            {
                _renderer.sprite = spawnData.Config.Sprites.RandomElement();
            }

            StartMove().Forget();
        }

        private async UniTaskVoid StartMove()
        {
            var linkedSource =
                CancellationTokenSource.CreateLinkedTokenSource(_lifeToken.Token, _globalToken);
            await UniTaskAsyncEnumerable.EveryUpdate(PlayerLoopTiming.PreLateUpdate)
                .ForEachAsync(_ =>
                {
                    Move();
                    Rotate();
                }, cancellationToken: linkedSource.Token);
        }

        private void Rotate()
        {
            var angle = _rotationSpeed * Time.deltaTime;
            transform.rotation *= Quaternion.Euler(0f, 0f, angle);
        }

        private void Move()
        {
            var position = transform.position;
            CornerRect rendererBounds = _renderer.bounds;
            if (!_cameraView.IsObjectVisible(rendererBounds))
            {
                position = _cameraView.InversePosition(position, rendererBounds);
            }

            position += SpawnData.Direction * SpawnData.Config.Speed * Time.deltaTime;
            transform.position = position;
        }
    }
}