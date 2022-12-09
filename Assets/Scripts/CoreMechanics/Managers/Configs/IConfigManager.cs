using JetBrains.Annotations;

namespace CoreMechanics.Managers.Configs
{
    public interface IConfigManager : IManager
    {
        [CanBeNull]
        T Load<T>(string path) where T : ConfigBase;
    }
}