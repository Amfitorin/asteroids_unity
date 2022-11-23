using System;
using Core.Utils.Extensions;
using CoreMechanics.Systems;
using JetBrains.Annotations;
using UnityEngine;
using Object = UnityEngine.Object;

namespace CoreMechanics.ObjectLinks.UnityObjectLink
{
   [Serializable]
	public abstract class UnityObjectLinkAbstract<T> : IEquatable<UnityObjectLinkAbstract<T>>, IObjectLink<T>
		where T : Object
	{
		private T _cached;

		private T Cached
		{
			get => _cached;
			set => _cached = value;
		}

		[SerializeField]
		private string _path;

		public string Path => _path;

		public bool Equals(UnityObjectLinkAbstract<T> other)
		{
			return this == other;
		}

		object IObjectLink.Resource => Load();

		void IObjectLink.Initialize(string path)
		{
			Initialize(path);
		}

		public void Initialize(UnityObjectLinkAbstract<T> other)
		{
			Initialize(other._path, other.Cached);
		}

		public void Initialize(string path, T obj = null)
		{
			_path = path;
			Cached = obj;
		}

		public void Preload()
		{
			Load();
		}

		[CanBeNull]
		protected T Load()
		{
#if UNITY_EDITOR
			if (!Application.isPlaying)
				return this.LoadInEditor();
#endif

			if (Cached == null)
				Cached = Load(_path);

			return Cached;
		}

		private T Load(string path)
		{
			if (typeof(Component).IsAssignableFrom(typeof(T)))
			{
				GameObject gameObject;
				if (AssetSystem.Instance.Loader.TryLoadResource(path, out gameObject))
					return null;
				if (gameObject == null)
					return null;
				var component = gameObject.GetComponent<T>();
				if (component == null)
					Debug.LogErrorFormat("Can't get component {0} from gameObject \"{1}\"", typeof(T), path);
				return component;
			}

			T res;
			return AssetSystem.Instance.Loader.TryLoadResource(path, out res) ? res : null;
		}

		[CanBeNull]
		public static implicit operator T(UnityObjectLinkAbstract<T> from)
		{
			return !ReferenceEquals(from, null) ? from.Load() : null;
		}

		public static bool operator ==(UnityObjectLinkAbstract<T> a, UnityObjectLinkAbstract<T> b)
		{
			if (ReferenceEquals(a, b))
				return true;

			if (!ReferenceEquals(a, null) && ReferenceEquals(b, null))
				return a.Path.IsNullOrEmpty();

			if (ReferenceEquals(a, null))
				return b.Path.IsNullOrEmpty();

			return a.Path == b.Path;
		}

		public static bool operator !=(UnityObjectLinkAbstract<T> a, UnityObjectLinkAbstract<T> b)
		{
			return !(a == b);
		}

		public override bool Equals(object other)
		{
			if (ReferenceEquals(null, other)) return false;
			if (ReferenceEquals(this, other)) return true;

			var link = other as UnityObjectLinkAbstract<T>;
			if (link != null)
				return this == link;

			return false;
		}

		public override int GetHashCode()
		{
			return (_path != null ? _path.GetHashCode() : 0);
		}

		public override string ToString()
		{
			return _path.IsNullOrEmpty() ? "(!null)" : _path;
		}
	}
}