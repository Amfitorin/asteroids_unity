using System;
using System.Linq;
using CoreMechanics.Managers.Configs;
using CoreMechanics.ObjectLinks.Configs;
using Model.Level;
using UnityEngine;

namespace Model.Configs.Level
{
    [CreateAssetMenu(menuName = "Configs/LevelsConfig")]
    public class LevelsConfig : ConfigBase
    {
        [SerializeField]
        private LevelSettings[] _levels;

        public LevelSettings[] Levels => _levels;

        protected override void OnValidate()
        {
            base.OnValidate();
            foreach (var level in _levels)
                if (_levels.Count(x => x.Level == level.Level) > 1)
                    Debug.LogError($"duplicate level settings {level.Level}");
        }
    }

    [Serializable]
    public class LevelsConfigLink : ConfigLink<LevelsConfig>
    {
    }
}