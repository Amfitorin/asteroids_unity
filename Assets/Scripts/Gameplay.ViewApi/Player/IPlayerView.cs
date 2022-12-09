using System;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Gameplay.ViewApi.Player
{
    public interface IPlayerView
    {
        event Action Died;
        public UniTask SpawnPLayer(Vector2 spawnPosition);
    }
}