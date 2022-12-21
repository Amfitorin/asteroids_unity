using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Gameplay.ViewApi.Enemy
{
    public interface INloComponent
    {
        Transform Root { get; }
        Transform BulletRoot { get; }
        CancellationTokenSource LifeToken { get; }
        void ChangeDirection(Vector3 direction);
        UniTask<Vector3> WaitDie();
        UniTask WaitInvisible();
    }
}