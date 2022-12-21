using System.Threading;
using CoreMechanics.ObjectLinks.UnityObjectLink;
using CoreMechanics.Systems;
using Cysharp.Threading.Tasks;
using Gameplay.ViewApi.CameraView;
using Gameplay.ViewApi.Enemy;
using Model.Configs.Enemy;
using UnityEngine;
using UnityEngine.Assertions;

namespace Gameplay.Enemy
{
    public class NloView : INloView
    {
        private readonly ICameraView _cameraView;
        private readonly Transform _root;
        private readonly IObjectSpawnSystem _spawnSystem;
        private Nlo _active;
        private CancellationTokenSource _tokenSource;

        public NloView(IObjectSpawnSystem spawnSystem, ICameraView cameraView, CancellationTokenSource tokenSource,
            Transform root)
        {
            _spawnSystem = spawnSystem;
            _cameraView = cameraView;
            _tokenSource = tokenSource;
            _root = root;
        }

        public async UniTask<INloComponent> Spawn(NloConfig config, Vector3 position, Vector3 direction)
        {
            Assert.IsNull(_active, "Have another nlo instance");
            var nlo = await _spawnSystem.SpawnObject<Nlo>(config.Prefab, _root, position);
            await nlo.Init(config, direction, _cameraView, _tokenSource, _spawnSystem);
            _active = nlo;
            return nlo;
        }

        public async UniTask Destroy()
        {
            if (_active == null)
                return;
            await _spawnSystem.DestroyObject(_active);
            _active = null;
        }

        public void SetupTokenSource(CancellationTokenSource tokenSource)
        {
            if (_active != null)
            {
                _active.SetupTokenSource(tokenSource);
                _tokenSource = tokenSource;
            }
        }
    }
}