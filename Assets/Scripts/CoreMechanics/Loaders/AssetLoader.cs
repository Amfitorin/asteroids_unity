using System;
using System.Collections;
using System.Collections.Generic;
using Core.Utils.Extensions;
using UnityEditor.Build.Content;
using UnityEngine;

namespace CoreMechanics.Loaders
{
   public sealed class AssetLoader : AssetLoaderBase
	{
		private class PackInfo
		{
			public string FileName;
			public Dictionary<string, ulong> Offsets;
		}
		
		private readonly Dictionary<string, AssetBundle> _assetsList = new();
		private readonly Dictionary<string, AssetBundle> _bundles = new();
		private readonly Dictionary<string, AssetBundleInfo> _metadata = new();

		private IDisposable _metadataLoader;
		private PackInfo[] _externalPackInfos;

		private readonly IAssetLoaderCache _loaderCache;

		public AssetLoader(IAssetLoaderCache loaderCache)
		{
			_loaderCache = loaderCache;
		}
		

		public override void Release()
		{
			if (_metadataLoader != null)
			{
				_metadataLoader.Dispose();
				_metadataLoader = null;
			}
		}

		public override T Load<T>(string path)
		{
			if (path.IsNullOrEmpty())
				return null;

			path = GetAssetPath(path);
			var obj = _loaderCache.GetFromCache(path) as T;
			if (obj != null)
				return obj;
			

			if (!_loaderCache.PutToCache(path, obj))
			{
				Debug.LogErrorFormat("Can't load asset '{0}' with type {1} from bundle '{2}'", path, typeof(T), "");
				return null;
			}

			return obj;
		}

		public override IEnumerator Load<T>(string path, Action<T> callback)
		{
			T obj = null;
			if (!path.IsNullOrEmpty())
			{
				if (!TryLoadResource(path, out obj))
				{
				}
			}

			try
			{
				callback(obj);
			}
			catch (Exception e)
			{
				Debug.LogException(e);
			}
			yield return null;
		}

		protected override bool IsAssetExists(string fullAssetName)
		{
			if (string.IsNullOrEmpty(fullAssetName))
				return false;

			return true;
		}
		
		
		
	}
}