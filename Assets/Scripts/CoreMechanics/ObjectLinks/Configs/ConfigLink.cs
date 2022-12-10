using System;
using CoreMechanics.Managers.Configs;
using JetBrains.Annotations;

namespace CoreMechanics.ObjectLinks.Configs
{
    [Serializable]
    public class ConfigLink<T> : ConfigLinkAbstract<T> where T : ConfigBase
    {
        [CanBeNull]
        public T Config => Load();
    }
}