using System;
using Unity.Collections;
using UnityEditor;
using UnityEngine;

namespace CoreMechanics.Managers.Configs
{
    public abstract class ConfigBase : ScriptableObject, IEquatable<ConfigBase>
    {
        [SerializeField]
        [ReadOnly]
        private string _path;

        protected virtual void OnValidate()
        {
#if UNITY_EDITOR
            if (!EditorUtility.IsPersistent(this))
                return;
            _path = AssetDatabase.GetAssetPath(this);
#endif
        }

        public bool Equals(ConfigBase other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return base.Equals(other) && _path == other._path;
        }

        public static bool operator ==(ConfigBase c1, ConfigBase c2)
        {
            if (ReferenceEquals(c1, c2))
                return true;
            if (ReferenceEquals(c1, null) || ReferenceEquals(c2, null))
                return false;
            return c1._path.Equals(c2._path);
        }

        public static bool operator !=(ConfigBase c1, ConfigBase c2)
        {
            return !(c1 == c2);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != GetType()) return false;
            return Equals((ConfigBase)obj);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(base.GetHashCode(), _path);
        }
    }
}