using System;
using CoreMechanics.ObjectLinks.UnityObjectLink;
using UnityEngine;

namespace UI.View.Game
{
    [Serializable]
    public class LifeView
    {
        [SerializeField]
        private GameObjectLink _lifePrefab;

        [SerializeField]
        private Transform _lifesRoot;

        public void Update(int lifes)
        {
        }
    }
}