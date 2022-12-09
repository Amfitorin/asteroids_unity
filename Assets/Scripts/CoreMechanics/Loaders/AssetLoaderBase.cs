using System;
using System.Collections;
using Core.Utils.Extensions;
using CoreMechanics.Managers.Configs;
using UnityEngine;
using Object = UnityEngine.Object;

namespace CoreMechanics.Loaders
{
    public abstract class AssetLoaderBase : IAssetLoader
    {
	    private readonly IConfigManager _configsManager;
	    private const string AssetsPath = "Assets/Bundles/";
	    private const string ResourcesPath = "Assets/Resources/";

	    protected AssetLoaderBase()
	    {
		    _configsManager = new ConfigManagerFabric().GetManager();
	    }

	    public bool TryLoadResource<T>(string path, out T res) where T : Object
		{
			res = null;

			if (string.IsNullOrEmpty(path))
				return false;

			if (LoadFromResources(path, out res))
			{
				return true;
			}

			var resource = Load<T>(path);
			if (resource != null)
			{
				res = resource;
				return true;
			}

			Debug.LogError("Asset from bundle can't be loaded");
			return false;
		}

	    public bool TryLoadConfig<T>(string path, out T res) where T : ConfigBase
	    {
		    res = _configsManager.Load<T>(path);
		    return true;
	    }

	    public abstract T Load<T>(string assetPath) where T : Object;

	    public abstract IEnumerator Load<T>(string assetPath, Action<T> future) where T : Object;

	    protected string GetAssetPath(string path)
		{
			if (path.IsNullOrEmpty())
				return string.Empty;

			return TryPatch(path, out var patchedPath) ? patchedPath : path;
		}

	    public abstract void Release();

	    private bool TryPatch(string originalPath, out string patchPath)
		{
			if (!originalPath.StartsWith(AssetsPath))
			{
				patchPath = "";
				return false;
			}

			var path = $"{AssetsPath}Patch/{originalPath[AssetsPath.Length..]}";
			if (!IsAssetExists(path))
			{
				patchPath = "";
				return false;
			}

			patchPath = path;
			return true;
		}


	    private static bool LoadFromResources<T>(string path, out T res) where T : Object
		{
			res = null;
			if (!path.StartsWith(ResourcesPath))
				return false;

			path = path[ResourcesPath.Length..];
			var dot = path.LastIndexOf('.');
			var slash = path.LastIndexOf('/');
			path = dot > slash ? path[..dot] : path;
			res = Resources.Load<T>(path);
			if (res == null)
				Debug.LogErrorFormat("Can't find asset '{0}' with type {1}", path, typeof(T));
			return true;
		}

	    protected abstract bool IsAssetExists(string fullAssetName);
    }
}