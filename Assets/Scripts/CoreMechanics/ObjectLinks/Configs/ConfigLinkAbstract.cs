using System;
using Core.Utils.Extensions;
using CoreMechanics.Managers.Configs;
using CoreMechanics.Systems;
using JetBrains.Annotations;
using UnityEngine;

namespace CoreMechanics.ObjectLinks.Configs
{
    [Serializable]
    public class ConfigLinkAbstract<T>
        : IEquatable<ConfigLinkAbstract<T>>, IObjectLink<T> where T : ConfigBase
    {
        [SerializeField]
        private string _path;

        public bool Equals(ConfigLinkAbstract<T> other)
        {
            return this == other;
        }

        object IObjectLink.Resource => Load();

        public string Path => _path;

        public void Initialize(string path)
        {
            _path = path;
        }

        public void Initialize<TNext>(ConfigLinkAbstract<TNext> link) where TNext : T
        {
            Initialize(link._path);
        }

        [CanBeNull]
        protected T Load()
        {
#if UNITY_EDITOR
            if (!Application.isPlaying)
                return this.LoadInEditor();
#endif
            if (_path.IsNullOrEmpty() || !AssetSystem.Instance.Loader.TryLoadConfig<T>(_path, out var res))
                return null;

            return res;
        }

        [CanBeNull]
        public static implicit operator T(ConfigLinkAbstract<T> from)
        {
            return !ReferenceEquals(from, null) ? from.Load() : null;
        }

        public static bool operator ==(ConfigLinkAbstract<T> a, ConfigLinkAbstract<T> b)
        {
            if (ReferenceEquals(a, b))
                return true;

            if (!ReferenceEquals(a, null) && ReferenceEquals(b, null))
                return a._path.IsNullOrEmpty();

            if (ReferenceEquals(a, null))
                return b._path.IsNullOrEmpty();

            return a._path.Equals(b._path);
        }

        public static bool operator !=(ConfigLinkAbstract<T> a, ConfigLinkAbstract<T> b)
        {
            return !(a == b);
        }

        public override bool Equals(object other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;

            var link = other as ConfigLinkAbstract<T>;
            if (link != null)
                return this == link;

            var cfg = other as ConfigBase;
            if (cfg != null)
                return Load() == cfg;

            return false;
        }

        public override int GetHashCode()
        {
            // ReSharper disable once NonReadonlyMemberInGetHashCode
            return _path.GetHashCode();
        }

        public override string ToString()
        {
            return _path.IsNullOrEmpty() ? "(null!)" : _path;
        }
    }
}