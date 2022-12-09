using UnityEngine;

namespace CoreMechanics.Managers.Configs
{
    public class ConfigManagerFabric : IManagerFabric<IConfigManager>
    {
        public IConfigManager GetManager()
        {
            if (Application.isEditor)
                return new EditorConfigManager();
            return new ConfigManager();
        }
    }
}