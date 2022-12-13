using System;
using System.Linq;
using CoreMechanics.Managers.Configs;
using CoreMechanics.ObjectLinks.Configs;
using Model.Level;
using UnityEngine;
using UnityEngine.Assertions;

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
            Assert.IsFalse(_levels == null || _levels.Length == 0, $"Levels settings is empty {name}");
            Array.Sort(_levels, (x, y) => x.Level.CompareTo(y.Level));
                
        }

        public LevelSettings GetLevelSettings(int level)
        {
            if (level <= 0)
            {
                return _levels[0];
            }

            for (var i = 0; i < _levels.Length; i++)
            {
                var settings = _levels[i];
                if (level == settings.Level)
                {
                    return settings;
                }

                if (i == _levels.Length - 1)
                {
                    return settings;
                }

                if (level < settings.Level)
                {
                    return _levels[Math.Max(0, i - 1)];
                }
            }

            return _levels.Last();
        }
    }

    [Serializable]
    public class LevelsConfigLink : ConfigLink<LevelsConfig>
    {
    }
}