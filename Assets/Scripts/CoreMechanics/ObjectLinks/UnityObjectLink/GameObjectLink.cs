using System;
using UnityEngine;

namespace CoreMechanics.ObjectLinks.UnityObjectLink
{
    [Serializable]
    public class GameObjectLink : UnityObjectLink<GameObject>
    {
        public static readonly GameObjectLink Empty = new();
    }
}