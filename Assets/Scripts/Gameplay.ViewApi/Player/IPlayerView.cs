using System;
using CoreMechanics.ObjectLinks.UnityObjectLink;
using Cysharp.Threading.Tasks;
using Model.Configs.Player;
using UnityEngine;

namespace Gameplay.ViewApi.Player
{
    public interface IPlayerView
    {
        event Action Died;
        public UniTask<Transform> SpawnPLayer(PLayerConfig config, Vector3 spawnPosition);
        public UniTask Despawn();
        void AddRotation(float angle);
        void MoveTo(Vector3 position);
        Bounds GetBounds();
        Vector3 GetBaseGunPoint();
        void ApplySpeed(float percent);
    }
}