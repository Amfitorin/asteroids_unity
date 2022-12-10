using CoreMechanics.ObjectLinks.UnityObjectLink;
using UnityEngine;

namespace Gameplay.Player
{
    public class PLayerComponent : MonoBehaviour
    {
        [SerializeField]
        private Transform _mainRoot;

        [SerializeField]
        private Transform _bulletRoot;

        [SerializeField]
        private GameObjectLink _bullet;
    }
}