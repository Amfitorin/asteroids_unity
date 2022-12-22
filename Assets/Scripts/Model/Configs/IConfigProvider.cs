using Model.Configs.Level;
using Model.Configs.Player;
using Model.Configs.Pool;
using Model.Configs.UI;

namespace Model.Configs
{
    public interface IConfigProvider
    {
        PLayerConfig PLayerConfig { get; }
        LevelsConfig LevelsConfig { get; }
        PoolSettingsConfig PoolSettings { get; }
        UIPrefabsConfig UIPrefabs { get; }
    }
}