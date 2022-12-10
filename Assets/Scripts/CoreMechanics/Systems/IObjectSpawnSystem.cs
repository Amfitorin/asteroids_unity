using CoreMechanics.ObjectLinks.UnityObjectLink;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace CoreMechanics.Systems
{
    public interface IObjectSpawnSystem
    {
        UniTask SpawnObject<T>(GameObjectLink prefab, Transform root, Vector3 position);
        UniTask DestroyObject(Object obj);
    }
}