using Model.Configs.Level;
using Model.Configs.Player;

namespace Model.Configs
{
    public interface IConfigProvider
    {
        PLayerConfig PLayerConfig { get; }
        LevelsConfig LevelsConfig { get; }
    }
}