using System;
using Cysharp.Threading.Tasks;
using Gameplay.ViewApi.Gun;
using Model.Configs.Player;
using UnityEngine;

namespace Gameplay.ViewApi.Player
{
    public interface IPlayerView
    {
        event Action Died;
        ILaserComponent Laser { get; }
        public UniTask<Transform> SpawnPLayer(PLayerConfig config, Vector3 spawnPosition);
        public UniTask Despawn();
        void AddRotation(float angle);
        void MoveTo(Vector3 position);
        Bounds GetBounds();
        Transform BaseGunTransform();
        Transform ExtraGunTransform();
        void ApplySpeed(float percent);
    }
}