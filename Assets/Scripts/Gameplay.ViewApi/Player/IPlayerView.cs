using System;
using CoreMechanics.ObjectLinks.UnityObjectLink;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Gameplay.ViewApi.Player
{
    public interface IPlayerView
    {
        event Action Died;
        public UniTask<Transform> SpawnPLayer(GameObjectLink prefab, Vector3 spawnPosition);
        public UniTask Despawn();
        void AddRotation(float angle);
        void MoveTo(Vector3 position);
        Bounds GetBounds();

    }
}