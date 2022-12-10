using CoreMechanics.ObjectLinks.UnityObjectLink;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace CoreMechanics.Systems
{
    public interface IObjectSpawnSystem
    {
        UniTask<T> SpawnObject<T>(GameObjectLink prefab, Transform root, Vector3 position) where T : Object;
        UniTask DestroyObject(GameObject obj);
    }
}