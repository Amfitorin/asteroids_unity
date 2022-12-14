using System.Collections.Generic;
using System.Threading;
using Core.Utils.Extensions;
using CoreMechanics.Systems;
using Cysharp.Threading.Tasks;
using Gameplay.ViewApi.CameraView;
using Gameplay.ViewApi.Gun;
using Model.Configs.Gun;
using UnityEngine;

namespace Gameplay.Gun
{
    public class BulletView : IBulletView
    {
        private readonly IObjectSpawnSystem _spawnSystem;
        private readonly ICameraView _cameraView;
        private readonly CancellationTokenSource _tokenSource;
        private readonly Transform _bulletRoot;
        private readonly List<BulletGun> _bullets = new();

        public BulletView(IObjectSpawnSystem spawnSystem, ICameraView cameraView, CancellationTokenSource tokenSource,
            Transform bulletRoot)
        {
            _spawnSystem = spawnSystem;
            _cameraView = cameraView;
            _tokenSource = tokenSource;
            _bulletRoot = bulletRoot;
        }

        public async UniTask<IBulletComponent> RunBullet(BulletConfig config, Vector3 position, Vector3 direction)
        {
            var bullet = await _spawnSystem.SpawnObject<BulletGun>(config.Prefab, _bulletRoot, position);
            bullet.Init(config, direction, _cameraView, _tokenSource);
            _bullets.Add(bullet);
            return bullet;
        }

        public async UniTask DestroyBullet(IBulletComponent bullet)
        {
            var bulletGun = bullet as BulletGun;
            _bullets.RemoveWithReplaceLast(bulletGun);
            await _spawnSystem.DestroyObject(bulletGun);
        }
    }
}