using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Gameplay.ViewApi.Enemy
{
    public interface IAsteroidComponent
    {
        AsteroidSpawnData SpawnData { get; }
        UniTask<Vector3> WaitDie();
    }
}