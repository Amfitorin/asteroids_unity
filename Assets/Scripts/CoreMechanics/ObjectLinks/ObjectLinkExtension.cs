using JetBrains.Annotations;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace CoreMechanics.ObjectLinks
{
    public static class ObjectLinkExtension
    {
#if UNITY_EDITOR
        [CanBeNull]
        public static T LoadInEditor<T>(this IObjectLink<T> link) where T : Object
        {
            if (link == null)
                return default(T);
            var path = link.Path;
            return typeof(Component).IsAssignableFrom(typeof(T))
                ? AssetDatabase.LoadAssetAtPath<GameObject>(path).GetComponent<T>()
                : AssetDatabase.LoadAssetAtPath<T>(path);
        }
#endif
    }
}