using Core.Singleton;
using CoreMechanics.App;
using Gameplay.App;
using UnityEngine;

namespace Gameplay
{
    public class AppManager : Singleton<AppManager>
    {
        private const string EventProviderName = "eventProvider"; 
        public IAppController AppController { get; private set; }

        protected override void Initialize()
        {
            var go = new GameObject(EventProviderName);
            Object.DontDestroyOnLoad(go);
            var provider = go.AddComponent<AppEventProvider>();
            
            AppController = new AppController(provider);
        }
    }
}