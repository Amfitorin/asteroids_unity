using CoreMechanics.ObjectLinks.UnityObjectLink;
using UnityEngine;

namespace Gameplay.Loaders
{
    public class WorldLoader : MonoBehaviour
    {
        [SerializeField]
        private GameObjectLink _worldPrefab;

        private void Awake()
        {
            Instantiate(_worldPrefab.Resource);
        }
    }
}