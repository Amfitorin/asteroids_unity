using System;
using JetBrains.Annotations;
using Object = UnityEngine.Object;

namespace CoreMechanics.ObjectLinks.UnityObjectLink
{
    [Serializable]
    public class UnityObjectLink<T> : UnityObjectLinkAbstract<T> where T : Object
    {
        [CanBeNull]
        public T Resource => Load();
    }
}