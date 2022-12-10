using System;
using UnityEngine;

namespace CoreMechanics.ObjectLinks.UnityObjectLink
{
    [Serializable]
    public class SpriteLink : UnityObjectLink<Sprite>
    {
        public static readonly SpriteLink Empty = new();
    }
}