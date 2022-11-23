using Core.Singleton;
using CoreMechanics.Loaders;
using UnityEngine;

namespace CoreMechanics.Systems
{
    public class AssetSystem : Singleton<AssetSystem>
    {
        private IAssetLoaderCache _loaderCache;
        public IAssetLoader Loader { get; private set; }

        protected override void Initialize()
        {
            _loaderCache = new AssetLoaderCache();
            if (Application.isEditor)
            {
                Loader = new EditorAssetLoader(_loaderCache);
            }
            else
            {
                Loader = new AssetLoader(_loaderCache);
            }
        }

        protected override void DoRelease()
        {
            base.DoRelease();
            _loaderCache.ClearCache();
        }
    }
}