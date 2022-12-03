using Cysharp.Threading.Tasks;
using Cysharp.Threading.Tasks.Triggers;
using UnityEngine;

namespace Gameplay.Enemy
{
    public class Asteroid : MonoBehaviour, IAsyncOnTriggerEnter2DHandler
    {
        
        public async UniTask<Collider2D> OnTriggerEnter2DAsync()
        {
            throw new System.NotImplementedException();
        }
    }
}