using System;
using CoreMechanics.ObjectLinks.UnityObjectLink;
using UnityEngine;

namespace Model.Gun
{
    [Serializable]
    public abstract class GunSettings
    {
        [SerializeField]
        private float _cooldown;

        [SerializeField]
        private GameObjectLink _prefab;

        public float Cooldown => _cooldown;

        public GameObjectLink Prefab => _prefab;
    }
}